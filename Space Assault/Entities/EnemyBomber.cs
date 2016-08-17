using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities.Weapon;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities
{
    class EnemyBomber : AEnemys
    {
        private bool neuerAnflug = false;


        public EnemyBomber(Vector3 position)
        {
            SpawnPos = position;
            Position = position;
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 0.5f;
            TurnSpeed = 5.0f;

            //_moveSpeedBackward = -0.5f;

            Health = 40;

            Gun = new PhotonBomb();
            Gun.Initialize();
        }


        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemyship");
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0) IsDead = true;

            Spheres = Collider3D.UpdateBoundingSphere(this);

            //besser mit Zeit
            if (gameTime.TotalGameTime > (GetBetterwithTime.Add(TimeSpan.FromSeconds(60))))
            {
                Health = Health + 30;
                Gun.makeDmg += 10;
                GetBetterwithTime = gameTime.TotalGameTime;
                //Console.WriteLine("UpDATED");
            }
            //TODO: health, armor update

        }

        public override void Intelligence(GameTime gameTime, Vector3 targedPosition, ref List<Bullet> bulletList)
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


            if (distanceToStation < 400 && neuerAnflug == false)
            {
                Gun.Shoot(gameTime, Position - RotationMatrix.Forward * 22.0f, RotationMatrix, 0.7f, ref bulletList);
            }

        }

    }
}

