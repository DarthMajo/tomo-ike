using System;
using System.Collections.Generic;

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
            rg = new RoomGenerator(3, 10); // TODO: Fix the hard coded vars
        }

        public MapGenerator(int sizeX, int sizeY, double fillPercent)
        {
            mapSizeX = sizeX;
            mapSizeY = sizeY;
            fill = fillPercent;
            maxAttempts = 10000; // TODO: Have this as a config var
            map = new Map(mapSizeX, mapSizeY);
            rg = new RoomGenerator(3, 10); // TODO: Fix the hard coded vars
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
                rg.BuildRoom(map, nextRoomInfo["door"], nextRoomInfo["target"]);
                // TODO: REMOVE THIS LINE BELOW SOME TIME
                // Console.WriteLine(map.ToString());
                attempts++;
            }
        }

        // Private Functions
        private double CalcFilled()
        {
            int usedTiles = 0;
            for(int x = 0; x < map.MapSizeX; x++)
                for(int y = 0; y < map.MapSizeY; y++)
                    if(map.GetTile(x, y).Value != TomoIke.TileType.BLANK)
                        usedTiles++;
            return (double)usedTiles / map.Area;
        }

        private void PlaceInitialRoom()
        {
            int roomSizeX = rand.Next(rg.MinimumRoomSize, rg.MaximumRoomSize);
            int roomSizeY = rand.Next(rg.MinimumRoomSize, rg.MaximumRoomSize);
            int roomPosX = mapSizeX / 2 - roomSizeX / 2;
            int roomPosY = mapSizeY / 2 - roomSizeY / 2;
            // TODO: REMOVE THIS LINE BELOW SOME TIME
            // Console.WriteLine("Initial room: Size " + roomSizeX.ToString() + "x" + roomSizeY.ToString() + " at location " + roomPosX.ToString() + "x" + roomPosY.ToString());
            rg.BuildInitalRoom(
                posX: roomPosX,
                posY: roomPosY,
                sizeX: roomSizeX,
                sizeY: roomSizeY,
                m: map,
                doorX:-1,
                doorY:-1
            );
        }
    }
}
