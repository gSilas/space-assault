﻿using Microsoft.Xna.Framework;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyBomber : AEnemys
    {
        public EnemyBomber(Vector3 position)
        {
            SpawnPos = position;
            Position = position;

            _trail = new Trail(new EnemyBomberTrailSettings());

            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 0.8f;
            TurnSpeed = 2.0f;

            //_moveSpeedBackward = -0.5f;
            KillMoney = Global.EnemyBomberKillMoney;
            Health = Global.EnemyBomberHealth;
            gunMakeDmg = Global.EnemyBomberDamage;

            Gun = new Weapon(3000);
        }
        public override void Update(GameTime gameTime)
        {
            if (Health <= 0) IsDead = true;
            Gun.Update(gameTime);
            Spheres = Collider3D.UpdateBoundingSphere(this);
            //TODO: health, armor update
        }
    }
}

