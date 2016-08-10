using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using SpaceAssault.Entities.Weapon;

using Microsoft.Xna.Framework.Graphics;


namespace SpaceAssault.Entities
{
    public abstract class AEnemys : AEntity
    {
        protected Vector3 _spawnPos;
        protected float _moveSpeedForward;
        protected float _turnSpeed;

        protected TimeSpan _getBetterwithTime;
        public int _health;
        protected int _armor;
        protected bool _isDead;
        protected AWeapon _gun;


        public bool IsDead
        {
            get { return _isDead; }
            protected set { _isDead = value; }
        }

 

        public void Reset()
        {
            RotationMatrix = Matrix.Identity;
            _health = 100;
            _armor = 100;
            Position = _spawnPos;
        }
        public List<Bullet> GetBulletList()
        {
            return _gun.ListOfBullets;
        }
        public override void Draw()
        {
            _gun.Draw();

            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = RotationMatrix * Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
                    effect.View = Global.Camera.ViewMatrix;
                    effect.Projection = Global.Camera.ProjectionMatrix;
                }
                mesh.Draw();
            }

        }

        public void FlyVector(Vector3 direction)
        {

            direction.Normalize();
            float vectorDirection;
            for (float i = 0.5f; i < _turnSpeed; i++)
            {
                vectorDirection = RotationMatrix.Forward.Z * direction.X - RotationMatrix.Forward.X * direction.Z;
                if (vectorDirection > 0.01)
                {
                    //turn left
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(0.5f));
                }
                else if (vectorDirection < -0.01)
                {
                    //turn right
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(-0.5f));
                }
            }

            Position -= RotationMatrix.Forward * _moveSpeedForward;


        }
        public abstract void Shoot(Vector3 direction);
  

        public abstract void Intelligence(Vector3 targedPosition);
    }
}
