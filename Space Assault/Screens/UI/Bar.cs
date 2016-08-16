using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Screens.UI
{
    class Bar
    {
        private Texture2D _tex;
        private Rectangle _rect;
        private Color _col;
        private int _segmentCount;
        private int _max;
        private Point _size;

        public Bar(Rectangle targetRec, Color color, int maxValue)
        {
            _rect = targetRec;
            _col = color;
            _max = maxValue;
        }
        public void LoadContent()
        {
            _tex = Global.ContentManager.Load<Texture2D>("Images/UI/BarSegment");
            _size = new Point(_tex.Width / 2, _tex.Height / 2);
            _segmentCount = _max / (_rect.Size.X / _size.X);
        }

        public void Draw(int currValue, int maxValue)
        {
            var location = _rect.Location;
            var count = currValue / _segmentCount;

            if (_max != maxValue){
                _segmentCount = (maxValue / (_rect.Size.X / _size.X));
                _max = maxValue;
            }

            Global.UIBatch.Begin();

            for (int x = 0; x < count; x++)
            {
                Global.UIBatch.Draw(_tex, new Rectangle(new Point(_rect.Location.X+ (_size.X* x),_rect.Location.Y),_size), _col);
            }

            Global.UIBatch.End();
        }
    }
}
