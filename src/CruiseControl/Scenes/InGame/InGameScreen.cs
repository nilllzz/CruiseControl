using CruiseControl.GameWorld;
using GameDevCommon.Drawing;
using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using static Core;

namespace CruiseControl.Scenes.InGame
{
    internal class InGameScreen : Screen, IWorldScreen
    {
        private InGameWorld _world;
        private Camera _camera;
        private InGameRenderer _renderer;
        private NetworkClient _client;

        public Camera Camera => _camera;
        public ObjectRenderer Renderer => _renderer;
        public World World => _world;

        public InGameScreen()
        {
            _renderer = new InGameRenderer();
            _camera = new FollowCamera();
            //_camera = new OverviewCamera();
            _world = new InGameWorld(this);
            _client = new NetworkClient(_world);
        }

        internal override void LoadContent()
        {
            Renderer.LoadContent();
            Task.Run(async () =>
            {
                _world.LoadContent();

                await OffsetCamera();

                _client.Open();
            });
        }
        
        private async Task OffsetCamera()
        {
            if (_camera is OverviewCamera)
            {
                bool found = false;
                do
                {
                    var floor = _world.ActiveContainer.GetFloor(_camera.Position);
                    if (floor != null)
                    {
                        var y = floor.GetHeightForPosition(new Vector2(_camera.Position.X, _camera.Position.Z));
                        _camera.Position += new Vector3(0, y, 0);
                        found = true;
                    }
                    else
                    {
                        await Task.Delay(10);
                    }
                } while (!found);
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            Controller.GraphicsDevice.ResetFull();
            Controller.GraphicsDevice.ClearFull(Color.SkyBlue);

            Renderer.PrepareRender(Camera);

            World.Render(gameTime);
        }

        internal override void Update(GameTime gameTime)
        {
            World.Update(gameTime);
            Camera.Update();
        }
    }
}
