﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;
using SpaceAssault.ScreenManagers;

/// <summary>
///  Movement, Schießen, Health, Sterben, neu Spawnen.
/// </summary>
namespace SpaceAssault.Entities
{
    class Drone : AEntity
    {
        private float _turnSpeed;
        private float _moveSpeedForward;
        private float _moveSpeedBackward;
        private float _moveSpeedLeft;
        private float _moveSpeedRight;
        private float _moveSpeedModifier;
        private float _moveSpeedModifierSideways;
        private float _speedScaling = 2f;
        private int _shieldpast;
        private TimeSpan _shieldrefreshdelay;
        private bool _wasDamaged = false;
        private bool _alternatingGunLogic = false;
        private bool _isNotDead;
        private Matrix _rotationMatrixLaser = Matrix.Identity;
        private InputState _input;

        public int maxHealth;
        public int health;
        public int armor;
        public int makeDmg;
        public int maxRange;
        public int maxShield;
        public int shield;
        public Weapon gunPrimary;
        public Weapon gunSecondary;
        public Vector3 curVelocity;

        public Drone(Vector3 position, int makeDmg, int maxHealth, int armor, int maxShield)
        {
            _input = new InputState();
            _trail = new Trail(new DroneTrailSettings());
            gunPrimary = new Weapon(200);
            gunSecondary = new Weapon(5000);
            maxRange = Global.MapRingRadius + 200;

            this.makeDmg = makeDmg;
            this.maxHealth = maxHealth;
            this.armor = armor;
            this.maxShield = maxShield;
            _shieldpast = this.maxShield;
            shield = maxShield;
            health = maxHealth;
            Position = position;
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 6.0f;
            _moveSpeedForward = 1.0f * _speedScaling;
            _moveSpeedBackward = -1.0f * _speedScaling;
            _moveSpeedLeft = 1.0f * _speedScaling;
            _moveSpeedRight = -1.0f * _speedScaling;

            _isNotDead = true;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/drone");
            Spheres = Collider3D.UpdateBoundingSphere(this);
            gunPrimary.LoadContent();
            gunSecondary.LoadContent();
            //RotationMatrix = Matrix.CreateRotationX();
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update();
            gunPrimary.Update(gameTime);
            gunSecondary.Update(gameTime);

            Spheres = Collider3D.UpdateBoundingSphere(this);

            if (gameTime.TotalGameTime > (_shieldrefreshdelay.Add(TimeSpan.FromSeconds(3))))
            {

                if (shield == _shieldpast)
                    _wasDamaged = false;
                _shieldpast = shield;
                _shieldrefreshdelay = gameTime.TotalGameTime;
            }
            if (_wasDamaged == false && shield < maxShield)
                shield += 1;

            if (health <= 0 || Vector2.Distance(new Vector2(Position.X, Position.Z), Vector2.Zero) > maxRange) IsNotDead = false;
        }

        public bool IsNotDead
        {
            get { return _isNotDead; }
            protected set { _isNotDead = value; }
        }

        public void getHit(int howMuch)
        {
            _wasDamaged = true;
            if (shield >= 0)
                shield -= howMuch;
            else
            {
                health -= (howMuch - howMuch * (armor / 20));
            }
        }
        public void HandleInput(GameTime gameTime, Bullet.BulletType curBullet, ref List<Bullet> bulletList)
        {
            Vector3 _oldPosition = Position;

            // handling rotation of the drone

            // Laser - Mouse control
            Vector3 screenDirection = this.Position - _input.getMouseInWorldPos();
            screenDirection.Normalize();
            float worldDirection;
            for (int i = 0; i < (_turnSpeed / 0.5f); i++)
            {
                worldDirection = _rotationMatrixLaser.Forward.Z * screenDirection.X - _rotationMatrixLaser.Forward.X * screenDirection.Z;
                if (Math.Abs(worldDirection) > 0.01)
                    _rotationMatrixLaser *= Matrix.CreateRotationY(MathHelper.ToRadians(Math.Sign(worldDirection) * 0.5f));
            }

            // Drone - Mouse control
            Vector3 direction = new Vector3(Global.GraphicsManager.PreferredBackBufferWidth / 2.0f, 0, Global.GraphicsManager.PreferredBackBufferHeight / 2.0f) - _input.MousePosition;
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


            // foward & backward movement
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //forward
                if (_moveSpeedModifier < _moveSpeedForward) _moveSpeedModifier += 0.04f;
                else _moveSpeedModifier = _moveSpeedForward;
                //Position -= new Vector3(0, 0, 1) * _moveSpeedModifier;

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //backward
                if (_moveSpeedModifier > _moveSpeedBackward) _moveSpeedModifier -= 0.04f;
                else _moveSpeedModifier = _moveSpeedBackward;
                //Position -= new Vector3(0, 0, 1) * _moveSpeedModifier;
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
            Position -= new Vector3(0, 0, 1) * _moveSpeedModifier;

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
            Position -= new Vector3(1, 0, 0) * _moveSpeedModifierSideways;

            // shooting the gun
            if (_input.IsLeftMouseButtonPressed())
            {
                if (_alternatingGunLogic && gunPrimary.Shoot(gameTime, curBullet, Global.DroneDmg, Position - _rotationMatrixLaser.Left * 3.6f - _rotationMatrixLaser.Forward * 11.0f, _rotationMatrixLaser.Forward, ref bulletList))
                {
                    _alternatingGunLogic = false;
                }
                else if (gunPrimary.Shoot(gameTime, curBullet, Global.DroneDmg, Position - _rotationMatrixLaser.Right * 3.6f - _rotationMatrixLaser.Forward * 11.0f, _rotationMatrixLaser.Forward, ref bulletList))
                {
                    _alternatingGunLogic = true;
                }
            }

            if (_input.IsRightMouseButtonPressed())
            {
                if (Global.NumberOfRockets > 0)
                {
                    if (gunSecondary.Shoot(gameTime, Bullet.BulletType.BigJoe, 100, Position - _rotationMatrixLaser.Forward * 11.0f, _rotationMatrixLaser.Forward, ref bulletList))
                        Global.NumberOfRockets -= 1;
                }
            }

            curVelocity = Position - _oldPosition;
            droneMovementBoundaries();
        }

        public void droneMovementBoundaries()
        {
            //cant fly into station sphere
            Vector3 posNormalized = Position;
            posNormalized.Normalize();
            while (Position.Length() < 120)
            {
                Position += posNormalized;
            }
        }
    }
}
