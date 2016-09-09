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

        private Random _rand;
        private TimeSpan _lastChunkTime;


        public FleetBuilder()
        {
            EnemyShips = new List<List<AEnemys>>();
            _bulletList = new List<Bullet>();
            _rand = new Random();
        }

        public void Update(GameTime gameTime, Vector3 targetPosition)
        {
            List<Bullet> _removeBulletList = new List<Bullet>();
            List<AEnemys> _removeAEnemys = new List<AEnemys>();
            List<List<AEnemys>> _removeSquadrons = new List<List<AEnemys>>();

            // updating bullets
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Update(gameTime);
                if (bullet._bulletLifeTime < 0)
                {
                    _removeBulletList.Add(bullet);
                }

                // BombTrail
                if (bullet._trail != null)
                    bullet._trail.Update(gameTime, bullet.Position);
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
                    if (ship.IsDead)
                    {
                        Global.Money += ship.KillMoney;
                        _removeAEnemys.Add(ship);
                        continue;
                    }

                    // Trail
                    if (ship._trail != null)
                        ship._trail.Update(gameTime, ship.Position);

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
                //Formation(targetPosition);
                _lastChunkTime = gameTime.TotalGameTime;
            }
        }
        public void Draw()
        {
            foreach (Bullet bullet in _bulletList)
            {
                // BombTrail
                if (bullet._trail != null)
                    bullet._trail.Draw();
                bullet.Draw();
            }

            foreach (var shipSquadron in EnemyShips)
            {
                foreach (var ship in shipSquadron)
                {
                    if (ship._trail != null)
                        ship._trail.Draw();
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
            List<AEnemys> _addList = new List<AEnemys>();

            _addList.Add(new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand))); 
            _addList.Add(new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand)));
            _addList.Add(new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z)));
            foreach (var ship in _addList)
                ship.LoadContent();

            EnemyShips.Add(_addList);
        }
        private void Arrow(Vector3 SpawnPosition)
        {
            int Rand = _rand.Next(40, 60);
            int zRand = _rand.Next(40, 60);
            List<AEnemys> _addList = new List<AEnemys>();

            _addList.Add(new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand)));
            _addList.Add(new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand)));
            _addList.Add(new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z)));
            _addList.Add(new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + 2 * Rand)));
            foreach (var ship in _addList)
                ship.LoadContent();

            EnemyShips.Add(_addList);
        }
        private void BomberDouble(Vector3 SpawnPosition)
        {
            int Rand = _rand.Next(40, 60);
            int zRand = _rand.Next(40, 60);
            List<AEnemys> _addList = new List<AEnemys>();
            _addList.Add(new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand)));
            _addList.Add(new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand)));
            foreach (var ship in _addList)
                ship.LoadContent();

            EnemyShips.Add(_addList);
        }
    }
}
