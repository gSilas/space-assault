using Microsoft.Xna.Framework;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Screens.UI;
using System;

namespace SpaceAssault.Screens
{
    class DeadWinScreen : MenuScreen
    {
        private Dialog captainDialog;
        private UIItem captain;
        private bool isWin;
        private int deadTime;
        bool voice;

        public DeadWinScreen(bool winning) : base("End")
        {
            isWin = winning;
            captainDialog = new Dialog(0, 0, 320, 400, 8, false, true);
            captain = new UIItem();
            deadTime = 17000;
            voice = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            captainDialog.LoadContent();
            captain.LoadContent("Images/captain", 4);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (deadTime <= 0)
            {
                if(isWin) Global.HighScorePoints += Global.Money;
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen(), new HighscoreMenuScreen(true));
            }

        }

        public override void Draw(GameTime gameTime)
        {
            if (isWin)
            {
                DrawCaptainDialog(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 200, Global.GraphicsManager.GraphicsDevice.Viewport.Height / 2 - 100), "                    You Succeded!\n\n        General Stargaz\n\nI am proud of you Pilot, you did your\njob very well. I couldn't have done\nit better myself.\nHere, take that medal and some\nvacation on this Spa Station not\nfar from your home Planet.\nThank you for your service, Pilot!\nDismissed!");
                deadTime -= gameTime.ElapsedGameTime.Milliseconds;

                if (!voice)
                {
                    Global.Music.Stop();
                    voice = true;
                    Global.SpeakerVolume = 0;
                }
                if (Global.Music.Finished)
                    Global.Music = Global.MusicEngine.Play2D("voice_win", Global.MusicVolume / 10, false);
            }
            else
            {
                DrawCaptainDialog(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 200, Global.GraphicsManager.GraphicsDevice.Viewport.Height / 2 - 100), "                    You Died!\n\n        General Stargaz\n\nThis Pilot did his duty in combat with\ngreat courage and steadfast dedication\neven after he was outnumbered by\nthe hundreds.\nHe sacrificed his life to defend the\nones who couldn't themselves. ");
                deadTime -= gameTime.ElapsedGameTime.Milliseconds;

                if (!voice)
                {
                    Global.Music.Stop();
                    voice = true;
                    Global.SpeakerVolume = 0;
                }
                if (Global.Music.Finished)
                    Global.Music = Global.MusicEngine.Play2D("voice_loss", Global.MusicVolume / 10, false);
            }
        }

        void DrawCaptainDialog(Point pos, string msg)
        {
            captainDialog.Draw(new Point(pos.X, pos.Y - 170), msg);
            captain.Draw(new Point(pos.X + 10, pos.Y - 165), 1, Color.White);
        }

    }
}
