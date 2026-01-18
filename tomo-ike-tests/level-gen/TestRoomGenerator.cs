using TomoIke;

namespace tomo_ike_tests;

public class TestRoomGenerator()
{
    [Fact]
    public void TestRoomSizeConstraints()
    {
        RoomGenerator rg = new RoomGenerator(3, 25);
        Assert.Equal(3, rg.MinimumRoomSize);
        Assert.Equal(25, rg.MaximumRoomSize);
    }

    [Fact]
    public void TestRoomSizeConstraintsEqual()
    {
        // It should be allowed that maps have only one consistent room size
        var exception = Record.Exception(() => new RoomGenerator(25,25));
        Assert.Null(exception);
    }

    [Fact]
    public void TestRoomSizeConstraintsInvalid()
    {
        Assert.Throws<ArgumentException>(() => new RoomGenerator(2, 25));
        Assert.Throws<ArgumentException>(() => new RoomGenerator(-55, 3));
        Assert.Throws<ArgumentException>(() => new RoomGenerator(55, 3));
    }

    [Fact]
    public void TestRoomGeneratorInitialRoom()
    {
        Map m = new Map(100, 100);
        RoomGenerator rg = new RoomGenerator(5, 10);
        rg.BuildInitalRoom(m, 0, 0, 10, 10, 9, 5);

        Assert.Equal(TileType.WALL, m.GetTile(0,0).Value);
        Assert.Equal(TileType.WALL, m.GetTile(9,9).Value);
        Assert.Equal(TileType.WALL, m.GetTile(9,0).Value);
        Assert.Equal(TileType.WALL, m.GetTile(0,9).Value);
        Assert.Equal(TileType.DOOR, m.GetTile(9,5).Value);
        Assert.Equal(TileType.FLOOR, m.GetTile(1,1).Value);
        Assert.Equal(TileType.FLOOR, m.GetTile(5,5).Value);
        Assert.Equal(TileType.FLOOR, m.GetTile(1,8).Value);
        Assert.Equal(TileType.FLOOR, m.GetTile(8,8).Value);
    }
}