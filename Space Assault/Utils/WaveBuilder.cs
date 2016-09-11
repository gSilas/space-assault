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
        private int _timeBetweenWaves;
        private int _time;
        private int _waveCount;
        private int _max;
        private Dialog _dialog;
        private Dialog _waveDialog;

        public WaveBuilder(int timeInMilliseconds, int maxwave)
        {
            _currentWave = new Wave(1);
            _waveCount++;
            _max = maxwave;
            _timeBetweenWaves = timeInMilliseconds;
            _time = timeInMilliseconds;
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
                _time -= gameTime.ElapsedGameTime.Milliseconds;
                if(_time <= 0 && _waveCount < _max)
                {
                    _waveCount++;
                    _currentWave.UnLoadContent();
                    _currentWave = new Wave(_waveCount);
                    _currentWave.LoadContent();
                    _time = _timeBetweenWaves;
                }
                else if(_time <= 0 && _waveCount >= _max)
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
                    if (_time <= _timeBetweenWaves/2)
                        _dialog.Draw("Wave " + (_waveCount + 1) + " coming\n\n\n\n\n\n" + (_time / 1000).ToString() + " until next wave!");
                    else
                        _dialog.Draw("Wave " + _waveCount + " ended!\n\n\n\n\n\n" + (_time / 1000).ToString() + " until next wave!");

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
                    Global.EnemyBomberColor = Color.DarkOliveGreen;

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
                    Global.EnemyBomberColor = Color.DarkOliveGreen;

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
                    Global.EnemyBomberColor = Color.DarkOliveGreen;

                    Global.EnemyFighterHealth = 70;
                    Global.EnemyFighterDamage = 10;
                    Global.EnemyFighterKillMoney = 25;

                    Global.EnemyFighter3Health = 70;
                    Global.EnemyFighter3Damage = 10;
                    Global.EnemyFighter3KillMoney = 25;

                    Global.EnemyBomber2Health = 100;
                    Global.EnemyBomber2Damage = 800;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    break;

                case 4:
                    Global.EnemyColor = Color.DarkGreen;
                    Global.EnemyFighterColor = Color.YellowGreen;
                    Global.EnemyBomberColor = Color.DarkGreen;

                    Global.EnemyFighterHealth = 100;
                    Global.EnemyFighterDamage = 15;
                    Global.EnemyFighterKillMoney = 25;

                    Global.EnemyFighter3Health = 100;
                    Global.EnemyFighter3Damage = 15;
                    Global.EnemyFighter3KillMoney = 25;

                    Global.EnemyBomber2Health = 150;
                    Global.EnemyBomber2Damage = 800;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    break;

                case 5:
                    Global.EnemyColor = Color.DarkGreen;
                    Global.EnemyFighterColor = Color.YellowGreen;
                    Global.EnemyBomberColor = Color.DarkGreen;

                    Global.EnemyFighterHealth = 180;
                    Global.EnemyFighterDamage = 20;
                    Global.EnemyFighterKillMoney = 60;

                    //Enemy Elite Pilot
                    Global.EnemyFighter2Health = 800;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 500;

                    Global.EnemyFighter3Health = 180;
                    Global.EnemyFighter3Damage = 20;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 300;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(6, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber2);
                    break;

                case 6:
                    Global.EnemyColor = Color.YellowGreen;
                    Global.EnemyFighterColor = Color.Orange;
                    Global.EnemyBomberColor = Color.YellowGreen;

                    Global.EnemyFighterHealth = 180;
                    Global.EnemyFighterDamage = 25;
                    Global.EnemyFighterKillMoney = 60;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 180;
                    Global.EnemyFighter3Damage = 25;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 300;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(6, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(6, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber2);
                    break;

                case 7:
                    Global.EnemyColor = Color.YellowGreen;
                    Global.EnemyFighterColor = Color.Orange;
                    Global.EnemyBomberColor = Color.YellowGreen;

                    Global.EnemyFighterHealth = 240;
                    Global.EnemyFighterDamage = 25;
                    Global.EnemyFighterKillMoney = 60;

                    Global.EnemyFighter2Health = 500;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 240;
                    Global.EnemyFighter3Damage = 25;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 300;
                    Global.EnemyBomber2Damage = 500;
                    Global.EnemyBomber2KillMoney = 50;

                    _boids.addRandomBoids(8, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(8, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(7, Boids.EnemyType.Bomber2);                    
                    break;

                case 8:
                    Global.EnemyColor = Color.Orange;
                    Global.EnemyFighterColor = Color.DarkOrange;
                    Global.EnemyBomberColor = Color.Orange;

                    Global.EnemyFighterHealth = 290;
                    Global.EnemyFighterDamage = 30;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 600;
                    Global.EnemyFighter2Damage = 50;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 290;
                    Global.EnemyFighter3Damage = 30;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 400;
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

                    Global.EnemyFighterHealth = 290;
                    Global.EnemyFighterDamage = 35;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 600;
                    Global.EnemyFighter2Damage = 60;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 290;
                    Global.EnemyFighter3Damage = 35;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomber2Health = 400;
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

                    Global.EnemyFighterHealth = 300;
                    Global.EnemyFighterDamage = 40;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 600;
                    Global.EnemyFighter2Damage = 60;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 300;
                    Global.EnemyFighter3Damage = 40;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 500;
                    Global.EnemyBomberDamage = 600;
                    Global.EnemyBomberKillMoney = 400;

                    Global.EnemyBomber2Health = 400;
                    Global.EnemyBomber2Damage = 500;
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

                    Global.EnemyFighterHealth = 300;
                    Global.EnemyFighterDamage = 40;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 650;
                    Global.EnemyFighter2Damage = 65;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 300;
                    Global.EnemyFighter3Damage = 40;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 500;
                    Global.EnemyBomberDamage = 600;
                    Global.EnemyBomberKillMoney = 400;

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

                    Global.EnemyFighterHealth = 300;
                    Global.EnemyFighterDamage = 40;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 650;
                    Global.EnemyFighter2Damage = 65;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 300;
                    Global.EnemyFighter3Damage = 40;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 500;
                    Global.EnemyBomberDamage = 600;
                    Global.EnemyBomberKillMoney = 400;

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

                    Global.EnemyFighterHealth = 300;
                    Global.EnemyFighterDamage = 40;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 650;
                    Global.EnemyFighter2Damage = 65;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 300;
                    Global.EnemyFighter3Damage = 40;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 500;
                    Global.EnemyBomberDamage = 600;
                    Global.EnemyBomberKillMoney = 400;

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

                    Global.EnemyFighterHealth = 300;
                    Global.EnemyFighterDamage = 40;
                    Global.EnemyFighterKillMoney = 70;

                    Global.EnemyFighter2Health = 700;
                    Global.EnemyFighter2Damage = 80;
                    Global.EnemyFighter2KillMoney = 250;

                    Global.EnemyFighter3Health = 300;
                    Global.EnemyFighter3Damage = 40;
                    Global.EnemyFighter3KillMoney = 50;

                    Global.EnemyBomberHealth = 500;
                    Global.EnemyBomberDamage = 600;
                    Global.EnemyBomberKillMoney = 400;

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

                    Global.EnemyBomberHealth = 500;
                    Global.EnemyBomberDamage = 600;
                    Global.EnemyBomberKillMoney = 400;

                    Global.EnemyBossHealth = 6000;
                    Global.EnemyBossDamage = 100;
                    Global.EnemyBossKillMoney = 5000;

                    Global.EnemyAttackTowerHealth = 1;
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
