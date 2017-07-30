using CruiseControl.GameWorld;
using CruiseControl.GameWorld.Objects.ConfigScreen;
using CruiseControl.GameWorld.Objects.Vehicles;
using CruiseControl.GameWorld.Objects.Vehicles.Cord;
using CruiseControl.GameWorld.Objects.Vehicles.Reichswagen;
using GameDevCommon;
using GameDevCommon.Drawing;
using GameDevCommon.Input;
using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core;

namespace CruiseControl.Scenes.Configurator
{
    internal class ConfiguratorScreen : Screen, IWorldScreen
    {
        private ConfiguratorWorld _world;
        private ConfiguratorCamera _camera;
        private ConfiguratorRenderer _renderer;

        private Vehicle _vehicle;

        private SpriteBatch _batch;
        private SpriteFont _font, _fontSmall;
        private RenderTarget2D _uiTarget;
        private int _categoryIndex = 0;
        private int[] _itemIndex;
        private int _previewPositionIndex = 0;
        private Dictionary<string, ConfigOption[]> _options;

        public Camera Camera => _camera;
        public ObjectRenderer Renderer => _renderer;
        public World World => _world;

        public ConfiguratorScreen()
        {
            _renderer = new ConfiguratorRenderer();
            _camera = new ConfiguratorCamera();
            _world = new ConfiguratorWorld(this);
            _vehicle = new C15(_world, Vector3.Zero);
            _vehicle.ControlSource = VehicleControlSource.Config;
            _camera.ApplyPreviewPosition(_vehicle.PreviewPositions[0]);

            PrepareOptions();
        }

        private void PrepareOptions()
        {
            _options = _vehicle.AvailableConfigOptions.GroupBy(option => option.Category)
                .ToDictionary(e => e.Key, e => e.ToArray());

            _itemIndex = new int[_options.Count];
        }

        internal override void LoadContent()
        {
            Renderer.LoadContent();
            _world.AddObject(_vehicle);
            _world.AddObject(new Road(_world, new Vector3(0f), 0f, 1000));
            _world.LoadContent();

            _batch = new SpriteBatch(Controller.GraphicsDevice);
            _font = Controller.Content.Load<SpriteFont>("Fonts/CustomFontLarge");
            _fontSmall = Controller.Content.Load<SpriteFont>("Fonts/CustomFont");
            _uiTarget = RenderTargetManager.CreateRenderTarget(GameController.RENDER_WIDTH, GameController.RENDER_HEIGHT);
        }

        internal override void Draw(GameTime gameTime)
        {
            RenderWorld(gameTime);
            RenderUI();
        }

        private void RenderWorld(GameTime gameTime)
        {
            Controller.GraphicsDevice.ResetFull();
            Controller.GraphicsDevice.ClearFull(new Color(235, 235, 235));

            Renderer.PrepareRender(Camera);

            World.Render(gameTime);
        }

        private void RenderUI()
        {
            const int length = 140;
            const int bufferLength = 4;

            RenderTargetManager.BeginRenderToTarget(_uiTarget);
            Controller.GraphicsDevice.ClearFull(Color.Transparent);

            _batch.Begin(SpriteBatchUsage.Default);

            _batch.DrawGradient(new Rectangle(8, 8, length + 4, 30), new Color(0, 0, 0, 200), new Color(0, 0, 0, 150), false);

            var categoryCount = _options.Count;
            var itemLength = (length - ((categoryCount - 1) * bufferLength)) / categoryCount;
            for (int i = 0; i < categoryCount; i++)
            {
                _batch.DrawRectangle(new Rectangle(10 + i * (itemLength + bufferLength), 10, itemLength, 4),
                    i == _categoryIndex ? Color.White : new Color(255, 255, 255, 100));
            }

            _batch.DrawString(_font, _options.Keys.ElementAt(_categoryIndex), new Vector2(10, 18), Color.White,
                0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);

            var options = _options[_options.Keys.ElementAt(_categoryIndex)];
            for (int i = 0; i < options.Length; i++)
            {
                var option = options[i];

                bool isEquipped = _vehicle.Options.Any(o => o.Name == option.Name);
                if (i == _itemIndex[_categoryIndex])
                {
                    var backgroundColor = isEquipped ? new Color(32, 94, 221, 200) : new Color(0, 0, 0, 200);
                    _batch.DrawRectangle(new Rectangle(8, 40 + i * 26, length + 4, 24), backgroundColor);
                }
                else
                {
                    _batch.DrawRectangle(new Rectangle(8, 40 + i * 26, length + 4, 24), new Color(100, 100, 100, 150));
                }
                if (isEquipped)
                    _batch.DrawRectangle(new Rectangle(8, 40 + i * 26, length + 4, 2), new Color(32, 94, 221));

                _batch.DrawString(_fontSmall, option.Name, new Vector2(10, 48 + i * 26), Color.White,
                    0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                if (option.Category == ConfigOption.CATEGORY_BODY_COLORS)
                {
                    _batch.DrawCircle(new Vector2(130, 44 + i * 26), 16, new Color(0, 0, 0, 100));
                    _batch.DrawCircle(new Vector2(131, 45 + i * 26), 14, VehicleColorProvider.GetColor(option.Name));
                }
            }

            var descSize = System.Math.Max(_fontSmall.MeasureString(_vehicle.Description).X + 8, length * 2);
            _batch.DrawGradient(new Rectangle(length + 16, 8, (int)descSize, 30), new Color(0, 0, 0, 200), new Color(0, 0, 0, 150), false);

            _batch.DrawString(_fontSmall, _vehicle.Manufacturer, new Vector2(length + 20, 14), Color.White);
            var manufacturerSize = _fontSmall.MeasureString(_vehicle.Manufacturer);
            _batch.DrawString(_font, _vehicle.ModelName, new Vector2(length + 24 + manufacturerSize.X, 10), Color.White);
            _batch.DrawString(_fontSmall, _vehicle.Description, new Vector2(length + 20, 28), Color.White);

            _batch.End();

            RenderTargetManager.EndRenderToTarget();

            _batch.Begin(SpriteBatchUsage.Default);
            _batch.Draw(_uiTarget, Vector2.Zero, Color.White);
            _batch.End();
        }

        private Random _random = new Random();
        private List<ConfigScreenProp> _props = new List<ConfigScreenProp>();

        internal override void Update(GameTime gameTime)
        {
            if (_random.Next(0, 500) == 0)
            {
                var tree = new Tree(_world, new Vector3((float)(-30 * _random.NextDouble() - 10), 0f, 40f));
                _world.AddObject(tree);
                _world.LoadContent();
                _props.Add(tree);
            }
            if (_random.Next(0, 500) == 0)
            {
                var tree = new Tree(_world, new Vector3((float)(30 * _random.NextDouble() + 10), 0f, 40f));
                _world.AddObject(tree);
                _world.LoadContent();
                _props.Add(tree);
            }

            Camera.Update();
            World.Update(gameTime);

            UpdatePreview();
            UpdateUI();

            int i = 0;
            bool removedObj = false;
            while (i < _props.Count && !removedObj)
            {
                var prop = _props[i];
                if (prop.Position.Z < 0f && Vector3.Distance(prop.Position, Vector3.Zero) > 50)
                {
                    _world.RemoveObject(prop);
                    _props.Remove(prop);
                    removedObj = true;
                }
                i++;
            }
        }

        private void UpdatePreview()
        {
            var controlsHandler = GetComponent<ControlsHandler>();
            if (_vehicle.PreviewPositions.Length > 1)
            {
                if (controlsHandler.RightPressed(PlayerIndex.One, InputDirectionType.WASD, InputDirectionType.RightThumbStick))
                {
                    _previewPositionIndex++;
                    if (_previewPositionIndex == _vehicle.PreviewPositions.Length)
                        _previewPositionIndex = 0;

                    _camera.ApplyPreviewPosition(_vehicle.PreviewPositions[_previewPositionIndex]);
                }
                if (controlsHandler.LeftPressed(PlayerIndex.One, InputDirectionType.WASD, InputDirectionType.RightThumbStick))
                {
                    _previewPositionIndex--;
                    if (_previewPositionIndex == -1)
                        _previewPositionIndex = _vehicle.PreviewPositions.Length - 1;

                    _camera.ApplyPreviewPosition(_vehicle.PreviewPositions[_previewPositionIndex]);
                }
            }
        }

        private void UpdateUI()
        {
            var gamePadHandler = GetComponent<GamePadHandler>();
            var controlsHandler = GetComponent<ControlsHandler>();
            var keyboardHandler = GetComponent<KeyboardHandler>();

            if (gamePadHandler.ButtonPressed(PlayerIndex.One, Buttons.RightShoulder) || keyboardHandler.KeyPressed(Keys.Right))
            {
                _categoryIndex++;
                if (_categoryIndex == _options.Count)
                    _categoryIndex = 0;
            }
            if (gamePadHandler.ButtonPressed(PlayerIndex.One, Buttons.LeftShoulder) || keyboardHandler.KeyPressed(Keys.Left))
            {
                _categoryIndex--;
                if (_categoryIndex == -1)
                    _categoryIndex = _options.Count - 1;
            }

            var itemCount = _options.ElementAt(_categoryIndex).Value.Length;
            if (itemCount > 1)
            {
                if (controlsHandler.DownPressed(PlayerIndex.One, InputDirectionType.ArrowKeys, InputDirectionType.LeftThumbStick))
                {
                    _itemIndex[_categoryIndex]++;
                    if (_itemIndex[_categoryIndex] == itemCount)
                        _itemIndex[_categoryIndex] = 0;
                }
                if (controlsHandler.UpPressed(PlayerIndex.One, InputDirectionType.ArrowKeys, InputDirectionType.LeftThumbStick))
                {
                    _itemIndex[_categoryIndex]--;
                    if (_itemIndex[_categoryIndex] == -1)
                        _itemIndex[_categoryIndex] = itemCount - 1;
                }
            }

            if (gamePadHandler.ButtonPressed(PlayerIndex.One, Buttons.A) || keyboardHandler.KeyPressed(Keys.Space))
            {
                var option = _options.ElementAt(_categoryIndex).Value[_itemIndex[_categoryIndex]];
                if (option.ToggleCategory)
                {
                    var currentOption = _vehicle.Options.First(o => o.Category == option.Category);
                    if (currentOption.Name != option.Name)
                    {
                        var options = _vehicle.Options.ToList();
                        options.Remove(currentOption);
                        options.Add(option);

                        _world.RemoveObject(_vehicle);
                        _vehicle = new C15(_world, Vector3.Zero, options.ToArray());
                        _vehicle.ControlSource = VehicleControlSource.Config;
                        _world.AddObject(_vehicle);
                        _world.LoadContent();
                    }
                }
                else
                {
                    var options = _vehicle.Options.ToList();
                    if (options.Any(o => o.Name == option.Name))
                        options.Remove(option);
                    else
                        options.Add(option);

                    _world.RemoveObject(_vehicle);
                    _vehicle = new C15(_world, Vector3.Zero, options.ToArray());
                    _vehicle.ControlSource = VehicleControlSource.Config;
                    _world.AddObject(_vehicle);
                    _world.LoadContent();
                }
            }
        }
    }
}
