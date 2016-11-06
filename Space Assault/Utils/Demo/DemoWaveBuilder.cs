using Microsoft.Xna.Framework;
using SpaceAssault.Entities;
using SpaceAssault.Screens.UI;
using System;
using System.Collections.Generic;

namespace SpaceAssault.Utils.Demo
{
    class DemoWaveBuilder
    {
        private Wave _currentWave;
        private int _timeBetweenWaves;
        private int _time;
        private int _waveCount;
        private int _max;
        private Dialog _dialog;
        private Dialog _waveDialog;

        public DemoWaveBuilder(int timeInMilliseconds, int maxwave)
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
        public int WaveCount {
            get
            {
                return _waveCount;
            }
            set
            {
                if (value > _max-1)
                    _waveCount = 2;
                else
                    _waveCount = value;
            }
        }
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
                else if(_waveCount >= _max)
                {
                    HasEnded = true;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            _currentWave.Draw();

            switch (Wave._waveNumber)
            {
                case 1:
                    _waveDialog.Draw("Wave " + (_waveCount).ToString() + " Example \n" + _currentWave.ShipList.Count + " Ships remaining");
                    break;
                case 2:
                    _waveDialog.Draw("Wave " + (_waveCount).ToString() + " original GameWave \n" + _currentWave.ShipList.Count + " Ships remaining");
                    break;
                case 3:
                    _waveDialog.Draw("Wave " + (_waveCount).ToString() + " Death \n" + _currentWave.ShipList.Count + " Ships remaining");
                    break;
            }

            if (_currentWave.ShipList.Count <= 0 && _waveCount <= _max)
            {
                if (_time <= _timeBetweenWaves/2)
                   _dialog.Draw("Wave " + (_waveCount + 1) + " coming\n\n\n\n\n\n" + (_time / 1000).ToString() + " until next wave!");
                else
                   _dialog.Draw("Wave " + _waveCount + " ended!\n\n\n\n\n\n" + (_time / 1000).ToString() + " until next wave!");
            }
        }
    }

    class Wave
    {
        private Boids _boids;
        public static int _waveNumber = 1;
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
                    Global.EnemyFighterColor = Color.LightGreen;

                    Global.EnemyFighterHealth = 10;
                    Global.EnemyFighterDamage = 0;
                    Global.EnemyFighterKillMoney = 8000;

                    Global.EnemyBomber2Health = 10;
                    Global.EnemyBomber2Damage = 0;
                    Global.EnemyBomber2KillMoney = 8000;

                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber2);
                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter);
                    break;

                case 2:
                    Global.EnemyColor = Color.Crimson;
                    Global.EnemyFighterColor = Color.Red;
                    Global.EnemyBomberColor = Color.Red;
                    Global.EnemyBossColor = Color.DarkGoldenrod;

                    Global.EnemyFighterHealth = 50;
                    Global.EnemyFighterDamage = 0;
                    Global.EnemyFighterKillMoney = 2000;

                    Global.EnemyFighter2Health = 50;
                    Global.EnemyFighter2Damage = 0;
                    Global.EnemyFighter2KillMoney = 2000;

                    Global.EnemyBomberHealth = 50;
                    Global.EnemyBomberDamage = 0;
                    Global.EnemyBomberKillMoney = 2000;

                    Global.EnemyBossHealth = 50;
                    Global.EnemyBossDamage = 0;
                    Global.EnemyBossKillMoney = 2000;

                    Global.EnemyAttackTowerHealth = 50;
                    Global.EnemyAttackTowerDamage = 0;
                    Global.EnemyAttackTowerKillMoney = 2000;

                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(6, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(1,Boids.EnemyType.Boss);
                    break;

                case 3:
                    Global.EnemyColor = Color.Crimson;
                    Global.EnemyFighterColor = Color.Red;
                    Global.EnemyBomberColor = Color.Red;

                    Global.EnemyBomberHealth = 100000;
                    Global.EnemyBomberDamage = 1000;
                    Global.EnemyBomberKillMoney = 0;

                    _boids.addRandomBoids(10, Boids.EnemyType.Bomber);
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
