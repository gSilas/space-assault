using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceAssault.Screens.UI;
using SpaceAssault.Utils;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Screens
{
    class CreditsMenuScreen : MenuScreen
    {
        MenuEntry team;
        MenuEntry softwareOther;
        MenuEntry thirdAssets;
        MenuEntry SpecialThanks;

        private Dialog _itemDialog;
        private UIItem _acagamics;

        public SortedDictionary<int, string> ShopText = new SortedDictionary<int, string>();
        // Constructor.
        public CreditsMenuScreen() : base("Credits")
        {


            // Create our menu entries.
            team =new MenuEntry("Developer Team");
            softwareOther=new MenuEntry("Software & Other");
            thirdAssets=new MenuEntry("3rd Party Assets");
            SpecialThanks=new MenuEntry("Special Thanks");

            ShopText = new SortedDictionary<int, string>();
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(team);
            MenuEntries.Add(softwareOther);
            MenuEntries.Add(thirdAssets);
            MenuEntries.Add(SpecialThanks);
            MenuEntries.Add(back);

            _itemDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 110, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 650, 480, 640, 8, false, true);
            _acagamics = new UIItem();

            ShopText.Add(0, "\n Our Team:\n     - Philipp 'the_slasher' Thoms\n     - Dustin 'renderThis' Boettcher\n     - Hans-Martin 'debuGger' Wulfmeyer\n     - Daniel 'garbage_collctr' Micheel");
            ShopText.Add(1, "\n This game was created with Microsoft Visual Studio \n and Monogame.\n\n 3D modeling work done with 3Ds Max and Blender. \n\n External library Irrklang used as sound engine.\n\n All graphical assets were made with Krita or MS Paint.");
            ShopText.Add(2, "\n Music:+\n   "+ '\u0022'+"Unrelenting"+ '\u0022'+"\n   "+ '\u0022'+"Truth of the Legend"+ '\u0022'+"\n   "+ '\u0022'+"SpaceFighterLoop" + '\u0022'+"\n   "+ '\u0022'+"Cyborg Ninja"+ '\u0022'+"\n   "+ '\u0022'+"ShinyTech2"+ '\u0022'+ "\n      by Kevin MacLeod (incompetech.com),\n      licensed under Creative Commons: By Attribution \n      3.0 License creativecommons.org/licenses/by/3.0/ \n\n SoundEffects made with www.bfxr.net \n\n Space Assault uses the " + '\u0022'+ "ModeNine" + '\u0022'+" Font ");
            ShopText.Add(3, "\n Special Thanks to\n     AcaGamics\n     random french dude who did the voiceover\n and our testers:\n     - Jan-Ole Perschewski\n     - Julia Heise\n     - Delia Wulfmeyer\n     - Marcel Micheel\n     - Arne Herdick\n ");
            ShopText.Add(4, "");

            _itemDialog.LoadContent();
            _acagamics.LoadContent("Images/acagamics", 1);
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
            _acagamics.Draw(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 + 450, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 640), 1, Color.White);
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
