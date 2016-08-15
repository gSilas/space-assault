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
        private ISoundEngine _engine;
        private ISound _shootSound;
        public override void Initialize()
        {
            ListOfBullets = new List<Bullet>();
            RemoveListOfBullets = new List<Bullet>();
            GlobalTimeSpan = TimeSpan.FromSeconds(0);
            LastShotTime = TimeSpan.FromSeconds(0);
            CoolDownTime = TimeSpan.FromMilliseconds(200d);
            makeDmg = 10;
            DmgStation = false;
        }
        
        public override void LoadContent()
        {
            BulletModel = Global.ContentManager.Load<Model>("Models/laser");
            BulletModel2 = Global.ContentManager.Load<Model>("Models/laser2");
            _engine = new ISoundEngine();
        }

        public override bool Shoot(Vector3 position, Matrix droneRotateMatrix, float travelspeed)
        {
            if (GlobalTimeSpan > LastShotTime.Add(CoolDownTime))
            {
                var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
                _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
                _shootSound = _engine.Play3D("Content/Media/Music/Laser_Shoot.wav", curListenerPos + new Vector3D(0, 15, 0), false, true, StreamMode.AutoDetect, true);
                _shootSound.Volume = 0.5f;
                _shootSound.Paused = false;

                if (GameplayScreen._dronepdate)
                    ListOfBullets.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel));
                else
                    ListOfBullets.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel2));
                LastShotTime = GlobalTimeSpan;
                return true;
            }
            return false;

        }
        public override bool Shoot2(Vector3 position, Matrix droneRotateMatrix, float travelspeed)
        {
            if (GlobalTimeSpan > LastShotTime.Add(CoolDownTime))
            {
                _engine.SetListenerPosition(new Vector3D(Global.Camera.Position.X, Global.Camera.Position.Y, Global.Camera.Position.Z), new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z));
                _shootSound = _engine.Play3D("Content/Media/Music/Laser_Shoot.wav", new Vector3D(0, 0, 0), false, false, StreamMode.AutoDetect, true);
                _shootSound.Volume = 0.5f;

                ListOfBullets.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel));
                LastShotTime = GlobalTimeSpan;
                return true;
            }
            return false;
        }
    }
}
