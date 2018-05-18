using System.Collections.Generic;
using System.Drawing;
using System.IO;
using LibMCRcon.WorldData;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LibMCRcon.Remote
{


   
    
    public class MinecraftWorldFileJson
    {
        public string MapBase()
        {
                return S == null ? ".0.0.0" : R > 1 ? $".{S.X}.{S.Z}.{R}" : $".{S.X}.{S.Z}";
            
        }

        public VoxelEx S { get; set; }
        public int R { get; set; }

        public List<MinecraftFileJson> SourceList { get; set; }
        public bool Completed { get; set; }
        public bool Rendered { get; set; }

        public int WorldScaleIndex()
        {

            return (int)(Math.Log(R) / Math.Log(2));
        }

        public MinecraftWorldFileJson ScaleOut()
        {
            var upR = (int)(Math.Pow(2, (Math.Log(R) / Math.Log(2)) + 1));
            return ScaleOut(upR);
        }
        public int PixelSizeScaleOut(int WorldR)
        {
            var currentIdx = (int)(Math.Log(R) / Math.Log(2));
            var scaleIdx = (int)(Math.Log(WorldR) / Math.Log(2));
            var upR = (int)(Math.Pow(2, scaleIdx - currentIdx));
            return 512 / upR;
        }
        public MinecraftWorldFileJson ScaleOut(int WorldR)
        {
           
            var mfx = new MinecraftFileEx(S.X, S.Z, R, WorldR);
            var mwf = new MinecraftWorldFileJson() { S = mfx.S.Copy(), R = mfx.Regions, SourceList = new List<MinecraftFileJson>() };
            mwf.SourceList.Add(mfx.MapOp);
            return mwf;
        }

        public static Process JavaTopoProc(string RegionPath, string JavaBinary = "java.exe")
        {
            //TMCMR
            string JarName = Path.Combine(RegionPath, "tmcmr.jar");

            Process proc = new Process()
            { EnableRaisingEvents = false };

            proc.StartInfo.FileName = JavaBinary;
            proc.StartInfo.WorkingDirectory = RegionPath;

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo.CreateNoWindow = true;

            return proc;
        }


        public MinecraftWorldFileJson StitchWorld(string ImgPath, int WorldR)
        {

            var w = ScaleOut(WorldR);
            var size = PixelSizeScaleOut(WorldR);

            void MakeImage(string ImgType)
            {

                var FileName = Path.Combine(ImgPath, $"{ImgType}{w.MapBase()}.png");
                var b = GetImg(FileName, new Size(512, 512));
                var g = Graphics.FromImage(b);

                foreach (var mf in w.SourceList)
                {
                    var p = GetImg(Path.Combine(ImgPath, $"{ImgType}{mf.TileBase()}.png"), new Size(size, size));
                    g.DrawImage(p, mf.MapXo, mf.MapZo, size, size);
                    p.Dispose();
                }

                b.Save(FileName);
                b.Dispose();
                g.Dispose();
            }



            MakeImage("topo");
            MakeImage("tile");

            return w;
        }

        public void Stitch(string ImgPath)
        {
            void MakeImage(string ImgType)
            {

                var FileName = Path.Combine(ImgPath, $"{ImgType}{MapBase()}.png");
                var b = GetImg(FileName, new Size(512, 512));
                var g = Graphics.FromImage(b);

                foreach (var mf in SourceList)
                {
                    var p = GetImg(Path.Combine(ImgPath, $"{ImgType}{mf.TileBase()}.png"), new Size(256, 256));
                    g.DrawImage(p, mf.MapXo, mf.MapZo, 256, 256);
                    p.Dispose();
                }

                b.Save(FileName);
                b.Dispose();
                g.Dispose();
            }


            MakeImage("tile");
            MakeImage("topo");
        }
        public void Stitch(MCTransfer Region, MCTransfer World)
        {
            void MakeImage(string ImgType)
            {

                var FileName = $"{ImgType}{MapBase()}.png";
                var b = GetImg(World, FileName, new Size(512, 512));
                var g = Graphics.FromImage(b);

                foreach (var mf in SourceList)
                {
                    var p = GetImg((R == 2) ? Region : World, $"{ImgType}{mf.TileBase()}.png", new Size(256, 256));

                    g.DrawImage(p, mf.MapXo, mf.MapZo, 256, 256);
                    p.Dispose();
                }

                using (var mem = new MemoryStream())
                {
                    b.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                    mem.Position = 0;

                    World.TransferNext(FileName, mem, TxRx.SEND);
                }

                b.Dispose();
                g.Dispose();
            }


            MakeImage("tile");
            MakeImage("topo");

        }
        public void StitchIntoWorld(string ImgPath, MCTransfer World)
        {
            var size = PixelSizeScaleOut(R);

            void MakeImage(string ImgType)
            {
                

                var FileName = $"{ImgType}{MapBase()}.png";
                var b = GetImg(World, FileName, new Size(512, 512));
                var g = Graphics.FromImage(b);

                foreach (var mf in SourceList)
                {
                    var p = GetImg(Path.Combine(ImgPath, $"{ImgType}{mf.TileBase()}.png"), new Size(size, size));
                    g.DrawImage(p, mf.MapXo, mf.MapZo, 256, 256);
                    p.Dispose();
                }

                using (var mem = new MemoryStream())
                {
                    b.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                    b.Save(Path.Combine(ImgPath, FileName));

                    mem.Position = 0;

                    World.TransferNext(FileName, mem, TxRx.SEND);
                }

                b.Dispose();
                g.Dispose();
            }


            MakeImage("tile");
            MakeImage("topo");

        }
        public MinecraftWorldFileJson StitchWorld(MCTransfer Region, MCTransfer World, int WorldR)
        {

            var w = ScaleOut(WorldR);
            var size = PixelSizeScaleOut(WorldR);

            void MakeImage(string ImgType)
            {

                var FileName = $"{ImgType}{w.MapBase()}.png";
                var b = GetImg(World, FileName, new Size(512, 512));
                var g = Graphics.FromImage(b);

                foreach (var mf in w.SourceList)
                {
                    var p = GetImg((w.R == 2) ? Region : World, $"{ImgType}{mf.TileBase()}.png", new Size(size, size));
                    g.DrawImage(p, mf.MapXo, mf.MapZo, size, size);
                    p.Dispose();
                }


                using (var mem = new MemoryStream())
                {
                    b.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                    mem.Position = 0;

                     World.TransferNext(FileName, mem, TxRx.SEND);
                }

                b.Dispose();
                g.Dispose();

            }



             MakeImage("topo");
             MakeImage("tile");

            return w;
        }

     

        public void RenderTopo(string MCAPath, string ImgPath)
        {
            if (SourceList[0].R == 1)
            {
                //Render Level
                Rendered = true;
                
                foreach (var mca in SourceList.FindAll(x => x.Rendered == false))
                {
                    try
                    {
                        mca.RenderTopo(MCAPath, ImgPath);
                    }
                    catch (Exception ex)
                    {
                        Rendered = false;
                        throw ex;
                    }
                }

            }
        }
        public void RenderTile(string MCAPath, string ImgPath, string JavaPath)
        {
            if (SourceList[0].R == 1)
            {
                //Render Level
                Rendered = true;
                var javaProc = JavaTopoProc(MCAPath, JavaPath);
                foreach (var mca in SourceList.FindAll(x => x.Rendered == false))
                {
                    try
                    {
                        mca.RenderTile(MCAPath, ImgPath, javaProc);
                    }
                    catch (Exception ex)
                    {
                        Rendered = false;
                        throw ex;
                    }
                }

            }
        }

        public void Render(string MCAPath, string ImgPath, string JavaPath)
        {

            if (SourceList[0].R == 1)
            {
                //Render Level
                Rendered = true;
                var javaProc = JavaTopoProc(MCAPath, JavaPath);
                foreach (var mca in SourceList.FindAll(x => x.Rendered == false))
                {
                    try
                    {
                        mca.FullRender(MCAPath, ImgPath, javaProc);
                    }
                    catch (Exception ex)
                    {
                        Rendered = false;
                        throw ex;
                    }
                }

            }

        }

        private Bitmap GetImg(string filename,Size Scale)
        {
            Bitmap fb;
            Bitmap b;

            if (File.Exists(filename))
            {

                fb = new Bitmap(filename);
                b = new Bitmap(fb, Scale);
                fb.Dispose();
            }
            else
                b = new Bitmap(Scale.Width,Scale.Height);

            return b;

        }
        private Bitmap GetImg(MCTransfer transfer, string filename, Size Scale)
        {
            Bitmap fb;
            Bitmap b;


            using (MemoryStream ms = new MemoryStream())
            {
                if (true == transfer.TransferNext(filename, ms, TxRx.RECEIVE))
                {
                    fb = new Bitmap(ms);
                    b = new Bitmap(fb, Scale);
                    fb.Dispose();
                }
                else
                    b = new Bitmap(Scale.Width, Scale.Height);

            }
            return b;
        }

    }

    public class MinecraftFileJson
    {
        public string TileBase()
        {
            return S == null ? ".0.0.0" : R > 1 ? $".{S.X}.{S.Z}.{R}" : $".{S.X}.{S.Z}";
        }

        public VoxelEx S { get; set; }
        public int R { get; set; }

        public int MapXo { set; get; }
        public int MapZo { set; get; }

        public bool DL { get; set; }
        public bool UP { get; set; }
        public bool Rendered { get; set; }
        public string RenderError { get; set; }

        public void TopoRender(string RegionDirectory, string ImgsDirectory, Process TogosJavaProc)
        {

            byte[][] MapData = new byte[][] { new byte[512 * 512], new byte[512 * 512] };

            FileInfo Hdt = new FileInfo(Path.Combine(RegionDirectory, $"r{TileBase()}.hdt"));
            FileStream tempFS = Hdt.Open(FileMode.Open, FileAccess.Read);

            tempFS.Read(MapData[0], 0, 512 * 512);
            tempFS.Read(MapData[1], 0, 512 * 512);
            tempFS.Close();


            Rendering.MCRegionMaps.RenderTopoPngFromRegion(MapData, ImgsDirectory, S.X, S.Z);

            FileInfo lwFS = new FileInfo(Path.Combine(ImgsDirectory, $"topo{TileBase()}.png"));
            if (lwFS.Exists)
                lwFS.LastWriteTime = Hdt.LastWriteTime;

        }

        public void RenderTopo(string RegionDirectory, string ImgsDirectory)
        {
            byte[][] MapData = new byte[][] { new byte[512 * 512], new byte[512 * 512] };
            Color[] BlockData = new Color[512 * 512];

            RegionMCA mca = new RegionMCA(RegionDirectory);

            mca.LoadRegion(S.X, S.Z);

            Rendering.MCRegionMaps.RenderDataFromRegion(mca, MapData, BlockData);
            Rendering.MCRegionMaps.RenderTopoPngFromRegion(MapData, ImgsDirectory, S.X, S.Z);
            //LibMCRcon.Rendering.MCRegionMaps.RenderBlockPngFromRegion(MapData, BlockData, ImgsDir.FullName, RV);


            FileInfo mcaH = new FileInfo(Path.Combine(RegionDirectory, $"r{TileBase()}.hdt"));

            using (FileStream tempFS = mcaH.Create())
            {

                tempFS.Write(MapData[0], 0, 512 * 512);
                tempFS.Write(MapData[1], 0, 512 * 512);
                tempFS.Flush();
                tempFS.Close();

            }

            mcaH.LastWriteTime = mca.LastModified;
           
            FileInfo lwFS = new FileInfo(Path.Combine(ImgsDirectory, $"topo{TileBase()}.png"));
            if (lwFS.Exists)
                lwFS.LastWriteTime = mca.LastModified;

          
        }
        public void RenderTile(string RegionDirectory, string ImgsDirectory, Process TogosJavaProc)
        {
            TogosJavaProc.StartInfo.Arguments = $"-jar {Path.Combine(RegionDirectory, "tmcmr.jar")} -f -o {ImgsDirectory} {Path.Combine(RegionDirectory, $"r{TileBase()}.mca")}";
            if (TogosJavaProc.Start() == true)
                TogosJavaProc.WaitForExit();

            FileInfo mca = new FileInfo(Path.Combine(RegionDirectory, $"r{TileBase()}.mca"));

            FileInfo lwFS = new FileInfo(Path.Combine(ImgsDirectory, $"tile{TileBase()}.png"));
            if (lwFS.Exists && mca.Exists)
                lwFS.LastWriteTime = mca.LastWriteTime;
        }

        public void FullRender(string RegionDirectory, string ImgsDirectory, Process TogosJavaProc)
        {
            try
            {
                RenderTopo(RegionDirectory, ImgsDirectory);
                RenderTile(RegionDirectory, ImgsDirectory, TogosJavaProc);
                Rendered = true;

            }
            catch (Exception ex)
            {
                Rendered = false;
                throw ex;
            }

        }
    }

    public class MinecraftFileEx : RegionEx
    {

        public int X
        {
            get
            { return W.X; }
            set
            {
                W.X = value;
                O.X = VoxelEx.Offset(Regions, value);
                S.X = VoxelEx.Segment(Regions, value);
            }
        }
        public int Z
        {
            get
            { return W.Z; }
            set
            {
                W.Z = value;
                O.Z = VoxelEx.Offset(Regions, value);
                S.Z = VoxelEx.Segment(Regions, value);

            }

        }
        public int Y
        {
            get { return W.Y; }
            set { W.Y = value; }
        }

        public int Xs
        {
            get { return S.X; }
            set
            {
                S.X = value;
                W.X = VoxelEx.Ordinate(Regions, value, O.X);
            }
        }
        public int Zs
        {
            get { return S.Z; }
            set
            {
                S.Z = value;
                W.Z = VoxelEx.Ordinate(Regions, value, O.Z);
            }
        }

        public int Xo
        {
            get { return O.X; }
            set
            {
                O.X = value;
                W.X = VoxelEx.Ordinate(Regions, S.X, value);
            }
        }
        public int Zo
        {
            get { return O.Z; }
            set
            {
                O.Z = value;
                W.Z = VoxelEx.Ordinate(Regions, S.Z, value);
            }
        }

        public bool Mark { get; set; }

        public MinecraftFileEx() { }
        public MinecraftFileEx(RegionEx CopySource)
        {
            W = CopySource.W.Copy();
            S = CopySource.S.Copy();
            O = CopySource.O.Copy();
            M = CopySource.M.Copy();

            Regions = CopySource.Regions;
            MapRegions = CopySource.MapRegions;

        }
        public MinecraftFileEx(int Xs, int Zs, int R) : base(Xs, Zs, R) { }
        public MinecraftFileEx(int Xs, int Zs, int R, int WorldR) : base(Xs, Zs, R) { SetWorldScale(WorldR); }

        public string MapComponentFile(string FileType = null)
        {
            if (MapRegions == 1)
                return string.Format(FileType ?? FileTemplate(MCFileKind.TILE), $"{M.X}.{M.Z}");
            else
                return string.Format(FileType ?? FileTemplate(MCFileKind.TILE), $"{M.X}.{M.Z}.{MapRegions}");
        }
        public string MapFile(string FileType = null)
        {
            if (Regions == 1)
                return string.Format(FileType ?? FileTemplate(MCFileKind.TILE), $"{S.X}.{S.Z}");
            else
                return string.Format(FileType ?? FileTemplate(MCFileKind.TILE), $"{S.X}.{S.Z}.{Regions}");
        }

        public List<MinecraftFileEx> BuildMapComponents()
        {
            var lst = new List<MinecraftFileEx>();
            var r1 = Regions / MapRegions;


            var s1 = new RegionEx(S.X, S.Z, Regions);
            s1.SetSegmentOffset(MapRegions);


            for (int z = 0; z < r1; z++)
                for (int x = 0; x < r1; x++)
                {
                    var m = new RegionEx(s1.S.X + x, s1.S.Z + z, MapRegions);
                    m.SetWorldScale(Regions);
                    lst.Add(new MinecraftFileEx(m));
                }

            return lst;
        }

        public string FileTemplate(MCFileKind MCKind = MCFileKind.TILE)
        {
            switch (MCKind)
            {
                case MCFileKind.MCA:
                    return "r.{0}.mca";
                case MCFileKind.HDT:
                    return "r.{0}.hdt";
                case MCFileKind.POI:
                    return "poi.{0}.png";
                case MCFileKind.TILE:
                    return "tile.{0}.png";
                case MCFileKind.TOPO:
                    return "topo.{0}.png";
                default:
                    return "unk.{0}.bin";
            }
        }
        public MCTransferInfo GetTransferInfo(MCFileKind MCKind = MCFileKind.NOTPARSED)
        {
            return new MCTransferInfo() { X = X, Z = Z, Regions = Regions, FileName = MapFile(FileTemplate(MCKind)), Xs = Xs, Zs = Zs };

        }

        #region "Image Manipulation" 

        private Bitmap GetImg(string filename)
        {
            Bitmap fb;
            Bitmap b;

            if (File.Exists(filename))
            {

                fb = new Bitmap(filename);
                b = new Bitmap(fb);
                fb.Dispose();
            }
            else
                b = new Bitmap(512, 512);

            return b;

        }
        private Bitmap GetImg(MinecraftTransfer transfer, string filename, int W, int H)
        {
            Bitmap fb;
            Bitmap b;


            using (MemoryStream ms = new MemoryStream())
            {
                if (transfer.TransferNext(filename, ms, TxRx.RECEIVE))
                {
                    fb = new Bitmap(ms);
                    b = new Bitmap(fb);
                    fb.Dispose();
                }
                else
                    b = new Bitmap(W, H);


            }
            return b;
        }

        #endregion

        #region "Map Generation"
        public MinecraftFileJson MapOp
        {
            get
            {
                return new MinecraftFileJson() { S = M.Copy(), R = MapRegions, MapXo = Xo, MapZo = Zo };
            }
        }

        public MinecraftWorldFileJson WorldOp
        {
            get
            {
                return new MinecraftWorldFileJson() { S = W.Copy(), R = Regions, SourceList = new List<MinecraftFileJson>() { MapOp } };
            }
        }
        #endregion


    }
  
    


    
}