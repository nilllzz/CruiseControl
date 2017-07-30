using CruiseControl.GameWorld;
using GameDevCommon.Rendering;

namespace CruiseControl.Scenes
{
    interface IWorldScreen
    {
        Camera Camera { get; }
        ObjectRenderer Renderer { get; }
        World World { get; }
    }
}
