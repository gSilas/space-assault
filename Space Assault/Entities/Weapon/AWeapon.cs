using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Utils;

namespace Space_Assault.Entities.Weapon
{
    public abstract class AWeapon
    {
        //
        //Basic
        public uint Damage;
        public uint Schussfrequenz;
        public uint CooldownOnShoot = 100;
        public bool IsReadyToShoot=true;

        //Overheat
        public uint Overheat = 0;
        public uint MaxOverheat;
        public uint OverheatPerShot;
        public bool IsOverheat = false;

        //draw
        public List<AAmmunition> ListOfBullets;

        //Schusslogik für einzelne Waffen
        public abstract void shoot(Vector3 position, Vector3 direction, float travelspeed);

        public void Draw(Camera camera)
        {
            foreach (AAmmunition bullet in ListOfBullets)
            {
                bullet.Draw(camera);
            }
        }
    }
}
