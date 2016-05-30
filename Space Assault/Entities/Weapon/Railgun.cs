using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Assault.Entities.Weapon
{
    class Railgun: AWeapon
    {
        public Railgun(String name, int damage, int schussfrequenz, int ammunition, int maxoverheat, int overheatpershot)
        {
            Name=name;
            Damage = damage;
            Schussfrequenz = schussfrequenz;
            Ammunition = ammunition;
            MaxOverheat = maxoverheat;
            OverheatPerShot = overheatpershot;
        }
    }
}
