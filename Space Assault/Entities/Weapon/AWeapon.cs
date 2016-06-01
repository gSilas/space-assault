using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Utils;

namespace Space_Assault.Entities.Weapon
{
    public abstract class AWeapon
    {
        //draw
        public List<AAmmunition> ListOfBullets;
        private Model _bulletModel;

        //Ammunition
        public Vector3 _direction;
        public Vector3 _position;

        public abstract void Initialize();

        public abstract void LoadContent();

        //Schusslogik für einzelne Waffen
        public abstract void shoot(Vector3 position, Vector3 direction, float travelspeed);

        public void Update(GameTime gameTime)
        {
            foreach (AAmmunition bullet in ListOfBullets)
            {
                bullet.Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach (AAmmunition bullet in ListOfBullets)
            {
                bullet.Draw();
            }
        }
        
    }
}
