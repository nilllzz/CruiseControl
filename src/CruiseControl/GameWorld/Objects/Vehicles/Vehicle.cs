using CruiseControl.Content;
using CruiseControl.GameWorld.Objects.Vehicles.Wheels;
using CruiseControl.Scenes.Configurator;
using GameDevCommon.Drawing;
using GameDevCommon.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core;

namespace CruiseControl.GameWorld.Objects.Vehicles
{
    internal abstract class Vehicle : GameObject
    {
        public float Speed { get; protected set; } = 0f;
        public float Turning { get; protected set; } = 0f;
        public ConfigOption[] Options { get; protected set; }
        public VehicleControlSource ControlSource { get; set; } = VehicleControlSource.NPC;
        public abstract Vector3 ThirdPersonCameraOffset { get; }

        private List<Wheel> _wheels;
        private (WheelType type, float height)[] _wheelHeights = new(WheelType, float)[4];

        public Vehicle(IObjectContainer parent, Vector3 position, Vector3 rotation)
            : this(parent, position, rotation, new ConfigOption[0])
        { }

        public Vehicle(IObjectContainer parent, Vector3 position, Vector3 rotation, ConfigOption[] options)
            : base(parent)
        {
            _position = position;
            _rotation = rotation;
            EquipDefaultOptions(options);
        }

        public override void Update()
        {
            if (ControlSource == VehicleControlSource.Player)
                UpdateControls();

            _position += Vector3.Transform(new Vector3(0, 0, Speed), Matrix.CreateRotationY(_rotation.Y));

            if (Speed > 0f)
            {
                _rotation.Y -= Turning * Speed * 0.15f;
            }

            var floor = _container.GetFloor(_position);
            if (floor != null)
            {
                var pos2D = new Vector2(_position.X, _position.Z);
                var wheelPositions = _wheels.Select(w => w.GetTransformedPosition()).ToArray();
                _wheelHeights = wheelPositions
                    .Select(v => _container.GetFloor(v))
                    .Select((f, i) =>
                    {
                        if (f == null)
                            return _wheelHeights[i];
                        else
                            return (_wheels[i].WheelType, f.GetHeightForPosition(new Vector2(wheelPositions[i].X, wheelPositions[i].Z)));
                    }).ToArray();

                _position.Y = _wheelHeights.Average(item => item.height);

                var xDiff = Vector3.Distance(_wheels.First(w => w.WheelType == WheelType.FrontLeft).Offset,
                        _wheels.First(w => w.WheelType == WheelType.FrontRight).Offset);
                var zDiff = Vector3.Distance(_wheels.First(w => w.WheelType == WheelType.FrontLeft).Offset,
                        _wheels.First(w => w.WheelType == WheelType.RearLeft).Offset);

                _rotation.X = -(float)((Math.Atan((GetWheelHeight(WheelType.FrontLeft) - GetWheelHeight(WheelType.RearLeft)) / zDiff) +
                    Math.Atan((GetWheelHeight(WheelType.FrontRight) - GetWheelHeight(WheelType.RearRight)) / zDiff)) / 2);
                _rotation.Z = (float)((Math.Atan((GetWheelHeight(WheelType.FrontLeft) - GetWheelHeight(WheelType.FrontRight)) / xDiff) +
                    Math.Atan((GetWheelHeight(WheelType.RearLeft) - GetWheelHeight(WheelType.RearRight)) / xDiff)) / 2);
            }

            CreateWorld();

            foreach (var wheel in _wheels)
            {
                wheel.UpdateWheel();
            }

            base.Update();
        }

        private void UpdateControls()
        {
            var gamePad = GetComponent<GamePadHandler>();
            var keyboard = GetComponent<KeyboardHandler>();
            var acc = 0f;
            if (gamePad.IsConnected(PlayerIndex.One))
                acc = gamePad.GetRightTrigger(PlayerIndex.One);
            else
                acc = keyboard.KeyDown(Keys.W) ? 0.5f : 0f;

            if (acc > 0f)
                Speed += acc * 0.01f;
            else if (Speed > 0f)
            {
                Speed -= 0.002f;
                if (Speed <= 0f)
                    Speed = 0f;
            }

            if (gamePad.IsConnected(PlayerIndex.One))
                Turning = gamePad.GetLeftStick(PlayerIndex.One).X;
            else
                Turning = (keyboard.KeyDown(Keys.D) ? 1f : 0f) -
                    (keyboard.KeyDown(Keys.A) ? 1f : 0f);
        }

        public void UpdateNetwork(Vector3 position, Vector3 rotation, float speed, float turning)
        {
            _position = position;
            _rotation = rotation;
            Speed = speed;
            Turning = turning;
        }

        private float GetWheelHeight(WheelType type)
            => _wheelHeights.Any(w => w.type == type) ? _wheelHeights.First(w => w.type == type).height : 0f;

        protected void AddWheel(Wheel wheel)
        {
            if (_wheels == null)
                _wheels = new List<Wheel>();

            _wheels.Add(wheel);
            _container.AddObject(wheel);
        }

        public override void OnRemove()
        {
            foreach (var wheel in _wheels)
                _container.RemoveObject(wheel);

            base.OnRemove();
        }

        protected void SetTexture(string source, Color color)
        {
            var texture = GetComponent<Resources>().LoadTexture(source).Clone();
            texture.Replace(new Color(255, 0, 255), color);
            texture.Name += color.GetHashCode().ToString();
            Texture = texture;
        }

        protected bool HasOption(string name)
        {
            return Options.Any(o => o.Name == name);
        }

        #region properties

        #endregion

        #region description

        public abstract string ModelName { get; }
        public abstract string Description { get; }
        public abstract int ModelYear { get; }
        public abstract string Manufacturer { get; }

        #endregion

        #region configurator settings

        public ConfigOption[] AvailableConfigOptions { get; protected set; }

        protected abstract void PrepareConfigOptions();

        public virtual PreviewPosition[] PreviewPositions
            => new[]
            {
                new PreviewPosition
                {
                    Name = "Default View",
                    Position = new Vector3(2, 2, 2),
                    Yaw = MathHelper.PiOver4,
                    Pitch = -0.1f,
                }
            };

        protected IEnumerable<ConfigOption> GenerateConfigs(string category, string[] options, bool toggleCategory = false)
        {
            return options.Select(o => new ConfigOption { Name = o, Category = category, ToggleCategory = toggleCategory });
        }

        private static Random _r = new Random();

        protected void EquipDefaultOptions(ConfigOption[] options)
        {
            PrepareConfigOptions();

            var categories = AvailableConfigOptions.Where(o => o.ToggleCategory && (options == null || !options.Any(eo => eo.Category == o.Category)))
                .GroupBy(option => option.Category)
                .ToDictionary(e => e.Key, e => e.ToArray());

            Options = categories.Select(c => c.Value[0]).Concat(options ?? new ConfigOption[0]).ToArray();
            //Options = categories.Select((c, i) => c.Value[_r.Next(0, categories.Values.ElementAt(i).Length)]).Concat(options ?? new ConfigOption[0]).ToArray();
        }

        #endregion
    }
}
