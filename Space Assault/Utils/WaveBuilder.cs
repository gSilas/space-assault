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
                        _dialog.Draw("Wave " + (_waveCount + 1) + " coming\n\n\n\n\n" + (-gameTime.TotalGameTime.Subtract((_timeOfEmptyness.Add(_timeBetweenWaves))).Seconds).ToString() + " until next wave!");
                    else
                        _dialog.Draw("Wave " + _waveCount + " ended!\n\n\n\n\n" + (-gameTime.TotalGameTime.Subtract((_timeOfEmptyness.Add(_timeBetweenWaves))).Seconds).ToString() + " until next wave!");

                }
            }
        }
    }

    class Wave
    {
        private Boids _boids;
        private int _waveNumber = 1;
        public List<AEnemys> ShipList { get { return _boids.ships; } }
        public List<Bullet> BulletList { get { return _boids.bullets; } }
        public Wave(int waveNumber)
        {
            _boids = new Boids();
            _waveNumber = waveNumber;
        }
        public void LoadContent()
        {
            switch (_waveNumber)
            {
                case 0:
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber2);
                    
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 25;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 60;
                            ship.gunMakeDmg = 100;
                            ship.KillMoney = 50;
                        }
                    }
                    break;

                case 1:
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber2);
                    _boids.addRandomBoids(1, Boids.EnemyType.Boss);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 25;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 25;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 60;
                            ship.gunMakeDmg = 200;
                            ship.KillMoney = 50;
                        }
                        else if (ship.GetType() == typeof(EnemyBoss))
                        {
                            ship.Health = 1000;
                            ship.gunMakeDmg = 200;
                            ship.KillMoney = 5000;
                        }
                    }
                    break;
                case 2:
                    _boids.addRandomBoids(4, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(3, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(4, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 25;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 25;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 60;
                            ship.gunMakeDmg = 500;
                            ship.KillMoney = 50;
                        }
                    }
                    break;

                case 3:
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 50;
                        }

                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 50;
                        }

                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 500;
                            ship.KillMoney = 70;
                        }
                    }
                    break;

                case 4:
                    _boids.addRandomBoids(7, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(6, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 50;
                        }

                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 50;
                        }

                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 500;
                            ship.KillMoney = 70;
                        }
                    }
                    break;

                case 5:
                    //Enemy Elite Pilot
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(6, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 10;
                            ship.KillMoney = 60;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 400;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 500;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 30;
                            ship.gunMakeDmg = 5;
                            ship.KillMoney = 50;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 500;
                            ship.KillMoney = 70;
                        }
                    }
                    break;

                case 6:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(6, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 20;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 400;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 500;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 20;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 90;
                            ship.gunMakeDmg = 600;
                            ship.KillMoney = 100;
                        }
                    }
                    break;

                case 7:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(6, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 20;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 400;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 500;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 20;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 90;
                            ship.gunMakeDmg = 600;
                            ship.KillMoney = 100;
                        }
                    }
                    
                    break;

                case 8:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(2, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(8, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 200;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 300;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 120;
                            ship.gunMakeDmg = 600;
                            ship.KillMoney = 100;
                        }
                    }
                    break;

                case 9:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(3, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(10, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 200;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 300;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 120;
                            ship.gunMakeDmg = 600;
                            ship.KillMoney = 100;
                        }
                    }
                    break;

                case 10:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(3, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(1, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 200;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 300;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber))
                        {
                            ship.Health = 500;
                            ship.gunMakeDmg = 3000;
                            ship.KillMoney = 1000;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 600;
                            ship.KillMoney = 70;
                        }
                    }
                    break;
                case 11:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(3, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(1, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 200;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 300;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber))
                        {
                            ship.Health = 500;
                            ship.gunMakeDmg = 3000;
                            ship.KillMoney = 1000;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 800;
                            ship.KillMoney = 70;
                        }
                    }
                    break;

                case 12:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(3, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(7, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(8, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 200;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 300;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber))
                        {
                            ship.Health = 500;
                            ship.gunMakeDmg = 3000;
                            ship.KillMoney = 1000;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 800;
                            ship.KillMoney = 70;
                        }
                    }

                    break;

                case 13:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(3, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(2, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(10, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 200;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 300;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber))
                        {
                            ship.Health = 500;
                            ship.gunMakeDmg = 3000;
                            ship.KillMoney = 1000;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 800;
                            ship.KillMoney = 70;
                        }
                    }
                    break;

                case 14:
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber2);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 200;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 300;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 80;
                            ship.gunMakeDmg = 30;
                            ship.KillMoney = 70;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber))
                        {
                            ship.Health = 500;
                            ship.gunMakeDmg = 3000;
                            ship.KillMoney = 1000;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber2))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 800;
                            ship.KillMoney = 70;
                        }
                    }
                    break;

                case 15:
                    //BOSS is needet
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter);
                    _boids.addRandomBoids(5, Boids.EnemyType.Fighter2);
                    _boids.addRandomBoids(10, Boids.EnemyType.Fighter3);
                    _boids.addRandomBoids(5, Boids.EnemyType.Bomber);
                    foreach (var ship in ShipList)
                    {
                        if (ship.GetType() == typeof(EnemyFighter))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 10;
                            ship.KillMoney = 60;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter2))
                        {
                            ship.Health = 400;
                            ship.gunMakeDmg = 50;
                            ship.KillMoney = 500;
                        }
                        else if (ship.GetType() == typeof(EnemyFighter3))
                        {
                            ship.Health = 50;
                            ship.gunMakeDmg = 10;
                            ship.KillMoney = 60;
                        }
                        else if (ship.GetType() == typeof(EnemyBomber))
                        {
                            ship.Health = 500;
                            ship.gunMakeDmg = 3000;
                            ship.KillMoney = 1000;
                        }

                    }
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
