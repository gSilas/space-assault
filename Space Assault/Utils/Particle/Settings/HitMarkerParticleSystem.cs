using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceAssault.Utils.Particle.Settings
{
    class HitMarkerParticleSystem : ParticleSystem
    {
           public HitMarkerParticleSystem()
            { }

            protected override void InitializeSettings(ParticleSettings settings)
            {
                settings.TextureName = "hit_marker";

                settings.MaxParticles = 100;

                settings.Duration = TimeSpan.FromSeconds(0.2);
                settings.DurationRandomness = 0;

                settings.MinHorizontalVelocity = 20;
                settings.MaxHorizontalVelocity = 30;

                settings.MinVerticalVelocity = -20;
                settings.MaxVerticalVelocity = 20;

                settings.EndVelocity = 0;

                settings.MinColor = Color.White;
                settings.MaxColor = Color.White;

                settings.MinRotateSpeed = -1;
                settings.MaxRotateSpeed = 1;

                settings.MinStartSize = 8;
                settings.MaxStartSize = 8;

                settings.MinEndSize = 8;
                settings.MaxEndSize = 8;

                settings.BlendState = BlendState.Additive;
            }
    }
}