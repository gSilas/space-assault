
using Microsoft.Xna.Framework;

namespace Space_Assault.Entities.Weapon
{
    class Railgun: AWeapon
    {
        
        public Railgun(uint damage, uint schussfrequenz, uint maxoverheat, uint overheatpershot){
           
            Damage = damage;
            Schussfrequenz = schussfrequenz;
          
            MaxOverheat = maxoverheat;
            OverheatPerShot = overheatpershot;
        }


        public override void shoot(Vector3 position, Vector3 direction, float travelspeed)
        {

            if (Overheat < MaxOverheat && IsOverheat==false && IsReadyToShoot==true)
            {
                NormalBullet bullet = new NormalBullet(position, direction, travelspeed);
                ListOfBullets.Add(bullet);
                Overheat += OverheatPerShot;
                //wenn die frequenz 100 ist, hat die waffe keinen cooldown) 
                if (Schussfrequenz != 100)
                {
                    CooldownOnShoot = 0;
                    IsReadyToShoot = false;
                }
                    
            }
            else
            {
                if (Overheat > MaxOverheat)
                {
                    IsOverheat = true;
                    Overheat = Overheat - 3*OverheatPerShot;
                    if (Overheat <= 0)
                        IsOverheat = false;
                }
                
                Overheat = Overheat - 2 * OverheatPerShot;
                if (IsReadyToShoot == false)
                {
                    CooldownOnShoot += Schussfrequenz;
                    if (CooldownOnShoot >= 100)
                        IsReadyToShoot = true;
                }
             

            }
        }
    }
}
