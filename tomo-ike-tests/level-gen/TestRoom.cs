using TomoIke;

namespace tomo_ike_tests;

public class TestRoom
{
    [Fact]
    public void TestConstructors()
    {
        Room r1 = new Room();
        Assert.Equal(0, r1.PositionX);
        Assert.Equal(0, r1.PositionY);
        Assert.Equal(RoomType.DEFAULT, r1.Type);

        Room r2 = new Room(42, 55, 6, 7, RoomType.END);
        Assert.Equal(42, r2.PositionX);
        Assert.Equal(55, r2.PositionY);
        Assert.Equal(6, r2.SizeX);
        Assert.Equal(7, r2.SizeY);
        Assert.Equal(RoomType.END, r2.Type);
    }

    [Fact]
    public void TestInvalidRoomSizes()
    {
        Assert.Throws<ArgumentException>(() => new Room(0, 0, -3, 4));
        Assert.Throws<ArgumentException>(() => new Room(0, 0, 3, -2));
        Assert.Throws<ArgumentException>(() => new Room(0, 0, 2, 4));
        Assert.Throws<ArgumentException>(() => new Room(0, 0, 3, 1));
        Room validRoom = new Room(0, 0, 3, 3);
    }
}