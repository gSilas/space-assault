using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities.Weapon
{
    public abstract class AWeapon
    {
        //draw
        protected List<Bullet> ListOfBullets;

        protected Model _bulletModel;

        public abstract void Initialize();

        public abstract void LoadContent();

        //Schusslogik für einzelne Waffen
        public abstract void Shoot(Vector3 position, Vector3 direction, float travelspeed);

        public void Update(GameTime gameTime)
        {
            foreach (Bullet bullet in ListOfBullets)
            {
                bullet.Update(gameTime);
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
