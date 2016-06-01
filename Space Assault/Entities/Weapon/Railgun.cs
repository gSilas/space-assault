
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities.Weapon
{
    class RailGun: AWeapon
    {
        public override void Initialize()
        {
            ListOfBullets = new List<AAmmunition>();
        }

        public override void LoadContent()
        {
            _bulletModel = Global.ContentManager.Load<Model>("Models/bullet");
        }

        public override void Shoot(Vector3 position, Vector3 direction, float travelspeed)
        {
            RailBullet bullet = new RailBullet(position, direction, travelspeed);
            bullet.LoadContent(_bulletModel);
            bullet.Initialize();
            ListOfBullets.Add(bullet);
        }
    }
}
