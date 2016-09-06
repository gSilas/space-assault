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
        private Wave _lastWave;
        private TimeSpan _timeBetweenWaves;
        private TimeSpan _timeOfEmptyness;
        private int _enemyCount;
        private int _increment;
        private int _waveCount;
        private bool _timeSet;
        private Dialog _dialog;

        int[] fighterStats;
        int[] bomberStats;

        public WaveBuilder(TimeSpan timeBetweenWaves, int enemyCount, int increment)
        {
            _currentWave = new Wave();
            _waveCount++;
            _enemyCount = enemyCount;
            _increment = increment;
            _timeBetweenWaves = timeBetweenWaves;
            fighterStats = new int[3] { 5, 5, 20 };
            bomberStats = new int[3] { 5, 5, 20 };
        }
        public void LoadContent()
        {
            _dialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width-450, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 178, 128,384, 8, false, true);
            _currentWave.LoadContent(_enemyCount, ref fighterStats, ref bomberStats);
            _dialog.LoadContent();
        }
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
                if(gameTime.TotalGameTime > (_timeOfEmptyness.Add(_timeBetweenWaves)))
                {
                    _currentWave = new Wave();
                    _enemyCount += _increment;
                    _currentWave.LoadContent(_enemyCount, ref fighterStats, ref bomberStats);
                    _waveCount++;
                    _timeSet = false;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            _currentWave.Draw();

            if (_currentWave.ShipList.Count <= 0)
            {
                if(gameTime.TotalGameTime > (_timeOfEmptyness.Add(_timeBetweenWaves)).Subtract(TimeSpan.FromSeconds(5d)))
                    _dialog.Draw("Wave " + (_waveCount + 1) + " coming\n"+(_enemyCount +_increment).ToString() + " ships incoming!\n\n" + (-gameTime.TotalGameTime.Subtract((_timeOfEmptyness.Add(_timeBetweenWaves))).Seconds).ToString() + " until next wave!");
                else
                    _dialog.Draw("Wave " + _waveCount + " ended!\n" + "You destroyed " + _enemyCount.ToString() + " ships!\n" +"Bomber Stats: " + bomberStats[0] + " " + bomberStats[1] + " " + bomberStats[2] + "\n" + "Fighter Stats: " + fighterStats[0] + " " + fighterStats[1] + " " + fighterStats[2] + "\n"+ (-gameTime.TotalGameTime.Subtract((_timeOfEmptyness.Add(_timeBetweenWaves))).Seconds).ToString() + " until next wave!");               
            }
        }
    }

    class Wave
    {
        private Boids _boids;
        public Wave()
        {
            _boids = new Boids();
        }
        public void LoadContent(int count,ref int[] fighterStats,ref int[] bomberStats)
        {
            int bCount = (int)(count * 0.25f);
            int f2count = (int)(count * 0.25f);
            _boids.addRandomBoids(count - bCount - f2count, Boids.EnemyType.Fighter);
            _boids.addRandomBoids(f2count, Boids.EnemyType.Fighter2);
            _boids.addRandomBoids(bCount, Boids.EnemyType.Bomber);

            foreach (var ship in ShipList)
            {
                if(ship.GetType() == typeof(EnemyFighter))
                {
                    ship.Health = fighterStats[0] + 5;
                    ship.gunMakeDmg = fighterStats[1] + 2;
                    ship.KillMoney = fighterStats[2] + 5;
                    fighterStats[0] = ship.Health;
                    fighterStats[1] = ship.gunMakeDmg;
                    fighterStats[2] = ship.KillMoney;
                }
                else if (ship.GetType() == typeof(EnemyFighter2))
                {
                    ship.Health = fighterStats[0];
                    ship.gunMakeDmg = fighterStats[1];
                    ship.KillMoney = fighterStats[2];
                }
                else if (ship.GetType() == typeof(EnemyBomber))
                {
                    ship.Health = bomberStats[0] + 10;
                    ship.gunMakeDmg = bomberStats[1] + 5;
                    ship.KillMoney = bomberStats[2] + 10;
                    bomberStats[0] = ship.Health;
                    bomberStats[1] = ship.gunMakeDmg;
                    bomberStats[2] = ship.KillMoney;
                }                 
            }
        }
        public List<AEnemys> ShipList { get { return _boids._ships; } }
        public List<Bullet> BulletList { get { return _boids._bullets; } }
        public void Update(GameTime gameTime, ref AsteroidBuilder asteroidField, ref DroneBuilder droneFleet)
        {
            List<AEntity> avoidList = new List<AEntity>();
            avoidList.AddRange(asteroidField._asteroidList);
            avoidList.AddRange(droneFleet._droneShips);
            _boids.Update(gameTime, avoidList);
        }
        public void Draw()
        {
            _boids.Draw();
        }
    }
}
