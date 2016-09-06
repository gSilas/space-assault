
using Microsoft.Xna.Framework;

namespace SpaceAssault.Screens
{
    class HighscoreMenuScreen : MenuScreen
    {
        MenuEntry back;

        // Constructor.
        public HighscoreMenuScreen() : base("Highscore")
        {
            // Create our menu entries.
            back = new MenuEntry("Back");

            // Hook up menu event handlers.
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(back);
        }

        // Draws the menu.
        public override void Draw(GameTime gameTime)
        {
            drawMenuEntries(gameTime);
            _frame.Draw(false);

            if (this.TransitionPosition <= 0f)
            {
                int zeilenAbstand = 20;
                int spaltenAbstand = 200;
                int spawnPointX = 150;
                int spawnPointY = 200;
                for (int i = 0; i < Global.HighScoreList._listLength; i++)
                {
                    Global.SpriteBatch.DrawString(Global.GameFont, (i + 1) + ". Platz", new Vector2(spawnPointX, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                    Global.SpriteBatch.DrawString(Global.GameFont, Global.HighScoreList._scoresList[i].Name, new Vector2(spawnPointX + spaltenAbstand, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                    Global.SpriteBatch.DrawString(Global.GameFont, (Global.HighScoreList._scoresList[i].Points).ToString(), new Vector2(spawnPointX + spaltenAbstand * 2, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                }
            }
        }

    }
}
