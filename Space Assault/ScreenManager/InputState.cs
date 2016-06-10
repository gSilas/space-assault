using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;


namespace SpaceAssault
{
    // Helper for reading input from keyboard and mouse
    public class InputState
    {
        public const int MaxInputs = 4;
        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly KeyboardState[] LastKeyboardStates;

        // Constructs a new input state.
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            LastKeyboardStates = new KeyboardState[MaxInputs];

        }

        // Reads the latest state of the keyboard and gamepad.
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
            }
        }


        // Helper for checking if a key was newly pressed during this update. The
        // controllingPlayer parameter specifies which player to read input for.
        // If this is null, it will accept input from any player. When a keypress
        // is detected, the output playerIndex reports which player pressed it.
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                        LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex));
            }
        }


        // Checks for a "menu select" input action.
        // The controllingPlayer parameter specifies which player to read input for.
        // If this is null, it will accept input from any player. When the action
        // is detected, the output playerIndex reports which player pressed it.
        public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex);
        }


        // Checks for a "menu cancel" input action.
        // The controllingPlayer parameter specifies which player to read input for.
        // If this is null, it will accept input from any player. When the action
        // is detected, the output playerIndex reports which player pressed it.
        public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex);
        }


        // Checks for a "menu up" input action.
        // The controllingPlayer parameter specifies which player to read
        // input for. If this is null, it will accept input from any player.
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex);
        }


        // Checks for a "menu down" input action.
        // The controllingPlayer parameter specifies which player to read
        // input for. If this is null, it will accept input from any player.
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex);
        }


        // Checks for a "pause the game" input action.
        // The controllingPlayer parameter specifies which player to read
        // input for. If this is null, it will accept input from any player.
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex);
        }
    }
}
