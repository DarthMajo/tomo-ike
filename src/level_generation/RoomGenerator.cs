using System;

public class RoomGenerator
{
    // Variables
    int maximumRoomSize;
    int minimumRoomSize;

    // Properties
    public int MaximumRoomSize
    {
        get { return maximumRoomSize; }
        set { maximumRoomSize = value; }
    }

    public int MinimumRoomSize
    {
        get { return minimumRoomSize; }
        set { minimumRoomSize = value; }
    }

    // Constructor
    public RoomGenerator(int minSize, int maxSize)
    {
        if(minSize < 3 || maxSize < 3)
            throw new ArgumentException("Room size cannot be smaller than 3!");
        if(maxSize < minSize)
            throw new ArgumentException("The maximum room size cannot be smaller than the minimum!");

        minimumRoomSize = minSize;
        maximumRoomSize = maxSize;
    }

    // Public Functions
    public void BuildInitalRoom(
        Map m,
        int posX, int posY,
        int sizeX, int sizeY,
        int doorX, int doorY
    )
    {
        // Builds a room of size x and y on the map
        // Nothing smart about it, just plops it in there
        for(int x = posX; x < posX + sizeX; x++)
        {
            for(int y = posY; y < posY + sizeY; y++)
            {
                if(x == doorX && y == doorY)
                    m.SetTile(x, y, TomoIke.TileType.DOOR);
                else if(ShouldBeWallTile(posX, posY, sizeX, sizeY, x, y))
                    m.SetTile(x, y, TomoIke.TileType.WALL);
                else
                    m.SetTile(x, y, TomoIke.TileType.FLOOR);
            }
        }
    }

    public void BuildRoom()
    {
        
    }

    // Private Functions
    private int CalcDepth(Map m, Tile door, TomoIke.Direction direction)
    {
        for(int i = 1; i < MaximumRoomSize; i++)
        {
            (int, int) displacer = GetDisplacer(i, direction);
            if(
                m.GetTile(
                    door.LocationX + displacer.Item1, 
                    door.LocationY + displacer.Item2
                ).Value != 0
            )
                return i + 1;
        }
        return 0;

        (int, int) GetDisplacer(int i, TomoIke.Direction dir)
        {
            switch(dir)
            {
                case TomoIke.Direction.North:
                    return (0, -i);
                case TomoIke.Direction.East:
                    return (i, 0);
                case TomoIke.Direction.South:
                    return (0, i);
                case TomoIke.Direction.West:
                    return (-i, 0);
            }
            return (0, 0);
        }
    }

    private TomoIke.Direction FindDepthDirection(Tile door, Tile target)
    {
        int xDiff = door.LocationX - target.LocationX;
        int yDiff = door.LocationY - target.LocationY;
        if(xDiff > 0)
            return TomoIke.Direction.West;
        else if(xDiff < 0)
            return TomoIke.Direction.East;
        else if(yDiff > 0)
            return TomoIke.Direction.South;
        else if(yDiff < 0)
            return TomoIke.Direction.North;
        else
            throw new Exception("The door and the target cannot be in the same location.");
    }

    private bool IsEmptyPlot(Map m, int posX, int posY, int sizeX, int sizeY)
    {
        // Check to see if the plot is within the map bounds
        if(
            !m.IsTileInBounds(posX, posY) || 
            !m.IsTileInBounds(posX + sizeX, posY + sizeY)
        )
            return false;
        
        // We do not care for the perimeter blocks
        for(int x = posX + 1; x < posX + sizeX - 1; x++)
            for(int y = posY + 1; y < posY + sizeY - 1; y++)
                if(m.GetTile(x, y).Value != 0)
                    return false;
        return true;
    }

    private bool ShouldBeWallTile(
        int cornerX, int cornerY,
        int sizeX, int sizeY,
        int currentX, int currentY
    )
    {
        // When drawing the rooms, the perimeter is the wall
        if(
            currentX != cornerX &&
            currentY != cornerY &&
            currentX != cornerX + sizeX - 1 &&
            currentY != cornerY + sizeY - 1
        )
            return false;
        return true;
    }
}