using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using IrrKlang;

namespace SpaceAssault.Entities
{

    public class Bullet : AEntity
    {
        private ISoundSource _shootSource;
        private ISoundEngine _engine;

        private Vector3 _moveDirection;
        private float _moveSpeed;
        public float _bulletLifeTime;
        protected int _makeDmg;
        protected bool _canDmgStation;
        public BulletType _bulletType;


        public Bullet(string soundFile, string modelFile, float moveSpeed, bool canDamageStation, int damage, Vector3 position, Matrix droneRotateMatrix)
        {
            if (soundFile.Length > 0)
            {
                //playing the sound
                _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
                _shootSource = _engine.AddSoundSourceFromFile(soundFile, StreamMode.AutoDetect, true);

                var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
                _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
                var _shootSound = _engine.Play3D(_shootSource, curListenerPos.X, curListenerPos.Y + 15f, curListenerPos.Z, false, true, true);
                _shootSound.Volume = 0.5f;
                _shootSound.Paused = false;
            }

            Model = Global.ContentManager.Load<Model>(modelFile);
            Position = position;
            RotationMatrix = droneRotateMatrix;
            Spheres = Collider3D.UpdateBoundingSphere(this);

            _makeDmg = damage;
            _canDmgStation = canDamageStation;
            _bulletLifeTime = (120f / moveSpeed)*1000;
            _moveDirection = droneRotateMatrix.Forward;
            _moveSpeed = moveSpeed;
        }

        public Bullet(BulletType bullet, int damage, Vector3 position, Matrix droneRotateMatrix)
        {
            _bulletType = bullet;
            Bullets curBullet;
            switch (bullet)
            {
                case BulletType.YellowLazer:
                    curBullet = new Bullets("Content/Media/Effects/Laser_Shoot.wav", "Models/laser", 6f, false);
                    break;
                case BulletType.BlueLazer:
                    curBullet = new Bullets("Content/Media/Effects/Laser_Shoot.wav", "Models/laser2", 6f, false);
                    break;
                case BulletType.PhotonBomb:
                    curBullet = new Bullets("", "Models/bullet", 0.7f, true);
                    break;
                case BulletType.BigJoe:
                    curBullet = new Bullets("", "Models/bullet", 2f, true);
                    break;
                case BulletType.EnemyLazer:
                    curBullet = new Bullets("Content/Media/Effects/Laser_Shoot.wav", "Models/laser", 3f, false);
                    break;
                default:
                    curBullet = new Bullets("Content/Media/Effects/Laser_Shoot.wav", "Models/laser", 6f, false);
                    break;
            }

            string soundFile = curBullet._soundFile;
            string modelFile = curBullet._modelFile;
            bool canDamageStation = curBullet._canDamageStation;
            float moveSpeed = curBullet._moveSpeed;

            if (soundFile.Length > 0)
            {
                //playing the sound
                _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
                _shootSource = _engine.AddSoundSourceFromFile(soundFile, StreamMode.AutoDetect, true);

                var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
                _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
                var _shootSound = _engine.Play3D(_shootSource, curListenerPos.X, curListenerPos.Y + 15f, curListenerPos.Z, false, true, true);
                _shootSound.Volume = 0.5f;
                _shootSound.Paused = false;
            }

            Model = Global.ContentManager.Load<Model>(modelFile);
            Position = position;
            RotationMatrix = droneRotateMatrix;
            Spheres = Collider3D.UpdateBoundingSphere(this);

            _makeDmg = damage;
            _canDmgStation = canDamageStation;
            _bulletLifeTime = (120f / moveSpeed) * 1000;
            _moveDirection = droneRotateMatrix.Forward;
            _moveSpeed = moveSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Position -= _moveDirection * _moveSpeed;
            _bulletLifeTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }

        public int makeDmg
        {
            get { return _makeDmg; }
        }

        public bool CanDamageStation
        {
            get { return _canDmgStation; }
        }

        public enum BulletType
        {
            YellowLazer,
            BlueLazer,
            PhotonBomb,
            BigJoe,
            EnemyLazer
        }

        internal class Bullets
        {
            public string _soundFile;
            public string _modelFile;
            public float _moveSpeed;
            public bool _canDamageStation;

            public Bullets(string soundFile, string modelFile, float moveSpeed, bool canDamageStation)
            {
                _soundFile = soundFile;
                _modelFile = modelFile;
                _moveSpeed = moveSpeed;
                _canDamageStation = canDamageStation;
            }
        }
    }
}
