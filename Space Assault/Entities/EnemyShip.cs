using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Entities.Weapon;
using System.Collections.Generic;
using System;
using SpaceAssault.Utils;
using SpaceAssault.ScreenManager;

namespace SpaceAssault.Entities
{
    class EnemyShip : AEntity
    {
        private Vector3 _spawnPos;
        private float _moveSpeedForward;
        //private float _moveSpeedBackward;
        //private float _turnSpeed;
        //private float _moveSpeedModifier;
        private Vector3 _direction= new Vector3(0,0,0);
        private float _moveSpeedModifier;
 
        public int _health;
        private int _armor;

        private AWeapon _gun;

        public EnemyShip(Vector3 position)
        {
            _spawnPos = position;
            Position = position;
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;
            
            _moveSpeedForward = 0.6f;
            //_turnSpeed = 10.0f;
            //_moveSpeedBackward = -0.5f;

            _health = 100;
            _armor = 100;
            _gun = new RailGun();
            _gun.Initialize();
        }
        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemyship");
            _gun.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            _gun.Update(gameTime);
            Position += _direction * _moveSpeedForward;
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
            _direction=direction;
        }


    }
}
