using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTan
{
    class HorizontalWall : Wall
    {
        public HorizontalWall(int x, int y, float width, float height, List<Ball> balls) :
            base(x, y, width, height, balls)
        { }

        protected override void changeBallVectorVelocity(Ball ball)
        {
            ball.ChangeMyDirection(BounceSide.BottomSide);
        }

        protected override bool doesBallCollideWithMe(Ball ball)
        {
            if(ball.Y < Y + Height)
            {
                ball.fixMyPosition(false, Y + Height);
                return true;
            }

            return false;
        }
    }
}
