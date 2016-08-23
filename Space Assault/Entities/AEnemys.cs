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
        public int Health;

        protected bool isDead;
        public Weapon Gun;
        public int gunMakeDmg = 0;

        public bool IsDead
        {
            get { return isDead; }
            protected set { isDead = value; }
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
                direction.Normalize();
                float vectorDirection = RotationMatrix.Forward.Z * direction.X - RotationMatrix.Forward.X * direction.Z;
                if (vectorDirection > 0.1 || vectorDirection < -0.1)
                {
                    for (int i = 1; i < (TurnSpeed/0.2f); i++)
                    {
                        if (vectorDirection > 0.1)
                        {
                            //Console.WriteLine("turn left");
                            RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(0.2f));
                        }
                        else if (vectorDirection < -0.1)
                        {
                            //Console.WriteLine("turn right");
                            RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(-0.2f));
                        }
                    }
                }
                else
                    Position -= RotationMatrix.Forward * MoveSpeedForward;
            }
        }

        public void FlyToDirection(Vector3 direction)
        {
            direction.Normalize();
            float vectorDirection = RotationMatrix.Forward.Z * direction.X - RotationMatrix.Forward.X * direction.Z;
            if (vectorDirection > 0.1 || vectorDirection < -0.1)
            {
                for (int i = 1; i < (TurnSpeed / 0.2f); i++)
                {
                    if (vectorDirection > 0.1)
                    {
                        Console.WriteLine("turn left");
                        RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(0.2f));
                    }
                    else if (vectorDirection < -0.1)
                    {
                        Console.WriteLine("turn right");
                        RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(-0.2f));
                    }
                }
            }
            else
                Position -= RotationMatrix.Forward * MoveSpeedForward;

        }

        public abstract void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList);
    }
}
