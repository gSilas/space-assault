using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceAssault.Entities;

namespace SpaceAssault.Utils
{
    public class Weapon
    {
        protected TimeSpan _globalTimeSpan;
        protected TimeSpan _coolDownTime;
        protected TimeSpan _lastShotTime;

        public Weapon(double coolDownMilis)
        {
            _coolDownTime = TimeSpan.FromMilliseconds(coolDownMilis);
        }

        public void Initialize()
        {
            _globalTimeSpan = TimeSpan.FromSeconds(0);
            _lastShotTime = TimeSpan.FromSeconds(0);
        }

        public bool Shoot(GameTime gameTime, Bullet.BulletType bullet, int damage, Vector3 position, Matrix droneRotateMatrix, ref List<Bullet> bulletList)
        {
            if (gameTime.TotalGameTime > _lastShotTime.Add(_coolDownTime))
            {
                bulletList.Add(new Bullet(bullet, damage, position, droneRotateMatrix));
                _lastShotTime = gameTime.TotalGameTime;
                return true;
            }
            return false;

        }
    }
}
