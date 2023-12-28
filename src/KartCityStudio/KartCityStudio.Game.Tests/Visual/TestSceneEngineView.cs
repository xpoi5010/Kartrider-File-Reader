using KartCityStudio.Game.Graphics.Engine;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneEngineView : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        public TestSceneEngineView()
        {
            Add(new KartEngineView() { });
        }
    }
}
