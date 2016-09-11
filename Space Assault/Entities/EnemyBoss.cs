using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;
using System;
using System.Collections.Generic;

namespace SpaceAssault.Entities
{
    class EnemyBoss : AEnemys
    {
        //The Boss has 4 Seperate AttackTowers who need to be destroyed seperatly
        internal struct Vec3Rectangle
        {
            private Vector3 _center;
            private float _width;
            private float _height;

            public Vec3Rectangle(Vector3 center, float width, float height)
            {
                _center = center;
                _height = height;
                _width = width;
            }
            public Vector3 EdgeTopRight
            {
                get { return new Vector3(_center.X + _width/2, _center.Y, _center.Z + _height / 2); }
            }
            public Vector3 EdgeTopLeft
            {
                get { return new Vector3(_center.X - _width / 2, _center.Y, _center.Z + _height / 2); }
            }
            public Vector3 EdgeBottomRight
            {
                get { return new Vector3(_center.X + _width / 2, _center.Y, _center.Z - _height / 2); }
            }
            public Vector3 EdgeBottomLeft
            {
                get { return new Vector3(_center.X - _width / 2, _center.Y, _center.Z - _height / 2); }
            }
            public Vector3 Center
            {
                get { return _center; }
                set { _center = value; }
            }
        }

        public List<AttackTower> towerList;
        private Vec3Rectangle _compositionRec;

        public EnemyBoss(Vector3 spawnposition)
        {
            SpawnPos = spawnposition;
            _trail = new Trail(new EnemyBomberTrailSettings());

            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 1.0f;
            TurnSpeed = 2.0f;
            KillMoney = Global.EnemyBossKillMoney;
            Health = Global.EnemyBossHealth;
            gunMakeDmg = Global.EnemyBossDamage;
            Gun = new Weapon(2000);

            //BIGJOE Rocket for Body
            _compositionRec = new Vec3Rectangle(spawnposition, 10, 10);
            Position = _compositionRec.Center;
            towerList = new List<AttackTower>();
            towerList.Add(new AttackTower(_compositionRec.EdgeBottomLeft, new Weapon(500), this));
            towerList.Add(new AttackTower(_compositionRec.EdgeBottomRight, new Weapon(500), this));
            towerList.Add(new AttackTower(_compositionRec.EdgeTopLeft, new Weapon(500), this));
            towerList.Add(new AttackTower(_compositionRec.EdgeTopRight, new Weapon(500), this));
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
                IsDead = true;
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.Update(gameTime);
            _compositionRec.Center = Position;
            towerList[0].Position = _compositionRec.EdgeBottomLeft;
            towerList[1].Position = _compositionRec.EdgeBottomRight;
            towerList[2].Position = _compositionRec.EdgeTopLeft;
            towerList[3].Position = _compositionRec.EdgeTopRight;

            foreach (AttackTower tower in towerList)
            {
                tower.RotateTowards(tower.Position - Global.Camera.Position);
                tower.Gun.Update(gameTime);
            }
        }

        public void shootTower(GameTime gameTime, Drone curDrone, ref List<Bullet> bullets)
        {
            foreach (AttackTower tower in towerList)
            {
                float distanceToTarget = Vector3.Distance(tower.Position, curDrone.Position);
                Vector3 futureDronePos = curDrone.Position + (distanceToTarget / tower.Gun.getBullet(Bullet.BulletType.EnemyLazer).moveSpeed) * curDrone.curVelocity;

                Vector3 direction = -(futureDronePos - tower.Position);
                direction.Normalize();
                tower.Direction = direction;
                tower.RotateTowards(direction);

                float vectorDirection = tower.RotationMatrix.Forward.Z * direction.X - tower.RotationMatrix.Forward.X * direction.Z;
                if (Math.Abs(vectorDirection) <= 0.15f && distanceToTarget < 350 && !tower.flyingAwayFromDrone)
                {
                    tower.Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, tower.gunMakeDmg, tower.Position, direction, ref bullets);
                }
            }
        }

        public override void LoadContent(Model model)
        {
            Model = model;     
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.LoadContent();
            foreach(var tower in towerList)
            {
                tower.LoadContent();
            }
        }

        public override void Draw(Color entityColor)
        {
            base.Draw(entityColor);
            foreach (var tower in towerList)
            {
                tower.Draw(entityColor);
            }
        }

        public override void Draw(Color entityColor, float alphaValue)
        {
            base.Draw(entityColor, alphaValue);
            foreach (var tower in towerList)
            {
                tower.Draw(entityColor, alphaValue);
            }
        }
    }
    internal class AttackTower : AEnemys
    {
        EnemyBoss _boss;
        public AttackTower(Vector3 position, Weapon gun, EnemyBoss boss)
        {
            SpawnPos = position;
            Position = position;
            Gun = gun;
            _boss = boss;
            RotationMatrix = Matrix.Identity;
            MoveSpeedForward = 0f;
            TurnSpeed = 100f;

            KillMoney = Global.EnemyAttackTowerKillMoney;
            Health = Global.EnemyAttackTowerHealth;
            gunMakeDmg = Global.EnemyAttackTowerDamage;
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
                IsDead = true;
            if (_boss.IsDead)
                this.IsDead = true;
            Spheres = Collider3D.UpdateBoundingSphere(this);
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/laser");
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.LoadContent();
        }
    }
}
