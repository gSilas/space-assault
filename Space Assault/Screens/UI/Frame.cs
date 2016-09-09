using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceAssault.Utils
{
    class Frame
    {
        private Texture2D _frame;
        private Texture2D _edge;
        private Point _size;
        private Random _rand;

        public void LoadContent()
        {
            //64x64 TILES
            _edge = Global.ContentManager.Load<Texture2D>("Images/UI/frame_edge");
            _frame = Global.ContentManager.Load<Texture2D>("Images/UI/frame_line");
            _rand = new Random();
            _size = new Point(_edge.Width / 2, _edge.Height / 2);
        }

        public void Draw()
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
            Global.UIBatch.End();
        }
    }
}
