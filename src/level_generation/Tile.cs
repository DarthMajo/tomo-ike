public class Tile
{
    // Variables
    private uint locX;
    private uint locY;
    private uint val;

    // Properties
    public uint LocationX
    {
        get { return locX; }
        set { locX = value; }
    }

    public uint LocationY
    {
        get { return locY; }
        set { locY = value; }
    }

    public uint Value
    {
        get { return val; }
        set { val = value;}
    }

    // Constructors
    public Tile() {}
}