using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;

namespace TomoIke
{
    public class Rooms
    {
        // Variables
        private List<RoomNode> roomMap;

        // Properties
        public List<RoomNode> RoomList
        {
            get { return roomMap; }
        }

        public int Count
        {
            get { return roomMap.Count; }
        }

        // Constructors
        public Rooms()
        {
            roomMap = new List<RoomNode>();
        }

        // Class Functions
        public void AddRoom(Room r)
        {
            RoomNode rn = new RoomNode(r);
            roomMap.Add(rn);
        }

        public void ConnectRooms(Room r1, Room r2)
        {
            RoomNode room1Node = GetRoomNode(r1);
            RoomNode room2Node = GetRoomNode(r2);

            room1Node.ConnectedRooms.Add(r2);
            room2Node.ConnectedRooms.Add(r1);
        }

        private RoomNode GetRoomNode(Room r)
        {
            RoomNode rn = null;
            foreach(RoomNode n in roomMap)
            {
                if(r == n.RoomObject)
                    return n;
            }
            return rn;
        }
    }

    public class RoomNode
    {
        // Variables
        private Room room;
        private List<Room> connectedRooms;

        // Properties
        public Room RoomObject
        {
            get { return room; }
            set { room = value; }
        }

        public List<Room> ConnectedRooms
        {
            get { return connectedRooms; }
            set { connectedRooms = value; }
        }

        // Constructors
        public RoomNode(Room r)
        {
            room = r;
            connectedRooms = new List<Room>();
        }
    }
}