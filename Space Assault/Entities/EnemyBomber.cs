using System;
using System.Collections.Generic;
using IrrKlang;
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

            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 0.5f;
            TurnSpeed = 2.0f;

            //_moveSpeedBackward = -0.5f;
            KillMoney = 100;
            Health = 40;

            Gun = new Weapon(3000d);
            gunMakeDmg = 5;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemy_bomber");
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
                gunMakeDmg += 10;
                KillMoney += 100;
                GetBetterwithTime = gameTime.TotalGameTime;
                //Console.WriteLine("UpDATED");
            }
            //TODO: health, armor update

        }

        public override void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList)
        {
            double distanceToTarget = (Position - targetPosition).Length();
            double distanceToStation = Position.Length();

            if (distanceToStation < 200)
                neuerAnflug = true;

            //flying away from drone
            if (distanceToTarget < 200)
                FlyToDirection(-(Position - targetPosition));

            if (neuerAnflug)
            {
                //flying away from station
                FlyToDirection(-(Position - new Vector3(0, 0, 0)));
                if (distanceToStation > 500)
                    neuerAnflug = false;
            }
            else
            {   
                //flying away from drone
                if (distanceToTarget < 200)
                    FlyToDirection(-(Position - targetPosition));
                else
                    FlyToPoint(new Vector3(0, 0, 0));
            }
            if (distanceToStation < 400 && neuerAnflug == false)
            {
                Gun.Shoot(gameTime, Bullet.BulletType.PhotonBomb, gunMakeDmg, Position - RotationMatrix.Forward * 22.0f, RotationMatrix, ref bulletList);
            }

        }


    }
}

