using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities.Weapon
{
    public class AAmmunition : AEntity
    {
        private Vector3 _direction;
        private float _travelspeed;

        public AAmmunition(Vector3 position, Vector3 direction, float travelspeed)
        {
            Position = position;
            _direction = direction;
            _direction.Normalize();
            _travelspeed = travelspeed;
        }

        public override void Initialize()
        {
            _direction.Normalize();
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }

        public void LoadContent(Model model)
        {
            Model = model;
        }

        public override void Update(GameTime gameTime)
        {
            Position += _direction*_travelspeed;
        }

    }
}
