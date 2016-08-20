using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyFighter : AEnemys
    {

        public EnemyFighter(Vector3 position)
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
            MoveSpeedForward = 0.2f;
            TurnSpeed = 1f;
            Health = 30;

            Gun = new Weapon(600d);
            Gun.Initialize();
            gunMakeDmg = 10;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemyship2");
            Spheres = Collider3D.UpdateBoundingSphere(this);
        }
        public override void Update(GameTime gameTime)
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);
            if (Health <= 0) IsDead = true;

            //Werden Besser jede Minute
            if (gameTime.TotalGameTime > (GetBetterwithTime.Add(TimeSpan.FromSeconds(60))))
            {
                Health = Health + 30;
                gunMakeDmg += 5;
                GetBetterwithTime = gameTime.TotalGameTime;
            }
        }

        public override void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList)
        {
            double distanceToTarged = Math.Sqrt(Math.Pow(Position.X - targetPosition.X, 2) + Math.Pow(Position.Z - targetPosition.Z, 2));
            FlyVector(Position - targetPosition);

            if (distanceToTarged < 150)
            {
                Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, gunMakeDmg, Position, RotationMatrix, ref bulletList);
            }
        }

    }
}
