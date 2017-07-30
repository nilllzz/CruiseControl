using GameDevCommon.Rendering;
using static Core;

namespace CruiseControl.Scenes.Configurator
{
    internal class ConfiguratorRenderer : ObjectRenderer
    {
        public ConfiguratorRenderer()
            : base(Controller.GraphicsDevice)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _effect.LightingEnabled = false;
        }
    }
}
