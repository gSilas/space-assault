using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;

namespace SpaceAssault.Entities.Weapon
{
    public class Bullet : AEntity
    {
        protected int _damage;
        protected bool _canDmgStation;

        private Vector3 _direction;
        private float _travelspeed;
        public int _bulletlife;

        public Bullet(Vector3 position, Matrix droneRotateMatrix, float travelspeed, Model model, int damage, bool canDamageStation)
        {
            Position = position;
            RotationMatrix = droneRotateMatrix;
            Model = model;
            Spheres = Collider3D.UpdateBoundingSphere(this);
            _bulletlife = 600;
            _direction = droneRotateMatrix.Forward;
            _travelspeed = travelspeed;
            _damage = damage;
            _canDmgStation = canDamageStation;
        }
        public override void Update(GameTime gameTime)
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Position -= _direction*_travelspeed;
            _bulletlife--;         
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public int makeDmg
        {
            get { return _damage; }
        }

        public bool CanDamageStation
        {
            get { return _canDmgStation; }
        }
    }
}
