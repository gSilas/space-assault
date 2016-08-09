using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAssault.Utils
{
    static class MouseHandler
    {
        public static Vector2 Position 
        {
            get { return Mouse.GetState().Position.ToVector2(); }
        }

        public static MouseState MouseState
        {
            get { return Mouse.GetState();}
        }

    }
}
