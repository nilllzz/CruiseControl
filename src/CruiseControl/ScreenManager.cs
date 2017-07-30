using Microsoft.Xna.Framework;

namespace CruiseControl
{
    internal class ScreenManager : IGameComponent
    {
        internal Screen ActiveScreen { get; private set; }

        public void Initialize()
        {
            var screen = new Scenes.InGame.InGameScreen();
            //var screen = new Scenes.Configurator.ConfiguratorScreen();
            SetScreen(screen);
        }

        internal void SetScreen(Screen screen)
        {
            ActiveScreen = screen;
        }
    }
}
