using TomoIke;

namespace tomo_ike_tests;

public class TestTile()
{
    [Fact]
    public void TestLocation()
    {
        // Tests the get/set
        Tile t = new Tile();
        t.LocationX = 42;
        t.LocationY = 57;
        Assert.Equal(42, t.LocationX);
        Assert.Equal(57, t.LocationY);
    }

    [Fact]
    public void TestValue()
    {
        // Tests the tile type
        Tile t = new Tile();

        t.Value = TileType.BLANK;
        Assert.Equal(TileType.BLANK, t.Value);
        
        t.Value = TileType.FLOOR;
        Assert.Equal(TileType.FLOOR, t.Value);
        Assert.NotEqual(TileType.BLANK, t.Value);
    }

    [Fact]
    public void TestValueConstructor()
    {
        // Tests the tile value constructor
        Tile t = new Tile(TileType.DOOR);
        Assert.Equal(TileType.DOOR, t.Value);
    }
}