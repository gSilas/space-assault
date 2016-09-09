using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceAssault.Screens.UI;
using SpaceAssault.Utils;

namespace SpaceAssault.Screens
{
    class CreditsMenuScreen : MenuScreen
    {
        MenuEntry Programmers;
        MenuEntry ProgramsANDLibraries;
        MenuEntry Sounds;
        MenuEntry SpecialThanks;

        private Dialog _itemDialog;

        public SortedDictionary<int, string> ShopText = new SortedDictionary<int, string>();
        // Constructor.
        public CreditsMenuScreen() : base("Credits")
        {


            // Create our menu entries.
            Programmers =new MenuEntry("Programmers");
            ProgramsANDLibraries=new MenuEntry("Programs and Libraries");
            Sounds=new MenuEntry("Effects");
            SpecialThanks=new MenuEntry("Special Thanks");

            ShopText = new SortedDictionary<int, string>();
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(Programmers);
            MenuEntries.Add(ProgramsANDLibraries);
            MenuEntries.Add(Sounds);
            MenuEntries.Add(SpecialThanks);
            MenuEntries.Add(back);

            _itemDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 110, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 650, 380, 685, 8, false, false);

            ShopText.Add(0, "Programmers:\n     - Philipp 'the_slasher' Thoms\n     - Dustin 'renderThis' Boettcher\n     - Hans-Martin 'debuGger' Wulfmeyer\n     - Daniel 'garbage_collctr' Micheel");
            ShopText.Add(1, "\n Programms:\n   This Games was created with: \n   Microsoft Visual Studio and Monogame\n\n Models:\n   All of the Models were made with 3Ds Max or Blender\n\n Libraries:\n   IrrKlang\n\n Art:\n   The Graphical assets were made with Krita");
            ShopText.Add(2, "\n Music:\n   MainMenu:\n        Unrelenting by Kevin MacLeod (incompetech.com)\n        Licensed under Creative Commons: By Attribution 3.0 License\n        http://creativecommons.org/licenses/by/3.0/ \n\n   HighscoreMenu:\n        Truth of the Legend by Kevin MacLeod (incompetech.com)\n        Licensed under Creative Commons: By Attribution 3.0 License\n        http://creativecommons.org/licenses/by/3.0/ \n\n   GamePlayScreen:\n        SpaceFighterLoop by Kevin MacLeod (incompetech.com)\n        Licensed under Creative Commons: By Attribution 3.0 License\n        http://creativecommons.org/licenses/by/3.0/ \n\n SoundEffects:\n   They were made with http://www.bfxr.net/ \n\n Font:\n   This Game uses the " + '\u0022'+ "modenine"+ '\u0022'+" Font ");
            ShopText.Add(3, "\n Special Thanks to\n    AcaGamics\n\n    and our Testers:\n       - Jan-Ole-Perschefski\n       - Julia Heise\n ");
            ShopText.Add(4, "");

            _itemDialog.LoadContent();
            _frame.LoadContent();

        }

        //#################################
        // Draw
        //#################################
        public override void Draw(GameTime gameTime)
        {
            drawMenuEntries(gameTime);


            string entry;
            ShopText.TryGetValue(selectedEntry, out entry);
            _itemDialog.Draw(entry);

            _frame.Draw();
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
