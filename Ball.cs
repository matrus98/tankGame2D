using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame2D
{
    class Ball : MoveableObject
    {

        public Ball(float x, float y, float width, float height, float vx = 5, float vy = 0) : base(x, y, width, height, vx, vy)
        {
            collisionMeshOffSet = new Coordinates[meshDensity];

            for (int i = 0; i < meshDensity; i++)
            {
                collisionMeshOffSet[i].x = Radius * (float)Math.Cos(i * (360 / meshDensity));
                collisionMeshOffSet[i].y = Radius * (float)Math.Sin(i * (360 / meshDensity));
            }
        }

        public float Radius { get { return Width / 2; } }
        public float CenterX { get { return X + Width / 2; } }
        public float CenterY { get { return Y + Height / 2; } }

        public readonly Coordinates[] collisionMeshOffSet;
        private readonly int meshDensity = 24;

        private bool left;
        private bool right;
        private bool top;
        private bool bottom;

        public void Move()
        {
            changeVelocities();

            X += VX;
            Y += VY;
        }

        private void changeVelocities()
        {
            if(left && bottom)
            {
                VX = -VX;
                VY = -VY;
            }
            else if (bottom && right)
            {
                VX = -VX;
                VY = -VY;
            }
            else if (right && top)
            {
                VX = -VX;
                VY = -VY;
            }
            else if (top && left)
            {
                VX = -VX;
                VY = -VY;
            }
            else if (left && right)
                VY = -VY;
            else if (top && bottom)
                VX = -VX;
            else if (left)
                VX = -VX;
            else if (right)
                VX = -VX;
            else if (top)
                VY = -VY;
            else if (bottom)
                VY = -VY;

            left = right = top = bottom = false;
        }

        public void ChangeMyDirection(BounceSide bounceSide) {
            switch (bounceSide)
            {
                case BounceSide.LeftSide:
                    //VX = -VX;
                    left = true;
                    break;
                case BounceSide.RightSide:
                    //VX = -VX;
                    right = true;
                    break;
                case BounceSide.UpperSide:
                    //VY = -VY;
                    top = true;
                    break;
                case BounceSide.BottomSide:
                    //VY = -VY;
                    bottom = true;
                    break;
                default:
                    break;
            }
        }

        public void fixMyPosition(bool verticalCollision, float param)
        {
            if (verticalCollision)
                X = param;
            else Y = param;
        }

        public void SetVelocities(float vx, float vy)
        {
            VX = vx;
            VY = vy;
        }
    }
}
