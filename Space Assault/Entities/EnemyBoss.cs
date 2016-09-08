using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyBoss : AEnemys
    {

        //The Boss has 4 Seperate AttackTowers who need to be destroyed seperatly
        internal class AttackTower : AEnemys
        {
            private Weapon _gun;
 
            public AttackTower(Vector3 position, int health, int damage, Weapon gun)
            {
                Health = health;
                SpawnPos = position;
                Position = position;
                Gun = gun;
        

            }

            public override void Update(GameTime gameTime)
            {
                if (Health <= 0)
                    IsDead = true;

                Spheres = Collider3D.UpdateBoundingSphere(this);
            }

            public override void LoadContent()
            {
                // NEEDS A TOWERMODEL
                //Model = Global.ContentManager.Load<Model>("Models/enemy_bomber");
                Spheres = Collider3D.UpdateBoundingSphere(this);
                Gun.LoadContent();

                HitSound = Engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/GetHitShips.wav", StreamMode.AutoDetect, true);

            }

            public override void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList)
            {
                double distanceToTarget = (Position - targetPosition).Length();
                double distanceToStation = Position.Length();


                //Shoots Tower Laser or Bombs?
                if (Gun == new Weapon(2000))
                {
                    Gun.Shoot(gameTime, Bullet.BulletType.PhotonBomb, gunMakeDmg, Position - RotationMatrix.Forward * 22.0f, RotationMatrix, ref bulletList);
                }
                if (Gun == new Weapon(400))
                {
                    if(distanceToTarget<=600)
                        Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, gunMakeDmg, Position - RotationMatrix.Forward * 22.0f, RotationMatrix, ref bulletList);

                }
            }
        }

        private AttackTower _tower1;
        private AttackTower _tower2;
        private AttackTower _tower3;
        private AttackTower _tower4;

        public EnemyBoss(Vector3 spawnposition,int shipHealth, int damage )
        {
            SpawnPos = spawnposition;
            Position = spawnposition;
            trail = new List<Trail>();
            TrailParticles = new TrailParticleSettings();
            trail.Add(new Trail(TrailParticles));

            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 0.5f;
            TurnSpeed = 2.0f;

            Position = spawnposition;
            Health = shipHealth;
            _tower1 = new AttackTower(Position, 600, 100, new Weapon(400));
            _tower2 = new AttackTower(Position, 600, 100, new Weapon(400));
            _tower3 = new AttackTower(Position, 600, 100, new Weapon(2000));
            _tower4 = new AttackTower(Position, 600, 100, new Weapon(2000));
        }


        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
                IsDead = true;

            Spheres = Collider3D.UpdateBoundingSphere(this);

        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemy_bomber");
            
            Spheres = Collider3D.UpdateBoundingSphere(this);

            _tower1.LoadContent();
            _tower2.LoadContent();
            _tower3.LoadContent();
            _tower4.LoadContent();

            FlySound = Engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/FlyBomber.wav", StreamMode.AutoDetect, true);
            HitSound = Engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/GetHitShips.wav", StreamMode.AutoDetect, true);
        }

        public override void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList)
        {
            throw new NotImplementedException();
        }
    }
}
