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




        public override void Shoot(Vector3 direction)
        {
            _gun.Shoot2(Position, RotationMatrix, 1f);
        }

        public override void Intelligence(Vector3 targedPosition)
        {

            double distanceToTarged = Math.Sqrt(Math.Pow(Position.X - targedPosition.X, 2) + Math.Pow(Position.Z - targedPosition.Z, 2));


            //if (distanceToTarged < 300)
            FlyVector(Position - targedPosition);

            if (distanceToTarged < 150)
            {
                Shoot(targedPosition);
            }



        }

    }
}
