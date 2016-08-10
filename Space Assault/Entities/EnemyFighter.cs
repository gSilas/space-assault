using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities.Weapon;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SpaceAssault.Entities
{
    class EnemyFighter : AEnemys
    {

        public EnemyFighter(Vector3 position)
        {
            _spawnPos = position;
            Position = position;
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;
            
            _moveSpeedForward = 0.5f;
            _turnSpeed = 5.0f;

           

            _health = 50;
            _armor = 100;
            _gun = new RailGun();
            _gun.Initialize();
        }
        /*
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
        */
        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemyship2");
            _gun.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            _gun.Update(gameTime);
            if (_health <= 0) IsDead = true;
            
            //Werden Besser jede Minute
            if (gameTime.TotalGameTime > (_getBetterwithTime.Add(TimeSpan.FromSeconds(60))))
            {
                _health = _health + 50;

                _getBetterwithTime = gameTime.TotalGameTime;
                //Console.WriteLine("UpDATED");
                _gun=new RailGun();
            }
            //TODO: health, armor update

        }

        public override void Shoot(Vector3 direction)
        {
            _gun.Shoot2(Position, RotationMatrix, 6f);
        }
        
        public override void Intelligence(Vector3 targedPosition)
        {

            double distanceToTarged = Math.Sqrt(Math.Pow(Position.X - targedPosition.X, 2) + Math.Pow(Position.Z - targedPosition.Z, 2));


           
            FlyVector(Position - targedPosition);

            if (distanceToTarged < 150)
            {
                //Shoot(targedPosition);
            }



        }

    }
}
