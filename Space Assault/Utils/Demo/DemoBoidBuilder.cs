using Microsoft.Xna.Framework;
using SpaceAssault.Entities;
using SpaceAssault.Screens.UI;
using System;
using System.Collections.Generic;

namespace SpaceAssault.Utils.Demo
{
    class DemoBoidBuilder
    {
        private BoidWave _currentWave;
        private int _timeBetweenWaves;
        private int _time;
        private int _waveCount;
        private int _max;
        private Dialog _dialog;
        private bool _showdialog = true;

        public DemoBoidBuilder(int timeInMilliseconds, int maxwave)
        {
            _currentWave = new BoidWave(1);
            _waveCount++;
            _max = maxwave;
            _timeBetweenWaves = timeInMilliseconds;
            _time = timeInMilliseconds;
        }
        public void LoadContent()
        {
            _dialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width - 400, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 50, 25, 380, 8, false, true);
            _currentWave.LoadContent();
            _dialog.LoadContent();
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
            _showdialog = false;
            _currentWave.Update(gameTime, ref asteroidField, ref droneFleet);
            if (_currentWave.ShipList.Count <= 0)
            {
                _time -= gameTime.ElapsedGameTime.Milliseconds;
                if(_time <= 0 && _waveCount < _max)
                {
                    _waveCount++;
                    _currentWave.UnLoadContent();
                    _currentWave = new BoidWave(_waveCount);
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
            if (_showdialog)
            _dialog.Draw("Press 'B' to activate the Boidsystem.");

        }
    }

    class BoidWave
    {
        private Boids _boids;
        public static int _waveNumber = 1;
        public List<AEnemys> ShipList { get { return _boids.collisionObjects; } }
        public List<Bullet> BulletList { get { return _boids.bullets; } }
        public BoidWave(int waveNumber)
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

                case 1:
                    Global.EnemyColor = Color.LightGreen;
                    Global.EnemyFighterColor = Color.LightGreen;

                    Global.EnemyFighterHealth = 10;
                    Global.EnemyFighterDamage = 0;
                    Global.EnemyFighterKillMoney = 2000;

                    Global.EnemyFighter2Health = 10;
                    Global.EnemyFighter2Damage = 0;
                    Global.EnemyFighter2KillMoney = 2000;

                    Global.EnemyFighter3Health = 10;
                    Global.EnemyFighter3Damage = 0;
                    Global.EnemyFighter3KillMoney = 2000;

                    Global.EnemyBomberHealth = 10;
                    Global.EnemyBomberDamage = 0;
                    Global.EnemyBomberKillMoney = 2000;

                    Global.EnemyBomber2Health = 10;
                    Global.EnemyBomber2Damage = 0;
                    Global.EnemyBomber2KillMoney = 2000;


                    _boids.addBoid(new Vector3(150, 0, 50), Boids.EnemyType.Fighter);
                    _boids.addBoid(new Vector3(250, 0, 50), Boids.EnemyType.Fighter);
                    _boids.addBoid(new Vector3(350, 0, 50), Boids.EnemyType.Fighter);

                    _boids.addBoid(new Vector3(150, 0, -50), Boids.EnemyType.Bomber2);
                    _boids.addBoid(new Vector3(250, 0, -50), Boids.EnemyType.Fighter2);
                    _boids.addBoid(new Vector3(350, 0, -50), Boids.EnemyType.Bomber2);

                    _boids.addBoid(new Vector3(150, 0, -150), Boids.EnemyType.Fighter3);
                    _boids.addBoid(new Vector3(250, 0, -150), Boids.EnemyType.Fighter3);
                    _boids.addBoid(new Vector3(350, 0, -150), Boids.EnemyType.Fighter3);

                    _boids.addBoid(new Vector3(450, 0, 25), Boids.EnemyType.Bomber);
           

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
