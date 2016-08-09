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

        public Background()
        {
            _basicEffect = new BasicEffect(Global.GraphicsManager.GraphicsDevice);
            _rand = new Random();
            _tileList = new List<Tile>();
            _tex = Global.ContentManager.Load<Texture2D>("Images/star");
            for (int x = 0; x < 50000; x += 500)
            {
                for (int y = 0; y < 50000; y += 500)
                {
                    Vector2 pos = new Vector2(x, y);                 
                    int xo = _rand.Next(0, 501);
                    int yo = _rand.Next(0, 501);
                    Tile t = new Tile(pos,xo,yo,_tex);
                    _tileList.Add(t);
                }
            }
        }

        public void Draw()
        {
            _basicEffect.World = Matrix.CreateRotationX(MathHelper.ToRadians(90))*Matrix.CreateWorld(new Vector3(-50000/2,-2500,-50000/2), Vector3.Forward, Vector3.Up)*Matrix.CreateScale(0.1f);
            _basicEffect.View = Global.Camera.ViewMatrix;
            _basicEffect.Projection = Global.Camera.ProjectionMatrix;
            _basicEffect.DiffuseColor = Color.Gold.ToVector3();
            _basicEffect.TextureEnabled = true;
            _basicEffect.DirectionalLight0.Enabled = true;
            _basicEffect.DirectionalLight0.SpecularColor = Color.Gold.ToVector3();
            _basicEffect.DirectionalLight0.DiffuseColor = Color.Gold.ToVector3();
            _basicEffect.DirectionalLight0.Direction = Vector3.Up;


            Global.BackgroundBatch.Begin(0, null, null, DepthStencilState.Default, RasterizerState.CullNone, _basicEffect);
            foreach (var tile in _tileList)
            {
                tile.Draw();
            }
            Global.BackgroundBatch.End();
        }

        internal class Tile
        {
            private Vector2 _position;
            private Texture2D _star;
            private Vector2 _offset;


            public Tile(Vector2 position,int x , int y, Texture2D tex)
            {

                _position = position;
                _star = tex;
                _offset = new Vector2(x,y);
            }

            public void Draw()
            {
                Global.BackgroundBatch.Draw(_star,_position + _offset, Color.White);         
            }
        }
    }

}
