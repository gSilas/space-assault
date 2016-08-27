﻿using Microsoft.Xna.Framework;
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

        public WaveBuilder(TimeSpan timeBetweenWaves, int enemyCount, int increment)
        {
            _currentWave = new Wave();
            _waveCount++;
            _enemyCount = enemyCount;
            _increment = increment;
            _timeBetweenWaves = timeBetweenWaves;
        }
        public void LoadContent()
        {
            _dialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width-450, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 178, 128,384, 8, false, true);
            _currentWave.LoadContent(_enemyCount);
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
                    _currentWave.LoadContent(_enemyCount);
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
                    _dialog.Draw("Wave " + _waveCount + " ended!\n" + "You destroyed " + _enemyCount.ToString() + " ships!\n\n" + (-gameTime.TotalGameTime.Subtract((_timeOfEmptyness.Add(_timeBetweenWaves))).Seconds).ToString() + " until next wave!");               
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
        public void LoadContent(int count)
        {
            int bCount = (int)(count * 0.8f);
            _boids.addRandomBoids(count-bCount, Boids.EnemyType.Fighter);
            _boids.addRandomBoids(bCount, Boids.EnemyType.Bomber);
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
