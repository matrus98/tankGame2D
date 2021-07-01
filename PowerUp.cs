using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTan
{
    class PowerUp : Obstacle, ILifeCycle
    {
        public PowerUp(int x, int y, float width, float height, List<Ball> balls) :
            base(x, y, width, height, balls)
        {
            MaxHealthPoints = 1;
            CurrentHealthPoints = 1;
        }

        #region Interface LifeCycle
        public int MaxHealthPoints { get; private set; }

        public int CurrentHealthPoints { get; private set; }

        public bool Am_I_Already_Destroyed => CurrentHealthPoints <= 0;

        public event Action<Obstacle, bool> OnObjectDestroyed;
        #endregion

        protected override bool doesBallCollideWithMe(Ball ball)
        {
            float radius = (float)Math.Sqrt(Math.Pow(X + Width / 2 - ball.CenterX, 2) + Math.Pow(Y + Height / 2 - ball.CenterY, 2));

            if (radius > ball.Radius + Width / 2)
                return false;

            return true;
        }

        protected override void onCollisionOccurance(Ball ball)
        {
            CurrentHealthPoints--;

            if (Am_I_Already_Destroyed)
                OnObjectDestroyed?.Invoke(this, true);
        }

        public void PerformFall(float margin, float bottomBorder)
        {
            Y += margin;

            if (Y >= bottomBorder)
                OnObjectDestroyed?.Invoke(this, false);
        }
    }
}
