using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SpaceAssault.Utils
{
    class Background
    {
        private List<Tuple<Tile, int>> _tileList;
        private Random _rand;
        private BasicEffect _basicEffect;
        private Texture2D _tex;
        private RasterizerState _rast;

        public Background()
        {
            _basicEffect = new BasicEffect(Global.GraphicsManager.GraphicsDevice);
            _rand = new Random();
            _tileList = new List<Tuple<Tile,int>>();
            _tex = Global.ContentManager.Load<Texture2D>("Images/star");

            //50*50 Tiles with 1 Star / Tile
            //StarPosition inbetween x = 1000 y = 1000

            for (int x = 0; x < 50000; x += 1000)
            {
                for (int y = 0; y < 50000; y += 1000)
                {
                    Vector2 pos = new Vector2(x + _rand.Next(0, 1001), y + _rand.Next(0, 1001));                 
                    int height = _rand.Next(50, 90) * -100;
                    Tile t = new Tile(pos,_tex);
                    _tileList.Add(Tuple.Create<Tile,int>(t,height));
                }
            }
        }

        public void Draw()
        {
            Global.BackgroundBatch.Begin(SpriteSortMode.Immediate, null, null, DepthStencilState.Default, RasterizerState.CullNone, _basicEffect);
            foreach (var tileTuple in _tileList)
            {               
                _basicEffect.World = Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateWorld(new Vector3(-50000 / 2, tileTuple.Item2, -50000 / 2), Vector3.Forward, Vector3.Up) * Matrix.CreateScale(0.8f);
                _basicEffect.View = Global.Camera.ViewMatrix;
                _basicEffect.Projection = Global.Camera.ProjectionMatrix;
                _basicEffect.DiffuseColor = Color.LightYellow.ToVector3();
                _basicEffect.TextureEnabled = true;
                tileTuple.Item1.Draw(); 
            }
            Global.BackgroundBatch.End();
        }

        internal class Tile
        {
            private Vector2 _position;
            private Texture2D _texture;


            public Tile(Vector2 position, Texture2D tex)
            {

                _position = position;
                _texture = tex;
            }

            public void Draw()
            {
                Global.BackgroundBatch.Draw(_texture,_position , Color.White);         
            }
        }
    }

}
