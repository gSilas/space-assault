using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities
{
    public abstract class AEnemys : AEntity
    {
        protected Vector3 SpawnPos;
        public float MoveSpeedForward;
        protected float TurnSpeed;
        public Vector3 _flyingDirection;
        public int Health;
        public int KillMoney; //The Money the player gets if he kills the unit
        public bool flyingAwayFromDrone;
        public bool flyingAwayFromStation;
        protected bool isDead;
        public Weapon Gun;
        public int gunMakeDmg = 0;

        protected bool FreshSpawn;

        public bool IsDead
        {
            get { return isDead; }
            protected set { isDead = value; }
        }

        public Vector3 Direction
        {
            get { return _flyingDirection; }
            set { _flyingDirection = value; }
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

        public void getHit(int HowMuchDMG)
        {
            Health -= HowMuchDMG;
            //playing the sound
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }
        public virtual void LoadContent(Model model)
        {
            Model = model;
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.LoadContent();
        }
    }

}
