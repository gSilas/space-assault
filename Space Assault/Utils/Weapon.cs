using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;
using IrrKlang;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Utils
{
    public class Weapon
    {
        protected TimeSpan _globalTimeSpan;
        protected TimeSpan _coolDownTime;
        protected TimeSpan _lastShotTime;

        private ISoundEngine _engine;

        //type of bullets that exist
        private BulletMemory yellowLazer;
        private BulletMemory blueLazer;
        private BulletMemory photonBomb;
        private BulletMemory bigJoe;
        private BulletMemory enemyLazer;

        internal class BulletMemory
        {
            public Model model;
            public ISoundSource soundSource;
            public float moveSpeed;
            public bool canDmgStation;

            public BulletMemory(string model, ISoundSource soundSource, float moveSpeed, bool canDmgStation)
            {
                this.model = Global.ContentManager.Load<Model>(model);
                this.soundSource = soundSource;
                this.moveSpeed = moveSpeed;
                this.canDmgStation = canDmgStation;
            }
        }

        public Weapon(double coolDownMilis)
        {
            _coolDownTime = TimeSpan.FromMilliseconds(coolDownMilis);        
            _globalTimeSpan = TimeSpan.FromSeconds(0);
            _lastShotTime = TimeSpan.FromSeconds(0);
        }

        public void LoadContent()
        {
            _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);

            ISoundSource lazer = _engine.AddSoundSourceFromFile("Content/Media/Effects/Laser_Shoot_try.wav", StreamMode.AutoDetect, true);
            ISoundSource lazer2= _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/Laser_Shoot3.wav", StreamMode.AutoDetect, true);
            ISoundSource lazer3= _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/Laser_Shoot5.wav", StreamMode.AutoDetect, true);
            ISoundSource rocket= _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/RocketShot.wav", StreamMode.AutoDetect, true);
            yellowLazer = new BulletMemory("Models/laser", lazer, 6f, false);
            blueLazer = new BulletMemory("Models/laser2", lazer2, 6f, false);
            photonBomb = new BulletMemory("Models/bullet", lazer, 0.7f, true);
            bigJoe = new BulletMemory("Models/bullet", rocket, 4f, false);
            enemyLazer = new BulletMemory("Models/laser", lazer3, 6f, false);
        }

        public bool Shoot(GameTime gameTime, Bullet.BulletType bullet, int damage, Vector3 position, Matrix droneRotateMatrix, ref List<Bullet> bulletList)
        {
            if (gameTime.TotalGameTime > _lastShotTime.Add(_coolDownTime))
            {
                _lastShotTime = gameTime.TotalGameTime;

                //playing the sound
                Vector3D curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
                _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
                ISound _shootSound = _engine.Play3D(getBullet(bullet).soundSource, curListenerPos.X, curListenerPos.Y + 15f, curListenerPos.Z, false, true, true);
                _shootSound.Volume = Global.SpeakerVolume / 10;
                _shootSound.Paused = false;

                // adding bullet to ref list
                bulletList.Add(new Bullet(bullet, getBullet(bullet).model, getBullet(bullet).moveSpeed, getBullet(bullet).canDmgStation, damage, position, droneRotateMatrix));
                return true;
            }
            return false;
        }

        private BulletMemory getBullet(Bullet.BulletType bullet)
        {
            switch (bullet)
            {
                case Bullet.BulletType.YellowLazer:
                    return yellowLazer;

                case Bullet.BulletType.BlueLazer:
                    return blueLazer;

                case Bullet.BulletType.PhotonBomb:
                    return photonBomb;

                case Bullet.BulletType.BigJoe:
                    return bigJoe;

                case Bullet.BulletType.EnemyLazer:
                    return enemyLazer;

                default:
                    return yellowLazer;

            }
        }

    }
}
