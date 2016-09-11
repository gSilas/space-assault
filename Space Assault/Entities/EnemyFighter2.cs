using Microsoft.Xna.Framework;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyFighter2 : AEnemys
    {
        public EnemyFighter2(Vector3 position)
        {
            SpawnPos = position;
            Position = position;
            _trail = new Trail(new EnemyTrailSettings());

            RotationMatrix = Matrix.Identity;
            MoveSpeedForward = 1.2f;
            TurnSpeed = 8.0f;

            KillMoney = Global.EnemyFighter2KillMoney;
            Health = Global.EnemyFighter2Health;
            gunMakeDmg = Global.EnemyFighter2Damage;

            Gun = new Weapon(300);
        }
        public override void Update(GameTime gameTime)
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.Update(gameTime);
            if (Health <= 0)
            {

                IsDead = true;

            }
        }
    }
}
