using CruiseControl.Scenes;
using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CruiseControl.GameWorld
{
    internal abstract class World
    {
        protected IWorldScreen _parentScreen;
        protected List<IObjectContainer> _containers;
        protected IObjectContainer _activeContainer;

        public Vector3 CameraPosition => _parentScreen.Camera.Position;
        public ObjectRenderer Renderer => _parentScreen.Renderer;
        public IObjectContainer ActiveContainer => _activeContainer;
        public Vector3 Offset { get; protected set; }

        public World(IWorldScreen screen)
        {
            _parentScreen = screen;
        }
        
        public virtual void Update(GameTime gameTime)
        {
            lock (_containers)
            {
                _containers.ForEach(c => c.Update(gameTime));
            }
        }

        public virtual void Render(GameTime gameTime)
        {
            lock (_containers)
            {
                _containers.ForEach(c => c.Render(gameTime));
            }
        }
    }
}
