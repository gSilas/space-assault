using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAssault.ScreenManagers
{
    // Helper for reading input from keyboard and mouse
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;
        public KeyboardState LastKeyboardState;


        // Reads the latest state of the keyboard and gamepad.
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }


        // Helper for checking if a key was newly pressed during this update.
        public bool IsNewKeyPress(Keys key)
        {
                return (CurrentKeyboardState.IsKeyDown(key) &&
                        LastKeyboardState.IsKeyUp(key));
        }


        // Checks for a "menu select" input action.
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) ||
                   IsNewKeyPress(Keys.Enter);
        }


        // Checks for a "menu cancel" input action.
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape);
        }


        // Checks for a "menu up" input action.
        public bool IsMenuUp()
        {
            return IsNewKeyPress(Keys.Up);
        }


        // Checks for a "menu down" input action.
        public bool IsMenuDown()
        {
            return IsNewKeyPress(Keys.Down);
        }


        // Checks for a "pause the game" input action.
        public bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape);
        }

        //mouse stuff
        public Vector3 MousePosition
        {
            get { return new Vector3(Mouse.GetState().Position.X, 0, Mouse.GetState().Position.Y); }
        }

        public bool IsLeftMouseButtonPressed()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }
    }
}
