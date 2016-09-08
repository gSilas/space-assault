
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceAssault.Utils.Particle.Settings
{
    /// <summary>
    /// Custom particle system for leaving smoke trails behind the rocket projectiles.
    /// </summary>
    class BombTrailSettings : ParticleSystem
    {
        public BombTrailSettings()
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "engineBlue";

            settings.MaxParticles = 300;

            settings.Duration = TimeSpan.FromSeconds(0.5);

            settings.DurationRandomness = 0;

            settings.EmitterVelocitySensitivity = 0.1f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            settings.MinColor = Color.Black;
            settings.MaxColor = Color.DarkGray;

            settings.MinRotateSpeed = -4;
            settings.MaxRotateSpeed = 4;

            settings.MinStartSize = 40;
            settings.MaxStartSize = 42;

            settings.MinEndSize = 1;
            settings.MaxEndSize = 3;


            settings.BlendState = BlendState.Additive;
        }
    }
}
