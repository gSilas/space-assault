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
        private float _standardSpeed;

        public Asteroid(Model model, Vector3 position, float angle, Vector3 direction, float movespeed)
        {
            Model = model;
            _angle = angle;
            Position = position;
            _direction = direction;
            _speed = movespeed;
            _standardSpeed = movespeed;
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
            for (int i = 0; i < Spheres.Length; i++)
            {
                    Spheres[i].Radius = Spheres[i].Radius * 0.8f;
            }
            Position += _direction * _speed;
        }
        public Vector3 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public void NegateDirection()
        {
            _direction *= -1;
        }
        public float MaxRadius()
        {
            float max = 0f;
            for (int i = 0; i < Spheres.Length; i++)
            {
                max = Math.Max(max, Spheres[i].Radius);
            }
            return max;
        }
    }
}
