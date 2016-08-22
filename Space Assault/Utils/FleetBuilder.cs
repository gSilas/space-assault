using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceAssault.Entities;

namespace SpaceAssault.Utils
{
    class FleetBuilder
    {
        public List<List<AEnemys>> EnemyShips;
        public List<Bullet> _bulletList;
        private List<AEnemys> _addList;

        private Random _rand;

        private TimeSpan _lastChunkTime;
        protected TimeSpan _globalTimeSpan;


        public FleetBuilder()
        {
            EnemyShips = new List<List<AEnemys>>();
            _addList = new List<AEnemys>();
            _bulletList = new List<Bullet>();
            _rand = new Random();
        }

        public void Update(GameTime gameTime, Vector3 targetPosition)
        {
            List<Bullet> _removeBulletList = new List<Bullet>();
            List<AEnemys> _removeAEnemys = new List<AEnemys>();
            List<List<AEnemys>> _removeSquadrons = new List<List<AEnemys>>();

            // updating bullets
            _globalTimeSpan = gameTime.TotalGameTime;
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Update(gameTime);
                if (bullet._bulletLifeTime < 0)
                {
                    _removeBulletList.Add(bullet);
                }
            }
            foreach (Bullet bullet in _removeBulletList)
            {
                _bulletList.Remove(bullet);
            }

            // updating every ship
            foreach (var shipSquadron in EnemyShips)
            {
                foreach (var ship in shipSquadron)
                {
                    if (ship.IsDead == true)
                    {
                        _removeAEnemys.Add(ship);
                        continue;
                    }

                    // Trail
                    for (int i = 0; i < ship.trail.Count; i++)
                    {
                        ship.trail[i].Update(gameTime, ship.Position);
                    }
                    ship.TrailParticles.Update(gameTime);

                    ship.Intelligence(gameTime, targetPosition, ref _bulletList);
                    ship.Update(gameTime);
                }

                if(shipSquadron.Count == 0)
                {
                    _removeSquadrons.Add(shipSquadron);
                }
            }

            foreach (var shipSquadron in EnemyShips)
            {
                foreach (var ship in _removeAEnemys)
                {
                    shipSquadron.Remove(ship);
                }
            }
            foreach (var squad in _removeSquadrons)
            {
                EnemyShips.Remove(squad);
            } 

            // adding fleets
            if (gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromSeconds(Global.FleetSpawnTime))))
            {
                //maximale Anzahl an Flotten
                Formation(targetPosition);
                _lastChunkTime = gameTime.TotalGameTime;
            }
        }
        public void Draw()
        {
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Draw();
            }

            foreach (var shipSquadron in EnemyShips)
            {
                foreach (var ship in shipSquadron)
                {
                    ship.TrailParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
                    ship.TrailParticles.Draw();
                    ship.Draw();
                }
            }
        }
        //drone position damit alles außerhalb des Bilschirms spawned
        private void Formation(Vector3 DronePosition)
        {
            int zdist;
            int xoffset;
            for (int i = 0; i < 1; i++)
            {
                Vector3 position = new Vector3();
                switch (_rand.Next(1, 4))
                {
                    case 1:
                        //unten rechts
                        zdist = _rand.Next(0, 400);
                        xoffset = _rand.Next(-35, 35);
                        position.X = DronePosition.X + 350 + xoffset;
                        position.Z = DronePosition.Z + zdist;
                        position.Y = 0f;
                        Console.WriteLine("Neue Flotte von unten rechts");
                        break;
                    case 2:
                        //oben rechts
                        zdist = _rand.Next(-400, 0);
                        xoffset = _rand.Next(-35, 35);
                        position.X = DronePosition.X + 350 + xoffset;
                        position.Z = DronePosition.Z + zdist;
                        position.Y = 0f;
                        Console.WriteLine("Neue Flotte von oben rechts");
                        break;
                    case 3:
                        //unten links
                        zdist = _rand.Next(0, 400);
                        xoffset = _rand.Next(-35, 35);
                        position.X = DronePosition.X - 350 + xoffset;
                        position.Z = DronePosition.Z + zdist;
                        position.Y = 0f;
                        Console.WriteLine("Neue Flotte von unten links");
                        break;
                    case 4:
                        //oben links
                        zdist = _rand.Next(-400, 0);
                        xoffset = _rand.Next(-35, 35);
                        position.X = DronePosition.X - 350 + xoffset;
                        position.Z = DronePosition.Z + zdist;
                        position.Y = 0f;
                        Console.WriteLine("Neue Flotte von oben links");
                        break;
                }
                int formationchoice = _rand.Next(1, 4);
                switch (formationchoice)
                {
                    case 1:
                        BomberDouble(position);
                        break;
                    case 2:
                        Arrow(position);
                        break;
                    case 3:
                        FighterTriangle(position);
                        break;
                }
              
            }


        }

        private void FighterTriangle(Vector3 SpawnPosition)
        {
            int Rand= _rand.Next(40, 60);
            int zRand = _rand.Next(40, 60);
            EnemyFighter ship = new EnemyFighter(SpawnPosition+new Vector3(SpawnPosition.X+Rand, 0, SpawnPosition.Z + Rand));
            EnemyFighter ship2 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            EnemyFighter ship3 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X+Rand, 0, SpawnPosition.Z));

            ship.LoadContent();
            _addList.Add(ship);
            
            ship2.LoadContent();
            _addList.Add(ship2);

            ship3.LoadContent();
            _addList.Add(ship3);

            EnemyShips.Add(_addList);
        }
        private void Arrow(Vector3 SpawnPosition)
        {
            int Rand = _rand.Next(40, 60);
            int zRand = _rand.Next(40, 60);

            EnemyBomber ship = new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand));
            EnemyFighter ship2 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            EnemyFighter ship3 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z));
            EnemyFighter ship4 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z+2*Rand));
          
            ship.LoadContent();
            _addList.Add(ship);

            ship2.LoadContent();
            _addList.Add(ship2);

            ship3.LoadContent();
            _addList.Add(ship3);

            ship4.LoadContent();
            _addList.Add(ship4);
      

            EnemyShips.Add(_addList);
        }
        private void BomberDouble(Vector3 SpawnPosition)
        {
            int Rand = _rand.Next(40, 60);
            int zRand = _rand.Next(40, 60);
            EnemyBomber ship = new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand));
            EnemyBomber ship2 = new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            ;
            ship.LoadContent();
            _addList.Add(ship);

            ship2.LoadContent();
            _addList.Add(ship2);


            EnemyShips.Add(_addList);
        }
    }
}
