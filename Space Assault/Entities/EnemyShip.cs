using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities.Weapon;
using System.Collections.Generic;

namespace SpaceAssault.Entities
{
    class EnemyShip : AEntity
    {
        private Vector3 _spawnPos;
        private float _moveSpeedForward;
        //private float _moveSpeedBackward;
        private float _turnSpeed;
        //private float _moveSpeedModifier;
        //private Vector3 _direction;
        //private float _moveSpeedModifier;
 
        public int _health;
        private int _armor;
        private bool _isNotDead;

        private AWeapon _gun;

        public EnemyShip(Vector3 position)
        {
            _spawnPos = position;
            Position = position;
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;
            
            _moveSpeedForward = 0.5f;
            _turnSpeed = 5.0f;

            //_moveSpeedBackward = -0.5f;

            _health = 100;
            _armor = 100;
            _gun = new RailGun();
            _gun.Initialize();
        }

        public void Reset()
        {
            RotationMatrix = Matrix.Identity;
            _health = 100;
            _armor = 100;
            Position = _spawnPos;
        }

        public bool IsNotDead
        {
            get { return _isNotDead; }
            protected set { _isNotDead = value; }
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemyship2");
            _gun.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            _gun.Update(gameTime);
            if (_health <= 0) this.Reset();
            //Position += _direction * _moveSpeedForward;
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
        public void FlyVector(Vector3 direction)
        {
            //_direction=direction;
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
            Position -= RotationMatrix.Forward * _moveSpeedForward;
        }



        public void Shoot(Vector3 direction)
        {
            _gun.Shoot2(Position, RotationMatrix, 6f);
        }

        public void Inteligence(Vector3 targedPosition)
        {
            
            double distanceToTarged = Math.Sqrt(Math.Pow(Position.X - targedPosition.X, 2) + Math.Pow(Position.Z - targedPosition.Z, 2));


            //if (distanceToTarged < 300)
                FlyVector(Position - targedPosition);
            if (distanceToTarged < 150)
            {
                FlyVector(new Vector3(0, 0, 0));
                Shoot(targedPosition);
            }

         

        }

    }
}
