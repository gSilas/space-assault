using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Utils;

namespace Space_Assault.Entities.Weapon
{
    public abstract class AWeapon
    {
        //Basic
        public String Name;
        public int AttackSpeed;
        public int Damage;
        public int Ammunition;
        //Overheat
        public int maxOverheat;
        public int overheatPerShot;
        

    }
}
