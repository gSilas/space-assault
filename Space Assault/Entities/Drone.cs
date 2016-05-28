using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

/// <summary>
///  Movement, Schießen, Health, Sterben, neu Spawnen.
/// </summary>
namespace Space_Assault.Entities
{
    class Drone : AEntity
    {
        private Vector3 _spawnPos;
        private float _turnSpeed;
        private float _moveSpeed;
        private float _moveSpeedModifier;
        private float health;
        private float armor;
        private bool movementWasTrue;


        public Drone(Vector3 position)
        {
            _spawnPos = position;
            Position = position;
        }

        public override void Initialize()
        {
            movementWasTrue = false;
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 50.0f;
            _moveSpeed = 1.0f;
            health = 100;
            armor = 100;
        }

        public void Reset()
        {
            movementWasTrue = false;
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 1.0f;
            _moveSpeed = 1.0f;
            health = 100;
            armor = 100;
            _moveSpeedModifier = 0;
            Position = _spawnPos;
        }

        public override void LoadContent(ContentManager cm)
        {
            Model = cm.Load<Model>("Models/drone");
        }

        public override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position -= RotationMatrix.Forward * _moveSpeed;
                movementWasTrue = true;
            }
            else if (movementWasTrue == true)
            {
                if (_moveSpeedModifier < _moveSpeed)
                {
                    Position -= RotationMatrix.Forward * (_moveSpeed - _moveSpeedModifier);
                    _moveSpeedModifier += 0.02f;
                }
                else
                {
                    movementWasTrue = false;
                    _moveSpeedModifier = 0;
                }
            }

            //TODO: health, armor update
        }

        public void turn(Vector3 direction)
        {
            direction.Normalize();
            float vectorDirection;
            for (float i = 0.5f; i < _turnSpeed; i++)
            {
                vectorDirection = RotationMatrix.Forward.Z * direction.X - RotationMatrix.Forward.X * direction.Z;
                Console.WriteLine(vectorDirection);
                if (vectorDirection > 0.01)
                {
                    //turn left
                    Console.Write("   ~~ left");
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(0.5f));
                }
                else if (vectorDirection < -0.01)
                {
                    Console.Write("   ~~ right");
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(-0.5f));
                }
            }
        }
    }
}
