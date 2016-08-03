using System;
using Microsoft.Xna.Framework;

namespace SpaceAssault.ScreenManagers
{
    // Enum describes the screen transition state.
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    // A screen is a single layer that has update and draw logic, and which
    // can be combined with other layers to build up a complex menu system.
    // For instance the main menu, the options menu, the "are you sure you
    // want to quit" message box, and the main game itself are all implemented
    // as screens.
    public abstract class GameScreen
    {
        // Normally when one screen is brought up over the top of another,
        // the first screen will transition off to make room for the new
        // one. This property indicates whether the screen is only a small
        // popup, in which case screens underneath it do not need to bother
        // transitioning off.
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;

        // Indicates how long the screen takes to
        // transition on when it is activated.
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        TimeSpan transitionOnTime = TimeSpan.Zero;

        // Indicates how long the screen takes to
        // transition off when it is deactivated.
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        TimeSpan transitionOffTime = TimeSpan.Zero;

        // Gets the current position of the screen transition, ranging
        // from zero (fully active, no transition) to one (transitioned
        // fully off to nothing).
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        float transitionPosition = 1;


        // Gets the current alpha of the screen transition, ranging
        // from 1 (fully active, no transition) to 0 (transitioned
        // fully off to nothing).
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        // Gets the current screen transition state.
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransitionOn;


        // There are two possible reasons why a screen might be transitioning
        // off. It could be temporarily going away to make room for another
        // screen that is on top of it, or it could be going away for good.
        // This property indicates whether the screen is exiting for real:
        // if set, the screen will automatically remove itself as soon as the
        // transition finishes.
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting = false;


        // Checks whether this screen is active and can respond to user input.
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                       (screenState == ScreenState.TransitionOn ||
                        screenState == ScreenState.Active);
            }
        }

        bool otherScreenHasFocus;


        // Gets the manager that this screen belongs to.
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;

        // Gets the index of the player who is currently controlling this screen,
        // or null if it is accepting input from any player. This is used to lock
        // the game to a specific player profile. The main menu responds to input
        // from any connected gamepad, but whichever player makes a selection from
        // this menu is given control over all subsequent screens, so other gamepads
        // are inactive until the controlling player returns to the main menu.
        /*public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        PlayerIndex? controllingPlayer;*/


        // Load graphics content for the screen.
        public virtual void LoadContent() { }

        // Unload content for the screen.
        public virtual void UnloadContent() { }


        // Allows the screen to run logic, such as updating the transition position.
        // Unlike HandleInput, this method is called regardless of whether the screen
        // is active, hidden, or in the middle of a transition.
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                // If the screen is going away to die, it should transition off.
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Active;
                }
            }
        }


        // Helper for updating the screen transition position.
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }


        // Allows the screen to handle user input. Unlike Update, this method
        // is only called when the screen is active, and not when some other
        // screen has taken the focus.
        public virtual void HandleInput(InputState input) { }


        // This is called when the screen should draw itself.
        public virtual void Draw(GameTime gameTime) { }


        // Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        // instantly kills the screen, this method respects the transition timings
        // and will give the screen a chance to gradually transition off.
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }
    }
}
