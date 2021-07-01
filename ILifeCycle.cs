using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame2D
{
    interface ILifeCycle
    {
        int MaxHealthPoints { get; }
        int CurrentHealthPoints { get; }
        bool Am_I_Already_Destroyed { get; }

        event Action<Obstacle, bool> OnObjectDestroyed;
    }
}
