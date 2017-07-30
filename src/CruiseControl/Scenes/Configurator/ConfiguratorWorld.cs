using CruiseControl.GameWorld;
using CruiseControl.GameWorld.Objects;
using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using static Core;

namespace CruiseControl.Scenes.Configurator
{
    internal class ConfiguratorWorld : World, IObjectContainer
    {
        public IWorldScreen Screen => _parentScreen;
        public Vector3 Size => Vector3.One;

        private RenderObjectCollection _objects;

        public ConfiguratorWorld(IWorldScreen screen)
            : base(screen)
        {
            _objects = new RenderObjectCollection();
        }

        public void AddObject(GameObject obj)
        {
            _objects.Add(obj);
        }

        public void RemoveObject(GameObject obj)
        {
            obj.OnRemove();
            _objects.Remove(obj);
            obj.Dispose();
        }

        public void LoadContent()
        {
            foreach (var obj in _objects.Where(o => !o.LoadedContent))
            {
                obj.LoadContent();
            }
        }

        public override void Render(GameTime gameTime)
        {
            Controller.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            for (int i = 0; i < _objects.OpaqueObjects.Count(); i++)
                Renderer.Render(_objects.OpaqueObjects.ElementAt(i));

            Controller.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            for (int i = 0; i < _objects.TransparentObjects.Count(); i++)
                Renderer.Render(_objects.TransparentObjects.ElementAt(i));
        }

        public override void Update(GameTime gameTime)
        {
            _objects.ForEach(o => o.Update());
            _objects.Sort();
        }

        public Floor GetFloor(Vector3 position) => null;
    }
}
