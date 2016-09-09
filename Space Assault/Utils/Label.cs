using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils
{
    class Label
    {
        private SpriteFont _font;
        private Vector2 _vector;
        private Color _color;
        private string _label;

        public Label(string font, string label, int x, int y, Color color)
        {
            _font = Global.ContentManager.Load<SpriteFont>("Fonts/" + font);
            _label = label;
            _vector = new Vector2(x, y);
            _color = color;
        }

        public Label(string label, int x, int y, Color color)
        {
            _font = Global.GameFont;
            _label = label;
            _vector = new Vector2(x, y);
            _color = color;
        }

        public void Update(){}

        public void Draw()
        {
            Global.SpriteBatch.DrawString(_font, _label, _vector, _color);
        }

        public void Draw(int variable)
        {
            Global.SpriteBatch.DrawString(_font, _label + variable, _vector, _color);
        }
    }
}