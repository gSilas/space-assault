using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyBomber : AEnemys
    {

        private bool neuerAnflug = false;

        public EnemyBomber(Vector3 position)
        {
            SpawnPos = position;
            Position = position;
            trail = new List<Trail>();
            TrailParticles = new TrailParticleSystem();
            trail.Add(new Trail(TrailParticles));
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 0.5f;
            TurnSpeed = 5.0f;

            //_moveSpeedBackward = -0.5f;

            Health = 40;

            Gun = new Weapon(3000d);
            Gun.Initialize();
            gunMakeDmg = 5;
        }


        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemyship");
            Spheres = Collider3D.UpdateBoundingSphere(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0) IsDead = true;

            Spheres = Collider3D.UpdateBoundingSphere(this);

            //besser mit Zeit
            if (gameTime.TotalGameTime > (GetBetterwithTime.Add(TimeSpan.FromSeconds(60))))
            {
                Health = Health + 30;
                gunMakeDmg += 10;
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
                Gun.Shoot(gameTime, Bullet.BulletType.PhotonBomb, gunMakeDmg, Position - RotationMatrix.Forward * 22.0f, RotationMatrix, ref bulletList);
            }

        }

    }
}

