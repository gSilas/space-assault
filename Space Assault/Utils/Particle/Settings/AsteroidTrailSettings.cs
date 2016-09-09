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

            settings.MaxParticles = 300;

            settings.Duration = TimeSpan.FromSeconds(1.2);

            settings.DurationRandomness = 0;

            settings.MinHorizontalVelocity = -1;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            settings.MinColor = Color.DarkSlateBlue;
            settings.MaxColor = Color.DarkSlateBlue;

            settings.MinRotateSpeed = -4;
            settings.MaxRotateSpeed = 4;

            settings.MinStartSize = 22;
            settings.MaxStartSize = 26;

            settings.MinEndSize = 0;
            settings.MaxEndSize = 3;

            settings.BlendState = BlendState.Additive;
        }
    }
}
