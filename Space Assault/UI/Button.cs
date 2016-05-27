using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Assault.Entities
{
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rect;

        Color color = new Color(255, 255, 255);

        public Vector2 size;

        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;
            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);
        }

        bool down;
        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rect = new Rectangle((int)position.X, (int)position.Y,(int)size.X, (int)size.X);

            Rectangle mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRect.Intersects(rect))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3; else color.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else if(color.A < 255)
            {
                color.A += 3;
                isClicked = false;
            }
        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, color);
        }
    }
}
