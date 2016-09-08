using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceAssault.Utils.Particle.Settings
{
    class AsteroidTrailSettings : ParticleSystem
    {
   
        public AsteroidTrailSettings()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "engineBlue";

            settings.MaxParticles = 1000;

            settings.Duration = TimeSpan.FromSeconds(0.8);

            settings.DurationRandomness = 0;

            settings.EmitterVelocitySensitivity = 0.1f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            settings.MinColor = new Color(155, 255, 155, 255);
            settings.MaxColor = new Color(205, 255, 200, 255);

            settings.MinRotateSpeed = -4;
            settings.MaxRotateSpeed = 4;

            settings.MinStartSize = 15;
            settings.MaxStartSize = 15;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 3;

            //this makes asteroids transparent, don't know why
            //settings.BlendState = BlendState.Additive;
        }
    }
}
