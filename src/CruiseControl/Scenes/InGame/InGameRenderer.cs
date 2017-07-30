using GameDevCommon.Rendering;
using Microsoft.Xna.Framework;
using static Core;

namespace CruiseControl.Scenes.InGame
{
    internal class InGameRenderer : ObjectRenderer
    {
        public InGameRenderer()
            : base(Controller.GraphicsDevice)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _effect.LightingEnabled = false;
            _effect.FogEnabled = false;
            _effect.FogColor = new Color(235, 235, 235).ToVector3();
            _effect.FogStart = 90;
            _effect.FogEnd = 100;
        }
    }
}
