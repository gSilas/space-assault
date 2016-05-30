using System;
using Microsoft.Xna.Framework;

namespace Space_Assault.Entities.Weapon
{
    public abstract class AWeapon
    {
        //Basic
        public String Name;
        public int Schussfrequenz;
        public int Damage;
        public AAmmunition TypeOfAmmu;
        //Overheat
        public int MaxOverheat;
        public int OverheatPerShot;

        public abstract void shoot(Vector3 position, Vector3 direction, Vector3 travelspeed);

    }
}
