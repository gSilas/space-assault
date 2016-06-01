using Microsoft.Xna.Framework;

namespace Space_Assault.Entities.Weapon
{
    public abstract class AAmmunition : AEntity
    {
       // private Vector3 _position;
        private Vector3 _direction;
        private float _travelspeed;

        public AAmmunition(Vector3 position, Vector3 direction, float travelspeed)
        {
            Position = position;
            _direction = direction;
            _travelspeed = travelspeed;

            _direction = direction;
            _direction.Normalize();
            _travelspeed = travelspeed;

        }

        public override void Initialize()
        {
            _direction.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            Position += _direction*_travelspeed;
        }

    }
}
