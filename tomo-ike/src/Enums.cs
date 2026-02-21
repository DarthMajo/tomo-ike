namespace TomoIke
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public enum RoomType
    {
        DEFAULT = 0,
        SPAWN = 1,
        END = 2
    }

    public enum TileType
    {
        INVALID = -1,
        BLANK = 0,
        DOOR = 68,
        FLOOR = 46,
        WALL = 88
    }
}