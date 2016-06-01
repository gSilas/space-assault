using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Utils;

namespace Space_Assault.Entities.Weapon
{
    public abstract class AWeapon:AEntity
    {
        //Weaponstuff
        //Basic
        private uint Damage;
        private uint Schussfrequenz;
        private uint CooldownOnShoot = 100;
        private bool IsReadyToShoot=true;
        //Overheat
        public uint Overheat = 0;
        public uint MaxOverheat;
        public uint OverheatPerShot;
        public bool IsOverheat = false;
        //draw
        public List<AAmmunition> ListOfBullets;

        //Ammunition
        public Vector3 _direction;
        public float _travelspeed;


        public override void Initialize()
        {
            _direction.Normalize();
        }

        public void LoadContent(Model model)
        {
            Model = model;
        }


        public override void LoadContent()
        {
            throw new System.NotImplementedException();
        } 

        public override void Update(GameTime gameTime)
        {
            Position += _direction * _travelspeed;
        }



        //Schusslogik für einzelne Waffen
        public abstract void shoot(Vector3 position, Vector3 direction, float travelspeed);

        public void Draw()
        {
            foreach (AAmmunition bullet in ListOfBullets)
            {
                bullet.Draw();
            }
        }
        
    }
}
