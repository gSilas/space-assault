using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Space_Assault.Utils
{
    static class MouseHandler
    {
        public static Vector3 Position 
        {
            get { return new Vector3(Mouse.GetState().Position.X,0, Mouse.GetState().Position.Y); }
        }

        public static MouseState MouseState
        {
            get { return Mouse.GetState();}
        }

    }
}
