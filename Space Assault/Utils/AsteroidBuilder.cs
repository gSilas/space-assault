using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Entities;

namespace Space_Assault.Utils
{
    class AsteroidBuilder
    {
        private int _chunkSize;
        private Vector3 _referencePoint;
        private int _distance;
        private Model _model;
        private List<Asteroid> _asteroids;

        public AsteroidBuilder(int chunkSize, int distance, Vector3 pointOfReference)
        {
            _chunkSize = chunkSize;
            _distance = distance;
            _referencePoint = pointOfReference;
            _referencePoint.Normalize();
            _asteroids = new List<Asteroid>();
            BuildField();
        }

        private void BuildField()
        {
            for (int i = 0; i <= _chunkSize; i++)
            {
                Asteroid ast = new Asteroid(new Vector3(10*i,0,-100*i+30*i), 0,new Vector3(1,0,-1), 0.5f);
                ast.Initialize();
                ast.LoadContent(_model);
                _asteroids.Add(ast);
            }
        }

        public void LoadContent()
        { 
            _model = Global.ContentManager.Load<Model>("Models/asteroid");
        }

        public void Update(GameTime gameTime)
        {
            foreach (var ast in _asteroids)
            {
                ast.Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach (var ast in _asteroids)
            {
                ast.Draw();
            }
        }
    }
}
