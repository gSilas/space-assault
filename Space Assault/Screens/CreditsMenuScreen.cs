namespace SpaceAssault.Screens
{
    class CreditsMenuScreen : MenuScreen
    {
        // Constructor.
        public CreditsMenuScreen() : base("Credits")
        {
            // Create our menu entries.
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(back);
        }
    }
}
//Music Credit MainMenu: 
/*"Unrelenting" Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 3.0 License
http://creativecommons.org/licenses/by/3.0/
//Music Credit HighscoreMenu:
"Truth of the Legend" Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 3.0 License
http://creativecommons.org/licenses/by/3.0/

Music Credit GamePlayScreen:
"Space Fighter Loop" Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 3.0 License
http://creativecommons.org/licenses/by/3.0/

*/
