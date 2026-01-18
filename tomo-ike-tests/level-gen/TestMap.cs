using TomoIke;

namespace tomo_ike_tests;

public class TestMap
{
    [Fact]
    public void TestArea()
    {
        // Tests the area property
        TomoIke.Map m = new TomoIke.Map(100, 100);
        Assert.Equal(10000, m.Area);
    }

    [Fact]
    public void TestAreaUsable()
    {
        // Tests the area usable property
        TomoIke.Map m = new TomoIke.Map(100, 100);
        Assert.Equal(9801, m.AreaUsable);
    }

    [Fact]
    public void TestGetTile()
    {
        // Tests the ability to get a tile
        TomoIke.Map m = new TomoIke.Map(100, 100);
        Tile t1 = new Tile();
        t1.LocationX = 42;
        t1.LocationY = 42;
        t1.Value = TileType.DOOR;

        // The before state
        Assert.Equal(TileType.BLANK, m.GetTile(42, 42).Value);

        // Set and check
        m.SetTile(t1);
        Assert.Equal(TileType.DOOR, m.GetTile(42, 42).Value);
    }

    [Fact]
    public void TestGetAllTilesofValue()
    {
        // We'll put 3 tiles in and see if we get those 3 tiles back
        TomoIke.Map m = new TomoIke.Map(100, 100);

        // The tiles
        Tile t1 = new Tile();
        t1.LocationX = 42;
        t1.LocationY = 42;
        t1.Value = TileType.DOOR;

        Tile t2 = new Tile();
        t2.LocationX = 33;
        t2.LocationY = 67;
        t2.Value = TileType.DOOR;

        m.SetTile(t1);
        m.SetTile(t2);
        m.SetTile(19, 89, TileType.DOOR);

        // Now let's see if we get those 3
        List<Tile> tileList = m.GetAllTilesOfValue(TileType.DOOR);
        Assert.Equal(3, tileList.Count);
        Assert.Contains(tileList, t => 
            t.LocationX == 42 &&
            t.LocationY == 42 &&
            t.Value == TileType.DOOR
        );
        Assert.Contains(tileList, t => 
            t.LocationX == 33 &&
            t.LocationY == 67 &&
            t.Value == TileType.DOOR
        );
        Assert.Contains(tileList, t => 
            t.LocationX == 19 &&
            t.LocationY == 89 &&
            t.Value == TileType.DOOR
        );
    }

    [Fact]
    public void TestMapSize()
    {
        // Testing the map sizes
        TomoIke.Map m = new TomoIke.Map(160, 90);
        Assert.Equal(160, m.MapSizeX);
        Assert.Equal(90, m.MapSizeY);
    }

    [Fact]
    public void TestMapSizeInvalid()
    {
        // A map with a negative size literally cannot exist
        Assert.Throws<ArgumentException>(() => new TomoIke.Map(-5, 24));
        Assert.Throws<ArgumentException>(() => new TomoIke.Map(50, -426));
    }

    [Fact]
    public void TestPerimeterTile()
    {
        // Tests the area property
        TomoIke.Map m = new TomoIke.Map(100, 100);

        // Testing some hard coded spots
        Assert.True(m.IsPerimeterTile(0,0));
        Assert.True(m.IsPerimeterTile(99,99));
        Assert.True(m.IsPerimeterTile(0,42));
        Assert.True(m.IsPerimeterTile(42,99));
        Assert.True(m.IsPerimeterTile(42,0));
        Assert.False(m.IsPerimeterTile(1, 1));
        Assert.False(m.IsPerimeterTile(98, 98));
        Assert.False(m.IsPerimeterTile(42, 42));
        Assert.False(m.IsPerimeterTile(-92, 67));
        Assert.False(m.IsPerimeterTile(1, 98));

        // Now we are going to count how many perimeter tiles there are
        int count = 0;
        for(int x = 0; x < m.MapSizeX; x++)
        {
            for(int y = 0; y < m.MapSizeY; y++)
            {
                if(m.IsPerimeterTile(x,y))
                    count++;
            }
        }
        Assert.Equal(396, count);
    }

    [Fact]
    public void TestTileInBounds()
    {
        // Test the tile within the map
        TomoIke.Map m = new TomoIke.Map(100, 100);

        Assert.True(m.IsTileInBounds(0,0));
        Assert.True(m.IsTileInBounds(99,99));
        Assert.True(m.IsTileInBounds(42,42));
        Assert.True(m.IsTileInBounds(69,23));
        Assert.True(m.IsTileInBounds(0,99));
        Assert.False(m.IsTileInBounds(-1,0));
        Assert.False(m.IsTileInBounds(0,-1));
        Assert.False(m.IsTileInBounds(100,100));
        Assert.False(m.IsTileInBounds(99,100));
        Assert.False(m.IsTileInBounds(100,99));
        Assert.False(m.IsTileInBounds(42,-5));
    }
}