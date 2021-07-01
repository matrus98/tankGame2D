using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TankGame2D
{
    class Square : Obstacle, ILifeCycle
    {
        public Square(int x, int y, float width, float height, List<Ball> balls, int HealthPoints) :
            base(x, y, width, height, balls)
        {
            MaxHealthPoints = HealthPoints;
            CurrentHealthPoints = HealthPoints;
        }

        public float CenterX { get { return X + Width / 2; } }
        public float CenterY { get { return Y + Height / 2; } }

        #region Interface LifeCycle
        public int MaxHealthPoints { get; private set; }

        public int CurrentHealthPoints { get; private set; }

        public bool Am_I_Already_Destroyed => CurrentHealthPoints <= 0;

        public event Action<Obstacle, bool> OnObjectDestroyed;
        #endregion

        protected override void onCollisionOccurance(Ball ball)
        {
            CurrentHealthPoints--;

            if(Am_I_Already_Destroyed)
                OnObjectDestroyed?.Invoke(this, true);
        }

        protected override bool doesBallCollideWithMe(Ball ball) 
        {
            float main_radius = (float)Math.Sqrt(Math.Pow(CenterX - ball.CenterX, 2) + Math.Pow(CenterY - ball.CenterY, 2));

            if (main_radius > ball.Radius + Width * Math.Sqrt(2) / 1.5f)
                return false;

            float fault_tolerance = 1;

            List<Coordinates> ballMeshOffSet = ball.collisionMeshOffSet.Where(m =>
                    m.x + ball.CenterX >= X - fault_tolerance &&
                    m.x + ball.CenterX <= X + Width + fault_tolerance &&
                    m.y + ball.CenterY >= Y - fault_tolerance &&
                    m.y + ball.CenterY <= Y + Height + fault_tolerance
                ).ToList();

            if (ballMeshOffSet.Count == 0)
                return false;

            Coordinates deepestPoint = ballMeshOffSet[ballMeshOffSet.Count / 2];
            deepestPoint.x = deepestPoint.x + ball.CenterX;
            deepestPoint.y = deepestPoint.y + ball.CenterY;


            if (deepestPoint.x - ball.VX < X + fault_tolerance && deepestPoint.x >= X - fault_tolerance)
            {
                ball.ChangeMyDirection(BounceSide.LeftSide);
                ball.fixMyPosition(true, X - ball.Width);
                return true;
            }
            if (deepestPoint.x - ball.VX > X + Width - fault_tolerance && deepestPoint.x <= X + Width + fault_tolerance)
            {
                ball.ChangeMyDirection(BounceSide.RightSide);
                ball.fixMyPosition(true, X + Width);
                return true;
            }
            if (deepestPoint.y - ball.VY < Y + fault_tolerance && deepestPoint.y >= Y - fault_tolerance)
            {
                ball.ChangeMyDirection(BounceSide.UpperSide);
                ball.fixMyPosition(false, Y - ball.Height);
                return true;
            }
            if (deepestPoint.y - ball.VY > Y + Height - fault_tolerance && deepestPoint.y <= Y + Height + fault_tolerance)
            {
                ball.ChangeMyDirection(BounceSide.BottomSide);
                ball.fixMyPosition(false, Y + Height);
                return true;
            }


            return false;
        }

        public void PerformFall(float margin)
        {
            Y += Height;
            Y += margin;
        }
    }
}
