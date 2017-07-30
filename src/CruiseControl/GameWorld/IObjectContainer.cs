using CruiseControl.GameWorld.Objects;
using CruiseControl.Scenes;
using Microsoft.Xna.Framework;

namespace CruiseControl.GameWorld
{
    interface IObjectContainer
    {
        IWorldScreen Screen { get; }
        Vector3 Offset { get; }
        Vector3 Size { get; }
        void AddObject(GameObject obj);
        void RemoveObject(GameObject obj);
        void LoadContent();
        void Update(GameTime gameTime);
        void Render(GameTime gameTime);
        Floor GetFloor(Vector3 position);
    }
}
