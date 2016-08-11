using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SpaceAssault.Utils
{
    class Background
    {
        private List<Tile> _tileList;
        private Random _rand;
        private BasicEffect _basicEffect;
        private Texture2D _tex;
        private RasterizerState _rast;
        private bool _setworld;

        public Background()
        {
            _basicEffect = new BasicEffect(Global.GraphicsManager.GraphicsDevice);
            _rand = new Random();
            _tileList = new List<Tile>();
            _tex = Global.ContentManager.Load<Texture2D>("Images/star");
            _setworld = false;
            //50*50 Tiles with 1 Star / Tile
            //StarPosition inbetween x = 1000 y = 1000

            for (int x = 0; x < 15000; x += 500)
            {
                for (int y = 0; y < 15000; y += 500)
                {
                    Vector2 pos = new Vector2(x + _rand.Next(0, 501), y + _rand.Next(0, 501));                 
                    float scale = (float)_rand.NextDouble();
                    Tile t = new Tile(pos,_tex, scale);
                    _tileList.Add(t);
                }
            }
        }

        public void Draw(float angle,Vector3 pos)
        {
            if (!_setworld)
            {
                _basicEffect.World = Matrix.CreateRotationX(MathHelper.ToRadians(angle)) * Matrix.CreateWorld(pos, Vector3.Forward, Vector3.Up);
                _basicEffect.View = Global.Camera.ViewMatrix;
                _basicEffect.Projection = Global.Camera.ProjectionMatrix;
                _basicEffect.DiffuseColor = Color.LightYellow.ToVector3();
                _basicEffect.TextureEnabled = true;
                _setworld = true;
            }
            Global.BackgroundBatch.Begin(SpriteSortMode.Deferred, null, null, DepthStencilState.Default, RasterizerState.CullNone, _basicEffect);
            foreach (var tile in _tileList)
            {                             
                tile.Draw(); 
            }
            Global.BackgroundBatch.End();
        }

        internal class Tile
        {
            private Texture2D _texture;
            private Rectangle _rect;

            public Tile(Vector2 position, Texture2D tex,float scale)
            {
                
                _texture = tex;
                _rect = tex.Bounds;
                _rect.Location = position.ToPoint();
                _rect.Height = (int)(_rect.Height * scale);
                _rect.Width = (int)(_rect.Width * scale);
            }

            public void Draw()
            {
                Global.BackgroundBatch.Draw(_texture,_rect, Color.White);         
            }
        }
    }

}
