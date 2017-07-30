using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CruiseControl.GameWorld.Objects
{
    internal abstract class GameObject : Base3DObject<VertexPositionNormalTexture>, IComparable
    {
        private float _cameraDistance;
        protected Vector3 _position;
        protected Vector3 _rotation;
        protected IObjectContainer _container;

        public Vector3 Position => _position;
        public Vector3 Rotation => _rotation;

        public GameObject(IObjectContainer parent)
        {
            _container = parent;
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z)
                * Matrix.CreateTranslation(_container.Offset + _position);
            base.CreateWorld();
        }
        
        public virtual void OnRemove() { }

        public override void Update()
        {
            base.Update();
            _cameraDistance = Math.Abs(Vector3.Distance(_position, _container.Screen.Camera.Position));
        }
        
        public int CompareTo(object other)
        {
            if (other is GameObject g)
            {
                if (g._cameraDistance < _cameraDistance)
                    return -1;
                else
                    return 1;
            } 
            else
            {
                return -1;
            }
        }
    }
}
