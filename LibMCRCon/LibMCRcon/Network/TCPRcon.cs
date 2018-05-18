using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Net.Sockets;



//!Classes directly related to the minecraft server.
namespace LibMCRcon.RCon
{
    /// <summary>
    /// Extending a Queue of RConPacket(s), a TCP stream connection with background threads for asyncronous send/receive.
    /// </summary>
    public class TCPRcon : Queue<RconPacket>
    {


        public enum TCPState { IDLE, CONNECTING, CONNECTED, CLOSING, CLOSED, ABORTED };
        public enum RConState { IDLE, AUTHENTICATE, READY, NETWORK_FAIL, AUTHENTICATE_FAIL };
        public string LastTCPError { get; set; }

        public string RConHost { get; set; }
        public string RConPass { get; set; }
        public int RConPort { get; set; }

        public TCPState StateTCP { get; private set; }
        public RConState StateRCon { get; private set; }

        protected bool AbortTCP { get; set; }

        Thread bgCommThread;
        Queue<RconPacket> cmdQue = new Queue<RconPacket>();

        int sessionID = -1;
        /// <summary>
        /// Default constructor, will still need RCon server url, password and port.
        /// </summary>
        public TCPRcon()
            : base()
        {
          


        }
        /// <summary>
        /// Create a TCPRcon connection.  Does not open on creation.
        /// </summary>
        /// <param name="MineCraftServer">DNS address of the rcon server.</param>
        /// <param name="port">Port RCon is listening to on the server.</param>
        /// <param name="password">Configured password for the RCon server.</param>
        public TCPRcon(string host, int port, string password)
            : base()
        {
            RConHost = host;
            RConPort = port;
            RConPass = password;

        }

        /// <summary>
        /// Asynchronous que of command.  Will be sent and response collected as soon as possible.
        /// </summary>
        /// <param name="Command">The string command to send, no larger than rcon message specification for minecraft's implementation.</param>
        public void QueCommand(String Command)
        {
            cmdQue.Enqueue(RconPacket.CmdPacket(Command, sessionID));
        }

        /// <summary>
        /// Clones the current connection allowing another session to the rcon server.
        /// </summary>
        /// <returns>Return TCPRcon with the same host,port, and password.</returns>
        public TCPRcon CopyConnection()
        {

            TCPRcon r = new TCPRcon(RConHost, RConPort, RConPass);
            if (r.StartComms() == false)
                return null;

            return r;
        }

        /// <summary>
        /// Start the asynchronous communication process.
        /// </summary>
        /// <returns>True of successfully started, otherwise false.</returns>
        public bool StartComms()
        {

            if (bgCommThread != null)
                if (bgCommThread.IsAlive)
                {
                    StopComms();
                }

            bgCommThread = null;
            TimeCheck tc;

            tc = new TimeCheck(10000);


            bgCommThread = new Thread(ConnectAndProcess)
            {
                IsBackground = true
            };

            StateTCP = TCPState.IDLE;
            StateRCon = RConState.IDLE;

            bgCommThread.Start();
            while(tc.Expired == false)
            {

                if (StateTCP == TCPState.CONNECTED)
                    if (StateRCon == RConState.READY)
                        return true;

                if (StateTCP == TCPState.ABORTED)
                    break;
             
            }



            return false;
        }
        /// <summary>
        /// Stop communication and close all connections.  Will block until complete or timed out.
        /// </summary>
        public void StopComms()
        {

            StateTCP = TCPState.CLOSING;
            AbortTCP = true;
            if (bgCommThread != null)
                if (bgCommThread.IsAlive)
                    bgCommThread.Join();

            bgCommThread = null;
            StateRCon = RConState.IDLE;
        }
        /// <summary>
        /// True if connected and active.
        /// </summary>
        public bool IsConnected { get { return StateTCP == TCPState.CONNECTED; } }
        /// <summary>
        /// True if the asynchronous thread is running.
        /// </summary>
        public bool IsStarted { get { return bgCommThread.IsAlive; } }
        /// <summary>
        /// True if the connection is open and the queue is ready for commands.
        /// </summary>
        public bool IsReadyForCommands { get { return StateTCP == TCPState.CONNECTED && StateRCon == RConState.READY; } }

        private void ConnectAndProcess()
        {

            DateTime transmitLatch = DateTime.Now.AddMilliseconds(-1);
            Random r = new Random();
           

            using (TcpClient cli = new TcpClient())
            {


                sessionID = r.Next(1, int.MaxValue) + 1;

                StateTCP = TCPState.CONNECTING;
                StateRCon = RConState.IDLE;

                AbortTCP = false;
                try
                {

                    cli.ConnectAsync(RConHost, RConPort).Wait(5000);

                    if (cli.Connected == false)
                    {
   
                        AbortTCP = true;
                        StateTCP = TCPState.ABORTED;
                        StateRCon = RConState.NETWORK_FAIL;
                        return;
                    }

                    StateTCP = TCPState.CONNECTED;
                    StateRCon = RConState.AUTHENTICATE;

                    RconPacket auth = RconPacket.AuthPacket(RConPass, sessionID);
                    auth.SendToNetworkStream(cli.GetStream());

                    if (auth.IsBadPacket == false)
                    {
                        RconPacket resp = new RconPacket();
                        resp.ReadFromNetworkSteam(cli.GetStream());

                        if (resp.IsBadPacket == false)
                        {
                            if (resp.SessionID == -1 && resp.ServerType == 2)
                                StateRCon = RConState.AUTHENTICATE_FAIL;

                        }
                        else
                            StateRCon = RConState.NETWORK_FAIL;
                    }


                    if (StateTCP == TCPState.CONNECTED)
                    {
                        if (cli.Connected == false)
                        {
                            StateTCP = TCPState.ABORTED;
                            AbortTCP = true;

                        }

                        if (StateRCon != RConState.AUTHENTICATE)
                        {
                            AbortTCP = true;
                            StateTCP = TCPState.ABORTED;
                            StateRCon = RConState.AUTHENTICATE_FAIL;
                            return;
                        }
                        else
                            StateRCon = RConState.READY;
                    }


                    Comms(cli);

                    AbortTCP = true;
                    StateTCP = TCPState.ABORTED;
                }

                catch (Exception e)
                {
                    LastTCPError = e.Message;
                    AbortTCP = true;
                    StateRCon = RConState.NETWORK_FAIL;
                }

                finally
                {
                    if(cli.Connected == true)
                       cli.Close();
                }


            }

        }

        private void Comms(TcpClient cli)
        {

            TimeCheck tc = new TimeCheck();
            Int32 dT = 200;

            cli.SendTimeout = 5000;
            cli.ReceiveTimeout = 20000;

            try
            {

                if (cli.Connected == false) //Not connected, shut it down...
                {
                    StateRCon = RConState.NETWORK_FAIL;
                    StateTCP = TCPState.ABORTED;
                    AbortTCP = true;
                }


                tc.Reset(dT);

                while (AbortTCP == false)
                {

                    do
                    {
                        if (cli.Available > 0)
                        {


                            RconPacket resp = new RconPacket();
                            resp.ReadFromNetworkSteam(cli.GetStream());

                            if (resp.IsBadPacket == true)
                            {
                                StateTCP = TCPState.ABORTED;
                                StateRCon = RConState.NETWORK_FAIL;
                                AbortTCP = true;
                                break;

                            }

                            if (Count > 1500)
                            {
                                StateRCon = RConState.IDLE;
                                StateTCP = TCPState.ABORTED;
                                AbortTCP = true;
                                break;
                            }
                            else
                            {

                                Enqueue(resp);
                                StateRCon = RConState.READY;
                            }

                            if (tc.Expired == false)
                                tc.Reset(dT);
                        }

                        Thread.Sleep(1);


                    } while (tc.Expired == false || cli.Available > 0);

                    if (AbortTCP == true)
                        break;


                    if (cmdQue.Count > 0)
                    {
                        RconPacket Cmd = cmdQue.Dequeue();

                        Cmd.SendToNetworkStream(cli.GetStream());
                        tc.Reset(dT);
                    }

                    Thread.Sleep(1);
                }
            }

            catch (Exception ee)
            {
                AbortTCP = true;
                LastTCPError = ee.Message;
                StateTCP = TCPState.ABORTED;
                StateRCon = RConState.NETWORK_FAIL;
            }

            
        }

        private void ShutDownComms()
        {

            AbortTCP = true;
            if (bgCommThread.IsAlive)
                bgCommThread.Join();
            else
            {
                StateTCP = TCPState.ABORTED;
                StateRCon = RConState.IDLE;
            }

        }
        /// <summary>
        /// Execute a command and wait for a response, blocking main calling thread.  Once response given return.
        /// </summary>
        /// <param name="formatedCmd">Allows for C# style formated string, final result in a minecraft style command.</param>
        /// <param name="args">Same arguments supplied to the string.format function.</param>
        /// <returns>If command is sent and a response given, the repsonse is removed from the response que an returned.</returns>
        public string ExecuteCmd(string formatedCmd, params object[] args)
        {
            return ExecuteCmd(string.Format(formatedCmd, args));
        }
        /// <summary>
        /// Execute a command and wait for a response, blocking main calling thread.  Once response given return.
        /// </summary>
        /// <param name="Cmd">Command to be sent to the rcon server for the minecraft server to execute.</param>
        /// <returns>If command is sent and a response given, the repsonse is removed from the response que an returned.</returns>
        public string ExecuteCmd(string Cmd)
        {

            if (AbortTCP == true)
                return "RCON_ABORTED";

            RconPacket p;
            StringBuilder sb = new StringBuilder();

            TimeCheck tc = new TimeCheck();

            QueCommand(Cmd);

            while (Count == 0)
            {
                Thread.Sleep(100);
                if (AbortTCP == true) break;
                if (tc.Expired == true) break;
            }

            while (Count > 0)
            {
                p = Dequeue();
                sb.Append(p.Response);

                if (AbortTCP == true) break;
            }

            return sb.ToString();

        }


    }

}
