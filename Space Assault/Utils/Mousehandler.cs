

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Space_Assault.Utils
{
    static class Mousehandler
    {
        public static Vector3 Position 
        {
            get { return new Vector3(Mouse.GetState().Position.X, Mouse.GetState().Position.Y,0); }
        }
    }
}
