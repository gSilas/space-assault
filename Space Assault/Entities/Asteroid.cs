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


        public Asteroid(Model model, Vector3 position, float angle, Vector3 direction, float movespeed, TimeSpan originTime)
        {
            Model = model;
            _angle = angle;
            Position = position;
            _direction = direction;
            _speed = movespeed;
            _originTime = originTime;
        }

        public bool IsDead { get; set; } = false;

        public override void LoadContent()
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);
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
            return new Asteroid(Model, Position, _angle,_direction,_speed, _originTime);
        }
    }
}
