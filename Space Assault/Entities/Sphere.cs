using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Entities
{
    class Sphere : AEntity
    {
        private float _angle;


        public Sphere(Vector3 position, float angle)
        {
            _angle = angle;
            Position = position;
            Scale = 0.5f;
        }


        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/sphere");
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
