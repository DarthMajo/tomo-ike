using System;
using System.Drawing;

namespace TomoIke
{
    // Classes
    public class Room
    {
        // Variables
        private int posx;
        private int posy;
        private int sizex;
        private int sizey;
        private RoomType type;

        // Properties
        public int PositionX
        {
            get { return posx; }
            set { posx = value; }
        }

        public int PositionY
        {
            get { return posy; }
            set { posy = value; }
        }

        public int SizeX
        {
            get { return sizex; }
            set 
            {
                if(value < GlobalConstants.MINIMUMROOMSIZE)
                    throw new ArgumentException("Tried making a room size X less than the minimum value.");
                sizex = value; 
            }
        }

        public int SizeY
        {
            get { return sizey; }
            set {
                if(value < GlobalConstants.MINIMUMROOMSIZE)
                    throw new ArgumentException("Tried making a room size Y less than the minimum value.");
                sizey = value;
            }
        }

        public RoomType Type
        {
            get { return type; }
            set { type = value; }
        }

        // Constructor
        public Room()
        {
            PositionX = 0;
            PositionY = 0;
            SizeX = GlobalConstants.MINIMUMROOMSIZE;
            SizeY = GlobalConstants.MINIMUMROOMSIZE;
            Type = RoomType.DEFAULT;
        }

        public Room(int px, int py, int sx, int sy, RoomType t=RoomType.DEFAULT)
        {
            PositionX = px;
            PositionY = py;
            SizeX = sx;
            SizeY = sy;
            Type = t;
        }
    }
}