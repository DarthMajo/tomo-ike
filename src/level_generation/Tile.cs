public class Tile
{
    // Variables
    private int locX;
    private int locY;
    private TomoIke.TileType val;

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

    public TomoIke.TileType Value
    {
        get { return val; }
        set { val = value;}
    }

    // Constructors
    public Tile() {}

    public Tile(TomoIke.TileType tileType)
    {
        Value = tileType;
    }
}