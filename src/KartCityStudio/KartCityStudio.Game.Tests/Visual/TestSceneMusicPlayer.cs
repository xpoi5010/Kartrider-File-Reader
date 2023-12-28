using KartCityStudio.Game.Graphics.UserInterface;
using KartLibrary.File;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneMusicPlayer : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        [Resolved]
        private KartStorageSystem storageSystem { get; set; }

        private KCSMusicPlayer musicPlayer;

        public TestSceneMusicPlayer()
        {
            Add(musicPlayer = new KCSMusicPlayer()
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
            });
        }
    }
}
