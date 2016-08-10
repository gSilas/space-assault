using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities.Weapon;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SpaceAssault.Entities
{
    class EnemyBomber : AEnemys
    {
        private bool neuerAnflug = false;


        public EnemyBomber(Vector3 position)
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
            _gun = new PhotonBomb();
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
            if (_health <= 0) IsDead = true;

            if (gameTime.TotalGameTime > (_getBetterwithTime.Add(TimeSpan.FromSeconds(60))))
            {
                _health = _health + 50;

                _getBetterwithTime = gameTime.TotalGameTime;
                //Console.WriteLine("UpDATED");
            }
            //TODO: health, armor update

        }


        public override void Shoot(Vector3 direction)
        {
            //_gun.Shoot2(Position, RotationMatrix, 1f);
            _gun.Shoot(Position - RotationMatrix.Forward * 22.0f, RotationMatrix, 1f);
        }

        public override void Intelligence(Vector3 targedPosition)
        {

            double distanceToTarged = Math.Sqrt(Math.Pow(Position.X - targedPosition.X, 2) + Math.Pow(Position.Z - targedPosition.Z, 2));
            double distanceToStation = Math.Sqrt(Math.Pow(Position.X - 0, 2) + Math.Pow(Position.Z - 0, 2));

            //Console.WriteLine(distanceToStation);
            //Console.WriteLine(neuerAnflug);

            if (distanceToStation < 200)
                neuerAnflug = true;
           
            if (distanceToTarged < 200)
                FlyVector(-(Position - targedPosition));

            if (neuerAnflug == true)
            {
                FlyVector(-(Position - new Vector3(0, 0, 0)));
                if (distanceToStation > 500)
                    neuerAnflug = false;
            }
            if (neuerAnflug == false)
                if (distanceToTarged < 200)
                    FlyVector(-(Position - targedPosition));
                else
                {
                    FlyVector(Position - new Vector3(0, 0, 0));
                }
            


            if (distanceToStation < 400&&neuerAnflug==false)
                {
                    Shoot(targedPosition);
                }
            }

        }

    }

