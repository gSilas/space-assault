using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Space_Assault.Entities
{
    class Drone : AEntity
    {
        private float _turnSpeed;
        private float _moveSpeed;
        private Vector3 _rotation;


        public Drone(Vector3 position)
        {
            Position = position;
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 1.0f;
            _moveSpeed = 1.0f;
        }

        public override void LoadContent(ContentManager cm)
        {
            Model = cm.Load<Model>("Models/drone");
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) && Position.Y <= 160)
            {
                Position -= RotationMatrix.Forward*_moveSpeed;
            }
        }

        public void turn(Vector3 direction)
        {
            float vectorDirection = RotationMatrix.Forward.Z * direction.X - RotationMatrix.Forward.X * direction.Z;

            if (vectorDirection > 0)
            {
                Console.WriteLine("Left turn");
                RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(_turnSpeed));
            }
            else if (vectorDirection < 0)
            {
                Console.WriteLine("Right turn");
                RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(-_turnSpeed));
            }
        }
    }
}
