using System;
using System.Collections.Generic;
using TomoIke;

public class RoomGenerator
{
    // Variables
    private int maximumRoomSize;
    private int minimumRoomSize;
    private Random rand;

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

    public Random RandomProperty
    {
        get { return rand; }
        set { rand = value; }
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

    public void BuildRoom(Map m, Tile door, Tile target)
    {
        // Find from the door the depth the room can be
        Direction direction = FindDepthDirection(door, target);
        int maxDepth = CalcDepth(m, door, direction);

        // Calculate initial room sizes
        int roomSizeXLimit = direction == Direction.North || direction == Direction.South ? maximumRoomSize : maxDepth;
        int roomSizeYLimit = direction == Direction.East || direction == Direction.West ? maximumRoomSize : maxDepth;
        if(roomSizeXLimit <= minimumRoomSize)
            return;
        if(roomSizeYLimit <= minimumRoomSize)
            return;
        int roomSizeX = rand.Next(minimumRoomSize, roomSizeXLimit);
        int roomSizeY = rand.Next(minimumRoomSize, roomSizeYLimit);

        // Now here is the tricky part. We know how far we can go one direction, but not the other
        // We will try with the rooms initial size
        // If that that does not work, we will randomly displace the room until all options are gone
        // If we are out of options, reduce the size of the room in non-depth direction
        List<int> originalEntrances;
        //if(direction == Direction.North || direction == Direction.South)
            
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

    private (Tile, Tile) ChooseValidDoorTile(Map m)
    {
        // Get all wall tiles as a list
        List<Tile> wallTiles = m.GetAllTilesOfValue(TomoIke.TileType.WALL);

        // Valid door tiles need a floor on one side and a blank spot on the
        // other side.
        // We will choose a wall tile at random and see if this is valid.
        // We will go until the list is exhausted.
        while(wallTiles.Count > 0)
        {
            int randomWallIndex = rand.Next(0, wallTiles.Count - 1);
            Tile wall = wallTiles[randomWallIndex];

            // First, find where the floor tile is
            // TODO: Could be a function :)
            Tile floor;
            Tile target;
            if(m.GetTile(wall.LocationX - 1, wall.LocationY).Value == TomoIke.TileType.FLOOR)
            {
                floor = m.GetTile(wall.LocationX - 1, wall.LocationY);
                target = m.GetTile(wall.LocationX + 1, wall.LocationY);
            }
            else if(m.GetTile(wall.LocationX + 1, wall.LocationY).Value == TomoIke.TileType.FLOOR)
            {
                floor = m.GetTile(wall.LocationX + 1, wall.LocationY);
                target = m.GetTile(wall.LocationX - 1, wall.LocationY);
            }
            else if(m.GetTile(wall.LocationX, wall.LocationY - 1).Value == TomoIke.TileType.FLOOR)
            {
                floor = m.GetTile(wall.LocationX, wall.LocationY - 1);
                target = m.GetTile(wall.LocationX, wall.LocationY + 1);
            }
            else if(m.GetTile(wall.LocationX, wall.LocationY + 1).Value == TomoIke.TileType.FLOOR)
            {
                floor = m.GetTile(wall.LocationX, wall.LocationY + 1);
                target = m.GetTile(wall.LocationX, wall.LocationY - 1);
            }
            else
            {
                // This is a corner piece
                wallTiles.Remove(wall);
                continue;
            }

            // Check the opposite tile
            if(m.GetTile(target.LocationX, target.LocationY).Value != TomoIke.TileType.BLANK)
            {
                wallTiles.Remove(wall);
                continue;
            }

            // Check if a 3x3 room can be built here
            if(m.IsPerimeterTile(target.LocationX, target.LocationY))
            {
                wallTiles.Remove(wall);
                continue;
            }

            // If we made it here, this is a valid door tile!
            return (wall, target);
        }
        return (null, null);
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