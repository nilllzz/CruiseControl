using Microsoft.Xna.Framework;

namespace CruiseControl
{
    internal abstract class Screen
    {
        internal virtual void LoadContent() { }
        internal abstract void Draw(GameTime gameTime);
        internal abstract void Update(GameTime gameTime);
    }
}
