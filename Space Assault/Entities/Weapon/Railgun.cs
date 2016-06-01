using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities.Weapon
{
    class RailGun: AWeapon
    {
        public override void Initialize()
        {
            ListOfBullets = new List<Bullet>();
        }

        public override void LoadContent()
        {
            _bulletModel = Global.ContentManager.Load<Model>("Models/asteroid");
        }

        public override void Shoot(Vector3 position, Vector3 direction, float travelspeed)
        {
            Bullet bullet = new Bullet(position, direction, travelspeed, _bulletModel);
            ListOfBullets.Add(bullet);
            Console.WriteLine(ListOfBullets.Count.ToString());
        }
    }
}
