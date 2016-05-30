using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

/// <summary>
///  Movement, Schießen, Health, Sterben, neu Spawnen.
/// </summary>
namespace Space_Assault.Entities
{
    class Drone : AEntity
    {
        private Vector3 _spawnPos;
        private float _turnSpeed;
        private float _moveSpeedForward;
        private float _moveSpeedBackward;
        private float _moveSpeedModifier;
        private float health;
        private float armor;
        private SoundEffectInstance _droneMoveSound;
        private List<SoundEffect> soundEffects;


        public Drone(Vector3 position)
        {
            _spawnPos = position;
            Position = position;
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 10.0f;
            _moveSpeedForward = 1.0f;
            _moveSpeedBackward = -0.5f;
            health = 100;
            armor = 100;
            soundEffects = new List<SoundEffect>();
        }

        public void Reset()
        {
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 1.0f;
            _moveSpeedForward = 1.0f;
            _moveSpeedBackward = -0.5f;
            health = 100;
            armor = 100;
            _moveSpeedModifier = 0;
            Position = _spawnPos;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/drone");
            soundEffects.Add(Global.ContentManager.Load<SoundEffect>("Sounds/droneMovement")); //only placeholder for now

            // Play that can be manipulated after the fact
            _droneMoveSound = soundEffects[0].CreateInstance();
            _droneMoveSound.Volume = 0.1f;
            _droneMoveSound.IsLooped = true;
        }

        public override void Update(GameTime gameTime)
        {

            //movement
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //forward
                _droneMoveSound.Play();
                if (_moveSpeedModifier < _moveSpeedForward) _moveSpeedModifier += 0.04f;
                else _moveSpeedModifier = _moveSpeedForward;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //backward
                _droneMoveSound.Play();
                if (_moveSpeedModifier > _moveSpeedBackward) _moveSpeedModifier -= 0.04f;
                else _moveSpeedModifier = _moveSpeedBackward;
            }
            else if (_moveSpeedModifier > 0.02f)
            {
                //forward slowing down
                _droneMoveSound.Stop();
                _moveSpeedModifier -= 0.02f;
            }
            else if (_moveSpeedModifier < -0.02f)
            {
                //backward slowing down
                _droneMoveSound.Stop();
                _moveSpeedModifier += 0.02f;
            }
            else
            {
                _moveSpeedModifier = 0.0f;
            }

            Position -= RotationMatrix.Forward * _moveSpeedModifier;

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
