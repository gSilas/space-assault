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
            _trail = new Trail(new EnemyTrailSettings());

            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 0.5f;
            TurnSpeed = 2.0f;

            //BIGJOE Rocket for Body
            Gun = new Weapon(1000);

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
        }
    }
}
