using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Entities.Weapon
{
    public abstract class AWeapon
    {
        protected int Damage;
        protected bool DmgStation;

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
        public abstract bool Shoot(Vector3 position, Matrix droneRotateMatrix, float travelspeed);
        public abstract bool Shoot2(Vector3 position, Matrix droneRotateMatrix, float travelspeed);

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

        public int makeDmg
        {
            get { return Damage; }
            set { Damage = value; }
        }

        public bool CanDamageStation
        {
            get { return DmgStation; }
            set { DmgStation = value; }
        }

    }
}
