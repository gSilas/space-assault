using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Entities.Weapon
{
    public abstract class AWeapon
    {
        //draw
        public List<Bullet> ListOfBullets;

        protected Model BulletModel;

        protected TimeSpan CoolDownTime;

        protected TimeSpan GlobalTimeSpan;

        protected TimeSpan LastShotTime;

        public abstract void Initialize();

        public abstract void LoadContent();

        //Schusslogik für einzelne Waffen
        public abstract void Shoot(Vector3 position, Vector3 direction, float travelspeed);

        public void Update(GameTime gameTime)
        {
            GlobalTimeSpan = gameTime.TotalGameTime;
            foreach (Bullet bullet in ListOfBullets)
            {
                bullet.Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach (Bullet bullet in ListOfBullets)
            {
                bullet.Draw();
            }
        }
        
    }
}
