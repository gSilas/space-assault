using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAssault.Screens
{
    class ShopScreen : MenuScreen
    {
        MenuEntry damageMenuEntry;
        MenuEntry healthMenuEntry;

        static int _damage = 1;
        public static int _health = 1;

        // Constructor.
        public ShopScreen()
            : base("Shop")
        {
            // Create our menu entries.
            damageMenuEntry = new MenuEntry(string.Empty);
            healthMenuEntry = new MenuEntry(string.Empty);
            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Close Shop");

            // Hook up menu event handlers.
            damageMenuEntry.Selected += damageMenuEntrySelected;
            healthMenuEntry.Selected += healthMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(damageMenuEntry);
            MenuEntries.Add(healthMenuEntry);
            MenuEntries.Add(back);
        }

        // Fills in the latest values for the options screen menu text.
        void SetMenuEntryText()
        {
            damageMenuEntry.Text = "Damage Level: " + _damage;
            healthMenuEntry.Text = "Health Level: " + _health;
        }

        // Event handler for when the Damage menu entry is selected.
        void damageMenuEntrySelected(object sender, EventArgs e)
        {
            if (_damage < 2)
                _damage++;
            SetMenuEntryText();
        }
        // Event handler for when the Health menu entry is selected.
        void healthMenuEntrySelected(object sender, EventArgs e)
        {
            if (_health < 2)
                _health++;
            SetMenuEntryText();
        }

    }
}

