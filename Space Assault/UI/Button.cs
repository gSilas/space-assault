using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Utils;

namespace Space_Assault.Entities
{
    class Button
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _rect;

        private Color _color = new Color(255, 255, 255);

        public Vector2 size;

        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            _texture = newTexture;
            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);
        }

        bool down;
        public bool isClicked;
        public void Update()
        {
            _rect = new Rectangle((int)_position.X, (int)_position.Y,(int)size.X, (int)size.X);

            Rectangle mouseRect = new Rectangle(MouseHandler.MouseState.X, MouseHandler.MouseState.Y, 1, 1);

            if (mouseRect.Intersects(_rect))
            {
                if (_color.A == 255) down = false;
                if (_color.A == 0) down = true;
                if (down) _color.A += 3; else _color.A -= 3;
                if (MouseHandler.MouseState.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else if(_color.A < 255)
            {
                _color.A += 3;
                isClicked = false;
            }
        }

        public void setPosition(Vector2 newPosition)
        {
            _position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rect, _color);
        }
    }
}
