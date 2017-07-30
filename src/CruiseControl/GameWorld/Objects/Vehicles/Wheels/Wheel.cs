using Microsoft.Xna.Framework;

namespace CruiseControl.GameWorld.Objects.Vehicles.Wheels
{
    internal abstract class Wheel : GameObject
    {
        protected Vehicle _owner;
        private Vector3 _referenceOwnerPosition, _offset;
        protected float _wheelRotation = 0f, _wheelTurning = 0f;

        public WheelType WheelType { get; private set; }
        public Vector3 Offset => _offset;

        public Wheel(IObjectContainer parent, Vehicle owner, Vector3 position, WheelType type)
            : base(parent)
        {
            _owner = owner;
            _offset = position;
            WheelType = type;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            _position = _offset + _owner.Position;
            _referenceOwnerPosition = _owner.Position;
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateRotationX(_wheelRotation) *
                Matrix.CreateRotationY(_wheelTurning) *
                Matrix.CreateTranslation(_offset) *
                Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) *
                Matrix.CreateTranslation(_container.Offset + _owner.Position);
        }

        public Vector3 GetTransformedPosition()
        {
            var pos = Vector3.Transform(_offset, Matrix.CreateRotationY(_rotation.Y)) + _owner.Position;
            return pos;
        }

        private float GetRotationAddition()
        {
            if (_owner.ControlSource == VehicleControlSource.Config)
            {
                return 0.06f;
            }
            else
            {
                const int frames = 225000;
                if (_owner.Speed == 0f)
                    return 0f;
                var meters = (_owner.Speed * 100) * 1000;
                var rotations = meters / 1.26f;
                var rpf = rotations / frames;
                return rpf * MathHelper.TwoPi;
            }
        }

        public void UpdateWheel()
        {
            var updateWorld = false;
            if (_referenceOwnerPosition != _owner.Position)
            {
                UpdatePosition();
                updateWorld = true;
            }
            if (_rotation != _owner.Rotation)
            {
                _rotation = _owner.Rotation;
                updateWorld = true;
            }
            if ((WheelType == WheelType.FrontLeft || WheelType == WheelType.FrontRight) && _owner.Turning != _wheelTurning)
            {
                _wheelTurning = -_owner.Turning * 0.8f;
                updateWorld = true;
            }

            if (updateWorld || _owner.ControlSource == VehicleControlSource.Config)
                CreateWorld();
        }

        public override void Update()
        {
            var rotationAddition = GetRotationAddition();
            if (rotationAddition != 0f)
            {
                _wheelRotation += rotationAddition;
            }

            base.Update();
        }
    }
}
