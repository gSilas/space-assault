using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Utils;

namespace Space_Assault.States
{
    class HighScoreEnter : IGameState, IUpdateableState, IDrawableState
    {
        private Texture2D entryField;
        private Vector2 pos;

        public void Initialize()
        {
            pos = new Vector2(100,Global.PreferredBackBufferHeight - 100);
        }

        public void LoadContent()
        {
            entryField = Global.ContentManager.Load<Texture2D>("UI/entry");
        }

        public void Update(GameTime elapsedTime)
        {

        }

        public void Draw(GameTime elapsedTime)
        {
            Global.SpriteBatch.Draw(entryField, pos, Color.Azure);
        }
        public void Kill()
        {
            IsStopped = true;
        }

        public void Resume()
        {
            if (IsStopped)
            {
                IsStopped = false;
            }
        }

        public bool IsStopped { get; set; }

        public bool Equals(IGameState other)
        {
            return other.GetType() == this.GetType();
        }

        public bool Equals(IUpdateable other)
        {
            return other.GetType() == this.GetType();
        }

        public bool Equals(IDrawableState other)
        {
            return other.GetType() == this.GetType();
        }
    }
}
