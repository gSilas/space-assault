using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
/// <summary>
/// with help from https://roecode.wordpress.com/2008/01/17/xna-framework-gameengine-development-part-6-inputgamecomponent/
/// </summary>
/// 
namespace SpaceAssault.ScreenManager
{
    // Helper for reading input from keyboard and mouse
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;
        public MouseState CurrentMouseState;

        public KeyboardState LastKeyboardState;
        public MouseState LastMouseState;

        private Point _lastMouseLocation;

        private Vector2 _mouseMoved;
        public Vector2 MouseMoved
        {
            get { return _mouseMoved; }
        }

        // Reads the latest state of the keyboard and gamepad.
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            LastMouseState = CurrentMouseState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();

            _mouseMoved = new Vector2(LastMouseState.X - CurrentMouseState.X, LastMouseState.Y - CurrentMouseState.Y);
            _lastMouseLocation = new Point(CurrentMouseState.X, CurrentMouseState.Y);
        }


        // Helper for checking if a key was newly pressed during this update. The
        // controllingPlayer parameter specifies which player to read input for.
        // If this is null, it will accept input from any player. When a keypress
        // is detected, the output playerIndex reports which player pressed it.
        public bool IsNewKeyPress(Keys key)
        {
                return (CurrentKeyboardState.IsKeyDown(key) &&
                        LastKeyboardState.IsKeyUp(key));
        }


        // Checks for a "menu select" input action.
        // The controllingPlayer parameter specifies which player to read input for.
        // If this is null, it will accept input from any player. When the action
        // is detected, the output playerIndex reports which player pressed it.
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) ||
                   IsNewKeyPress(Keys.Enter);
        }


        // Checks for a "menu cancel" input action.
        // The controllingPlayer parameter specifies which player to read input for.
        // If this is null, it will accept input from any player. When the action
        // is detected, the output playerIndex reports which player pressed it.
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape);
        }


        // Checks for a "menu up" input action.
        // The controllingPlayer parameter specifies which player to read
        // input for. If this is null, it will accept input from any player.
        public bool IsMenuUp()
        {

            return IsNewKeyPress(Keys.Up);
        }


        // Checks for a "menu down" input action.
        // The controllingPlayer parameter specifies which player to read
        // input for. If this is null, it will accept input from any player.
        public bool IsMenuDown()
        {
            return IsNewKeyPress(Keys.Down);
        }


        // Checks for a "pause the game" input action.
        // The controllingPlayer parameter specifies which player to read
        // input for. If this is null, it will accept input from any player.
        public bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape);
        }
    }
}
