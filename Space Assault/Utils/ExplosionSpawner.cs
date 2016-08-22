using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace SpaceAssault.Entities
{
    class ExplosionSpawner
    {
        public List<Explosion> _explList;

        public ExplosionSpawner()
        {
            _explList = new List<Explosion>();
        }

        public void SpawnExplosion(Vector3 position, float radius, int makeDmg)
        {
            _explList.Add(new Explosion(position, radius, makeDmg));
        }

        public void Update(GameTime gameTime)
        {
            List<Explosion> explRemoveList = new List<Explosion>();

            foreach (var expl in _explList)
            {
                expl.Update(gameTime);
                if(expl._lifeTime <= 0)
                {
                    explRemoveList.Add(expl);
                }
            }

            foreach (var expl in explRemoveList)
            {
                _explList.Remove(expl);
            }
        }


        internal class Explosion : AEntity
        {
            public int _lifeTime;   //lifetime in miliseconds
            public int _makeDmg;

            public Explosion(Vector3 pos, float radius, int makeDmg)
            {
                _makeDmg = makeDmg;
                Position = pos;
                Spheres = new BoundingSphere[1];
                Spheres[0].Center = Position;
                Spheres[0].Radius = radius;
                _lifeTime = 800;
            }
            public override void Update(GameTime gameTime)
            {
                _lifeTime -= gameTime.ElapsedGameTime.Milliseconds;
            }

            public override void LoadContent()
            {
                throw new NotImplementedException();
            }
        }
    }
}
