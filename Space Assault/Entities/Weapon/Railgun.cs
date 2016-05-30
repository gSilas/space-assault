using System;
using Microsoft.Xna.Framework;

namespace Space_Assault.Entities.Weapon
{
    class Railgun: AWeapon
    {
        public Railgun(String name, int damage, int schussfrequenz, AAmmunition typeOfAmmu, int maxoverheat, int overheatpershot){
            Name=name;
            Damage = damage;
            Schussfrequenz = schussfrequenz;
            TypeOfAmmu = typeOfAmmu;
            MaxOverheat = maxoverheat;
            OverheatPerShot = overheatpershot;
        }


        public override void shoot(Vector3 position, Vector3 direction, Vector3 travelspeed)
        {
            NormalBullet bullet= new NormalBullet(position,direction,travelspeed);
        }
    }
}
