using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Utils;

namespace SpaceAssault.Screens
{
    // A popup message box screen, used to display "are you sure?"
    // confirmation messages.
    class MessageBoxScreen : GameScreen
    {
        string message;
        Texture2D _space;
        Texture2D _corner;
        Texture2D _border;

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
            _space = content.Load<Texture2D>("Images//UI/dialog_space");
            _corner = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_edge");
            _border = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_frame");
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
            Vector2 textSize = Global.Font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 0;
            const int vPad = 0;



            Rectangle _borderRectangleHorizontal = new Rectangle((int)textPosition.X - hPad,
                                                                 (int)textPosition.Y - vPad - 16,
                                                                 (int)textSize.X + hPad * 2,
                                                                 16);

            Rectangle _borderRectangleVertical   = new Rectangle((int)textPosition.X - hPad,
                                                                 (int)textPosition.Y - vPad,
                                                                 (int)textSize.Y + vPad * 2,
                                                                 16);

            Rectangle _spaceRectangle            = new Rectangle((int)textPosition.X - hPad,
                                                                 (int)textPosition.Y - vPad,
                                                                 (int)textSize.X + hPad * 2,
                                                                 (int)textSize.Y + vPad * 2);

            Rectangle _cornerRectangle           = new Rectangle((int)textPosition.X - hPad -16,
                                                                 (int)textPosition.Y - vPad -16,
                                                                 16,
                                                                 16);


            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            // Draw the space
            Global.SpriteBatch.Draw(_space, _spaceRectangle, Global.UIColor);

            // Draw the borders
            //top
            Global.SpriteBatch.Draw(_border, _borderRectangleHorizontal, Global.UIColor);
            //bottom
            _borderRectangleHorizontal.Y += (int)textSize.Y + vPad * 2 + 16;
            Global.SpriteBatch.Draw(_border, null, _borderRectangleHorizontal, null, null, 0, null , Global.UIColor, SpriteEffects.FlipVertically);
            //left
            Global.SpriteBatch.Draw(_border, null, _borderRectangleVertical, null, null, (float)Math.PI / 2.0f, null, Global.UIColor, SpriteEffects.FlipVertically);
            //right
            _borderRectangleVertical.X += (int)textSize.X + hPad * 2 + 16;
            Global.SpriteBatch.Draw(_border, null, _borderRectangleVertical, null, null, (float)Math.PI / 2.0f, null, Global.UIColor);

            // Draw the corners
            //lefttop
            Global.SpriteBatch.Draw(_corner, null, _cornerRectangle, null, null, 0, null, Global.UIColor);
            //leftbottom
            _cornerRectangle.Y += (int)textSize.Y + vPad * 2 + 16;
            Global.SpriteBatch.Draw(_corner, null, _cornerRectangle, null, null, 0, null, Global.UIColor, SpriteEffects.FlipVertically);
            //rightbottom
            _cornerRectangle.X += (int)textSize.X + hPad * 2 + 32;
           Global.SpriteBatch.Draw(_corner, null, _cornerRectangle, null, null, (float)Math.PI / 2.0f, null, Global.UIColor, SpriteEffects.FlipHorizontally);
            //righttop
            _cornerRectangle.X = (int)textPosition.X - hPad + (int)textSize.X + hPad * 2;
            _cornerRectangle.Y = (int)textPosition.Y - vPad -16;
           Global.SpriteBatch.Draw(_corner, null, _cornerRectangle, null, null, 0, null, Global.UIColor, SpriteEffects.FlipHorizontally);



            // Draw the message box text.
            Global.SpriteBatch.DrawString(Global.Font, message, textPosition, color);

        }
    }
}
