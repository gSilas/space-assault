using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Entities.Weapon;
using System.Collections.Generic;
using System;
using Space_Assault.Utils;

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
        public int _health;
        private int _armor;
        private List<SoundEffect> _soundEffects;
        public SoundEffectInstance _droneMoveSound;
        private SoundEffectInstance _zuppSound;

        private AWeapon _gun;

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
            _health = 100;
            _armor = 100;
            _soundEffects = new List<SoundEffect>();
            _gun = new RailGun();
            _gun.Initialize();
        }

        public void Reset()
        {
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 1.0f;
            _moveSpeedForward = 1.0f;
            _moveSpeedBackward = -0.5f;
            _health = 100;
            _armor = 100;
            _moveSpeedModifier = 0;
            Position = _spawnPos;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/drone");

            // Sounds
            _soundEffects.Add(Global.ContentManager.Load<SoundEffect>("Sounds/droneMovement")); //only placeholder for now
            _soundEffects.Add(Global.ContentManager.Load<SoundEffect>("Sounds/zupp"));


            // Play that can be manipulated after the fact
            _droneMoveSound = _soundEffects[0].CreateInstance();
            _droneMoveSound.Volume = 0.1f;
            _droneMoveSound.IsLooped = true;

            _zuppSound = _soundEffects[1].CreateInstance();
            _zuppSound.Volume = 0.3f;

            _gun.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 direction = new Vector3(Global.GraphicsManager.PreferredBackBufferWidth / 2.0f, 0, Global.GraphicsManager.PreferredBackBufferHeight / 2.0f) - MouseHandler.Position;
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

            //shooting the gun
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _zuppSound.Play();
                _gun.Shoot(Position, RotationMatrix.Forward, 6f);
            }

            _gun.Update(gameTime);


            //TODO: health, armor update
        }

        public List<Bullet> GetBulletList()
        {
            return _gun.ListOfBullets;
        }

        public override void Draw()
        {
            _gun.Draw();

            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = RotationMatrix * Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
                    effect.View = Global.Camera.ViewMatrix;
                    effect.Projection = Global.Camera.ProjectionMatrix;
                }
                mesh.Draw();
            }

        }
    }
}
