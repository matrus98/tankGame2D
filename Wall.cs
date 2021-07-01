using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTan
{
    abstract class Wall : Obstacle
    {
        public Wall(int x, int y, float width, float height, List<Ball> balls) :
            base(x, y, width, height, balls){ }

        protected override void onCollisionOccurance(Ball ball)
        {
            changeBallVectorVelocity(ball);
        }

        protected abstract void changeBallVectorVelocity(Ball ball);
    }
}
