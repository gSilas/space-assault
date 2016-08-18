using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceAssault.Utils
{
    class Frame
    {
        private Texture2D _frame;
        private Texture2D _edge;
        private Texture2D _screen;
        private Texture2D _crack;
        private Point _size;
        private Random _rand;
        private int _x;
        private int _y;

        public void LoadContent()
        {
            //64x64 TILES
            _edge = Global.ContentManager.Load<Texture2D>("Images/UI/frame_edge");
            _frame = Global.ContentManager.Load<Texture2D>("Images/UI/frame_line");
            _screen = Global.ContentManager.Load<Texture2D>("Images/UI/frame_screenoe");
            _crack = Global.ContentManager.Load<Texture2D>("Images/UI/screen_crack");
            _rand = new Random();
            _x = _rand.Next(0, Global.GraphicsManager.GraphicsDevice.Viewport.Width - _crack.Width/2);
            _y = _rand.Next(0, Global.GraphicsManager.GraphicsDevice.Viewport.Height - _crack.Height/2);
            _size = new Point(_edge.Width / 2, _edge.Height / 2);
        }

        public void Draw(bool crackedScreen)
        {
            Global.UIBatch.Begin();

            //Draw Edges

            Global.UIBatch.Draw(_edge, new Rectangle(new Point(0, 0), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width - _size.X, 0), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(0, Global.GraphicsManager.GraphicsDevice.Viewport.Height - _size.X), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width, Global.GraphicsManager.GraphicsDevice.Viewport.Height), _size), null, Color.White, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0.0f);

            //Draw Sides

            for (int x = _size.X; x < Global.GraphicsManager.GraphicsDevice.Viewport.Width - _size.X; x += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, Global.GraphicsManager.GraphicsDevice.Viewport.Height - _size.X), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, 0), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            }
            for (int y = _size.X; y < Global.GraphicsManager.GraphicsDevice.Viewport.Height - _size.X; y += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(0, y + _size.X), _size), null, Color.White,MathHelper.ToRadians(-90), Vector2.Zero, SpriteEffects.None, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width, y), _size), null, Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0.0f);
            }

            for (int x = _size.X; x < Global.GraphicsManager.GraphicsDevice.Viewport.Width - _size.X; x += _size.X)
            {
                for (int y = _size.X; y < Global.GraphicsManager.GraphicsDevice.Viewport.Height - _size.X; y += _size.X)
                {
                    Global.UIBatch.Draw(_screen, new Rectangle(new Point(x, y),_size), null, new Color(1f,1f,1f,0.07f),MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                }
            }

            if (crackedScreen)
            {

                Global.UIBatch.Draw(_crack, new Rectangle(new Point(_x, _y), new Point(_crack.Width/2,_crack.Height/2)), null, Color.White, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
            }

            Global.UIBatch.End();
        }
    }
}
