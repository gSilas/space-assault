using System;
using IrrKlang;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities
{
    class Asteroid : AEntity
    {
        private float _angle;
        private Vector3 _direction;
        private float _speed;
        public TimeSpan _originTime;
        private ISoundEngine _engine;
        private ISound _shootSound;

        public Asteroid(Vector3 position, float angle, Vector3 direction, float movespeed, TimeSpan originTime)
        {
            _angle = angle;
            Position = position;
            _direction = direction;
            _speed = movespeed;
            _originTime = originTime;
        }

        public void LoadContent(Model model)
        {
            Model = model;
            Spheres = Collider3D.UpdateBoundingSphere(this);
            _engine = new ISoundEngine();
        }

        public bool IsDead { get; set; } = false;

        public override void LoadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize()
        {
            _direction.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            _angle += 0.005f;
            RotationMatrix = Matrix.CreateRotationY(_angle);
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Position += _direction * _speed;
            if (IsDead)
            {
                Console.WriteLine("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
                var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
                _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
                _shootSound = _engine.Play3D("Content/Media/Music/Laser_Shoot.wav", curListenerPos + new Vector3D(0, 15, 0), false, true, StreamMode.AutoDetect, true);
                _shootSound.Volume = 5.5f;
                _shootSound.Paused = false;
            }
            
            
        }
        
        public Asteroid Clone()
        {
            return new Asteroid(Position,_angle,_direction,_speed, _originTime);
        }
    }
}
