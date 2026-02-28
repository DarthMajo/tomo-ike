using System;
using System.Collections.Generic;
using Godot;

namespace TomoIke
{
    public class MapGenerator
    {
        // Variables
        private int mapSizeX;
        private int mapSizeY;
        private double fill;
        private int maxAttempts;
        private Map map;
        private Random rand;
        private RoomGenerator rg;

        // Properties
        public int MapSizeX
        {
            get { return mapSizeX; }
        }

        public int MapSizeY
        {
            get { return mapSizeY; }
        }

        public Map GeneratedMap
        {
            get { return map; }
        }

        // Constructors
        public MapGenerator()
        {
            mapSizeX = 25;
            mapSizeY = 25;
            fill = 0.75;
            maxAttempts = 10000; // TODO: Have this as a config var
            map = new Map(mapSizeX, mapSizeY);
            rg = new RoomGenerator(map, 3, 10); // TODO: Fix the hard coded vars
        }

        public MapGenerator(int sizeX, int sizeY, double fillPercent)
        {
            mapSizeX = sizeX;
            mapSizeY = sizeY;
            fill = fillPercent;
            maxAttempts = 10000; // TODO: Have this as a config var
            map = new Map(mapSizeX, mapSizeY);
            rg = new RoomGenerator(map, 3, 10); // TODO: Fix the hard coded vars
        }

        // Public Functions
        public void Generate(int seed)
        {
            // Generate a map via a seed
            rand = new Random(seed);
            rg.RandomProperty = rand;

            // Place initial room
            PlaceInitialRoom();

            // Choose where the next room goes
            int attempts = 0;
            while(CalcFilled() < fill && attempts < maxAttempts)
            {
                Dictionary<string, Tile> nextRoomInfo = rg.ChooseValidDoorTile(map);
                Room newRoom = rg.BuildRoom(map, nextRoomInfo["door"], nextRoomInfo["target"]);
                if(newRoom != null)
                {
                    // Add the room
                    map.RoomCollection.AddRoom(newRoom);

                    // Calculate which room is the parent room by taking the room opposite of the target
                    foreach(RoomNode rn in map.RoomCollection.RoomList)
                    {
                        if(rn.RoomObject.InteriorExistsOnTile(nextRoomInfo["previous"]))
                        {
                            // Connect the child's room to the parent's room and vice versa
                            map.RoomCollection.ConnectRooms(rn.RoomObject, newRoom);
                            break;
                        }
                    }
                }
                attempts++;
            }

            // Setup various other room assets
            SetUpEndRoom();
        }

        // Private Functions
        private double CalcFilled()
        {
            int usedTiles = 0;
            for(int x = 0; x < map.MapSizeX; x++)
                for(int y = 0; y < map.MapSizeY; y++)
                    if(map.GetTile(x, y).Value != TileType.BLANK)
                        usedTiles++;
            return (double)usedTiles / map.Area;
        }

        private void PlaceInitialRoom()
        {
            // Places the initial room and adds it to the room collection.
            Room spawnRoom = rg.BuildInitalRoom();
            map.RoomCollection.AddRoom(spawnRoom);

            // For now we will put the player spawn in the center of the spawn room
            map.PlayerSpawnX = spawnRoom.PositionX + spawnRoom.SizeX / 2;
            map.PlayerSpawnY = spawnRoom.PositionY + spawnRoom.SizeY / 2;
        }

        private Room ChooseEndRoom()
        {
            // Choose a random room that is not the spawn room
            bool roomNotChosen = true;
            while(roomNotChosen)
            {
                // Choose a random room and make sure it is a blank room
                int index = rand.Next(map.RoomCollection.Count);
                if(map.RoomCollection.RoomList[index].RoomObject.Type == RoomType.DEFAULT)
                {
                    // If it is a blank room, then we have the end room
                    roomNotChosen = false;
                    return map.RoomCollection.RoomList[index].RoomObject;
                }
            }
            return null;
        }

        private Tile ChooseRoomTile(Room r)
        {
            int doorX = r.PositionX + rand.Next(r.SizeX - 2) + 1;
            int doorY = r.PositionY + rand.Next(r.SizeY - 2) + 1;
            return map.GetTile(doorX, doorY);
        }

        private void SetUpEndRoom()
        {
            // Retrieve the end room and set it as so
            Room endRoom = ChooseEndRoom();
            endRoom.Type = RoomType.END;

            // Choose a random tile to put the door on
            Tile endTile = ChooseRoomTile(endRoom);
            map.GoalPositionX = endTile.LocationX;
            map.GoalPositionY = endTile.LocationY;
        }
    }
}
