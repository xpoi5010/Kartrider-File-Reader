using KartCityStudio.Game.Graphics.Containers;
using KartCityStudio.Game.Graphics.UserInterface;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Containers;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneScrollContainer : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        private KCSScrollContainer<Drawable> mainScrollContainer;
        public TestSceneScrollContainer()
        {
            Add(mainScrollContainer = new KCSScrollContainer<Drawable>()
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1,1),
            });
            for(int i = 0; i < 30; i++)
                mainScrollContainer.Add(new BasicButton()
                {
                    RelativeSizeAxes = Axes.X,
                    Size = new osuTK.Vector2(1, 30),
                    Text = $"Hello!{i}",
                    Position = new osuTK.Vector2(0, i * 30)
                });
        }
    }
}
