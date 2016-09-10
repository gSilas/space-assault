using System;
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
        public Vector3 _direction;
        public int Health;
        public int KillMoney; //The Money the player gets if he kills the unit
        public bool flyingAwayFromDrone;
        public bool flyingAwayFromStation;
        protected bool isDead;
        public Weapon Gun;
        public int gunMakeDmg = 0;

        protected ISoundEngine Engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
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
            ISound Hit = Engine.Play2D(HitSound, false, true, false);
            Hit.Volume = Global.SpeakerVolume/10;
            Hit.Paused = false;
        }
    }
}
