using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Utils;

namespace Space_Assault.UI
{
    class Button
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _rect;
        public string name;
        public bool Pressed;
        private Color _color;


        public Button(Texture2D newTexture, Vector2 position)
        {
            _texture = newTexture;
            _position = position;
            Pressed = false;
            _color = new Color(255, 255, 255);
            _rect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Bounds.Width, _texture.Height);
        }

        public Button(string name, Texture2D newTexture, Vector2 position)
        {
            this.name = name;
            _texture = newTexture;
            _position = position;
            Pressed = false;
            _color = new Color(255, 255, 255);
            _rect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Bounds.Width, _texture.Height);
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

                else if (Pressed)
                {
                    _color.A = 255;
                    Pressed = false;
                }
            }
        }

        public void setPosition(Vector2 newPosition)
        {
            _position = newPosition;
        }

        public void Draw()
        {
            Global.SpriteBatch.Draw(_texture, _rect, _color);    
        }
    }
}
