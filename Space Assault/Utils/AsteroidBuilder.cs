using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Space_Assault.Entities;

namespace Space_Assault.Utils
{
    class AsteroidBuilder
    {
        private int _chunkSize;
        private Vector3 _referencePoint;
        private int _distance;
        private List<Asteroid> _asteroids;
        private ContentManager _cm;

        public AsteroidBuilder(int chunkSize, int distance, Vector3 pointOfReference,ContentManager cm)
        {
            _chunkSize = chunkSize;
            _distance = distance;
            _referencePoint = pointOfReference;
            _referencePoint.Normalize();
            _cm = cm;
            _asteroids = new List<Asteroid>();
            BuildField();
        }

        private void BuildField()
        {
            for (int i = 0; i <= _chunkSize; i++)
            {
                Asteroid ast = new Asteroid(new Vector3(10*i,0,-100*i+30*i), 0,Vector3.Backward, 0.5f);
                ast.Initialize();
                ast.LoadContent(_cm);
                _asteroids.Add(ast);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var ast in _asteroids)
            {
                ast.Update(gameTime);
            }
        }

        public void Draw(Camera _camera)
        {
            foreach (var ast in _asteroids)
            {
                ast.Draw(_camera);
            }
        }
    }
}
