using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LibMCRcon.Remote
{
    public class MinecraftFileSync
    {

        #region Eventing

        public class ProcessedVoxel : WorldData.Region
        {
            public DateTime Started { get; set; }
            public DateTime Ended { get; set; }
            public string Msg { get; set; }

        }
        
        public event EventHandler<MinecraftFileSyncEventArgs> UpdateEvent;
        
        protected virtual void OnFireEvent(MinecraftFileSyncEventArgs e, EventHandler<MinecraftFileSyncEventArgs> evt)
        {
            EventHandler<MinecraftFileSyncEventArgs> handler = evt;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnUpdateEvent(MinecraftFileSyncEventArgs e)
        {
            EventHandler<MinecraftFileSyncEventArgs> handler = UpdateEvent;
            if (handler != null)
                handler(this, e);
        }

        

        #endregion

        public MinecraftFileSync() : base() { }

        public MinecraftFileSync(string RegionPath, string ImgsPath, MinecraftTransfer mca, MinecraftTransfer imgs, MinecraftTransfer hdt)
            : this()
        {
            remoteMCA = mca;
            remoteIMGS = imgs;
            remoteHDT = hdt;

            this.RegionPath = RegionPath;
            this.ImgsPath = ImgsPath;

        }

        public bool Busy { get; private set; }


        public void Stop() { Busy = false; }

        public MinecraftTransfer remoteMCA { get; set; }
        public MinecraftTransfer remoteIMGS { get; set; }
        public MinecraftTransfer remoteHDT { get; set; }

        public MinecraftTransfer localMCA { get; set; }
        public MinecraftTransfer locaIMGS { get; set; }
        public MinecraftTransfer localHDT { get; set; }
        public MinecraftTransfer localWORLD { get; set; }


        public string RegionPath { get; set; }
        public string ImgsPath { get; set; }

        public int DLCount { get; private set; }
        public int RDCount { get; private set; }
        public int UPCount { get; private set; }

        public int Downloads { get; private set; }
        public int Uploads { get; private set; }
        public int Renders { get; private set; }


        private TransferQueue<MinecraftFile> upload = new TransferQueue<MinecraftFile>();
        private TransferQueue<MinecraftFile> render = new TransferQueue<MinecraftFile>();


        private Task T0, T1, T2, T3;
        private Queue<Action> ProcessQueue = new Queue<Action>();

       public void Process(TransferQueue<MinecraftFile> download, Boolean FullRender, Action Completed = null)
        {

            Action procTransfer = () =>
                {

                    Busy = true;

                    Downloads = 0;
                    Uploads = 0;
                    Renders = 0;

                    Process javatopoproc = MinecraftFile.JavaTopoProc(RegionPath);

                    Action UpdateCounts = () =>
                    {
                        UPCount = upload.Count;
                        DLCount = download.Count;
                        RDCount = render.Count;
                    };


                    Action actDownload = () =>
                    {
                        MinecraftFile xD;
                        download.IsIdle = false;

                        while (Busy == true)
                        {

                            if (download.Count > 0)
                            {
                                xD = null;
                                xD = download.Dequeue();

                                UPCount = upload.Count;


                                if (xD != null)
                                {
                                    xD.ShouldRender = xD.ShouldDownload;

                                    if (xD.ShouldDownload == true)
                                    {
                                        xD.DownloadMCA(remoteMCA, RegionPath)();
                                        Downloads += 1;
                                    }

                                    render.Enqueue(xD);

                                    download.MarkIdle();
                                }

                            }



                        }

                    };

                    Action actProcess = () =>
                    {
                        MinecraftFile xR = null;
                        render.IsIdle = false;
                        while (Busy == true)
                        {

                            if (render.Count > 0)
                            {
                                xR = null;
                                xR = render.Dequeue();

                                UpdateCounts();
                                if (xR != null)
                                {
                                    xR.ShouldUpload = xR.ShouldUpload == true ? true : xR.ShouldRender;

                                    if (xR.ShouldRender == true)
                                    {

                                        xR.Process(FullRender, RegionPath, ImgsPath, javatopoproc);
                                        Renders += 1;
                                        upload.Enqueue(xR);

                                    }
                                    else if (xR.ShouldUpload == true)
                                        upload.Enqueue(xR);

                                    render.MarkIdle();
                                }

                            }

                        }

                    };

                    Action actUpload = () =>
                    {
                        MinecraftFile xU;
                        upload.IsIdle = false;

                        while (Busy == true)
                        {


                            if (upload.Count > 0)
                            {
                                xU = null;
                                xU = upload.Dequeue();

                                UpdateCounts();
                                if (xU != null)
                                {
                                    if (xU.ShouldUpload)
                                    {
                                        xU.UploadImgs(remoteIMGS, ImgsPath)();
                                        xU.UploadHDT(remoteHDT, RegionPath)();
                                        Uploads += 1;
                                    }

                                    upload.MarkIdle();
                                }
                            }
                        }
                    };

                    //shuts down the tasks if no longer busy.
                    Action actBusy = () =>
                    {

                        while(Busy == true)
                        {
                            if (download.CheckIdle())
                                if (render.CheckIdle())
                                    if (upload.CheckIdle())
                                    {
                                        Busy = false;
                                        UpdateCounts();

                                        try
                                        {
                                            javatopoproc.Close();
                                            
                                            remoteMCA.Close();
                                            remoteIMGS.Close();
                                            remoteHDT.Close();

                                            OnUpdateEvent(new MinecraftFileSyncEventArgs(Completed));

                                            break;
                                        }
                                        catch (Exception) { }


                                    }
                        }

                    };



                    try
                    {
                        remoteMCA.Open();
                        remoteIMGS.Open();
                        remoteHDT.Open();


                        if (remoteMCA.IsOpen)
                        {
                            T1 = Task.Run(actDownload);
                            T2 = Task.Run(actProcess);
                            T3 = Task.Run(actUpload);
                            T0 = Task.Run(actBusy);
                        }

                    }
                    catch (Exception) { }


                };


            if (Busy == true)
            {
                ProcessQueue.Enqueue(procTransfer);
                return;
            }

            if (ProcessQueue.Count > 0)
            {
                ProcessQueue.Enqueue(procTransfer);
                procTransfer = ProcessQueue.Dequeue();

            }
            else
            {
                if (download.Count == 0)
                {
                    OnUpdateEvent(new MinecraftFileSyncEventArgs(Completed));
                    return;
                }

            }

            procTransfer();

       }


    }
  

    


    
}