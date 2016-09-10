using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;
using IrrKlang;

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

            KillMoney = 70;
            Health = 40;

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
