using System;
using Microsoft.Xna.Framework;

namespace SpaceAssault.Utils.Particle
{
    class ExplosionSystem
    {
        public ParticleSystem _system;
        public ParticleSystem _system2;
        public Vector3 _position;
        public int _state;
        private float _duration;
        private float _count;
        private float _radius;

        // Random number generator
        Random random = new Random();

        public ExplosionSystem(ParticleSystem particleSettings, Vector3 position, float duration)
        {
            _system = particleSettings;
            _position = position;
            _duration = duration;
            _count = 0;
        }

        public ExplosionSystem(ParticleSystem particleSettings, ParticleSystem particleSettings2, Vector3 position, float duration, float radius)
        {
            _system = particleSettings;
            _system2 = particleSettings2;
            _position = position;
            _duration = duration;
            _count = 0;
            _radius = radius;
        }

        public void Update(GameTime gameTime)
        {
            switch(_state)
            {
                case 0:
                    _system.AddParticle(_position, Vector3.Zero);
                    if (_count > _duration)
                        _state = 1;
                    break;
                case 1:
                    if (_count > _duration + 100)
                        _state = 2;
                    break;
            }

            if (_state != 2)
            { 
                _system.Update(gameTime);
                if (_system2 != null)
                    CircleExplosion(gameTime);
                _count++;
            }

        }

        public void Draw()
        {
            if (_state != 2)
            {
                _system.Draw();
                if (_system2 != null)
                    _system2.Draw();
            }
        }


        Vector3 RandomPointOnCircle()
        {
            double angle = random.NextDouble() * Math.PI * 2;

            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            return new Vector3(_position.X + x * _count, _position.Y, _position.Z + y * _count);
        }

        //#################################
        // Helper Update - Circle
        //#################################
        void CircleExplosion(GameTime gameTime)
        {
            const int borderParticlesPerFrame = 50;

            for (int i = 0; i < borderParticlesPerFrame; i++)
            {
                _system2.AddParticle(RandomPointOnCircle(), Vector3.Zero);
            }
            _system2.Update(gameTime);
        }

    }
}
