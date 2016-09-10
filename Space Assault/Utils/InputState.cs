using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAssault.Utils
{
    // Helper for reading input from keyboard and mouse
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;
        public KeyboardState LastKeyboardState;
        public MouseState CurrentMouseState;
        public MouseState LastMouseState;


        // Reads the latest state of the keyboard and gamepad.
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            LastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
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

        public bool IsMenuIncreasingSelect()
        {
            return CurrentKeyboardState.IsKeyDown(Keys.Space) ||
                   CurrentKeyboardState.IsKeyDown(Keys.Enter);
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
            get { return new Vector3(CurrentMouseState.Position.X, 0, CurrentMouseState.Position.Y); }
        }

        public bool IsLeftMouseButtonNewPressed()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton != ButtonState.Pressed);
        }

        public bool IsLeftMouseButtonPressed()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightMouseButtonNewPressed()
        {
            return (CurrentMouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton != ButtonState.Pressed);
        }

        public bool IsRightMouseButtonPressed()
        {
            return CurrentMouseState.RightButton == ButtonState.Pressed;
        }

        // projection of mouse from screen unto the 2d plane in the game
        public Vector3 getMouseInWorldPos()
        {
            Vector3 nearScreenPoint = new Vector3(CurrentMouseState.Position.ToVector2(), 0);
            Vector3 farScreenPoint = new Vector3(CurrentMouseState.Position.ToVector2(), 1);
            Vector3 nearWorldPoint = Global.GraphicsManager.GraphicsDevice.Viewport.Unproject(nearScreenPoint, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity);
            Vector3 farWorldPoint = Global.GraphicsManager.GraphicsDevice.Viewport.Unproject(farScreenPoint, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity);
            Vector3 direction = farWorldPoint - nearWorldPoint;
            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;
            zeroWorldPoint.Y = 0;
            return zeroWorldPoint;
        }
    }
}
