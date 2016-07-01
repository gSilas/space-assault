using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;

namespace SpaceAssault.Utils
{
    class AsteroidBuilder
    {
        private Vector3 _position;
        private Vector3 _direction;
        private Model _model;
        public List<Asteroid> Asteroids;
        private TimeSpan _lastChunkTime;

        public AsteroidBuilder()
        {
            Asteroids = new List<Asteroid>();
        }

        private void BuildChunks()
        {
            Asteroids.AddRange(RandomChunks());
        }

        public void LoadContent()
        {
           _model = Global.ContentManager.Load<Model>("Models/asteroid2");
            BuildChunks();
        }

        public void Update(GameTime gameTime, Vector3 targetPosition, float radiusFromTarget)
        {
            Random rand = new Random();
            int angle = rand.Next(1, 360);
            _position = new Vector3();
            _position.X = targetPosition.X + radiusFromTarget * (float)Math.Sin(angle);
            _position.Z = targetPosition.Z + radiusFromTarget * (float)Math.Cos(angle);
            _position.Y = 0f;

            _direction = (targetPosition - _position);
            _direction.Y = 0;
            _direction.Normalize();

            foreach (var ast in Asteroids)
            {
                ast.Update(gameTime);
            }
            if (gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromSeconds(3))))
            {
                BuildChunks();
                _lastChunkTime = gameTime.TotalGameTime;
            }
        }

        public void Draw()
        {
            foreach (var ast in Asteroids)
            {
                ast.Draw();
            }
        }

        private List<Asteroid> RandomChunks()
        {
           Random random = new Random();
           int select = random.Next(0, 3);
           return ChunkAsteroids(select);
        }

        private List<Asteroid> ChunkAsteroids(int identifier)
        {
            List<Asteroid> astList = new List<Asteroid>();
            for (int i =0; i < 360; i= i +30)
            {               
                if (identifier == 0)
                {
                    _position.X = ((float)(_position.X + 100 * Math.Cos(i)));
                    _position.Z = ((float)(_position.Z + 100 * Math.Sin(i)));
                }
                else if (identifier == 1)
                {
                    _position.X = ((float)(_position.X + 20 * Math.Tan(i)));
                    _position.Z = ((float)(_position.Z + 20 * Math.Atan(i)));
                }
                else if (identifier == 2)
                {
                    _position.X = ((float)(_position.X + 20 * Math.Sinh(i)));
                    _position.Z = ((float)(_position.Z + 20 * Math.Sin(i)));
                }
                Random rand = new Random();
                int angle = rand.Next(-360, 360);
                Asteroid ast = new Asteroid(_position,angle, _direction, 2f);
                ast.Initialize();
                ast.LoadContent(_model);
                astList.Add(ast);

            }
            
            return astList;
        }
    }
}
