namespace TomoIke
{
    public class Tile
    {
        // Variables
        private int locX;
        private int locY;
        private TileType val;

        // Properties
        public int LocationX
        {
            get { return locX; }
            set { locX = value; }
        }

        public int LocationY
        {
            get { return locY; }
            set { locY = value; }
        }

        public TileType Value
        {
            get { return val; }
            set { val = value;}
        }

        // Constructors
        public Tile() {}

        public Tile(TileType tileType)
        {
            Value = tileType;
        }
    }
}