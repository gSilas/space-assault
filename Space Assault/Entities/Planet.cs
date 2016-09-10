using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceAssault.Entities
{
    class Planet : AEntity
    {
        private float _angle;
        public Planet(Vector3 position, float angle)
        {
           _angle = angle;
           Position = position;
           Scale = 0.5f;
        }

        public override void LoadContent()
        {
           Model = Global.ContentManager.Load<Model>("Models/planet");
        }

        public override void Update(GameTime gameTime)
        {
            _angle += 0.0025f;
            RotationMatrix = Matrix.CreateRotationY(_angle);
        }
     }
}