
using Microsoft.Xna.Framework;

namespace SpaceAssault.Screens
{
    class HighscoreMenuScreen : MenuScreen
    {
        // Constructor.
        public HighscoreMenuScreen() : base("Highscore")
        {
            // Create our menu entries.
            MenuEntry back = new MenuEntry("Back");

            // Add entries to the menu.
            MenuEntries.Add(back);
        }


    }
}
