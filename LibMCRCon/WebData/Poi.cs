using System;

using System.Text;

namespace WebData
{
    [Serializable]
    public class Poi
    {
        private int mcX;
        private int mcY;
        private int rx;
        private int ry;
        private int ox;
        private int oy;

          

        public int X
        {
            get { return mcX; }
            set
            {
                mcX = value;

            }
        }
        public int Y
        {
            get { return mcY; }
            set
            {
                mcY = value;
            }
        }

        public Poi() { }

        public Poi(int MineCraftX, int MineCraftZ)
        {
            X = MineCraftX;
            Y = MineCraftZ;

            Calculate();
        }

        public void SetPoi(int MineCraftX, int MineCraftZ)
        {
            X = MineCraftX;
            Y = MineCraftZ;

            Calculate();
        }
        public void Calculate()
        {
            rx = (mcX < 0) ? (mcX / 512) - 1 : (mcX / 512);
            ox = (mcX < 0) ? 512 + (mcX - ((mcX / 512) * 512)) : mcX - ((mcX / 512) * 512);

            ry = (mcY < 0) ? (mcY / 512) - 1 : (mcY / 512);
            oy = (mcY < 0) ? 512 + (mcY - ((mcY / 512) * 512)) : mcY - ((mcY / 512) * 512);

        }

        public string RenderLargeBox()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"<img src=""point6.ico"" style=""position:absolute;top:{0}px;left:{1}px;z-index:1;"" />", oy - 31, ox - 31);
            return sb.ToString();
        }


        public void RenderPoiQuery(StringBuilder sb, string imgName)
        {

            sb.AppendFormat(@"<img src=""{2}"" style=""position:absolute;top:{0}px;left:{1}px;z-index:1;pointer-events:none;"" />", oy - 7, ox - 7, imgName);
        }

        public void RenderPoiText(StringBuilder sb)
        {

            sb.AppendFormat(@"<input type=""text"" style=""position:absolute;top:{0}px;left:{1}px;z-index:1"" />", oy + 7, ox);
        }

        public void RenderPoi(StringBuilder sb)
        {


            sb.AppendFormat(@"<a href=""pointinfo.aspx?x={2}&y={3}"" target=""_blank""><img src=""point4.ico"" style=""position:absolute;top:{0}px;left:{1}px;z-index:1;"" /></a>", oy - 7, ox - 7, mcX, mcY);

        }
        public string RenderPoi()
        {


            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"<a href=""pointinfo.aspx?x={2}&y={3}"" target=""_blank""><img src=""point4.ico"" style=""position:absolute;top:{0}px;left:{1}px;z-index:1;"" /></a>", oy - 7, ox - 7, mcX, mcY);
            return sb.ToString();
        }




    }

}
