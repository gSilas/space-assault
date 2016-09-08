using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils.Particle.Settings
{
    class AsteroidExplosionSettings : ParticleSystem
    {
        public AsteroidExplosionSettings()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "dust2";

            settings.MaxParticles = 100;

            settings.Duration = TimeSpan.FromSeconds(1.2);
            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 20;
            settings.MaxHorizontalVelocity = 30;

            settings.MinVerticalVelocity = -20;
            settings.MaxVerticalVelocity = 20;

            settings.EndVelocity = 0;

            settings.MinColor = new Color(255, 255, 255, 80);
            settings.MaxColor = new Color(255, 255, 255, 10);

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 80;
            settings.MaxStartSize = 100;

            settings.MinEndSize = 20;
            settings.MaxEndSize = 40;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
