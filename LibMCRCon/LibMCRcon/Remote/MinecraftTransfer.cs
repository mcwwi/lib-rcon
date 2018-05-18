using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LibMCRcon.Remote
{
    public class MCTransferInfoList
    {
        public DateTime MinListDate { get; set; }
        public DateTime MaxListDate { get; set; }
        public List<MCTransferInfo> RemoteList { get; set; }

        public MCTransferInfoList()
        {
            RemoteList = new List<MCTransferInfo>();
            MinListDate = DateTime.MaxValue;
            MaxListDate = DateTime.MinValue;
        }

        public MCTransferInfoList(List<MCTransferInfo> RemoteList)
        {
            this.RemoteList = RemoteList;
            MinListDate = DateTime.MaxValue;
            MaxListDate = DateTime.MinValue;
            RemoteList.ForEach(x => CalculateAge(x));
        }

        private void CalculateAge(MCTransferInfo mcf)
        {
            if (mcf.RemoteLastWrite < MinListDate)
                MinListDate = mcf.RemoteLastWrite;

            if (mcf.RemoteLastWrite > MaxListDate)
                MaxListDate = mcf.RemoteLastWrite;
        }
    }
    public class MCTransferInfo
    {
        public MineCraftRegionFileKind MCKind { get; set; } = MineCraftRegionFileKind.NOTPARSED;
        public bool IsValid { get; set; } = false;

        public int X { get; set; }
        public int Z { get; set; }
        public int Xs { get; set; }
        public int Zs { get; set; }
        public int Regions { get; set; }
        
        public long PoiTimestamp { get; set; } = long.MinValue;
        public DateTime RemoteLastWrite { get; set; }
        public DateTime LocalLastWrite { get; set; }

        public string FileName { get; set; }

        public MCTransferInfo() { }
        public MCTransferInfo(string FileName)
        {
            SetByFileName(FileName);
        }

        public void SetByFileName(string FileName)
        {
            this.FileName = FileName;

            string[] v = FileName.ToUpper().Split('.');

            if (v.Length > 3)
            {

                string e = v.Length > 5 ? v[6] : v.Length > 4 ? v[4] : v[3];
                if (int.TryParse(v[1], out int x) == true)
                {
                    if (int.TryParse(v[2], out int z) == true)
                    {

                        Xs = x;
                        Zs = z;

                        IsValid = true;
                        int r = 1;

                        switch (e)
                        {
                            case "MCA":
                                MCKind = MineCraftRegionFileKind.MCA;
                                break;

                            case "PNG":
                        


                                if (v.Length == 5)
                                    int.TryParse(v[3], out r);

                                if (r > 1)
                                {
                                    switch (v[0])
                                    {
                                        case "TOPO":
                                            MCKind = MineCraftRegionFileKind.TOPO;
                                            break;
                                        case "TILE":
                                            MCKind = MineCraftRegionFileKind.TILE;
                                            break;


                                        default:
                                            MCKind = MineCraftRegionFileKind.NOTPARSED;
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (v[0])
                                    {
                                        case "TOPO":
                                            MCKind = MineCraftRegionFileKind.TOPOX;
                                            break;

                                        case "TILE":
                                            MCKind = MineCraftRegionFileKind.TILEX;
                                            break;
                                   
           
                                        case "POI":
                                            MCKind = MineCraftRegionFileKind.POI;

                                            if (int.TryParse(v[3], out int wx) == true)
                                                if (int.TryParse(v[4], out int wz) == true)
                                                {

                                                    if (long.TryParse(v[5], out long poi) == true)
                                                    {
                                                        PoiTimestamp = poi;
                                                        X = wx;
                                                        Z = wz;
                                                    }
                                                }

                                            break;

                                        default:
                                            MCKind = MineCraftRegionFileKind.NOTPARSED;
                                            break;
                                    }

                                }

                                break;

                            case "HDT":
                                MCKind = MineCraftRegionFileKind.HDT;
                                break;
                            default:
                                MCKind = MineCraftRegionFileKind.NOTPARSED;
                                break;
                        }


                        if (IsValid == true)
                        {

                            Regions = r;

                        }
                        else
                        {
                            IsValid = false;
                            MCKind = MineCraftRegionFileKind.NOTPARSED;
                        }
                    }
                }
            }


        }

    }

    public class MCTransferJson<T> where T : class, new()
    {
        public T Data { get; set; }
        public bool LastError { get; set; }

        public MCTransferJson()
        {
            Data = new T();
            LastError = true;
        }
        public MCTransferJson(T Data)
        {
            this.Data = Data;
            LastError = false;
        }
    }

    public abstract class MCTransfer
    {


        public string RemotePath { get; set; }
        public bool StopTransfer { get; set; }
        public bool LastTranserSuccess { get; set; }
        public string LastError { get; set; }
       


        public string FullRemotePath(string RootPath, string FileName)
        {
            if (RemotePath == string.Empty)
                return string.Format("{0}/{1}", RootPath, FileName);
            else
                return string.Format("{0}/{1}/{2}", RootPath, RemotePath, FileName);


        }
        public TxRx Direction { get; set; }


        public abstract bool ValidateTransfer(FileInfo item, TxRx Direction);
        public abstract int TransferItemAge(FileInfo item, TxRx Direction);
        public abstract bool TransferNext(FileInfo item, TxRx Direction);
        public abstract bool TransferNext(string FileName, Stream item, TxRx Direction);
             
        public abstract bool Exists(string FileName);
        public abstract int Age(string FileName);

        public abstract List<MCTransferInfo> GetRemoteData();
        public abstract List<MCTransferInfo> GetRemoteData(string RemotePath);
        public abstract List<MCTransferInfo> GetRemoteData(string RemotePath, string Filter);

        public bool LockOut { get; set; }
        public abstract void Open();
        public abstract void Close();

        public bool IsOpen { get; set; }

        public void TransferRun(TransferQueue<FileInfo> Items, TxRx Direction, bool Continous = false)
        {
            TransferRun(Items, null, Direction, Continous);
        }
        public void TransferRun(TransferQueue<FileInfo> Items, TransferQueue<FileInfo> Finished, TxRx Direction, bool Continous = false)
        {
            if (IsOpen == false)
                Open();


            Action<FileInfo> Finish;

            if (Finished != null)
                Finish = (x) => { Finished.Enqueue(x); };
            else
                Finish = (x) => { };

            if (IsOpen)
            {

                FileInfo item;

                while (Items.Count > 0 || Continous)
                {

                    item = Items.Dequeue();

                    if (item != null)
                        if (ValidateTransfer(item, Direction))
                            TransferNext(item, Direction);

                    Finish(item);


                    if (StopTransfer) break;
                }
            }

        }

        public bool Upload(FileInfo Item)
        {
            return TransferNext(Item, TxRx.SEND);
        }

        public bool Upload(Stream Item, string FileName)
        {
            return TransferNext(FileName, Item, TxRx.SEND);
        }
        public bool Download(FileInfo Item)
        {
            return TransferNext(Item, TxRx.RECEIVE);
        }
        public bool Download(Stream Item, string FileName)
        {
            return TransferNext(FileName, Item, TxRx.RECEIVE);
        }


        public bool UploadJsonObject<T>(T Item, string FileName) where T : new()
        {

            bool ok = false;
            var data = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Item));
            using (var mem = new MemoryStream(data))
            {
                ok = TransferNext(FileName, mem, TxRx.SEND);
            }
            return ok;


        }

        public bool UploadJsonObject<T>(T[] Item, string FileName) where T : new()
        {

            bool ok = false;
            var data = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Item));
            using (var mem = new MemoryStream(data))
            {
                ok = TransferNext(FileName, mem, TxRx.SEND);
            }
            return ok;


        }
        public MCTransferJson<T> DownloadJsonObject<T>(string FileName) where T : class, new()
        {

            using (var mem = new MemoryStream())
            {
                if (TransferNext(FileName, mem, TxRx.RECEIVE) == true)
                {
                    try
                    {
                        var js = System.Text.Encoding.UTF8.GetString(mem.ToArray());
                        return new MCTransferJson<T>(JsonConvert.DeserializeObject<T>(js));
                    }
                    catch
                    {
                        LastTranserSuccess = false;
                        throw;
                    }
                    
                }
                else
                    return new MCTransferJson<T>();

            }

        }
       
    }

    public abstract class MinecraftTransfer
    {

        public string RemotePath { get; set; }
        public bool StopTransfer { get; set; }
        public bool LastTranserSuccess { get; set; }

        public string FullRemotePath(string RootPath, string FileName)
        {
            if (RemotePath == string.Empty)
                return string.Format("{0}/{1}", RootPath, FileName);
            else
                return string.Format("{0}/{1}/{2}", RootPath, RemotePath, FileName);

            
        }
        public TxRx Direction { get; set; }
        
        
        public abstract bool ValidateTransfer(FileInfo item, TxRx Direction);
        public abstract int TransferItemAge(FileInfo item, TxRx Direction);
        public abstract bool TransferNext(FileInfo item, TxRx Direction);
        public abstract bool TransferNext(string FileName, Stream item, TxRx Direction);

        public abstract bool Exists(string FileName);
        public abstract int Age(string FileName);

        public abstract List<MinecraftFile> GetRemoteData();
        public abstract List<MinecraftFile> GetRemoteData(string RemotePath);
        public abstract List<MinecraftFile> GetRemoteData(string RemotePath, string Filter);

        public bool LockOut { get; set; }
        public abstract void Open();
        public abstract void Close();

        public bool IsOpen { get; set; }

        public async Task TransferRun(TransferQueue<FileInfo> Items, TxRx Direction, bool Continous = false)
        {
            await TransferRun(Items, null, Direction, Continous);
        }
        public async Task TransferRun(TransferQueue<FileInfo> Items, TransferQueue<FileInfo> Finished, TxRx Direction, bool Continous = false)
        {
            if (IsOpen == false)
                Open();

            Action actTransfer()
            {
                return () =>
                {
                    Action<FileInfo> Finish;

                    if (Finished != null)
                        Finish = (x) => { Finished.Enqueue(x); };
                    else
                        Finish = (x) => { };



                    if (IsOpen)
                    {

                        FileInfo item;

                        while (Items.Count > 0 || Continous)
                        {

                            item = Items.Dequeue();

                            if (item != null)
                                if (ValidateTransfer(item, Direction))
                                    TransferNext(item, Direction);

                            Finish(item);


                            if (StopTransfer) break;
                        }
                    }
                };
            }
            await Task.Run(actTransfer());
        }

        public async Task<bool> Upload(FileInfo Item)
        {
            return await Task.Run(() => TransferNext(Item, TxRx.SEND));
        }
        public async Task<bool> Upload(Stream Item, string FileName)
        {
            return await Task.Run(() => TransferNext(FileName,  Item, TxRx.SEND));
        }
        public async Task<bool> Download(FileInfo Item)
        {
            return await Task.Run(() => TransferNext(Item, TxRx.RECEIVE));
        }
        public async Task<bool> Download(Stream Item, string FileName)
        {
            return await Task.Run(() => TransferNext(FileName, Item, TxRx.RECEIVE));
        }


    }
  

    


    
}