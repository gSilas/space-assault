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
        public List<Bullet> RemoveListOfBullets;

        protected Model BulletModel;
        protected Model BulletModel2;

        protected TimeSpan CoolDownTime;

        protected TimeSpan GlobalTimeSpan;

        protected TimeSpan LastShotTime;

        public abstract void Initialize();

        public abstract void LoadContent();

        //Schusslogik für einzelne Waffen
        public abstract void Shoot(Vector3 position, Matrix droneRotateMatrix, float travelspeed);
        public abstract void Shoot2(Vector3 position, Matrix droneRotateMatrix, float travelspeed);

        public void Update(GameTime gameTime)
        {
            GlobalTimeSpan = gameTime.TotalGameTime;
            foreach (Bullet bullet in ListOfBullets)
            {
                bullet.Update(gameTime);
                if (bullet.Bulletlife < 0)
                {
                    RemoveListOfBullets.Add(bullet);
                }
            }
            foreach (Bullet bullet in RemoveListOfBullets)
            {
                ListOfBullets.Remove(bullet);
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
