using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Platform.Windows;
using osu.Framework.Screens;
using SDL2;

namespace KartCityStudio.Game
{
    public partial class KartCityStudioGame : KartCityStudioGameBase
    {
        private ScreenStack screenStack;

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
            Host.Window.Title = "KartCityStudio";
            Host.Window.CursorState = CursorState.Default;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            
            screenStack.Push(new MainScreen());
        }
    }
}
