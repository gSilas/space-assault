
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities
{
    class Drone : AEntity
    {
        private float _angle;
        private bool _up;
        private Vector3 _direction;
        private Vector3 _speed;

        public Drone(Vector3 position, float angle, Vector3 direction, Vector3 movespeed)
        {
            _angle = angle;
            Position = position;
            //kolision später
            //worldMatrix = Matrix.CreateWorld(base.Position, Vector3.Forward, Vector3.Up) * Matrix.CreateScale(0.05f);

            _direction = direction;
            _direction.Normalize();
            _speed = movespeed;
        }

        public override void Initialize()
        {
            _up = true;
        }

        public void Move(Vector3 direction)
        {
            _direction = direction;
            if (!(direction == Vector3.Zero)) _direction.Normalize();
            _direction *= _speed;
        }

        public override void LoadContent(ContentManager cm)
        {
            Model = cm.Load<Model>("Models/asteroid");
        }

        public override void Update(GameTime gameTime)
        {
            RotationMatrix = Matrix.Identity;
            Position += _direction;
        }
    }
}
