using System.Collections.Generic;
using System.Drawing;
using System.IO;
using LibMCRcon.WorldData;
using LibMCRcon.Remote;

namespace LibMCRcon.Rendering
{
    public static class MCRegionMaps
    {
        public static Color[][] BlockPalette()
        {
            Color[][] Blocks = new Color[256][];

            for (int x = 0; x < 256; x++)
            {
                switch (x)
                {
                    case 1://stone
                        Blocks[x] = new Color[] { Color.Gray };
                        break;
                    case 2://grass
                        Blocks[x] = new Color[] { Color.Green };
                        break;
                    case 3://dirt
                        Blocks[x] = new Color[] { Color.Brown };
                        break;
                    case 4://cobble
                        Blocks[x] = new Color[] { Color.LightGray };
                        break;
                    case 5://wood plank
                        Blocks[x] = new Color[] { ColorStep.MixColors(80, Color.Brown, Color.Black) };
                        break;
                    case 6://sapling
                        Blocks[x] = new Color[] { Color.Green };
                        break;
                    case 7://bed rock
                        Blocks[x] = new Color[] { Color.DarkGray };
                        break;
                    case 8://flowing water
                    case 9://water
                        Blocks[x] = new Color[] { Color.Blue };
                        break;

                    case 10://flo lava
                    case 11://lava
                        Blocks[x] = new Color[] { Color.Orange };
                        break;
                    case 12://sand
                        Blocks[x] = new Color[] { ColorStep.MixColors(90, Color.Beige, Color.Black) };
                        break;
                    case 13://gravel
                        Blocks[x] = new Color[] { ColorStep.MixColors(50, Color.Gray, Color.Black) };
                        break;
                    case 14://gold ore
                        Blocks[x] = new Color[] { Color.Yellow };
                        break;
                    case 15://iron ore
                    case 16://coal ore
                        Blocks[x] = new Color[] { ColorStep.MixColors(30, Color.Gold, Color.Black) };
                        break;
                    case 17://wood
                        Blocks[x] = new Color[] { Color.Brown };
                        break;
                    case 18://leaves
                    case 161://Acacia Leaves (0),(1) dark oak leaves
                    case 162://acacia wood (0), (1) dark oak wood
                        Blocks[x] = new Color[] { Color.DarkGreen };
                        break;
                    case 19://sponge
                        Blocks[x] = new Color[] { Color.Beige };
                        break;
                    case 20://glass
                        Blocks[x] = new Color[] { Color.LightBlue };
                        break;
                    case 21://lapis ore
                    case 22://lapis block
                        Blocks[x] = new Color[] { Color.DarkBlue };
                        break;
                    case 24://sandstone
                        Blocks[x] = new Color[] { ColorStep.MixColors(90, Color.Beige, Color.Black) };
                        break;
                    case 35://wool
                    case 41://gold block
                    case 42://iron block
                    case 43://x2 stone slab
                    case 44://stone slab
                        Blocks[x] = new Color[] { Color.Gray };
                        break;
                    //  case 45://bricks
                    case 49://obsidian
                        Blocks[x] = new Color[] { Color.DarkViolet };
                        break;
                    case 51://fire
                        Blocks[x] = new Color[] { Color.Orange };
                        break;
                    //case 52://monster spawner
                    case 56://diamond ore
                    case 57://diamond block
                        Blocks[x] = new Color[] { Color.LightBlue };
                        break;
                    case 59://wheat crops
                        Blocks[x] = new Color[] { Color.Wheat };
                        break;
                    case 60://farmland
                        Blocks[x] = new Color[] { Color.BurlyWood };
                        break;
                    case 73://redstone ore
                    case 74://glowing redstone ore
                        Blocks[x] = new Color[] { Color.Red };
                        break;
                    case 78://snow
                        Blocks[x] = new Color[] { Color.White };
                        break;
                    case 79://ice
                        Blocks[x] = new Color[] { Color.SkyBlue };
                        break;
                    case 80://snow block
                        Blocks[x] = new Color[] { Color.White };
                        break;
                    case 81://cactus
                        Blocks[x] = new Color[] { Color.MediumSpringGreen };
                        break;
                    case 82://clay
                        Blocks[x] = new Color[] { Color.Gray };
                        break;
                    case 83://sugar canes
                        Blocks[x] = new Color[] { Color.LimeGreen };
                        break;
                    case 86://pumpkins
                        Blocks[x] = new Color[] { Color.DarkOrange };
                        break;
                    case 87://netherrack
                        Blocks[x] = new Color[] { Color.DarkRed };
                        break;
                    case 88://soul sand
                        Blocks[x] = new Color[] { Color.DarkGray };
                        break;
                    case 89://glow stone
                        Blocks[x] = new Color[] { Color.Goldenrod };
                        break;
                    case 90://nether portal
                        Blocks[x] = new Color[] { Color.PaleVioletRed };
                        break;
                    case 91://jack o'Lantern
                        Blocks[x] = new Color[] { Color.DarkOrange };
                        break;
                    //case 95://stained glass
                    case 98://stone bricks
                        Blocks[x] = new Color[] { Color.Gray };
                        break;
                    case 99://mushroom block (brown)
                        Blocks[x] = new Color[] { ColorStep.MixColors(75, Color.Beige, Color.Brown) };
                        break;
                    case 100://mushroom block (red)
                        Blocks[x] = new Color[] { Color.DeepPink };
                        break;
                    case 103://melon block
                        Blocks[x] = new Color[] { Color.Lime };
                        break;
                    case 110://mycelium
                        Blocks[x] = new Color[] { Color.MediumAquamarine };
                        break;
                    case 112://nether brick
                        Blocks[x] = new Color[] { Color.Maroon };
                        break;
                    case 125://x2 wood slab
                    case 126://wood slab
                        Blocks[x] = new Color[] { Color.BurlyWood };
                        break;
                    case 129://emerald ore
                    case 133://emerald block
                        Blocks[x] = new Color[] { Color.LightGreen };
                        break;
                    case 137://command block
                    case 141://carrots
                        Blocks[x] = new Color[] { ColorStep.MixColors(80, Color.Orange, Color.White) };
                        break;
                    case 142://potatoes
                        Blocks[x] = new Color[] { Color.DarkGoldenrod };
                        break;
                    case 152://redstone block
                        Blocks[x] = new Color[] { Color.Red };
                        break;
                    case 153://nether quartz block
                    case 155://quartz block
                        Blocks[x] = new Color[] { Color.MintCream };
                        break;

                    case 159://white stained clay


                        Blocks[x] = new Color[16] {Color.Beige,Color.Beige,Color.Beige,Color.Beige,Color.Beige
                                            ,Color.Beige,Color.Beige,Color.Beige,Color.Beige,Color.Beige,Color.Beige,Color.Beige
                                            ,Color.Beige,Color.Beige,Color.Beige,Color.Beige
                                        };

                        break;

                    //case 160://stained glass

                    //case 165://slime block
                    //case 166://barrier
                    case 168://prismarine
                    case 169://sea lantern
                        Blocks[x] = new Color[] { Color.SeaGreen };
                        break;

                    case 170://hay bale
                        Blocks[x] = new Color[] { Color.LightYellow };
                        break;
                    case 171://carpet (0-white, 1-15)
                        Blocks[x] = new Color[16] {Color.Beige,Color.Beige,Color.Beige,Color.Beige,Color.Beige
                                            ,Color.Beige,Color.Beige,Color.Beige,Color.Beige,Color.Beige,Color.Beige,Color.Beige
                                            ,Color.Beige,Color.Beige,Color.Beige,Color.Beige
                                        };

                        break;
                    case 172://hardened clay
                        Blocks[x] = new Color[] { Color.Firebrick };
                        break;
                    case 173://block of coal
                        Blocks[x] = new Color[] { Color.Black };
                        break;
                    case 174://packed ice
                        Blocks[x] = new Color[] { Color.LightSkyBlue };
                        break;
                    case 179://red sandstone
                    case 181://x2 red sandstone slab
                    case 182://red sandstone slab

                        Blocks[x] = new Color[] { Color.Firebrick };
                        break;

                    default:
                        Blocks[x] = new Color[] { Color.Gray };
                        break;


                }
            }

            return Blocks;
        }
        public static Color[][] Palettes()
        {

            Color[] Water;
            Color[] Topo;

            List<ColorStep> cList = new List<ColorStep>
            {
                Color.Black.ColorStep(20),
                Color.Pink.ColorStep(20),
                Color.Blue.ColorStep(20),
                Color.FromArgb(0xDF, 0xC7, 0x00).ColorStep(20),
                Color.DarkGreen.ColorStep(20),
                Color.Orange.ColorStep(20),
                Color.Brown.ColorStep(20),
                Color.Plum.ColorStep(20),
                Color.Magenta.ColorStep(20),
                Color.Coral.ColorStep(20),
                Color.Aqua.ColorStep(20),
                Color.LightCyan.ColorStep(20),
                Color.Yellow.ColorStep(15)
            };
            Topo = ColorStep.CreatePallet(cList);


            cList.Clear();
            cList.Add(Color.Blue.ColorStep(50));
            cList.Add(Color.Aqua.ColorStep(50));
            cList.Add(Color.Teal.ColorStep(50));
            cList.Add(Color.Cyan.ColorStep(50));
            cList.Add(Color.SkyBlue.ColorStep(25));
            cList.Add(Color.Turquoise.ColorStep(25));

            Water = ColorStep.CreatePallet(cList);






            return new Color[][] { Topo, Water };
        }

        //public static void WorldStiched(string ImagesPath, Voxel V, int Size = 4, string ImgType = "topo")
        //{


        //    DirectoryInfo imgDir = new DirectoryInfo(ImagesPath);
        //    string SaveBitMap = string.Format(Path.Combine(imgDir.FullName, string.Format("{2}worldcent.{0}.{1}.{3}.png", V.Xs, V.Zs, ImgType, Size)));

        //    Voxel R = MinecraftOrdinates.Region(V);

        //    FileInfo fQ;
        //    Graphics g;
        //    Image bQ = null;

        //    Bitmap b1 = new Bitmap(Size * 3, Size * 3);
        //    g = Graphics.FromImage(b1);

        //    for (int zr = 0; zr < 3; zr++)
        //        for (int xr = 0; xr < 3; xr++)
        //        {

        //            fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs + (-1 + xr), R.Zs + (-1 + zr), ImgType)));
        //            if (fQ.Exists)
        //            {
        //                bQ = Image.FromFile(fQ.FullName);
        //                g.DrawImage(bQ, xr * Size, zr * Size, Size, Size);
        //                bQ.Dispose();
        //            }

        //        }

        //    g.Dispose();

        //    b1.Save(SaveBitMap, System.Drawing.Imaging.ImageFormat.Png);
        //    b1.Dispose();

        //    return;

        //}
        //public static void RegionStiched(string ImagesPath, Voxel V, string ImgType = "topo", int Size = 172)
        //{

        //    if (Size > 512 || Size < 0)
        //        return;


        //    DirectoryInfo imgDir = new DirectoryInfo(ImagesPath);
        //    string SaveBitMap = string.Format(Path.Combine(imgDir.FullName, string.Format("{2}regioncent.{0}.{1}.{3}.png", V.Xs, V.Zs, ImgType, Size)));

        //    Voxel R = MinecraftOrdinates.Region(V);

        //    FileInfo fQ;
        //    Graphics g;
        //    Image bQ = null;

        //    Bitmap b1 = new Bitmap(Size * 3, Size * 3);
        //    g = Graphics.FromImage(b1);

        //    for (int zr = 0; zr < 3; zr++)
        //        for (int xr = 0; xr < 3; xr++)
        //        {

        //            fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs + (-1 + xr), R.Zs + (-1 + zr), ImgType)));
        //            if (fQ.Exists)
        //            {
        //                bQ = Image.FromFile(fQ.FullName);
        //                g.DrawImage(bQ, xr * Size, zr * Size, Size, Size);
        //                bQ.Dispose();
        //            }

        //        }

        //    g.Dispose();

        //    b1.Save(SaveBitMap, System.Drawing.Imaging.ImageFormat.Png);
        //    b1.Dispose();

        //    return;

        //}
        //public static async void Stitched(MinecraftTransfer Images, MinecraftTransfer Stitched, Voxel V, string ImgType = "topo")
        //{


        //    string SaveBitMap = string.Format("{2}cent.{0}.{1}.png", V.Xs, V.Zs, ImgType);

        //    Voxel R = MinecraftOrdinates.Region(V);

        //    R.X -= 256;
        //    R.Z -= 256;

        //    int sx = R.Xo;
        //    int sy = R.Zo;

        //    Rectangle fR = new Rectangle();
        //    string fQ;

        //    Bitmap b1;
        //    Graphics g;
        //    Image bQ = null;

        //    Bitmap b2 = new Bitmap(512, 512);

        //    if (sx == 0 && sy == 0)
        //    {
        //        b1 = new Bitmap(512, 512);
        //        g = Graphics.FromImage(b1);


        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs, ImgType);


        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 0, 0);
        //                bQ.Dispose();
        //            }
        //        }

        //    }
        //    else if (sx > 0 && sy == 0)
        //    {
        //        b1 = new Bitmap(1024, 512);
        //        g = Graphics.FromImage(b1);

        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs, ImgType);

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 0, 0);
        //                bQ.Dispose();
        //            }
        //        }

        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs + 1, R.Zs, ImgType);

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 512, 0);
        //                bQ.Dispose();
        //            }
        //        }
        //    }

        //    else if (sx == 0 && sy > 0)
        //    {
        //        b1 = new Bitmap(512, 1024);
        //        g = Graphics.FromImage(b1);


        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs, ImgType);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 0, 0);
        //                bQ.Dispose();
        //            }
        //        }

        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs + 1, ImgType);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 0, 512);
        //                bQ.Dispose();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        b1 = new Bitmap(1024, 1024);
        //        g = Graphics.FromImage(b1);

        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs, ImgType);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 0, 0);
        //                bQ.Dispose();
        //            }
        //        }

        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs + 1, R.Zs, ImgType);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 512, 0);
        //                bQ.Dispose();
        //            }
        //        }

        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs + 1, ImgType);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 0, 512);
        //                bQ.Dispose();
        //            }
        //        }
        //        fQ = string.Format("{2}.{0}.{1}.png", R.Xs + 1, R.Zs + 1, ImgType);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            if (await Images.Download(ms,fQ))
        //            {
        //                ms.Position = 0;
        //                bQ = Image.FromStream(ms);
        //                g.DrawImage(bQ, 512, 512);
        //                bQ.Dispose();
        //            }
        //        }
        //    }




        //    g.Dispose();
        //    g = Graphics.FromImage(b2);

        //    fR.X = 0;
        //    fR.Y = 0;
        //    fR.Width = 512;
        //    fR.Height = 512;

        //    g.DrawImage(b1, fR, sx, sy, 512, 512, GraphicsUnit.Pixel);

        //    b1.Dispose();
        //    g.Dispose();

        //    using (MemoryStream ms = new MemoryStream())
        //    {

        //        b2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

        //        ms.Position = 0;
        //        await Stitched.Upload(ms,SaveBitMap);
        //        b2.Dispose();

        //    }
        //}
        //public static void Stitched(string ImagesPath, Voxel V,string ImgType = "topo")
        //{

        //    DirectoryInfo imgDir = new DirectoryInfo(ImagesPath);

        //    string SaveBitMap = string.Format(Path.Combine(imgDir.FullName, string.Format("{2}cent.{0}.{1}.png", V.Xs, V.Zs, ImgType)));
           
        //    Voxel R = MinecraftOrdinates.Region(V);
            
        //    R.X -= 256;
        //    R.Z -= 256;

        //    int sx = R.Xo;
        //    int sy = R.Zo;

        //    Rectangle fR = new Rectangle();
        //    FileInfo fQ;

        //    Bitmap b1;
        //    Graphics g;
        //    Image bQ = null;

        //    Bitmap b2 = new Bitmap(512, 512);

        //    if (sx == 0 && sy == 0)
        //    {
        //        b1 = new Bitmap(512, 512);
        //        g = Graphics.FromImage(b1);

        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 0, 0);
        //            bQ.Dispose();
        //        }
        //    }
        //    else if (sx > 0 && sy == 0)
        //    {
        //        b1 = new Bitmap(1024, 512);
        //        g = Graphics.FromImage(b1);

        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 0, 0);
        //            bQ.Dispose();
        //        }

        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs + 1, R.Zs, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 512, 0);
        //            bQ.Dispose();
        //        }
        //    }
        //    else if (sx == 0 && sy > 0)
        //    {
        //        b1 = new Bitmap(512, 1024);
        //        g = Graphics.FromImage(b1);


        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 0, 0);
        //            bQ.Dispose();
        //        }

        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs + 1, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 0, 512);
        //            bQ.Dispose();
        //        }
        //    }
        //    else
        //    {
        //        b1 = new Bitmap(1024, 1024);
        //        g = Graphics.FromImage(b1);

        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 0, 0);
        //            bQ.Dispose();
        //        }

        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs + 1, R.Zs, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 512, 0);
        //            bQ.Dispose();
        //        }

        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs, R.Zs + 1, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 0, 512);
        //            bQ.Dispose();
        //        }

        //        fQ = new FileInfo(Path.Combine(ImagesPath, string.Format("{2}.{0}.{1}.png", R.Xs + 1, R.Zs + 1, ImgType)));
        //        if (fQ.Exists)
        //        {
        //            bQ = Image.FromFile(fQ.FullName);
        //            g.DrawImage(bQ, 512, 512);
        //            bQ.Dispose();
        //        }
        //    }




        //    g.Dispose();
        //    g = Graphics.FromImage(b2);

        //    fR.X = 0;
        //    fR.Y = 0;
        //    fR.Width = 512;
        //    fR.Height = 512;

        //    g.DrawImage(b1, fR, sx, sy, 512 ,512, GraphicsUnit.Pixel);
            
        //    b1.Dispose();
        //    g.Dispose();


        //    b2.Save(SaveBitMap, System.Drawing.Imaging.ImageFormat.Png);
        //    b2.Dispose();
        //}
        //public static int GenerateWorldMaps(int Scale, string RegionPath, string ImgsPath)
        //{

        //    Voxel[] V = MinecraftOrdinates.WorldMapBoundries(RegionPath);
        //    if (512 % Scale != 0)
        //        return 0;

        //    int Size = 512 / Scale;

        //    int X = Math.Abs(V[0].Xs - V[1].Xs);
        //    int Z = Math.Abs(V[0].Zs - V[1].Zs);

        //    int A = ((X - (X % Scale)) / Scale) + ((X % Scale) == 0 ? 0 : 1);
        //    int B = ((Z - (Z % Scale)) / Scale) + ((Z % Scale) == 0 ? 0 : 1);

        //    Brush SolidBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        //    Bitmap topo;
        //    Bitmap tile;

        //    topo = new Bitmap(512, 512);

        //    Graphics gBlank = Graphics.FromImage(topo);
        //    gBlank.FillRectangle(SolidBrush, 0, 0, 512, 512);


        //    topo.Save(Path.Combine(ImgsPath, string.Format("worldtopo{0}Empty.png", Size)), System.Drawing.Imaging.ImageFormat.Png);
        //    topo.Save(Path.Combine(ImgsPath, string.Format("worldtile{0}Empty.png", Size)), System.Drawing.Imaging.ImageFormat.Png);

        //    gBlank.Dispose();
        //    topo.Dispose();

        //    for (int tZ = 0; tZ < B; tZ++)
        //        for (int tX = 0; tX < A; tX++)
        //        {

        //            topo = new Bitmap(512, 512);
        //            tile = new Bitmap(512, 512);

        //            Image img = null;

        //            Graphics gTopo = Graphics.FromImage(topo);
        //            Graphics gTile = Graphics.FromImage(tile);

        //            gTopo.FillRectangle(SolidBrush, 0, 0, 512, 512);
        //            gTile.FillRectangle(SolidBrush, 0, 0, 512, 512);

        //            int xA = (tX * Scale) + V[0].Xs;
        //            int zB = (tZ * Scale) + V[0].Zs;

        //            int wX1 = xA;
        //            int wZ1 = zB;

        //            int wX2 = xA + (Scale - 1);
        //            int wZ2 = zB + (Scale - 1);

        //            for (int z = 0; z < Scale; z++)
        //                for (int x = 0; x < Scale; x++)
        //                {

        //                    string ImgTopo = Path.Combine(ImgsPath, string.Format("topo.{0}.{1}.png", xA + x, zB + z));
        //                    string ImgTile = Path.Combine(ImgsPath, string.Format("tile.{0}.{1}.png", xA + x, zB + z));


        //                    if (File.Exists(ImgTopo))
        //                    {
        //                        img = Image.FromFile(ImgTopo);
        //                        gTopo.DrawImage(img, (x * Size), (z * Size), Size, Size);
        //                        img.Dispose();
        //                    }

        //                    if (File.Exists(ImgTile))
        //                    {
        //                        img = Image.FromFile(ImgTile);
        //                        gTile.DrawImage(img, (x * Size), (z * Size), Size, Size);
        //                        img.Dispose();
        //                    }

        //                }

        //            string TopoFile = Path.Combine(ImgsPath, string.Format("worldtopo{2}.{0}.{1}.{3}.{4}.{5}.{6}.png", tX, tZ, Size, wX1, wX2, wZ1, wZ2));
        //            string TileFile = Path.Combine(ImgsPath, string.Format("worldtile{2}.{0}.{1}.{3}.{4}.{5}.{6}.png", tX, tZ, Size, wX1, wX2, wZ1, wZ2));


        //            topo.Save(TopoFile, System.Drawing.Imaging.ImageFormat.Png);
        //            tile.Save(TileFile, System.Drawing.Imaging.ImageFormat.Png);

        //            gTopo.Dispose();
        //            gTile.Dispose();
        //            topo.Dispose();
        //            tile.Dispose();

        //        }

        //    return Size;
        //}

        public static void RenderBlockPngFromRegion(byte[][] TopoData, Color[] BlockData, string ImgPath, int Xs, int Zs)
        {
            byte[] hMap = TopoData[0];
            byte[] wMap = TopoData[1];

            Bitmap bit = new Bitmap(512, 512);

            Color[][] pal = Palettes();
            Color[] tRGB = pal[0];
            Color[] wRGB = pal[1];

            for (int zz = 0; zz < 512; zz++)
            {

                for (int xx = 0; xx < 512; xx++)
                {

                    int gI = (zz * 512) + xx;

                    if (wMap[gI] < 255)
                        bit.SetPixel(xx, zz, ColorStep.MixColors(35, BlockData[gI], wRGB[wMap[gI]]));
                    else
                        bit.SetPixel(xx, zz, BlockData[gI]);
                }

            }



            DirectoryInfo imgDir = new DirectoryInfo(ImgPath);
            string SaveBitMap = string.Format(Path.Combine(imgDir.FullName, MinecraftFile.FileName(MineCraftRegionFileKind.TILE, Xs, Zs)));
            bit.Save(SaveBitMap, System.Drawing.Imaging.ImageFormat.Png);
            bit.Dispose();

        }
        public static void RenderTopoPngFromRegion(byte[][] HeightData, string ImgPath, int Xs, int Zs)
        {
            byte[] hMap = HeightData[0];
            byte[] hWMap = HeightData[1];

            Bitmap bit = new Bitmap(512, 512);

            Color[][] pal = Palettes();
            Color[] tRGB = pal[0];
            Color[] wRGB = pal[1];

            for (int zz = 0; zz < 512; zz++)
            {

                for (int xx = 0; xx < 512; xx++)
                {

                    int gI = (zz * 512) + xx;

                    if (hWMap[gI] < 255)

                        bit.SetPixel(xx, zz, wRGB[hWMap[gI]]);
                    else
                    {

                        bit.SetPixel(xx, zz, tRGB[hMap[gI]]);
                    }




                }

            }



            DirectoryInfo imgDir = new DirectoryInfo(ImgPath);
            string SaveBitMap = string.Format(Path.Combine(imgDir.FullName, string.Format("topo.{0}.{1}.png", Xs, Zs)));

            bit.Save(SaveBitMap, System.Drawing.Imaging.ImageFormat.Png);
            bit.Dispose();

        }
  
        public static void RenderLegend(string ImgPath)
        {

            FileInfo legend = new FileInfo(Path.Combine(ImgPath, "legend.png"));
            if (legend.Exists == false)
            {

                Bitmap bit = new Bitmap(20, 512);

                Color[][] pal = Palettes();

                Color[] tRGB = pal[0];
                Color[] wRGB = pal[1];


                Graphics gBit = Graphics.FromImage(bit);

                for (int z = 0; z < 256; z++)
                {
                    gBit.DrawLine(new Pen(tRGB[z]), 0, 255 - z, 15, 255 - z);
                    gBit.DrawLine(new Pen(wRGB[z]), 0, 511 - z, 15, 511 - z);

                    if (z % 10 == 0)
                    {
                        gBit.DrawLine(new Pen(Color.Black), 16, 255 - z, 19, 255 - z);
                        gBit.DrawLine(new Pen(Color.Black), 16, 511 - z, 19, 511 - z);
                    }
                }

                gBit.Dispose();

                DirectoryInfo imgDir = new DirectoryInfo(ImgPath);
                string SaveBitMap = string.Format(Path.Combine(imgDir.FullName, "legend.png"));
                bit.Save(SaveBitMap, System.Drawing.Imaging.ImageFormat.Png);
                bit.Dispose();
            }
        }
        
        public static void RenderDataFromRegion(RegionMCA mca, byte[][] TopoData, Color[] Blocks = null)
        {

            byte[] hMap = TopoData[0];
            byte[] hWMap = TopoData[1];
            Color[][] BlockColors = BlockPalette();
            Voxel Chunk;


            if (mca.IsLoaded)
            {

                for (int zz = 0; zz < 32; zz++)
                    for (int xx = 0; xx < 32; xx++)
                    {


                        mca.SetOffset(65, xx * 16, zz * 16);
                        mca.RefreshChunk();
                        Chunk = mca.Chunk;


                        NbtChunk c = mca.NbtChunk(mca);
                        NbtChunkSection s;


                        for (int x = 0; x < 16; x++)
                            for (int z = 0; z < 16; z++)
                            {
                                Chunk.Xo = x;
                                Chunk.Zo = z;

                                // c = rVox.NbtChunk(mca);
                                // Debug.Print("{0} {1} {2}", Chunk.X, Chunk.Z, Chunk.Y);
                                int cl = c.Height(Chunk.Xo, Chunk.Zo);
                                if (cl > 255) cl = 255;
                                if (cl < 0) cl = 0;
                                if (cl > 0) cl--;


                                hMap[(Chunk.WorldZ * 512) + Chunk.WorldX] = (byte)cl;
                                hWMap[(Chunk.WorldZ * 512) + Chunk.WorldX] = 255;

                                Chunk.WorldY = cl;

                                s = mca.NbtChunkSection(mca);

                                int block = s.BlockID(Chunk.ChunkBlockPos());
                                int blockdata = s.BlockData(Chunk.ChunkBlockPos());

                                switch (block)
                                {
                                    case 8:
                                    case 9:


                                        for (int ycl = cl; ycl > 0; ycl--)
                                        {

                                            Chunk.WorldY = ycl;
                                            s = mca.NbtChunkSection(mca);
                                            block = s.BlockID(Chunk.ChunkBlockPos());


                                            if (block != 9 && block != 8)
                                            {

                                                hWMap[(Chunk.WorldZ * 512) + Chunk.WorldX] = (byte)ycl;

                                                if (Blocks != null)
                                                {
                                                    switch (block)
                                                    {
                                                        case 159:
                                                            blockdata = s.BlockData(Chunk.ChunkBlockPos());
                                                            if (blockdata > 15 || blockdata < 0)
                                                                Blocks[(Chunk.WorldZ * 512) + Chunk.WorldX] = BlockColors[block][0];
                                                            else
                                                                Blocks[(Chunk.WorldZ * 512) + Chunk.WorldX] = BlockColors[block][blockdata];

                                                            break;

                                                        default:
                                                            Blocks[(Chunk.WorldZ * 512) + Chunk.WorldX] = BlockColors[block][0];
                                                            break;
                                                    }
                                                }


                                                break;
                                            }
                                        }

                                        break;


                                    default:
                                        if (Blocks != null)
                                        {
                                            switch (block)
                                            {
                                                case 159:

                                                    if (blockdata > 15 || blockdata < 0)
                                                        Blocks[(Chunk.WorldZ * 512) + Chunk.WorldX] = BlockColors[block][0];
                                                    else
                                                        Blocks[(Chunk.WorldZ * 512) + Chunk.WorldX] = BlockColors[block][blockdata];

                                                    break;

                                                default:
                                                    Blocks[(Chunk.WorldZ * 512) + Chunk.WorldX] = BlockColors[block][0];
                                                    break;
                                            }
                                        }
                                        break;
                                }


                                /*
                                switch (block)
                                {
                                    case 1://stone
                                    case 2://grass
                                    case 3://dirt
                                    case 4://cobble
                                    case 5://wood plank
                                    case 6://sapling
                                    case 7://bed rock
                                    case 8://flowing water
                                    case 9://water
                                    case 10://flo lava
                                    case 11://lava
                                    case 12://sand
                                    case 13://gravel
                                    case 14://gold ore
                                    case 15://iron ore
                                    case 16://coal ore
                                    case 17://wood
                                    case 18://leaves
                                    case 19://sponge
                                    case 20://glass
                                    case 21://lapis ore
                                    case 22://lapis block
                                    case 24://sandstone
                                    case 35://wool
                                    case 41://gold block
                                    case 42://iron block
                                    case 43://x2 stone slab
                                    case 44://stone slab
                                    case 45://bricks
                                    case 49://obsidian
                                    case 51://fire
                                    case 52://monster spawner
                                    case 56://diamond ore
                                    case 57://diamond block
                                    case 59://wheat crops
                                    case 60://farmland
                                    case 73://redstone ore
                                    case 74://glowing redstone ore
                                    case 78://snow
                                    case 79://ice
                                    case 80://snow block
                                    case 81://cactus
                                    case 82://clay
                                    case 83://sugar canes
                                    case 86://pumpkins
                                    case 87://netherrack
                                    case 88://soul sand
                                    case 89://glow stone
                                    case 90://nether portal
                                    case 91://jack o'Lantern
                                    case 95://stained glass
                                    case 98://stone bricks
                                    case 99://mushroom block (brown)
                                    case 100://mushroom block (red)
                                    case 103://melon block
                                    case 110://mycelium
                                    case 112://nether brick
                                    case 125://x2 wood slab
                                    case 126://wood slab
                                    case 129://emerald ore
                                    case 133://emerald block
                                    case 137://command block
                                    case 141://carrots
                                    case 142://potatoes
                                    case 152://redstone block
                                    case 153://nether quartz block
                                    case 155://quartz block
                                        break;

                                    case 159://white stained clay
                                        switch (blockdata)
                                        {

                                            case 0://white
                                            case 1://orange
                                            case 2://magenta
                                            case 3://light blue
                                            case 4://yellow
                                            case 5://lime
                                            case 6://pink
                                            case 7://gray
                                            case 8://light gray
                                            case 9://cyan
                                            case 10://purple
                                            case 11://blue
                                            case 12://brown
                                            case 13://green
                                            case 14://red
                                            case 15://black
                                                break;

                                        }
                                        break;

                                    case 160://stained glass
                                    case 161://Acacia Leaves (0),(1) dark oak leaves
                                    case 162://acacia wood (0), (1) dark oak wood
                                    case 165://slime block
                                    case 166://barrier
                                    case 168://prismarine
                                    case 169://sea lantern
                                    case 170://hay bale
                                    case 171://carpet (0-white, 1-15)
                                    case 172://hardened clay
                                    case 173://block of coal
                                    case 174://packed ice
                                    case 179://red sandstone
                                    case 181://x2 red sandstone slab
                                    case 182://red sandstone slab

                                    default:
                                        break;


                                }
                                 */


                            }

                    }


            }

        }
       
        public static byte[][] RenderTopoDataFromRegion(RegionMCA mca)
        {

            byte[][] topo = new byte[][] { new byte[512 * 512], new byte[512 * 512] };

            RenderDataFromRegion(mca,  topo);

            return topo;

        }
        public static byte[][] RetrieveHDT(Voxel RV, string RegionPath)
        {
            byte[][] MapData = new byte[][] { new byte[512 * 512], new byte[512 * 512] };

            FileInfo mcaF = new FileInfo(Path.Combine(RegionPath, string.Format("r.{0}.{1}.hdt", RV.Xs, RV.Zs)));
            if (mcaF.Exists == true)
            {
                FileStream tempFS = mcaF.Open(FileMode.Open, FileAccess.Read);
                tempFS.Read(MapData[0], 0, 512 * 512);
                tempFS.Read(MapData[1], 0, 512 * 512);
                tempFS.Close();
            }

            return MapData;
        }



       
    }

   

}
