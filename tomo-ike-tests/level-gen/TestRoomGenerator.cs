using TomoIke;

namespace tomo_ike_tests;

public class TestRoomGenerator()
{
    [Fact]
    public void TestRoomSizeConstraints()
    {
        Map m = new Map(100, 100);
        RoomGenerator rg = new RoomGenerator(m, 3, 25);
        Assert.Equal(3, rg.MinimumRoomSize);
        Assert.Equal(25, rg.MaximumRoomSize);
    }

    [Fact]
    public void TestRoomSizeConstraintsEqual()
    {
        // It should be allowed that maps have only one consistent room size
        Map m = new Map(100, 100);
        var exception = Record.Exception(() => new RoomGenerator(m, 25,25));
        Assert.Null(exception);
    }

    [Fact]
    public void TestRoomSizeConstraintsInvalid()
    {
        Map m = new Map(100, 100);
        Assert.Throws<ArgumentException>(() => new RoomGenerator(m, 2, 25));
        Assert.Throws<ArgumentException>(() => new RoomGenerator(m, -55, 3));
        Assert.Throws<ArgumentException>(() => new RoomGenerator(m, 55, 3));
    }

    [Fact]
    public void TestRoomGeneratorInitialRoom()
    {
        Map m = new Map(10, 10);
        RoomGenerator rg = new RoomGenerator(m, 5, 10);
        rg.RandomProperty = new Random(1234);
        rg.BuildInitalRoom();

        Assert.Equal(26, m.GetAllTilesOfValue(TileType.WALL).Count);
        Assert.Equal(28, m.GetAllTilesOfValue(TileType.FLOOR).Count);
    }
}