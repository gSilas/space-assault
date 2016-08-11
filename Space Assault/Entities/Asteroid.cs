using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities
{
    class Asteroid : AEntity
    {
        private float _angle;
        private Vector3 _direction;
        private float _speed;
        public TimeSpan _originTime;

        public Asteroid(Vector3 position, float angle, Vector3 direction, float movespeed, TimeSpan originTime)
        {
            _angle = angle;
            Position = position;
            _direction = direction;
            _speed = movespeed;
            _originTime = originTime;
        }

        public void LoadContent(Model model)
        {
            Model = model;
            Spheres = Collider3D.UpdateBoundingSphere(this);
        }


        public override void LoadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize()
        {
            _direction.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            _angle += 0.005f;
            RotationMatrix = Matrix.CreateRotationY(_angle);
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Position += _direction * _speed;
        }
        
        public Asteroid Clone()
        {
            return new Asteroid(Position,_angle,_direction,_speed, _originTime);
        }
    }
}
