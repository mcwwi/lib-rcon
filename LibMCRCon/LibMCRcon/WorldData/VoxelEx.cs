namespace LibMCRcon.WorldData
{
    public class VoxelEx
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; set; } = 0;

        public bool EqualsXZ(VoxelEx obj)
        {
            return obj.X == X && obj.Z == Z;
        }

        public VoxelEx Copy()
        {
            return new VoxelEx() { X = X, Z = Z, Y = Y };
        }

        public void ToOrdinate(VoxelEx vxSegment, VoxelEx vxOffset, int Regions = 1)
        {
            X = Ordinate(Regions, vxSegment.X, vxOffset.X);
            Z = Ordinate(Regions, vxSegment.Z, vxOffset.Z);
            Y = Ordinate(1, vxSegment.Y, vxOffset.Y, 256);
        }
        public void ToSegment(VoxelEx vxOrdinate, int Regions = 1)
        {
            X = Segment(Regions, vxOrdinate.X);
            Z = Segment(Regions, vxOrdinate.Z);
            Y = Segment(Regions, vxOrdinate.Y, 256);
        }
        public void ToOffset(VoxelEx vxOrdinate, int Regions = 1)
        {
            X = Offset(Regions, vxOrdinate.X);
            Z = Offset(Regions, vxOrdinate.Z);
            Y = Offset(1, vxOrdinate.Y, 256);
        }
        public void ToWorldMap(VoxelEx vxOrdinate, int Regions)
        {
            var vx = new VoxelEx();
            vx.ToOffset(vxOrdinate, Regions);

            X = Segment(1, vx.X, 512 / Regions);
            Z = Segment(1, vx.Z, 512 / Regions);
            Y = vxOrdinate.Y;

        }

        public static int Segment(int Regions, int Ordinate, int Size = 512)
        {
            return Ordinate < 0 ? (((Ordinate + 1) / (Size * Regions)) - 1) : (Ordinate / (Size * Regions));
        }
        public static int Offset(int Regions, int Ordinate, int Size = 512)
        {
            return (Ordinate - (Segment(Regions, Ordinate,Size) * (Size * Regions))) / Regions;
        }
        public static int Ordinate(int Regions, int Segment, int Offset, int Size = 512)
        {
            if (Segment < 0)
                return -((-Segment * (Regions * Size)) - (Offset * Regions));
            else
                return (Segment * (Regions * Size)) + (Offset * Regions);

        }

    }

    
}