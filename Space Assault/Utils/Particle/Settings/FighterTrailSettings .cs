
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils.Particle.Settings
{
    class FighterTrailSettings : ParticleSystem
    {
        public FighterTrailSettings()
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "engineBlue";

            settings.MaxParticles = 300;

            settings.Duration = TimeSpan.FromSeconds(0.6);

            settings.DurationRandomness = 0;

            settings.EmitterVelocitySensitivity = 0.1f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            settings.MinColor = new Color(255, 0, 0, 70);
            settings.MaxColor = new Color(255, 0, 0, 70);

            settings.MinRotateSpeed = -4;
            settings.MaxRotateSpeed = 4;

            settings.MinStartSize = 25;
            settings.MaxStartSize = 25;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 3;

            settings.BlendState = BlendState.Additive;
        }
    }
}
