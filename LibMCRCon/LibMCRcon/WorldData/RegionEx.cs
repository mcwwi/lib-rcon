namespace LibMCRcon.WorldData
{
    public class RegionEx
    {
        public VoxelEx W { get; set; } = new VoxelEx();
        public VoxelEx S { get; set; } = new VoxelEx();
        public VoxelEx O { get; set; } = new VoxelEx();
        public VoxelEx M { get; set; } = new VoxelEx();

        public int MapRegions { get; set; }
        public int Regions { get; set; }

        public int PixelsPerRegion() { return 512 / Regions; }

        public RegionEx():this(0,0,0,1) { }
        public RegionEx(int Xs, int Zs, int Xo, int Zo, int Y, int R)
        {
            S.X = Xs; S.Z = Zs; S.Y = 0; Regions = R;
            O.X = Xo; O.Z = Zo; O.Y = Y;
            W.ToOrdinate(S, O, R);
        }
        public RegionEx(int X, int Y, int Z, int R)
        {
            W.X = X; W.Y = Y; W.Z = Z; Regions = R;
            S.ToSegment(W, R);
            O.ToOffset(W, R);
        }
        public RegionEx(int Xs, int Zs, int R)
        {
            S.X = Xs; S.Z = Zs;
            O.X = 0; O.Z = 0;
            W.ToOrdinate(S, O, R);

            Regions = R;
        }

        public void Copy(RegionEx Target)
        {
            Target.W = W.Copy();
            Target.S = S.Copy();
            Target.O = O.Copy();
            Target.Regions = Regions;
        }

        public RegionEx Copy()
        {
            return new RegionEx() { W = W.Copy(), S = S.Copy(), O = O.Copy(), Regions = Regions };

        }
        public RegionEx CopySegmentOffset(int ToRegions)
        {
            var newR = Copy();
            newR.SetSegmentOffset(ToRegions);

            return newR;
        }
        public RegionEx CopyWorldMap(int ToRegions)
        {
            var rx = Copy();

            rx.SetSegmentOffset(ToRegions);
            rx.M = S.Copy();

            return rx;
        }
        
        public void Reset(int SegmentX, int SegmentZ, int Regions = 1, int OffsetX = 0, int OffsetZ = 0, int? Y = null)
        {
            if (Y.HasValue)
                O.Y = Y.Value;
                        
            S.X = SegmentX; S.Z = SegmentZ;
            O.X = OffsetX; O.Z = OffsetZ;
            W.ToOrdinate(S, O, Regions);

            this.Regions = Regions;
        }
        public void ResetWorldScale(int SegmentX, int SegmentZ, int Regions = 1, int WorldRegions = 2)
        {
            Reset(SegmentX, SegmentZ, Regions);
            SetWorldScale(WorldRegions);
        }

        public void SetSegmentOffset(int ToRegions = 1)
        {

            S.ToSegment(W, ToRegions);
            O.ToOffset(W, ToRegions);
            Regions = ToRegions;
        }
        public void SetOrdinates()
        {
            W.ToOrdinate(S, O, Regions);
        }
        public void SetWorldScale(int ToRegions)
        {
            M = S.Copy(); MapRegions = Regions;
            SetSegmentOffset(ToRegions);

        }

    }

    
}