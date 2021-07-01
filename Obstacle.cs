using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame2D
{
    abstract class Obstacle
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

        protected float width, height;
        public float Width { get { return width; } }
        public float Height { get { return height; } }
        protected List<Ball> balls;

        public Obstacle(int x, int y, float width, float height, List<Ball> balls)
        {
            coordinates = new Coordinates();
            coordinates.x = x;
            coordinates.y = y;
            this.width = width;
            this.height = height;
            this.balls = balls;
        }

        public void DetectCollision()
        {
            foreach (Ball b in balls.FindAll(ball => doesBallCollideWithMe(ball)))
            {
                onCollisionOccurance(b);
            }
        }

        protected abstract bool doesBallCollideWithMe(Ball ball);

        protected abstract void onCollisionOccurance(Ball ball);
    }
}
