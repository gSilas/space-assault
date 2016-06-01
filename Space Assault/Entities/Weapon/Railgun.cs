
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.Entities.Weapon
{
    class Railgun: AWeapon
    {
        private uint _damage;
        private readonly uint _schussfrequenz;
        private uint _cooldownOnShoot = 100;
        private bool _isReadyToShoot = true;
        //Overheat
        private uint _overheat = 0;
        private readonly uint _maxOverheat;
        private readonly uint _overheatPerShot;
        private bool _isOverheat = false;


        public Railgun(uint damage, uint schussfrequenz, uint maxoverheat, uint overheatpershot)
        {
            LoadContent(Model = Global.ContentManager.Load<Model>("Models/asteroid"));
            _damage = damage;
            _schussfrequenz = schussfrequenz;
            _maxOverheat = maxoverheat;
            _overheatPerShot = overheatpershot;
        }

        public override void shoot(Vector3 position, Vector3 direction, float travelspeed)
        {

            AAmmunition bullet = new AAmmunition(position, direction, travelspeed);


            ListOfBullets = new List<AAmmunition>();
            if (_overheat < _maxOverheat && _isOverheat==false && _isReadyToShoot==true)
            {
               
                ListOfBullets.Add(bullet);
                _overheat += OverheatPerShot;
                
                //wenn die frequenz 100 ist, hat die waffe keinen cooldown) 
                if (_schussfrequenz != 100)
                {
                    _cooldownOnShoot = 0;
                    _isReadyToShoot = false;
                }
                    
            }
            else
            {
                if (_overheat > _maxOverheat)
                {
                    _isOverheat = true;
                    _overheat = _overheat - 3*_overheatPerShot;
                    if (_overheat <= 0)
                        _isOverheat = false;
                }
                
                _overheat = _overheat - 2 * _overheatPerShot;
                if (_isReadyToShoot == false)
                {
                    _cooldownOnShoot += _schussfrequenz;
                    if (_cooldownOnShoot >= 100)
                        _isReadyToShoot = true;
                }
             

            }
            
            Draw();
        }


    }
}
