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

        public bool Shoot(GameTime gameTime, BulletType bullet, int damage, Vector3 position, Matrix droneRotateMatrix, ref List<Bullet> bulletList)
        {
            Bullets curBullet;
            switch (bullet)
            {
                case BulletType.YellowLazer:
                    curBullet = new Bullets("Content/Media/Effects/Laser_Shoot.wav", "Models/laser", 6f, false);
                    break;
                case BulletType.BlueLazer:
                    curBullet = new Bullets("Content/Media/Effects/Laser_Shoot.wav", "Models/laser2", 6f, false);
                    break;
                case BulletType.PhotonBomb:
                    curBullet = new Bullets("", "Models/bullet", 0.7f, true);
                    break;
                case BulletType.EnemyLazer:
                    curBullet = new Bullets("Content/Media/Effects/Laser_Shoot.wav", "Models/laser", 3f, false);
                    break;
                default:
                    curBullet = new Bullets("Content/Media/Effects/Laser_Shoot.wav", "Models/laser", 6f, false);
                    break;
            }
            if (gameTime.TotalGameTime > _lastShotTime.Add(_coolDownTime))
            {
                bulletList.Add(new Bullet(curBullet._soundFile, curBullet._modelFile, curBullet._moveSpeed, curBullet._canDamageStation, damage, position, droneRotateMatrix));
                _lastShotTime = gameTime.TotalGameTime;
                return true;
            }
            return false;

        }

        public enum BulletType
        {
            YellowLazer,
            BlueLazer,
            PhotonBomb,
            EnemyLazer
        }

        internal class Bullets
        {
            public string _soundFile;
            public string _modelFile;
            public float _moveSpeed;
            public bool _canDamageStation;

            public Bullets(string soundFile, string modelFile, float moveSpeed, bool canDamageStation)
            {
                _soundFile = soundFile;
                _modelFile = modelFile;
                _moveSpeed = moveSpeed;
                _canDamageStation = canDamageStation;
            }
        }
    }
}
