using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities
{
    class Asteroid : AEntity
    {
        private float _angle;
        private bool _up;
        Vector3 _direction;

        public Asteroid(Vector3 position, float angle, Vector3 direction, Vector3 movespeed)
        {
            _angle = angle;
            Position = position;
            //kolision später
            //worldMatrix = Matrix.CreateWorld(base.Position, Vector3.Forward, Vector3.Up) * Matrix.CreateScale(0.05f);

            _direction = direction;
            _direction.Normalize();
            _direction *= movespeed;
        }

        public override void Initialize()
        {
            _up = true;
        }

        public override void LoadContent(ContentManager cm)
        {
            Model = cm.Load<Model>("Models/asteroid");
        }

        public override void Update(GameTime gameTime)
        {
            _angle += 0.005f;
            if (Position.Y < 1 && _up)
                Position += new Vector3(0, 0.002f, 0);
            else if (Position.Y < 0)
                _up = true;
            else
            {
                Position -= new Vector3(0, 0.002f, 0);
                _up = false;
            }
            RotationMatrix = Matrix.CreateRotationY(_angle) * Matrix.CreateTranslation(Position);

            Position += _direction;
        }
    }
}
