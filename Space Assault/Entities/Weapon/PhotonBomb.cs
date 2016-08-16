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

        public override bool Shoot(GameTime gameTime, Vector3 position, Matrix droneRotateMatrix, float travelspeed, ref List<Bullet> bulletList)
        {
            if (gameTime.TotalGameTime > LastShotTime.Add(CoolDownTime))
            {
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
                LastShotTime = gameTime.TotalGameTime;
                return true;
            }
            return false;

        }

    }
}
