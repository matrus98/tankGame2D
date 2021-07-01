using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame2D
{
    class Player : MoveableObject, ICannon
    {
        public Player(float x, float y, float width, float height, int ballShape = 10, float vx = 5, float vy = 0) : base(x, y, width, height, vx, vy)
        {
            Ball_Radius_When_Spawned = ballShape;
            CanShot = true;
        }

        public readonly float Core_Velocity_Core_Radius = 20;
        public int Ball_Radius_When_Spawned { get; private set; }

        #region Interface Cannon
        public bool CanShot { get; set; }
        public float CannonX { get { return X + (Width / 2); } }
        public float CannonY { get { return Y + (Height / 8); } }
        public float Ball_Starting_Velocity_Y { get; private set; }
        public float Ball_Starting_Velocity_X { get; private set; }

        public Ball Shot()
        {
            Ball ball = new Ball(CannonX - Ball_Radius_When_Spawned, CannonY - Ball_Radius_When_Spawned, Ball_Radius_When_Spawned * 2, Ball_Radius_When_Spawned * 2, Ball_Starting_Velocity_X, Ball_Starting_Velocity_Y);

            return ball;
        }

        public void CalculateAngle_And_DetermineVelocities(int cursorX, int cursorY)
        {
            float radius = (float)Math.Sqrt(Math.Pow(cursorX - CannonX, 2) + Math.Pow(cursorY - CannonY, 2));

            float sin_phi = (CannonY - cursorY) / radius;
            float cos_phi = (cursorX - CannonX) / radius;

            Ball_Starting_Velocity_Y = -(Core_Velocity_Core_Radius * sin_phi);
            Ball_Starting_Velocity_X = Core_Velocity_Core_Radius * cos_phi;
        }
        #endregion

        #region Movement
        public void MoveLeft(float leftBorder)
        {
            if (X >= leftBorder + velocity.vx)
                X -= velocity.vx;
        }

        public void MoveRight(float rightBorder)
        {
            if (X + Width <= rightBorder - velocity.vx)
                X += velocity.vx;
        }
        #endregion

        public void UpdateTank(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            Width = width;
            Height = height;
        }

        public void UpdateBallShape(int newShape)
        {
            Ball_Radius_When_Spawned = newShape;
        }
    }
}
