using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities.Weapon
{
    class RailGun: AWeapon
    {
        public override void Initialize()
        {
            ListOfBullets = new List<Bullet>();
            GlobalTimeSpan = TimeSpan.FromSeconds(0);
            LastShotTime = TimeSpan.FromSeconds(0);
            CoolDownTime = TimeSpan.FromMilliseconds(300d);
        }

        public override void LoadContent()
        {
            BulletModel = Global.ContentManager.Load<Model>("Models/bullet");
        }

        public override void Shoot(Vector3 position, Vector3 direction, float travelspeed)
        {
            if (GlobalTimeSpan > LastShotTime.Add(CoolDownTime))
            {
                ListOfBullets.Add(new Bullet(position, direction, travelspeed, BulletModel));
                LastShotTime = GlobalTimeSpan;
            }

        }
    }
}
