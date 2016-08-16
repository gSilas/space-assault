using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Screens;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities.Weapon
{
    class PhotonBomb : AWeapon
    {
        public override void Initialize()
        {
            ListOfBullets = new List<Bullet>();
            RemoveListOfBullets = new List<Bullet>();
            GlobalTimeSpan = TimeSpan.FromSeconds(0);
            LastShotTime = TimeSpan.FromSeconds(0);
            CoolDownTime = TimeSpan.FromMilliseconds(2000d);
            makeDmg = 5;
            DmgStation = true;
        }

        public override void LoadContent()
        {
            BulletModel = Global.ContentManager.Load<Model>("Models/bullet");
            BulletModel2 = Global.ContentManager.Load<Model>("Models/bullet"); // another model?
        }

        public override bool Shoot(Vector3 position, Matrix droneRotateMatrix, float travelspeed)
        {
            if (GlobalTimeSpan > LastShotTime.Add(CoolDownTime))
            {
                switch (ShopScreen._droneDamageLevel)
                {
                    case 1:
                        ListOfBullets.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel));
                        break;
                    case 2:
                        ListOfBullets.Add(new Bullet(position, droneRotateMatrix, travelspeed, BulletModel2));
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
