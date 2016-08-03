using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Entities.Weapon;
using System.Collections.Generic;
using System;
using SpaceAssault.Utils;
using SpaceAssault.ScreenManagers;

/// <summary>
///  Movement, Schießen, Health, Sterben, neu Spawnen.
/// </summary>
namespace SpaceAssault.Entities
{
    class Drone : AEntity
    {
        private Vector3 _spawnPos;
        private float _turnSpeed;
        private float _moveSpeedForward;
        private float _moveSpeedBackward;
        private float _moveSpeedLeft;
        private float _moveSpeedRight;
        private float _moveSpeedModifier;
        private float _moveSpeedModifierSideways;
        public int _health;
        private int _armor;

        private bool _isNotDead;

        private AWeapon _gun;

        //used for scaling all speed values beside turnSpeed;
        private float _speedScaling = 2f;

        public Drone(Vector3 position)
        {
            _spawnPos = position;
            Position = position;
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 10.0f;
            _moveSpeedForward = 1.0f * _speedScaling;
            _moveSpeedBackward = -0.5f * _speedScaling;
            _moveSpeedLeft = 0.5f * _speedScaling;
            _moveSpeedRight = -0.5f * _speedScaling;
            _health = 100;
            _armor = 100;
            _isNotDead = true;
            _gun = new RailGun();
            _gun.Initialize();
        }

        public void Reset()
        {
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 10.0f;
            _moveSpeedForward = 1.0f * _speedScaling;
            _moveSpeedBackward = -0.5f * _speedScaling;
            _moveSpeedLeft = 0.5f * _speedScaling;
            _moveSpeedRight = -0.5f * _speedScaling;
            _health = 100;
            _armor = 100;
            _isNotDead = true;
            _moveSpeedModifier = 0;
            _moveSpeedModifierSideways = 0;
            Position = _spawnPos;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/drone");
            _gun.LoadContent();
            //RotationMatrix = Matrix.CreateRotationX();
        }

        public override void Update(GameTime gameTime)
        {
            _gun.Update(gameTime);
            if (IsNotDead)
            {
                this.HandleInput();
            }
            if (_health <= 0) IsNotDead = false;
            //TODO: health, armor update
        }

        public bool IsNotDead
        {
            get { return _isNotDead; }
            protected set { _isNotDead = value; }
        }

        private void HandleInput()
        {
            /// <summary>
            /// handling rotation of the drone
            /// </summary>
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

            /// <summary>
            /// foward & backward movement
            /// </summary>
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //forward
                if (_moveSpeedModifier < _moveSpeedForward) _moveSpeedModifier += 0.04f;
                else _moveSpeedModifier = _moveSpeedForward;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //backward
                if (_moveSpeedModifier > _moveSpeedBackward) _moveSpeedModifier -= 0.04f;
                else _moveSpeedModifier = _moveSpeedBackward;
            }
            else if (_moveSpeedModifier > 0.02f)
            {
                //forward slowing down
                _moveSpeedModifier -= 0.02f;
            }
            else if (_moveSpeedModifier < -0.02f)
            {
                //backward slowing down
                _moveSpeedModifier += 0.02f;
            }
            else
            {
                _moveSpeedModifier = 0.0f;
            }
            Position -= RotationMatrix.Forward * _moveSpeedModifier;

            /// <summary>
            /// left & right movement
            /// </summary>
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //left
                if (_moveSpeedModifierSideways < _moveSpeedLeft) _moveSpeedModifierSideways += 0.04f;
                else _moveSpeedModifierSideways = _moveSpeedLeft;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                //right
                if (_moveSpeedModifierSideways > _moveSpeedRight) _moveSpeedModifierSideways -= 0.04f;
                else _moveSpeedModifierSideways = _moveSpeedRight;
            }
            else if (_moveSpeedModifierSideways > 0.02f)
            {
                //left slowing down
                _moveSpeedModifierSideways -= 0.02f;
            }
            else if (_moveSpeedModifierSideways < -0.02f)
            {
                //right slowing down
                _moveSpeedModifierSideways += 0.02f;
            }
            else
            {
                _moveSpeedModifierSideways = 0.0f;
            }
            Position -= RotationMatrix.Left * _moveSpeedModifierSideways;


            /// <summary>
            /// shooting the gun
            /// </summary>
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _gun.Shoot(Position + RotationMatrix.Left * 4.5f, RotationMatrix, 6f);
                _gun.Shoot(Position - RotationMatrix.Left * 4.5f, RotationMatrix, 6f);
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                _gun.Shoot(Position - RotationMatrix.Left * 4.5f, RotationMatrix, 6f);
            }
        }

        public List<Bullet> GetBulletList()
        {
            return _gun.ListOfBullets;
        }

        public override void Draw()
        {
            if (IsNotDead)
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
}
