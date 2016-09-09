
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Screens.UI
{
    class Dialog
    {
        private Texture2D _frame;
        private Texture2D _edge;
        private Texture2D _space;
        private Point _pos;
        private int _x;
        private int _y;
        private int _height;
        private int _width;
        private Point _size;
        private int _scale;
        private bool _shadow;
        private bool _transparent;
        private Color _uiColor;

        public Dialog(int x, int y, int height, int width,int scale,bool shadow, bool transparent)
        {
            _pos = new Point(x,y);
            _x = x;
            _y = y;
            _height = height;
            _width = width;
            _scale = scale;
            _shadow = shadow;
            _transparent = transparent;
            _uiColor = Global.UIColor;
        }

        public Point position
        {
            get { return _pos; }
            protected set { _pos = value;  }
        }

        public Point size
        {
            get { return _size; }
            protected set { _size = value; }
        }

        public void LoadContent()
        {
            _space = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_space");
            _edge = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_edge");
            _frame = Global.ContentManager.Load<Texture2D>("Images/UI/dialog_frame");
            _size = new Point(_frame.Width / _scale, _frame.Height / _scale);
        }

        public void Draw(string msg)
        {
            Global.UIBatch.Begin();
            _uiColor = Global.UIColor;
            if (_transparent)
                _uiColor.A = 100;
            if (_shadow)
            {
                for (int x = _x - 10; x < _width + _x; x += _size.X)
                {
                    for (int y = _y - 10; y < _height + _y; y += _size.X)
                    {
                        Global.UIBatch.Draw(_space, new Rectangle(new Point(x, y), _size), null, Color.DarkGray, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                    }
                }

            }

            Global.UIBatch.Draw(_edge, new Rectangle(_pos, _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x + _width, _y),_size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x, _y + _height), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x + _width +_size.X, _y + _height +_size.X), _size), null, _uiColor, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0.0f);

            for (int x = _x + _size.X; x < _width + _x; x += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, _y), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, _y + _height), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }
           
            
            for (int y = _y +_size.X; y < _height +_y; y += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(_width + _x, y +_size.X ), _size), null, _uiColor, MathHelper.ToRadians(-90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(_x + _size.X, y), _size), null, _uiColor, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }

            
            for (int x = _x; x < _width + _x - _size.X; x += _size.X)
            {
                for (int y = _y; y < _height +_y - _size.X; y += _size.X)
                {
                    Global.UIBatch.Draw(_space, new Rectangle(new Point(x + _size.X, y + _size.X), _size), null, _uiColor, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                }
            }

            if(msg != null)
            {
                Global.UIBatch.DrawString(Global.Font, msg, _pos.ToVector2() + new Vector2(_size.X, _size.Y), Color.White);
            }

            Global.UIBatch.End();
        }

        public void Draw(string msg, Color msgColor)
        {
            Global.UIBatch.Begin();
            _uiColor = Global.UIColor;
            if (_transparent)
                _uiColor.A = 100;
            if (_shadow)
            {
                for (int x = _x - 10; x < _width + _x; x += _size.X)
                {
                    for (int y = _y - 10; y < _height + _y; y += _size.X)
                    {
                        Global.UIBatch.Draw(_space, new Rectangle(new Point(x, y), _size), null, Color.DarkGray, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                    }
                }

            }

            Global.UIBatch.Draw(_edge, new Rectangle(_pos, _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x + _width, _y), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x, _y + _height), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(_x + _width + _size.X, _y + _height + _size.X), _size), null, _uiColor, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0.0f);

            for (int x = _x + _size.X; x < _width + _x; x += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, _y), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, _y + _height), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }


            for (int y = _y + _size.X; y < _height + _y; y += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(_width + _x, y + _size.X), _size), null, _uiColor, MathHelper.ToRadians(-90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(_x + _size.X, y), _size), null, _uiColor, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }


            for (int x = _x; x < _width + _x - _size.X; x += _size.X)
            {
                for (int y = _y; y < _height + _y - _size.X; y += _size.X)
                {
                    Global.UIBatch.Draw(_space, new Rectangle(new Point(x + _size.X, y + _size.X), _size), null, _uiColor, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                }
            }

            if (msg != null)
            {
                Global.UIBatch.DrawString(Global.Font, msg, _pos.ToVector2() + new Vector2(_size.X, _size.Y), msgColor);
            }

            Global.UIBatch.End();
        }

        public void Draw(Point pos, string msg)
        {
            Global.UIBatch.Begin();
            _uiColor = Global.UIColor;
            if (_transparent)
                _uiColor.A = 100;
            if (_shadow)
            {
                for (int x = pos.X - 10; x < _width + pos.X; x += _size.X)
                {
                    for (int y = pos.Y - 10; y < _height + pos.Y; y += _size.X)
                    {
                        Global.UIBatch.Draw(_space, new Rectangle(new Point(x, y), _size), null, Color.DarkGray, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                    }
                }

            }

            Global.UIBatch.Draw(_edge, new Rectangle(pos, _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(pos.X + _width, pos.Y), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(pos.X, pos.Y + _height), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(pos.X + _width + _size.X, pos.Y + _height + _size.X), _size), null, _uiColor, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0.0f);

            for (int x = pos.X + _size.X; x < _width + pos.X; x += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, pos.Y), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, pos.Y + _height), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }


            for (int y = pos.Y + _size.X; y < _height + pos.Y; y += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(_width + pos.X, y + _size.X), _size), null, _uiColor, MathHelper.ToRadians(-90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(pos.X + _size.X, y), _size), null, _uiColor, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }


            for (int x = pos.X; x < _width + pos.X - _size.X; x += _size.X)
            {
                for (int y = pos.Y; y < _height + pos.Y - _size.X; y += _size.X)
                {
                    Global.UIBatch.Draw(_space, new Rectangle(new Point(x + _size.X, y + _size.X), _size), null, _uiColor, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                }
            }

            if (msg != null)
            {
                Global.UIBatch.DrawString(Global.Font, msg, pos.ToVector2() + new Vector2(_size.X, _size.Y), Color.White);
            }

            Global.UIBatch.End();
        }

        public void Draw(Point pos, string msg, Color msgColor)
        {
            Global.UIBatch.Begin();
            _uiColor = Global.UIColor;
            if (_transparent)
                _uiColor.A = 100;
            if (_shadow)
            {
                for (int x = pos.X - 10; x < _width + pos.X; x += _size.X)
                {
                    for (int y = pos.Y - 10; y < _height + pos.Y; y += _size.X)
                    {
                        Global.UIBatch.Draw(_space, new Rectangle(new Point(x, y), _size), null, Color.DarkGray, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                    }
                }

            }

            Global.UIBatch.Draw(_edge, new Rectangle(pos, _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(pos.X + _width, pos.Y), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(pos.X, pos.Y + _height), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            Global.UIBatch.Draw(_edge, new Rectangle(new Point(pos.X + _width + _size.X, pos.Y + _height + _size.X), _size), null, _uiColor, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0.0f);

            for (int x = pos.X + _size.X; x < _width + pos.X; x += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, pos.Y), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(x, pos.Y + _height), _size), null, _uiColor, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }


            for (int y = pos.Y + _size.X; y < _height + pos.Y; y += _size.X)
            {
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(_width + pos.X, y + _size.X), _size), null, _uiColor, MathHelper.ToRadians(-90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
                Global.UIBatch.Draw(_frame, new Rectangle(new Point(pos.X + _size.X, y), _size), null, _uiColor, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
            }


            for (int x = pos.X; x < _width + pos.X - _size.X; x += _size.X)
            {
                for (int y = pos.Y; y < _height + pos.Y - _size.X; y += _size.X)
                {
                    Global.UIBatch.Draw(_space, new Rectangle(new Point(x + _size.X, y + _size.X), _size), null, _uiColor, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0.0f);
                }
            }

            if (msg != null)
            {
                Global.UIBatch.DrawString(Global.Font, msg, pos.ToVector2() + new Vector2(_size.X, _size.Y), msgColor);
            }

            Global.UIBatch.End();
        }
    }
}
