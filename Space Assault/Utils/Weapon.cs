using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;
using IrrKlang;

namespace SpaceAssault.Utils
{
    public class Weapon
    {
        protected static int _defaultCooldown;
        protected int _cooldownTime;

        private ISpaceSoundEngine _engine;

        //type of bullets that exist
        private BulletMemory yellowLazer;
        private BulletMemory blueLazer;
        private BulletMemory photonBomb;
        private BulletMemory bigJoe;
        private BulletMemory enemyLazer;
        private BulletMemory bossGun;

        public class BulletMemory
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

        public Weapon(int coolDownMilis)
        {
            _defaultCooldown = coolDownMilis;
            _cooldownTime = 0;
        }

        public void LoadContent()
        {
            _engine = new ISpaceSoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
            ISoundSource lazer;
            ISoundSource lazer2;
            ISoundSource lazer3;
            ISoundSource rocket;

            if (Global.EasterEgg)
            {
                lazer = _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/boooow.mp3", StreamMode.AutoDetect, true);
                lazer2 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/pooach.mp3", StreamMode.AutoDetect, true);
                lazer3 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/peeeew.mp3", StreamMode.AutoDetect, true);
                rocket = _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/paaaach.mp3", StreamMode.AutoDetect, true);
            }
            else
            {
                lazer = _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/Laser_Shoot_try.wav", StreamMode.AutoDetect, true);
                lazer2 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/Laser_Shoot3.wav", StreamMode.AutoDetect, true);
                lazer3 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/Laser_Shoot5.wav", StreamMode.AutoDetect, true);
                rocket = _engine.AddSoundSourceFromFile("Content/Media/Effects/Weapons/RocketShot.wav", StreamMode.AutoDetect, true);
            }

            yellowLazer = new BulletMemory("Models/laser", lazer, 6f, false);
            blueLazer = new BulletMemory("Models/laser2", lazer2, 6f, false);
            photonBomb = new BulletMemory("Models/bullet", rocket, 2f, true);
            bigJoe = new BulletMemory("Models/bullet", rocket, 4f, false);
            enemyLazer = new BulletMemory("Models/laser", lazer3, 6f, false);
            bossGun = new BulletMemory("Models/bullet", rocket,4f,true);
        }

        public bool Shoot(GameTime gameTime, Bullet.BulletType bullet, int damage, Vector3 position, Vector3 direction, ref List<Bullet> bulletList)
        {
            if (_cooldownTime <= 0)
            {
                //resetting cooldown
                _cooldownTime = _defaultCooldown;

                //playing the sound
                //Vector3D curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
                //_engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
                ISound _shootSound = _engine.Play2D(getBullet(bullet).soundSource, false, true, true);
                _shootSound.Volume = Global.SpeakerVolume / 10;
                _shootSound.Paused = false;

                // adding bullet to ref list
                bulletList.Add(new Bullet(bullet, getBullet(bullet).model, getBullet(bullet).moveSpeed, getBullet(bullet).canDmgStation, damage, position, direction));
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            _cooldownTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public BulletMemory getBullet(Bullet.BulletType bullet)
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

                case Bullet.BulletType.BossGun:
                    return bossGun;
                    
                default:
                    return yellowLazer;

            }
        }

    }
}
