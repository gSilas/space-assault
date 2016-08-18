using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Screens
{
    // A popup message box screen, used to display "are you sure?"
    // confirmation messages.
    class MessageBoxScreen : GameScreen
    {
        string message;
        Texture2D gradientTexture;

        public event EventHandler<EventArgs> Accepted;
        public event EventHandler<EventArgs> Cancelled;

        // Constructor
        public MessageBoxScreen(string message)
            : this(message, true)
        { }


        // Constructor lets the caller specify the text
        public MessageBoxScreen(string message, bool includeUsageText)
        {
            const string usageText = "\nEnter = OK" +
                                     "\nEsc = Cancel"; 
            
            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        // Loads graphics content for this screen. This uses the shared ContentManager
        // provided by the Game class, so the content will remain loaded forever.
        // Whenever a subsequent MessageBoxScreen tries to load this same content,
        // it will just get back another reference to the already loaded data.
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            gradientTexture = content.Load<Texture2D>("Images/gradient");
        }


        // Responds to user input, accepting or cancelling the message box.
        public override void HandleInput(InputState input)
        {
            if (input.IsMenuSelect())
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                    Accepted(this, EventArgs.Empty);

                ExitScreen();
            }
            else if (input.IsMenuCancel())
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, EventArgs.Empty);

                ExitScreen();
            }
        }

        // Draws the message box.
        public override void Draw(GameTime gameTime)
        {

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = Global.GameFont.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            // Draw the background rectangle.
            Global.SpriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            Global.SpriteBatch.DrawString(Global.GameFont, message, textPosition, color);

        }
    }
}
