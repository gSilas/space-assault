using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Assault.States
{
    public abstract class GameState
    {
        public abstract void Update();
        public abstract void Draw();
    }
}
