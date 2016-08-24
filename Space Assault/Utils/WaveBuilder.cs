using Microsoft.Xna.Framework;
using SpaceAssault.Entities;
using System.Collections.Generic;

namespace SpaceAssault.Utils
{
    class WaveBuilder
    {
        private Wave _currentWave;
        public WaveBuilder()
        {

        }
        public void Update(GameTime gameTime, ref List<Asteroid> asteroidList)
        {
            _currentWave.Update(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
            _currentWave.Draw();
        }
    }

    class Wave
    {
        private List<AEntity> _entityList;
        public Wave()
        {

        }
        public List<AEntity> EntityList
        {
            get { return _entityList; }
        }
        public void Update(GameTime gameTime)
        {
            foreach (var ent in _entityList)
            {
                ent.Update(gameTime);
            }
        }
        public void Draw()
        {
            foreach (var ent in _entityList)
            {
                ent.Draw();
            }
        }
    }
}
