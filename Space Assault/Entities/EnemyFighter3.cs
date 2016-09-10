using Microsoft.Xna.Framework;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyFighter3 : AEnemys
    {
        public EnemyFighter3(Vector3 position)
        {
            SpawnPos = position;
            Position = position;
            _trail = new Trail(new EnemyTrailSettings());

            RotationMatrix = Matrix.Identity;
            MoveSpeedForward = 1.2f;
            TurnSpeed = 8.0f;

            KillMoney = 50;
            Health = 30;

            Gun = new Weapon(600d);
            gunMakeDmg = 10;
        }
        public override void Update(GameTime gameTime)
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);

            if (Health <= 0)
            {

                IsDead = true;

            }
        }
    }
}
