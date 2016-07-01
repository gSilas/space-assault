using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Screens;

namespace SpaceAssault.Entities.Weapon
{
    class RailGun: AWeapon
    {
        public override void Initialize()
        {
            ListOfBullets = new List<Bullet>();
            RemoveListOfBullets = new List<Bullet>();
            GlobalTimeSpan = TimeSpan.FromSeconds(0);
            LastShotTime = TimeSpan.FromSeconds(0);
            CoolDownTime = TimeSpan.FromMilliseconds(200d);
        }

        public override void LoadContent()
        {
            BulletModel = Global.ContentManager.Load<Model>("Models/laser");
            BulletModel2 = Global.ContentManager.Load<Model>("Models/laser2");
        }

        public override void Shoot(Vector3 position, Matrix droneRotateMatrix, float travelspeed)
        {
            if (GlobalTimeSpan > LastShotTime.Add(CoolDownTime))
            {
                if(GameplayScreen._dronepdate)
                    ListOfBullets.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel));
                else
                    ListOfBullets.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel2));
                LastShotTime = GlobalTimeSpan;
            }

        }
    }
}
