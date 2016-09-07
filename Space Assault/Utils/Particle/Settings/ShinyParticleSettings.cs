using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceAssault.Utils.Particle.Settings
{
    class ShinyParticleSettings : ParticleSystem
    {
   
        public ShinyParticleSettings()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "engineBlue";

            settings.MaxParticles = 250;

            settings.Duration = TimeSpan.FromSeconds(0.8);

            settings.DurationRandomness = 0;

            settings.EmitterVelocitySensitivity = 0.1f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            settings.MinColor = new Color(155, 255, 155, 10);
            settings.MaxColor = new Color(205, 255, 200, 50);

            settings.MinRotateSpeed = -4;
            settings.MaxRotateSpeed = 4;

            settings.MinStartSize = 15;
            settings.MaxStartSize = 15;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 3;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
