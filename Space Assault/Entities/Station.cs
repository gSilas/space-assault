using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using System.Collections.Generic;

namespace SpaceAssault.Entities
{
    class Station : AEntity
    {
        private float _angle;
        private bool _up;
        public int _health;
        public int _maxhealth;

        private bool _isDead;

        public int _maxShield;
        public int _shield;
        private int _shieldpast;
        private TimeSpan _shieldrefreshdelay;
        private bool _wasDamaged = false;

        public Weapon Gun;
        public int makeDmg=10;
        private List<Bullet> _bulletlist; 


        public Station(Vector3 position, float angle)
        {
            _angle = angle;
            Position = position;
            Scale = 0.5f;
            Gun = new Weapon(1000);

            _up = true;
            _maxhealth = 10000;
            _health = _maxhealth;
            _maxShield = 5000;
            _shield = _maxShield;
            _bulletlist = new List<Bullet>();
        }

        public bool IsDead
        {
            get { return _isDead; }
            protected set { _isDead = value; }
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/station");
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.LoadContent();
            for (int i = 0; i < Spheres.Length; i++)
            {
                Spheres[i].Radius = Spheres[i].Radius * 0.75f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_health <= 0) IsDead = true;
            //Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.Update(gameTime);
            _angle += 0.005f;
            if (Position.Y < 1 && _up)
                Position += new Vector3(0, 0.002f, 0);
            else if (Position.Y < 0)
                _up = true;
            else
            {
                Position -= new Vector3(0, 0.002f, 0);
                _up = false;
            }
            RotationMatrix = Matrix.CreateRotationY(_angle);

            if (gameTime.TotalGameTime > (_shieldrefreshdelay.Add(TimeSpan.FromSeconds(3))) && !IsDead)
            {

                if (_shield == _shieldpast)
                    _wasDamaged = false;
                _shieldpast = _shield;
                _shieldrefreshdelay = gameTime.TotalGameTime;
                if (_health < _maxhealth) _health += 100;
            }
            if (_wasDamaged == false && _shield < _maxShield && !IsDead)
                _shield += _maxShield/1400;

            if (makeDmg != 0)
            {
                //Gun.Shoot(gameTime, Bullet.BulletType.BlueLazer, makeDmg, Position, RotationMatrix,ref _bulletlist);
            }
        }
        public void getHit(int howMuch)
        {
            _wasDamaged = true;
            if (_shield >= 0)
                _shield -= howMuch;
            else
            {
                _health -= howMuch;
            }
        }
    }
}
