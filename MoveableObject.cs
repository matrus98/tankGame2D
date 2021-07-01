using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame2D
{
    abstract class MoveableObject
    {
        protected Coordinates coordinates;
        public float X
        {
            get
            {
                return coordinates.x;
            }
            protected set
            {
                coordinates.x = value;
            }
        }
        public float Y
        {
            get
            {
                return coordinates.y;
            }
            protected set
            {
                coordinates.y = value;
            }
        }

        protected Velocity velocity;
        public float VX
        {
            get
            {
                return velocity.vx;
            }
            protected set
            {
                velocity.vx = value;
            }
        }
        public float VY
        {
            get
            {
                return velocity.vy;
            }
            protected set
            {
                velocity.vy = value;
            }
        }

        protected float width;
        protected float height;
        public float Width { 
            get { 
                return width; 
            }
            protected set
            {
                width = value;
            }
        }
        public float Height { 
            get { 
                return height; 
            }
            protected set
            {
                height = value;
            }
        }

        public MoveableObject(float x, float y, float width, float height, float vx = 5, float vy = 0)
        {
            this.X = x;
            this.Y = y;

            Width = width;
            Height = height;

            velocity.vx = vx;
            velocity.vy = vy;
        }
    }
}
