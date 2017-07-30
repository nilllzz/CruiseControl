using Microsoft.Xna.Framework;

namespace CruiseControl.GameWorld.Objects
{
    /// <summary>
    /// Cars can drive on this, defines height points
    /// </summary>
    internal abstract class Floor : GameObject
    {
        public Rectangle BoundingRectangle { get; protected set; }

        public Floor(IObjectContainer parent)
            : base(parent)
        { }

        public virtual float GetHeightForPosition(Vector2 position) => 0f;
    }
}
