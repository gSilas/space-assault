using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Screens;
using SpaceAssault.Utils;
using IrrKlang;

namespace SpaceAssault.Entities.Weapon
{
    class RailGun : AWeapon
    {
        private ISoundSource _shootSource;
        private ISoundEngine _engine;

        public override void Initialize()
        {
            GlobalTimeSpan = TimeSpan.FromSeconds(0);
            LastShotTime = TimeSpan.FromSeconds(0);
            CoolDownTime = TimeSpan.FromMilliseconds(200d);
            DmgStation = false;
            makeDmg = 10;
            _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
            _shootSource = _engine.AddSoundSourceFromFile("Content/Media/Music/Laser_Shoot.wav", StreamMode.AutoDetect, true);
        }
        
        public override void LoadContent()
        {
            BulletModel = Global.ContentManager.Load<Model>("Models/laser");
            BulletModel2 = Global.ContentManager.Load<Model>("Models/laser2");        
        }

        public override bool Shoot(Vector3 position, Matrix droneRotateMatrix, float travelspeed, ref List<Bullet> bulletList)
        {
            if (GlobalTimeSpan > LastShotTime.Add(CoolDownTime))
            {
                var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
                _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
                var _shootSound = _engine.Play3D(_shootSource, curListenerPos.X, curListenerPos.Y+15f, curListenerPos.Z, false, true, true);
                _shootSound.Volume = 0.5f;
                _shootSound.Paused = false;

                switch (ShopScreen._droneDamageLevel)
                {
                    case 1:
                        bulletList.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel, makeDmg, DmgStation));
                        break;
                    case 2:
                        bulletList.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel2, makeDmg, DmgStation));
                        break;
                    default:
                        break;
                }
                LastShotTime = GlobalTimeSpan;
                return true;
            }
            return false;

        }

    }
}
