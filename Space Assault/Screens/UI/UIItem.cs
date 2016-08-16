using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Screens.UI
{
    class UIItem
    {
        private Texture2D _tex;
        private Point _size;

        public void LoadContent(string texpath)
        {
            _tex = Global.ContentManager.Load<Texture2D>(texpath);
            _size = new Point(_tex.Width / 4, _tex.Height / 4);
        }

        public void Draw(Point pos, int count, Color col)
        {
            Global.UIBatch.Begin();

            for (int x = 0; x < count; x++)
            {
                Global.UIBatch.Draw(_tex, new Rectangle(new Point(pos.X + (_size.X * x), pos.Y), _size), col);
            }

            Global.UIBatch.End();
        }

    }
}
