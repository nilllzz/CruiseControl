using Microsoft.Xna.Framework;

namespace CruiseControl.GameWorld.Objects.ConfigScreen
{
    abstract class ConfigScreenProp : GameObject
    {
        public ConfigScreenProp(IObjectContainer parent)
            : base(parent)
        {
        }

        public override void Update()
        {
            _position.Z -= 0.05f;
            Alpha = 1f - Vector3.Distance(_position, Vector3.Zero) / 50f;

            base.Update();
            base.CreateWorld();
        }
    }
}
