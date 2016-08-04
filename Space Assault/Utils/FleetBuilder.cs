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
            if (gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromMilliseconds(1000))))
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
        private void Formation(Vector3 targetPosition)
        {
            int zdist;
            int xoffset;
            for (int i = 0; i < 2; i++)
            {
                zdist = rand.Next(-200, 200);
                xoffset = rand.Next(-35, 35);

                Vector3 position = new Vector3();
                position.X = targetPosition.X + 350 + xoffset;
                position.Z = targetPosition.Z  + zdist;
                position.Y = 0f;


                int angle = rand.Next(-360, 360);
                EnemyShip ship = new EnemyShip(position);
                ship.Initialize();
                ship.LoadContent();
                addList.Add(ship);
            }
            EnemyShips.AddRange(addList);
            addList.Clear();
        }
    }
}
