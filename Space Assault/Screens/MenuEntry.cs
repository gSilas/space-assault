using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Screens
{

    // Helper class represents a single entry in a MenuScreen. By default this
    // just draws the entry text string, but it can be customized to display menu
    // entries in different ways. This also provides an event that will be raised
    // when the menu entry is selected.
    class MenuEntry
    {
        // The text rendered for this entry.
        string text;

        // Tracks a fading selection effect on the entry.
        // The entries transition out of the selection effect when they are deselected.
        float selectionFade;

        // The position at which the entry is drawn. This is set by the MenuScreen
        // each frame in Update.
        Vector2 position;


        // Gets or sets the text of this menu entry.
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


        // Gets or sets the position at which to draw this menu entry.
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<EventArgs> Selected;

        // Method for raising the Selected event.
        protected internal virtual void OnSelectEntry()
        {
            Selected?.Invoke(this, EventArgs.Empty);
        }


        // Constructs a new menu entry with the specified text.
        public MenuEntry(string text)
        {
            this.text = text;
        }

        // Updates the menu entry.
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }


        // Draws the menu entry. This can be overridden to customize the appearance.
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {

            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.White : Color.DarkGray;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            
            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            Vector2 origin = new Vector2(0, Global.Font.LineSpacing / 2);

            Global.SpriteBatch.DrawString(Global.Font, text, position, color, 0,
                                   origin, scale, SpriteEffects.None, 0);
        }


        // Queries how much space this menu entry requires.
        public virtual int GetHeight()
        {
            return Global.Font.LineSpacing;
        }

        // Queries how wide the entry is, used for centering on the screen.
        public virtual int GetWidth()
        {
            return (int)Global.Font.MeasureString(Text).X;
        }
    }
}
