using System;
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
            MoveSpeedForward = 0.5f;
            TurnSpeed = 5.0f;
            Health = 30;
            Armor = 100;
            Gun = new RailGun();
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
            Gun.Update(gameTime);
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

        public override void Shoot(Vector3 direction)
        {
            Gun.Shoot2(Position, RotationMatrix, 6f);
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
