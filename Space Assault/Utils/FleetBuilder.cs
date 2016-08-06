using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;

namespace SpaceAssault.Utils
{
    class FleetBuilder
    {
        private Model _model;
        public List<EnemyShip> EnemyShips;
        private List<EnemyShip> addList;
        private TimeSpan _lastChunkTime;
        private Random rand;

        public FleetBuilder()
        {
            EnemyShips = new List<EnemyShip>();
            addList = new List<EnemyShip>();
            rand = new Random();
        }

        public void LoadContent()
        {
            _model = Global.ContentManager.Load<Model>("Models/enemyShip2");
        }

        public void Update(GameTime gameTime, Vector3 targetPosition)
        {
            foreach (var ship in EnemyShips)
            {
                ship.Update(gameTime);
            }
            if (gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromMilliseconds(2000))))
            {
                Formation(targetPosition);
                _lastChunkTime = gameTime.TotalGameTime;
            }
        }
        public void Draw()
        {
            foreach (var ship in EnemyShips)
            {
                ship.Draw();
            }
        }
        //drone position damit alles außerhalb des Bilschirms spawned
        private void Formation(Vector3 DronePosition)
        {
            int zdist;
            int xoffset;
            int formationchoice;
            for (int i = 0; i < 1; i++)
            {
                zdist = rand.Next(-200, 200);
                xoffset = rand.Next(-35, 35);

                Vector3 position = new Vector3();
                position.X = DronePosition.X + 350 + xoffset;
                position.Z = DronePosition.Z + zdist;
                position.Y = 0f;

                formationchoice = rand.Next(1, 4);
                //Console.WriteLine(formationchoice+"<<<<<<<<<<<<<<");
                switch (formationchoice)
                {
                    case 1:
                        //Console.WriteLine("Case 11111111111111111111111");
                        Double(position);
                        break;
                    case 2:
                        //Console.WriteLine("Case 222222222222222222222222");
                        Arrow(position);
                        break;
                    case 3:
                        //Console.WriteLine("Case 333333333333333333333333");
                        Triangle(position);
                        break;
                }
                Double(position);
            }


        }

        private void Triangle(Vector3 SpawnPosition)
        {
            int Rand= rand.Next(40, 60);
            int zRand = rand.Next(40, 60);
            EnemyShip ship = new EnemyShip(SpawnPosition+new Vector3(SpawnPosition.X+Rand, 0, SpawnPosition.Z + Rand));
            EnemyShip ship2 = new EnemyShip(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            EnemyShip ship3 = new EnemyShip(SpawnPosition + new Vector3(SpawnPosition.X+Rand, 0, SpawnPosition.Z));

            ship.Initialize();
            ship.LoadContent();
            addList.Add(ship);
            
            ship2.Initialize();
            ship2.LoadContent();
            addList.Add(ship2);
            //Console.WriteLine("WRONG PLACE");
            ship3.Initialize();
            ship3.LoadContent();
            addList.Add(ship3);

            EnemyShips.AddRange(addList);
            addList.Clear();
        }
        private void Arrow(Vector3 SpawnPosition)
        {
            int Rand = rand.Next(40, 60);
            int zRand = rand.Next(40, 60);
            EnemyShip ship = new EnemyShip(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand));
            EnemyShip ship2 = new EnemyShip(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            EnemyShip ship3 = new EnemyShip(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z));
            EnemyShip ship4 = new EnemyShip(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z+2*Rand));
          
            //EnemyShip ship4 = new EnemyShip(SpawnPosition+ new Vector3(0,0,0)); //funktioniert damit nicht  WAT
        
            ship.Initialize();
            ship.LoadContent();
            addList.Add(ship);

            ship2.Initialize();
            ship2.LoadContent();
            addList.Add(ship2);

            ship3.Initialize();
            ship3.LoadContent();
            addList.Add(ship3);

            ship4.Initialize();
            ship4.LoadContent();
            addList.Add(ship4);
      

            EnemyShips.AddRange(addList);
            addList.Clear();
        }
        private void Double(Vector3 SpawnPosition)
        {
            int Rand = rand.Next(40, 60);
            int zRand = rand.Next(40, 60);
            EnemyShip ship = new EnemyShip(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand));
            EnemyShip ship2 = new EnemyShip(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            

            ship.Initialize();
            ship.LoadContent();
            addList.Add(ship);

            ship2.Initialize();
            ship2.LoadContent();
            addList.Add(ship2);


            EnemyShips.AddRange(addList);
            addList.Clear();
        }
    }
}
