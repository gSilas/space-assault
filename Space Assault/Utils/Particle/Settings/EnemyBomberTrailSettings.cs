
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils.Particle.Settings
{

    class EnemyBomberTrailSettings : ParticleSystem
    {
        public EnemyBomberTrailSettings()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "engineBlue";

            settings.MaxParticles = 500;

            settings.Duration = TimeSpan.FromSeconds(2);

            settings.DurationRandomness = 0;

            settings.EmitterVelocitySensitivity = 0.1f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 0;

            settings.MinColor = new Color(255, 255, 255, 100);
            settings.MaxColor = new Color(255, 255, 255, 100);

            settings.MinRotateSpeed = 0;
            settings.MaxRotateSpeed = 0;

            settings.MinStartSize = 30;
            settings.MaxStartSize = 30;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 1;

            settings.BlendState = BlendState.Additive;
        }
    }
}
