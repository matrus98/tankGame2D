using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBTan
{
    interface ICannon
    {
        [DefaultValue(true)]
        bool CanShot { get; }
        float CannonX { get; }
        float CannonY { get; }
        float Ball_Starting_Velocity_X { get; }
        float Ball_Starting_Velocity_Y { get; }
        Ball Shot();
        void CalculateAngle_And_DetermineVelocities(int cursorX, int cursorY);
    }
}
