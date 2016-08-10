using System;
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
            Armor = 100;
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
            Gun.Update(gameTime);
            if (Health <= 0) IsDead = true;

            Spheres = Collider3D.UpdateBoundingSphere(this);
            
            //besser mit Zeit
            if (gameTime.TotalGameTime > (GetBetterwithTime.Add(TimeSpan.FromSeconds(60))))
            {
                Health = Health + 50;
                Gun.makeDmg += 5;
                GetBetterwithTime = gameTime.TotalGameTime;
                //Console.WriteLine("UpDATED");
            }
            //TODO: health, armor update

        }


        public override void Shoot(Vector3 direction)
        {
            //_gun.Shoot2(Position, RotationMatrix, 1f);
            Gun.Shoot(Position - RotationMatrix.Forward * 22.0f, RotationMatrix, 1f);
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

