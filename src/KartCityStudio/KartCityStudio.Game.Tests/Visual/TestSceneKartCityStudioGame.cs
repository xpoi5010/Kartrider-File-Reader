using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Platform;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneKartCityStudioGame : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        private KartCityStudioGame game;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new KartCityStudioGame();
            game.SetHost(host);

            AddGame(game);
        }
    }
}
