using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Utils;

namespace Space_Assault.Entities
{
    class Button
    {
        private Texture2D _texture;
        private Color _color;

        private Vector2 _position;
        private Rectangle _rect;
        private Vector2 _size;

        public bool Pressed;

        public Button(Texture2D newTexture, Vector2 position)
        {
            _texture = newTexture;
            _position = position;
            Pressed = false;
            _color = new Color(255, 255, 255);
            _size = new Vector2(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 8, Global.GraphicsManager.GraphicsDevice.Viewport.Height / 30);
            _rect = new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.X);
        }
        public void Update()
        { 
            Rectangle mouseRect = new Rectangle(MouseHandler.MouseState.X, MouseHandler.MouseState.Y, 1, 1);

            if (mouseRect.Intersects(_rect))
            {
                if (MouseHandler.MouseState.LeftButton == ButtonState.Pressed)
                {
                    _color.A = 255 / 2;
                    Pressed = true;
                }
                 
                else if(Pressed)
                {
                    _color.A = 255;
                    Pressed = false;
                }
            }
        }

        public void Draw()
        {
            Global.SpriteBatch.Draw(_texture, _rect, _color);
        }
    }
}
