using System.Collections.Generic;
using System.Drawing;
using System.IO;
using LibMCRcon.WorldData;

namespace LibMCRcon.Remote
{
    public class MinecraftFileCentered : MinecraftFile
    {
        public MinecraftFileCenteredOrientation Orientation { get; set; }
        public List<MinecraftFile> ContributionList { get; internal set; }

        public MinecraftFile Q1
        {
            get
            {
                return ContributionList.Count > 0 ? ContributionList[0] : null;
            }
        }
        public MinecraftFile Q2
        {
            get
            {
                return ContributionList.Count > 1 ? ContributionList[1] : null;
            }
        }
        public MinecraftFile Q3
        {
            get
            {
                if (Orientation == MinecraftFileCenteredOrientation.FULL)
                    return ContributionList.Count > 2 ? ContributionList[2] : null;
                else
                    return null;
            }
        }
        public MinecraftFile Q4
        {
            get
            {
                if (Orientation == MinecraftFileCenteredOrientation.FULL)
                    return ContributionList.Count > 3 ? ContributionList[3] : null;
                else
                    return null;
            }
        }

        public MinecraftFile R { get; private set; }

        public MinecraftFileCentered():base()
        {
            ContributionList = new List<MinecraftFile>();
        }

        

        public void Recenter(int NewXo, int NewZo, int RegionsPerWorld )
        {
            int X, Z;

            X = (this.WorldX + ((NewXo - 256) * this.RegionsPerWorld));
            Z = (this.WorldZ + ((NewZo - 256) * this.RegionsPerWorld));
                        
            this.RegionsPerWorld = RegionsPerWorld;
            this.WorldX = X;
            this.WorldZ = Z;

            CreateContributionList();
        }
        public void CreateContributionList()
        {
            ContributionList.Clear();

            R = new MinecraftFile(this, RegionsPerWorld,MCKind);
            R.WorldX -= (256 * RegionsPerWorld);
            R.WorldZ -= (256 * RegionsPerWorld);

            if (R.Xo == 0 && R.Zo == 0)
            {
                Orientation = MinecraftFileCenteredOrientation.SINGLE;
                ContributionList.Add(new MinecraftFile(R.Xs, R.Zs, RegionsPerWorld, MCKind));

            }
            else if (R.Xo > 0 && R.Zo == 0)
            {

                Orientation = MinecraftFileCenteredOrientation.HORIZONTAL;
                ContributionList.Add(new MinecraftFile(R.Xs, R.Zs, RegionsPerWorld, MCKind));
                ContributionList.Add(new MinecraftFile(R.Xs + 1, R.Zs, RegionsPerWorld, MCKind));

            }
            else if (R.Xo == 0 && R.Zo > 0)
            {

                Orientation = MinecraftFileCenteredOrientation.VERTICAL;
                ContributionList.Add(new MinecraftFile(R.Xs, R.Zs, RegionsPerWorld, MCKind));
                ContributionList.Add(new MinecraftFile(R.Xs, R.Zs + 1, RegionsPerWorld, MCKind));

            }
            else
            {

                Orientation = MinecraftFileCenteredOrientation.FULL;
                ContributionList.Add(new MinecraftFile(R.Xs, R.Zs, RegionsPerWorld, MCKind));
                ContributionList.Add(new MinecraftFile(R.Xs + 1, R.Zs, RegionsPerWorld, MCKind));
                ContributionList.Add(new MinecraftFile(R.Xs, R.Zs + 1, RegionsPerWorld, MCKind));
                ContributionList.Add(new MinecraftFile(R.Xs + 1, R.Zs + 1, RegionsPerWorld, MCKind));

            }


        }

        public static Bitmap RetrieveBitmap(MinecraftTransfer DL, string FileName)
        {

            using(MemoryStream ms = new MemoryStream())
            {
                if (DL.TransferNext(FileName, ms, TxRx.RECEIVE))
                     return new Bitmap(ms);
            }

            return null;

        }
        public static Bitmap RetrieveBitmap(string FullFilePath)
        {
            var F = new FileInfo(FullFilePath);
            if (F.Exists)
            {
                using (FileStream fs = F.OpenRead())
                    return new Bitmap(fs);

            }

            return null;

        }
        public static Image RetrieveImage(string FullFilePath)
        {
            var F = new FileInfo(FullFilePath);
            if (F.Exists)
            {
                using (FileStream fs = F.OpenRead())
                    return Image.FromStream(fs);

            }

            return null;

        }

        public static void SaveBitmap(MinecraftTransfer DL, string FileName, Bitmap Bitmap)
        {
            using(MemoryStream ms = new MemoryStream())
            {

                Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                DL.TransferNext(FileName, ms, TxRx.SEND);
                ms.Close();
            }

        }
        public static void SaveBitmap(string FullFilePath, Bitmap Bitmap)
        {
            var F = new FileInfo(FullFilePath);
            using (var fs = F.Open(FileMode.Create))
                Bitmap.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
        }

        public string mcCenteredFileName(string unqMasterName)
        {
            return string.Format("mc{5}.{0}.{1}.{2}.{3}.{4}.png", Orientation, MCKind, RegionsPerWorld, R.Xs, R.Zs, unqMasterName);
        }

        
        public void Render(MinecraftTransfer DL, string unqMasterName)
        {

            string unqBitMap = string.Format("{0}.png", unqMasterName);
            string SaveCenteredBitMap = mcCenteredFileName(unqMasterName);

            Rectangle fR = new Rectangle();

            Bitmap b1;
            Graphics g;
            Image bQ = null;

            Bitmap b2 = new Bitmap(512, 512);


            //skip all this if we can still use the master, just take a different crop from it.
            //otherwise rebuild master and use it.
            //b1 would be the master bitmap
            {
                if (Orientation == MinecraftFileCenteredOrientation.FULL)
                    b1 = new Bitmap(1024, 1024);
                else if (Orientation == MinecraftFileCenteredOrientation.HORIZONTAL)
                    b1 = new Bitmap(1024, 512);
                else if (Orientation == MinecraftFileCenteredOrientation.VERTICAL)
                    b1 = new Bitmap(512, 1024);
                else
                    b1 = new Bitmap(512, 512);

                g = Graphics.FromImage(b1);


                bQ = RetrieveBitmap(DL, Q1.FileName(MCKind));
                if (bQ != null)
                {
                    g.DrawImage(bQ, 0, 0);
                    bQ.Dispose();
                }

                if (Orientation == MinecraftFileCenteredOrientation.FULL || Orientation == MinecraftFileCenteredOrientation.HORIZONTAL)
                {

                    
                    bQ = RetrieveBitmap(DL, Q2.FileName(MCKind));
                    if (bQ != null)
                    {
                        g.DrawImage(bQ, 512, 0);
                        bQ.Dispose();
                    }

                }

                if (Orientation == MinecraftFileCenteredOrientation.FULL || Orientation == MinecraftFileCenteredOrientation.VERTICAL)
                {
                    if (Orientation == MinecraftFileCenteredOrientation.FULL)

                        bQ = RetrieveBitmap(DL, Q3.FileName(MCKind));
                    else
                        bQ = RetrieveBitmap(DL, Q2.FileName(MCKind));

                    if (bQ != null)
                    {
                        g.DrawImage(bQ, 0, 512);
                        bQ.Dispose();
                    }
                }

                if (Orientation == MinecraftFileCenteredOrientation.FULL)
                {
                    
                    bQ = RetrieveBitmap(DL, Q4.FileName(MCKind));
                    if (bQ != null)
                    {
                        g.DrawImage(bQ, 512, 512);
                        bQ.Dispose();
                    }

                }

                if (Orientation != MinecraftFileCenteredOrientation.SINGLE)
                    SaveBitmap(DL, SaveCenteredBitMap, b1);
                    //b1.Save(SaveCenteredBitMap, System.Drawing.Imaging.ImageFormat.Png);

                g.Dispose();
            }

            g = Graphics.FromImage(b2);

            fR.X = 0;
            fR.Y = 0;
            fR.Width = 512;
            fR.Height = 512;

            g.DrawImage(b1, fR, R.Xo, R.Zo, 512, 512, GraphicsUnit.Pixel);

            b1.Dispose();
            g.Dispose();

  //          b2.Save(Path.Combine(ContentPath, unqBitMap));
            SaveBitmap(DL, unqBitMap, b2);
            b2.Dispose();

        }
        public void Render(MinecraftTransfer DLWorld, MinecraftTransfer DLRegions,string tile_name, string topo_name, string content_path)
        {
            if (RegionsPerWorld == 1)
            {

                MCKind = MineCraftRegionFileKind.TILEX;
                Render(DLRegions, tile_name);
                
                MCKind = MineCraftRegionFileKind.TOPOX;
                Render(DLRegions, topo_name);

            }
            else
            {

                MCKind = MineCraftRegionFileKind.TILE;
                Render(DLWorld, topo_name);

                MCKind = MineCraftRegionFileKind.TOPO;
                Render(DLWorld, topo_name);

            }
        }
        
        public void Render(string localpath, string unqMasterName)
        {

            FileInfo fQ = null;
            DirectoryInfo imgDir = new DirectoryInfo(localpath);
            
            string SaveBitMap = string.Format(Path.Combine(imgDir.FullName, string.Format("{0}.png", unqMasterName)));
            string SaveCenteredBitMap = string.Format(Path.Combine(imgDir.FullName, string.Format("mc{0}.{1}.{2}.{3}.{4}.png", Orientation, MCKind,RegionsPerWorld, R.Xs, R.Zs)));


            Rectangle fR = new Rectangle();

            Bitmap b1;
            Graphics g;
            Image bQ = null;

            Bitmap b2 = new Bitmap(512, 512);


            //skip all this if we can still use the master, just take a different crop from it.
            //otherwise rebuild master and use it.
            //b1 would be the master bitmap

            if (File.Exists(SaveCenteredBitMap) == true)
            {
                b1 = new Bitmap(SaveCenteredBitMap);
            }
            else
            {
                if (Orientation == MinecraftFileCenteredOrientation.FULL)
                    b1 = new Bitmap(1024, 1024);
                else if (Orientation == MinecraftFileCenteredOrientation.HORIZONTAL)
                    b1 = new Bitmap(1024, 512);
                else if (Orientation == MinecraftFileCenteredOrientation.VERTICAL)
                    b1 = new Bitmap(512, 1024);
                else
                    b1 = new Bitmap(512, 512);

                g = Graphics.FromImage(b1);


                fQ = Q1.FileInfo(localpath, MCKind);
                if (fQ.Exists)
                {
                    bQ = Image.FromFile(fQ.FullName);
                    g.DrawImage(bQ, 0, 0);
                    bQ.Dispose();
                }

                if (Orientation == MinecraftFileCenteredOrientation.FULL || Orientation == MinecraftFileCenteredOrientation.HORIZONTAL)
                {

                    fQ = Q2.FileInfo(localpath, MCKind);
                    if (fQ.Exists)
                    {
                        bQ = Image.FromFile(fQ.FullName);
                        g.DrawImage(bQ, 512, 0);
                        bQ.Dispose();
                    }

                }

                if (Orientation == MinecraftFileCenteredOrientation.FULL || Orientation == MinecraftFileCenteredOrientation.VERTICAL)
                {
                    if (Orientation == MinecraftFileCenteredOrientation.FULL)
                        fQ = Q3.FileInfo(localpath, MCKind);
                    else
                        fQ = Q2.FileInfo(localpath, MCKind);

                    if (fQ.Exists)
                    {
                        bQ = Image.FromFile(fQ.FullName);
                        g.DrawImage(bQ, 0, 512);
                        bQ.Dispose();
                    }
                }

                if (Orientation == MinecraftFileCenteredOrientation.FULL)
                {
                    fQ = Q4.FileInfo(localpath, MCKind);
                    if (fQ.Exists)
                    {
                        bQ = Image.FromFile(fQ.FullName);
                        g.DrawImage(bQ, 512, 512);
                        bQ.Dispose();
                    }

                }

                if (Orientation != MinecraftFileCenteredOrientation.SINGLE)
                    b1.Save(SaveCenteredBitMap, System.Drawing.Imaging.ImageFormat.Png);

                g.Dispose();
            }
            
            g = Graphics.FromImage(b2);

            fR.X = 0;
            fR.Y = 0;
            fR.Width = 512;
            fR.Height = 512;

            g.DrawImage(b1, fR, R.Xo, R.Zo, 512, 512, GraphicsUnit.Pixel);

            b1.Dispose();
            g.Dispose();


            b2.Save(SaveBitMap, System.Drawing.Imaging.ImageFormat.Png);
            b2.Dispose();
        }
        public void Render(string tile_name, string topo_name, string imgs_path)
        {

            if (RegionsPerWorld == 1)
            {

                MCKind = MineCraftRegionFileKind.TILEX;
                Render(imgs_path, tile_name);

                MCKind = MineCraftRegionFileKind.TOPOX;
                Render(imgs_path, topo_name);

            }
            else
            {

                MCKind = MineCraftRegionFileKind.TILE;
                Render(imgs_path, tile_name);

                MCKind = MineCraftRegionFileKind.TOPO;
                Render(imgs_path, topo_name);

            }
        }

        public static void RegionList(List<MinecraftFile> ContributionList, List<MinecraftFile> MFList)
        {
            foreach (var x in ContributionList)
                RegionList(x.WorldX, x.WorldZ, x.RegionsPerWorld, MFList);
        }

        public static void RegionList(int X, int Z, int RegionsPerWorld, List<MinecraftFile> MFList)
        {


            Voxel W = MinecraftOrdinates.Region(255,X,Z);

            var xs1 = W.Xs;
            var zs1 = W.Zs;
            W.Xo = 0;
            W.Zo = 0;


            for (int zs = 0; zs < RegionsPerWorld; zs++)
                for (int xs = 0; xs < RegionsPerWorld; xs++)
                {
                    W.Xs = xs1 + xs;
                    W.Zs = zs1 + zs;
                    
                    MFList.Add(new MinecraftFile(W, 1));
                }

        }
    }
  

    


    
}