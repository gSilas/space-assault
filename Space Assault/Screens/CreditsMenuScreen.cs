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
