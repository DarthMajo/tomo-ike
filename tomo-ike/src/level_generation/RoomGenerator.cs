using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace TomoIke
{
    public class RoomGenerator
    {
        // Variables
        private Map map;
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
        public RoomGenerator(Map m, int minSize, int maxSize)
        {
            if(minSize < GlobalConstants.MINIMUMROOMSIZE || maxSize < GlobalConstants.MINIMUMROOMSIZE)
                throw new ArgumentException("Room size cannot be smaller than the minimum room size!");
            if(maxSize < minSize)
                throw new ArgumentException("The maximum room size cannot be smaller than the minimum!");

            map = m;
            minimumRoomSize = minSize;
            maximumRoomSize = maxSize;
        }

        // Public Functions
        public void BuildInitalRoom()
        {
            int roomSizeX = rand.Next(MinimumRoomSize, MaximumRoomSize);
            int roomSizeY = rand.Next(MinimumRoomSize, MaximumRoomSize);
            int roomPosX = map.MapSizeX / 2 - roomSizeX / 2;
            int roomPosY = map.MapSizeY / 2 - roomSizeY / 2;

            // Build the intial a room of size x and y on center of the map
            PlaceRoomTiles(roomPosX, roomPosY, roomSizeX, roomSizeY, -1, -1);
        }

        public void BuildRoom(Map m, Tile door, Tile target)
        {
            // Find from the door the depth the room can be
            Direction direction = FindDepthDirection(door, target);
            int maxDepth = CalcDepth(m, door, direction);

            // Calculate initial room size limits
            int roomSizeXLimit = direction == Direction.North || direction == Direction.South ? maximumRoomSize : maxDepth;
            int roomSizeYLimit = direction == Direction.East || direction == Direction.West ? maximumRoomSize : maxDepth;

            // Check if the room size limits are possible; abort if the limit is smaller than the min
            if(roomSizeXLimit <= minimumRoomSize)
                return;
            if(roomSizeYLimit <= minimumRoomSize)
                return;

            // Calculate initial room sizes
            int roomSizeX = rand.Next(minimumRoomSize, roomSizeXLimit);
            int roomSizeY = rand.Next(minimumRoomSize, roomSizeYLimit);

            // Now here is the tricky part. We know how far we can go one direction, but not the other
            // We will try with the rooms initial size
            // If that that does not work, we will randomly displace the room until all options are gone
            // If we are out of options, reduce the size of the room in non-depth direction
            List<int> originalEntrances;
            if(direction == Direction.North || direction == Direction.South)
                originalEntrances = Enumerable.Range(1, roomSizeX - 2).ToList();
            else
                originalEntrances = Enumerable.Range(1, roomSizeY - 2).ToList();
            
            List<int> entrances = new List<int>(originalEntrances);
            bool roomLayoutNotChosen = true;
            while(roomLayoutNotChosen && entrances.Count > 0)
            {
                // Choose a random entrance
                int entrance = entrances[rand.Next(0, entrances.Count - 1)];

                // Calculate the initial corner
                int posX = -1;
                int posY = -1;
                switch(direction)
                {
                    case Direction.North:
                        posX = target.LocationX - entrance;
                        posY = door.LocationY - roomSizeY + 1;
                        break;
                    case Direction.South:
                        posX = target.LocationX - entrance;
                        posY = door.LocationY;
                        break;
                    case Direction.East:
                        posX = door.LocationX;
                        posY = target.LocationY - entrance;
                        break;
                    default:
                        posX = door.LocationX - roomSizeX + 1;
                        posY = target.LocationY - entrance;
                        break;
                }

                // If we get in here, the room is placable!
                if(IsEmptyPlot(m, posX, posY, roomSizeX, roomSizeY))
                {
                    roomLayoutNotChosen = false;
                    PlaceRoomTiles(posX, posY, roomSizeX, roomSizeY, door.LocationX, door.LocationY);
                }

                // If this entrance failed, remove and try again
                entrances.Remove(entrance);

                // Check if we have ran out of rooms; if so, we cannot place a room here
                if(entrances.Count <= 0)
                {
                    if(direction == Direction.North || direction == Direction.South)
                        roomSizeX -= 1;
                    else
                        roomSizeY -= 1;

                    if(roomSizeX < minimumRoomSize)
                        break;
                    if(roomSizeY < minimumRoomSize)
                        break;

                    if(direction == Direction.North || direction == Direction.South)
                        originalEntrances = Enumerable.Range(1, roomSizeX - 1).ToList();
                    else
                        originalEntrances = Enumerable.Range(1, roomSizeY - 1).ToList();

                    entrances = new List<int>(originalEntrances);
                }
            }
        }

        public Dictionary<string, Tile> ChooseValidDoorTile(Map m)
        {
            // Start off with a blank template
            Dictionary<string, Tile> doorTileInfo = new Dictionary<string, Tile>
            {
                { "door", null },
                { "target", null }
            };

            // Get all wall tiles as a list
            List<Tile> wallTiles = m.GetAllTilesOfValue(TileType.WALL);

            // Valid door tiles need a floor on one side and a blank spot on the other side.
            // We will choose a wall tile at random and see if this is valid.
            // We will go until the list is exhausted.
            while(wallTiles.Count > 0)
            {
                Tile wall = wallTiles[rand.Next(0, wallTiles.Count - 1)];

                // First, find where the floor tile is
                Tile target;

                if(m.GetTile(wall.LocationX - 1, wall.LocationY).Value == TileType.FLOOR)
                    target = m.GetTile(wall.LocationX + 1, wall.LocationY);
                else if(m.GetTile(wall.LocationX + 1, wall.LocationY).Value == TileType.FLOOR)
                    target = m.GetTile(wall.LocationX - 1, wall.LocationY);
                else if(m.GetTile(wall.LocationX, wall.LocationY - 1).Value == TileType.FLOOR)
                    target = m.GetTile(wall.LocationX, wall.LocationY + 1);
                else if(m.GetTile(wall.LocationX, wall.LocationY + 1).Value == TileType.FLOOR)
                    target = m.GetTile(wall.LocationX, wall.LocationY - 1);
                else
                {
                    wallTiles.Remove(wall);
                    continue;
                }

                // Check the opposite tile
                if(target.Value != TileType.BLANK)
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

                // If we made it here, this should(TM) be a valid door tile!
                doorTileInfo["door"] = wall;
                doorTileInfo["target"] = target;
                break;
            }
            return doorTileInfo;
        }

        // Private Functions
        private int CalcDepth(Map m, Tile door, Direction direction)
        {
            int maxDepth = 1;
            for(int i = maxDepth; i <= MaximumRoomSize; i++)
            {
                (int, int) displacer = GetDisplacer(i, direction);
                if(
                    m.GetTile(
                        door.LocationX + displacer.Item1, 
                        door.LocationY + displacer.Item2
                    ).Value != 0
                )
                    return i;
            }
            return MaximumRoomSize;

            (int, int) GetDisplacer(int i, Direction dir)
            {
                switch(dir)
                {
                    case Direction.North:
                        return (0, -i);
                    case Direction.East:
                        return (i, 0);
                    case Direction.South:
                        return (0, i);
                    case Direction.West:
                        return (-i, 0);
                }
                return (0, 0);
            }
        }

        private Direction FindDepthDirection(Tile door, Tile target)
        {
            int xDiff = door.LocationX - target.LocationX;
            int yDiff = door.LocationY - target.LocationY;
            if(xDiff > 0)
                return Direction.West;
            else if(xDiff < 0)
                return Direction.East;
            else if(yDiff > 0)
                return Direction.North;
            else if(yDiff < 0)
                return Direction.South;
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

        private void PlaceRoomTiles(int posX, int posY, int sizeX, int sizeY, int doorX, int doorY)
        {
            for(int x = posX; x < posX + sizeX; x++)
            {
                for(int y = posY; y < posY + sizeY; y++)
                {
                    if(ShouldBeWallTile(posX, posY, sizeX, sizeY, x, y))
                        map.SetTile(x, y, TileType.WALL);
                    else
                        map.SetTile(x, y, TileType.FLOOR);
                }
            }

            // Finally, plop the door
            if(doorX >= 0 && doorY >= 0)
                map.SetTile(doorX, doorY, TileType.DOOR);
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
}