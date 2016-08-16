
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Screens.UI
{
    class Dialog
    {
        private Texture2D _frame;
        private Texture2D _edge;
        private Texture2D _frameButtons;
        private Texture2D _space;
        private Point _pos;
        private int _x;
        private int _y;
        private int _height;
        private int _width;
        private Point _size;

        public Dialog(int x, int y, int height, int width)
        {
            _pos = new Point(x,y);
            _x = x;
            _y = y;
            _height = height;
            _width = width;
        }

        public void LoadContent()
        {
            _space = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_space");
            _edge = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_edge");
            _frame = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_frame");
            _frameButtons = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_options");
            _size = new Point(_frame.Width / 2, _frame.Height / 2);
        }

        public void Draw(string msg)
        {
            Global.UIBatch.Begin();

            Global.UIBatch.Draw(_edge, new Rectangle(_pos, _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x + _width, _y),_size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x, _y + _height), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x + _width +_size.X, _y + _height +_size.X), _size), null, Color.White, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0.0f);

            for (int x = _x + _size.X; x < _width + _x; x += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, _y), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, _y + _height), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }
            Global.UIBatch.Draw(_frameButtons, new Rectangle(new Point(_width + _x - _size.X, _y), _size), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            
            for (int y = _y +_size.X; y < _height +_y; y += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(_width + _x, y +_size.X ), _size), null, Color.White, MathHelper.ToRadians(-90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(_x + _size.X, y), _size), null, Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }

            
            for (int x = _x; x < _width + _x - _size.X; x += _size.X)
            {
                for (int y = _y; y < _height +_y - _size.X; y += _size.X)
                {
                    Global.UIBatch.Draw(_space, new Rectangle(new Point(x + _size.X, y + _size.X), _size), null, Color.White, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                }
            }

            if(msg != null)
            {
                Global.UIBatch.DrawString(Global.DialogFont, msg, _pos.ToVector2() + new Vector2(_size.X, _height / 2), Color.White);
            }

            Global.UIBatch.End();
        }
    }
}
