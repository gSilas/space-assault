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
        public bool IsShiny;
        public Asteroid(Model model, Vector3 position, float angle, Vector3 direction, float movespeed, bool shiny)
        {
            Model = model;
            _angle = angle;
            Position = position;
            _direction = direction;
            _speed = movespeed;
            _standardSpeed = movespeed;
            IsShiny = shiny;
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
        public void Reflect(Vector3 otherAstdirection, Vector3 otherAstCenter, out Vector3 otherDirection)
        {
            var normal = Spheres[0].Center - otherAstCenter;
            normal.Normalize();
            var tA = Vector3.Dot(_direction, normal);
            var tB = Vector3.Dot(otherAstdirection, normal);
            var optimizedP = (float)Math.Min((2.0 * (tA - tB)) / 2, 0);
            _direction.X = _direction.X - (optimizedP * normal.X);
            _direction.Z = _direction.Z - (optimizedP * normal.Z);
            otherDirection = new Vector3();
            otherDirection.X = otherAstdirection.Z + (optimizedP * normal.X);
            otherDirection.Z = otherAstdirection.X + (optimizedP * normal.Z);
            //_direction.Normalize();
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
