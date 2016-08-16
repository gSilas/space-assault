using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities.Weapon;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities
{
    class EnemyFighter : AEnemys
    {

        public EnemyFighter(Vector3 position)
        {
            SpawnPos = position;
            Position = position;
        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;
            MoveSpeedForward = 1.2f;
            TurnSpeed = 5.0f;
            Health = 30;
          
            Gun = new EnemyLaser();
            Gun.Initialize();
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemyship2");
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);
            if (Health <= 0) IsDead = true;

            //Werden Besser jede Minute
            if (gameTime.TotalGameTime > (GetBetterwithTime.Add(TimeSpan.FromSeconds(60))))
            {
                Health = Health + 30;
                Gun.makeDmg += 5;
                GetBetterwithTime = gameTime.TotalGameTime;
                //Console.WriteLine("UpDATED");
                //Gun=new RailGun(); // why? 
            }
            //TODO: health, armor update
        }

        public override void Intelligence(Vector3 targetPosition, ref List<Bullet> bulletList)
        {
            double distanceToTarged = Math.Sqrt(Math.Pow(Position.X - targetPosition.X, 2) + Math.Pow(Position.Z - targetPosition.Z, 2));
            FlyVector(Position - targetPosition);

            if (distanceToTarged < 150)
            {
                Gun.Shoot(Position, RotationMatrix, 3f, ref bulletList);
            }
        }

    }
}
