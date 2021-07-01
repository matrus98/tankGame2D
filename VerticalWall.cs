using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTan
{
    class VerticalWall : Wall
    {
        public VerticalWall(int x, int y, float width, float height, List<Ball> balls) :
            base(x, y, width, height, balls)
        { }

        protected override void changeBallVectorVelocity(Ball ball)
        {
            ball.ChangeMyDirection(BounceSide.LeftSide);
        }

        protected override bool doesBallCollideWithMe(Ball ball) 
        {
            if (AmILeftWall())
            {
                if (ball.X <= X + Width)
                {
                    ball.fixMyPosition(true, X + Width);
                    return true;
                }
            }
            else
            {
                if(ball.X + ball.Width > X)
                {
                    ball.fixMyPosition(true, X - ball.Width);
                    return true;
                }
            }

            return false;
        }

        private bool AmILeftWall()
        {
            return X > 0 ? false : true;
        }
    }
}
