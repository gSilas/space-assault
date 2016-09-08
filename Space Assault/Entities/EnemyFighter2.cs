using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;
using IrrKlang;

namespace SpaceAssault.Entities
{
    class EnemyFighter2 : AEnemys
    {


        public EnemyFighter2(Vector3 position)
        {
            SpawnPos = position;
            Position = position;
            _trail = new Trail(new FighterTrailSettings());

            RotationMatrix = Matrix.Identity;
            MoveSpeedForward = 1.2f;
            TurnSpeed = 8.0f;

            KillMoney = 70;
            Health = 40;

            Gun = new Weapon(600d);
            gunMakeDmg = 10;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemy_fighter2");
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.LoadContent();

            //Engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
            FlySound = Engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/FlyFighter.wav", StreamMode.AutoDetect, true);
            HitSound = Engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/GetHitShips.wav", StreamMode.AutoDetect, true);
        }
        public override void Update(GameTime gameTime)
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);

            //playing the sound
            Vector3D curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
            Engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
            ISound Fly;
            //Fly = Engine.Play3D(FlySound, curListenerPos.X, curListenerPos.Y + 15f, curListenerPos.Z, false, true, false);
            //Fly.Volume = 0.5f;
            //Fly.Paused = false;



            if (Health <= 0)
            {

                IsDead = true;

            }
        }

        public override void Intelligence(GameTime gameTime, Vector3 targetPosition, ref List<Bullet> bulletList)
        {
            double distanceToTarget = Math.Sqrt(Math.Pow(Position.X - targetPosition.X, 2) + Math.Pow(Position.Z - targetPosition.Z, 2));
            FlyToPoint(targetPosition);

            if (distanceToTarget < 150)
            {
                Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, gunMakeDmg, Position, RotationMatrix, ref bulletList);
            }
        }


    }
}
