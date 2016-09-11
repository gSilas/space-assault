using Microsoft.Xna.Framework;
using SpaceAssault.Entities;
using SpaceAssault.Screens.UI;
using System;
using System.Collections.Generic;

namespace SpaceAssault.Utils
{
    class WaveBuilder
    {
        private Wave _currentWave;
        private TimeSpan _timeBetweenWaves;
        private TimeSpan _timeOfEmptyness;
        private int _waveCount;
        private int _max;
        private bool _timeSet;
        private Dialog _dialog;
        private Dialog _waveDialog;

        public WaveBuilder(TimeSpan timeBetweenWaves, int maxwave)
        {
            _currentWave = new Wave(1);
            _waveCount++;
            _max = maxwave;
            _timeBetweenWaves = timeBetweenWaves;
        }
        public void LoadContent()
        {
            _waveDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width - 380, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 232, 48, 320, 8, false, true);
            _dialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width - 380, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 178, 128, 320, 8, false, true);
            _currentWave.LoadContent();
            _dialog.LoadContent();
            _waveDialog.LoadContent();
        }
        public bool HasEnded = false;
        public List<AEnemys> ShipList { get { return _currentWave.ShipList; } }
        public List<Bullet> BulletList { get { return _currentWave.BulletList; } }
        public void Update(GameTime gameTime, ref AsteroidBuilder asteroidField, ref DroneBuilder droneFleet)
        {
            _currentWave.Update(gameTime, ref asteroidField, ref droneFleet);

            if (_currentWave.ShipList.Count <= 0)
            {
                if (!_timeSet)
                {
                    _timeOfEmptyness = gameTime.TotalGameTime.Duration();
                    _timeSet = true;
                }
                if(gameTime.TotalGameTime > (_timeOfEmptyness.Add(_timeBetweenWaves)) && _waveCount < _max)
                {
                    _waveCount++;
                    _currentWave.UnLoadContent();
                    _currentWave = new Wave(_waveCount);
                    _currentWave.LoadContent();
                    _timeSet = false;
                }
                else if(gameTime.TotalGameTime > (_timeOfEmptyness.Add(_timeBetweenWaves)) && _waveCount >= _max)
                {
                    HasEnded = true;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            _currentWave.Draw();
            _waveDialog.Draw("Wave " + (_waveCount).ToString() + "\n" + _currentWave.ShipList.Count + " Ships remaining");

            if (_currentWave.ShipList.Count <= 0 && _waveCount <= _max)
            {
                if (_waveCount == _max)
                {
                    _dialog.Draw("You have defeated the enemy threat!\nGood Job!");
                }
                else
                {
                    if (gameTime.TotalGameTime > (_timeOfEmptyness.Add(_timeBetweenWaves)).Subtract(TimeSpan.FromSeconds(5d)))
                        _dialog.Draw("Wave " + (_waveCount + 1) + " coming\n\n\n\n\n\n" + (-gameTime.TotalGameTime.Subtract((_timeOfEmptyness.Add(_timeBetweenWaves))).Seconds).ToString() + " until next wave!");
                    else
                        _dialog.Draw("Wave " + _waveCount + " ended!\n\n\n\n\n\n" + (-gameTime.TotalGameTime.Subtract((_timeOfEmptyness.Add(_timeBetweenWaves))).Seconds).ToString() + " until next wave!");

                }
            }
        }
    }

    class Wave
    {
        private Boids _boids;
        private int _waveNumber = 1;
        public List<AEnemys> ShipList { get { return _boids.collisionObjects; } }
        public List<Bullet> BulletList { get { return _boids.bullets; } }
        public Wave(int waveNumber)
        {
            _boids = new Boids();
            _waveNumber = waveNumber;
        }
        public void UnLoadContent()
        {
            _boids.UnLoadContent();
        }
        public void LoadContent()
        {
            _boids.LoadContent();
            switch (_waveNumber)
            {
                case 0:
                    Global.EnemyFighterHealth = 30;
                    Global.EnemyFighterDamage = 5;
                    Global.EnemyFighterKillMoney = 25;
      
                    Global.EnemyBomberHealth = 60;
                    Global.EnemyBomberDamage = 100;
                    Global.EnemyBomberKillMoney = 50;

                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber2);
                    break;

                case 1:
                    Global.EnemyColor = Color.LightGreen;
                    Global.EnemyFighterColor = Color.DarkGreen;
                    Global.EnemyBomberColor = Color.DarkGreen;

                    Global.EnemyFighterHealth = 30;
                    Global.EnemyFighterDamage = 5;
                    Global.EnemyFighterKillMoney = 25;

                    Global.EnemyFighter3Health = 30;
                    Global.EnemyFighter3Damage = 5;
                    Global.EnemyFighter3KillMoney = 25;

                    Global.EnemyBomber2Health = 60;
                    Global.EnemyBomber2Damage = 200;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber2);
                    break;
                case 2:
                    Global.EnemyColor = Color.LightGreen;
                    Global.EnemyFighterColor = Color.DarkGreen;
                    Global.EnemyBomberColor = Color.DarkGreen;

                    Global.EnemyFighterHealth = 70;
                    Global.EnemyFighterDamage = 5;
                    Global.EnemyFighterKillMoney = 25;

                    Global.EnemyFighter3Health = 70;
                    Global.EnemyFighter3Damage = 5;
                    Global.EnemyFighter3KillMoney = 25;

                    Global.EnemyBomber2Health = 100;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(3, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(3, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(3, Boids.EnemyType.Bomber2);
                    break;

                case 3:
                    Global.EnemyColor = Color.LightGreen;
                    Global.EnemyFighterColor = Color.DarkGreen;
                    Global.EnemyBomberColor = Color.DarkGreen;

                    Global.EnemyFighterHealth = 70;
                    Global.EnemyFighterDamage = 10;
                    Global.EnemyFighterKillMoney = 25;

                    Global.EnemyFighter3Health = 70;
                    Global.EnemyFighter3Damage = 10;
                    Global.EnemyFighter3KillMoney = 25;

                    Global.EnemyBomber2Health = 100;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    break;

                case 4:
                    Global.EnemyColor = Color.DarkGreen;
                    Global.EnemyFighterColor = Color.LightYellow;
                    Global.EnemyBomberColor = Color.LightYellow;

                    Global.EnemyFighterHealth = 70;
                    Global.EnemyFighterDamage = 15;
                    Global.EnemyFighterKillMoney = 25;

                    Global.EnemyFighter3Health = 70;
                    Global.EnemyFighter3Damage = 15;
                    Global.EnemyFighter3KillMoney = 25;

                    Global.EnemyBomber2Health = 100;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    break;

                case 5:
                    Global.EnemyColor = Color.DarkGreen;
                    Global.EnemyFighterColor = Color.LightYellow;
                    Global.EnemyBomberColor = Color.DarkGreen;

                    Global.EnemyFighterHealth = 100;
                    Global.EnemyFighterDamage = 15;
                    Global.EnemyFighterKillMoney = 60;

                    //Enemy Elite Pilot
                    Global.EnemyFighter2Health = 350;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 100;
                    Global.EnemyFighter3Damage = 15;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 150;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(6, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber2);
                    break;

                case 6:
                    Global.EnemyColor = Color.LightYellow;
                    Global.EnemyFighterColor = Color.Orange;
                    Global.EnemyBomberColor = Color.LightYellow;

                    Global.EnemyFighterHealth = 160;
                    Global.EnemyFighterDamage = 15;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 350;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 160;
                    Global.EnemyFighter3Damage = 15;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 150;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 100;

                    _boids.addRandomBoids(6, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(6, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber2);
                    break;

                case 7:
                    Global.EnemyColor = Color.LightYellow;
                    Global.EnemyFighterColor = Color.Orange;
                    Global.EnemyBomberColor = Color.LightYellow;

                    Global.EnemyFighterHealth = 150;
                    Global.EnemyFighterDamage = 20;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 150;
                    Global.EnemyFighter3Damage = 20;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 150;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 100;

                    _boids.addRandomBoids(8, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(8, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(7, Boids.EnemyType.Bomber2);                    
                    break;

                case 8:
                    Global.EnemyColor = Color.Orange;
                    Global.EnemyFighterColor = Color.DarkOrange;
                    Global.EnemyBomberColor = Color.Orange;

                    Global.EnemyFighterHealth = 200;
                    Global.EnemyFighterDamage = 20;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 350;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 150;
                    Global.EnemyFighter3Damage = 20;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 200;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 100;

                    _boids.addRandomBoids(8, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(8, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(8, Boids.EnemyType.Bomber2);
                    break;

                case 9:
                    Global.EnemyColor = Color.Orange;
                    Global.EnemyFighterColor = Color.DarkGoldenrod;
                    Global.EnemyBomberColor = Color.Orange;

                    Global.EnemyFighterHealth = 200;
                    Global.EnemyFighterDamage = 30;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 200;
                    Global.EnemyFighter3Damage = 30;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 250;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 100;

                    _boids.addRandomBoids(9, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(9, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(8, Boids.EnemyType.Bomber2);
                    break;

                case 10:
                    Global.EnemyColor = Color.DarkOrange;
                    Global.EnemyFighterColor = Color.Red;
                    Global.EnemyBomberColor = Color.DarkOrange;
                    //First TIme BIg Bomber

                    Global.EnemyFighterHealth = 200;
                    Global.EnemyFighterDamage = 30;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 200;
                    Global.EnemyFighter3Damage = 30;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 2500;
                    Global.EnemyBomberDamage = 4000;
                    Global.EnemyBomberKillMoney = 1000;

                    Global.EnemyBomber2Health = 300;
                    Global.EnemyBomber2Damage = 600;
                    Global.EnemyBomber2KillMoney = 100;

                    _boids.addRandomBoids(9, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(9, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(1, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    break;

                case 11:
                    Global.EnemyColor = Color.DarkOrange;
                    Global.EnemyFighterColor = Color.Crimson;
                    Global.EnemyBomberColor = Color.DarkOrange;

                    Global.EnemyFighterHealth = 200;
                    Global.EnemyFighterDamage = 30;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 200;
                    Global.EnemyFighter3Damage = 30;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 2000;
                    Global.EnemyBomberDamage = 2000;
                    Global.EnemyBomberKillMoney = 500;

                    Global.EnemyBomber2Health = 400;
                    Global.EnemyBomber2Damage = 600;
                    Global.EnemyBomber2KillMoney = 100;

                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    break;

                case 12:
                    Global.EnemyColor = Color.Crimson;
                    Global.EnemyFighterColor = Color.Red;
                    Global.EnemyBomberColor = Color.Crimson;

                    Global.EnemyFighterHealth = 250;
                    Global.EnemyFighterDamage = 30;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 250;
                    Global.EnemyFighter3Damage = 30;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 2400;
                    Global.EnemyBomberDamage = 2000;
                    Global.EnemyBomberKillMoney = 1000;

                    Global.EnemyBomber2Health = 400;
                    Global.EnemyBomber2Damage = 600;
                    Global.EnemyBomber2KillMoney = 100;

                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(6, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(3, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    break;

                case 13:
                    Global.EnemyColor = Color.Crimson;
                    Global.EnemyFighterColor = Color.Red;
                    Global.EnemyBomberColor = Color.MediumVioletRed;

                    Global.EnemyFighterHealth = 250;
                    Global.EnemyFighterDamage = 30;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 250;
                    Global.EnemyFighter3Damage = 30;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 2500;
                    Global.EnemyBomberDamage = 1500;
                    Global.EnemyBomberKillMoney = 1000;

                    Global.EnemyBomber2Health = 400;
                    Global.EnemyBomber2Damage = 600;
                    Global.EnemyBomber2KillMoney = 100;

                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    break;

                case 14:
                    Global.EnemyColor = Color.Crimson;
                    Global.EnemyFighterColor = Color.Red;
                    Global.EnemyBomberColor = Color.MediumVioletRed;

                    Global.EnemyFighterHealth = 200;
                    Global.EnemyFighterDamage = 30;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 200;
                    Global.EnemyFighter3Damage = 30;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 2000;
                    Global.EnemyBomberDamage = 1000;
                    Global.EnemyBomberKillMoney = 1000;

                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(6, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(6, Boids.EnemyType.Bomber);
                    break;

                case 15:
                    Global.EnemyColor = Color.Crimson;
                    Global.EnemyFighterColor = Color.Red;
                    Global.EnemyBomberColor = Color.Red;

                    Global.EnemyFighterHealth = 200;
                    Global.EnemyFighterDamage = 30;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyBomberHealth = 2000;
                    Global.EnemyBomberDamage = 1000;
                    Global.EnemyBomberKillMoney = 1000;

                    Global.EnemyBossHealth = 5000;
                    Global.EnemyBossDamage = 200;
                    Global.EnemyBossKillMoney = 5000;

                    Global.EnemyAttackTowerHealth = 2000;
                    Global.EnemyAttackTowerDamage = 15;
                    Global.EnemyAttackTowerKillMoney = 1250;

                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(6, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(1,Boids.EnemyType.Boss);
                    break;

                default:

                    break;
            }
           
        }
        public void Update(GameTime gameTime, ref AsteroidBuilder asteroidField, ref DroneBuilder droneFleet)
        {
            List<AEntity> avoidList = new List<AEntity>();
            avoidList.AddRange(asteroidField._asteroidList);
            avoidList.AddRange(droneFleet._droneShips);
            _boids.Update(gameTime, avoidList, droneFleet.GetActiveDrone());
        }
        public void Draw()
        {
            _boids.Draw();
        }
    }
}
