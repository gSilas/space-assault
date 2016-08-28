using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using IrrKlang;

namespace SpaceAssault.Entities
{
    public abstract class AEnemys : AEntity
    {
        protected Vector3 SpawnPos;
        public float MoveSpeedForward;
        protected float TurnSpeed;
        protected TimeSpan GetBetterwithTime;
        public Vector3 _direction;
        public int Health;
        public int KillMoney; //The Money the player gets if he kills the unit
        public bool flyingAwayFromDrone;
        public bool flyingAwayFromStation;
        protected bool isDead;
        public Weapon Gun;
        public int gunMakeDmg = 0;

        protected ISoundEngine Engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);  
        protected ISoundSource FlySound;
        protected ISoundSource HitSound;
        protected bool FreshSpawn;

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



        public void getHit(int HowMuchDMG)
        {
            Health -= HowMuchDMG;
            //playing the sound
            Vector3D curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
            Engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
            ISound Hit = Engine.Play3D(HitSound, curListenerPos.X, curListenerPos.Y + 15f, curListenerPos.Z, false, true, false);
            Hit.Volume = 1f;
            Hit.Paused = false;
            

        }
        public abstract void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList);

    }
}
