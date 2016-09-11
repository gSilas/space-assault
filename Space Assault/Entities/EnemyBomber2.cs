using Microsoft.Xna.Framework;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyBomber2 : AEnemys
    {
        public EnemyBomber2(Vector3 position)
        {
            SpawnPos = position;
            Position = position;

            _trail = new Trail(new EnemyBomberTrailSettings());

            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 0.9f;
            TurnSpeed = 4.0f;

            KillMoney = Global.EnemyBomber2KillMoney;
            Health = Global.EnemyBomber2Health;
            gunMakeDmg = Global.EnemyBomber2Damage;

            Gun = new Weapon(5000d);
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0) IsDead = true;

            Spheres = Collider3D.UpdateBoundingSphere(this);
            //TODO: health, armor update
        }
    }
}

