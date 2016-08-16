using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using SpaceAssault.Entities.Weapon;

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
        public AWeapon Gun;


        public bool IsDead
        {
            get { return isDead; }
            protected set { isDead = value; }
        }

        public void Reset()
        {
            RotationMatrix = Matrix.Identity;
            Health = 100;
            
            Position = SpawnPos;
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

        public void FlyVector(Vector3 direction)
        {

            direction.Normalize();
            float vectorDirection;
            for (float i = 0.5f; i < TurnSpeed; i++)
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

            Position -= RotationMatrix.Forward * MoveSpeedForward;


        }

        public abstract void Intelligence(GameTime gameTime, Vector3 targedPosition, ref List<Bullet> bulletList);
    }
}
