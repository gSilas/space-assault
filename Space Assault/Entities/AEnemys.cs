using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities
{
    public abstract class AEnemys : AEntity
    {
        protected Vector3 SpawnPos;
        protected float MoveSpeedForward;
        protected float TurnSpeed;
        protected TimeSpan GetBetterwithTime;
        public Vector3 _direction;
        public int Health;
        public int KillMoney; //The Money the player gets if he kills the unit

        protected bool isDead;
        public Weapon Gun;
        public int gunMakeDmg = 0;

        public bool IsDead
        {
            get { return isDead; }
            protected set { isDead = value; }
        }

        public Vector3 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public override void Draw()
        {
            if (Collider3D.BoundingFrustumIntersection(this))
            {
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

        }

        public void FlyToPoint(Vector3 target)
        {
            Vector3 direction = Position - target;
            if (direction.Length() > 1f)
            {
                RotateTowards(direction);
            }
            Position -= RotationMatrix.Forward * MoveSpeedForward;
        }


        public void FlyToDirection(Vector3 direction)
        {
            RotateTowards(direction);

            Position -= RotationMatrix.Forward * MoveSpeedForward;
        }

        public void RotateTowards(Vector3 direction)
        {
            for (int i = 1; i < (TurnSpeed / 0.5f); i++)
            {
                float vectorDirection = RotationMatrix.Forward.Z * direction.X - RotationMatrix.Forward.X * direction.Z;
                if (Math.Abs(vectorDirection) >= 0.01)
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(Math.Sign(vectorDirection) * 0.5f));
            }

        }

        public abstract void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList);

    }
}
