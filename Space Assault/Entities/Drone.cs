﻿using Microsoft.Xna.Framework;
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
            _turnSpeed = 10.0f;
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

            //forward movement
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (_moveSpeedModifier < _moveSpeed) _moveSpeedModifier += 0.04f;
                else _moveSpeedModifier = _moveSpeed;

                Position -= RotationMatrix.Forward * _moveSpeedModifier;
                movementWasTrue = true;

            }
            else if (movementWasTrue == true)
            {
                if (_moveSpeedModifier > 0.0f)
                {

                    Position -= RotationMatrix.Forward * _moveSpeedModifier;
                    _moveSpeedModifier -= 0.02f;
                }
                else movementWasTrue = false;
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
                if (vectorDirection > 0.01)
                {
                    //turn left
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(0.5f));
                }
                else if (vectorDirection < -0.01)
                {
                    //turn right
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(-0.5f));
                }
            }
        }
    }
}
