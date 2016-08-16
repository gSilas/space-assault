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


        protected Model BulletModel;
        protected Model BulletModel2;

        protected TimeSpan GlobalTimeSpan;
        protected TimeSpan CoolDownTime;

        protected TimeSpan LastShotTime;

        public abstract void Initialize();

        public abstract void LoadContent();

        //Schusslogik für einzelne Waffen
        public abstract bool Shoot(Vector3 position, Matrix droneRotateMatrix, float travelspeed, ref List<Bullet> bulletList);
        //public abstract bool Shoot2(Vector3 position, Matrix droneRotateMatrix, float travelspeed);

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
