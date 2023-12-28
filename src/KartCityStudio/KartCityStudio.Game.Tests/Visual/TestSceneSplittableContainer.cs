using KartCityStudio.Game.Graphics.Containers;
using KartCityStudio.Game.Graphics.UserInterface;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Containers;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneSplittableContainer : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        private KCSSplittableContainer mainSplittableContainer;
        private KCSSplittableContainer mainSplittableContainer2;
        private KCSSplittableContainer mainSplittableContainer3;
        public TestSceneSplittableContainer()
        {
            Add(mainSplittableContainer = new KCSSplittableContainer(Direction.Horizontal)
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
            });
            mainSplittableContainer.FirstContainer.Add(new BasicButton()
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
                Text = $"Left",
                Position = new osuTK.Vector2(0, 0)
            });
            mainSplittableContainer.SecondContainer.Add(mainSplittableContainer2 = new KCSSplittableContainer(Direction.Horizontal)
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
            });
            mainSplittableContainer2.FirstContainer.Add(new BasicButton()
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
                Text = $"Middle",
                Position = new osuTK.Vector2(0, 0)
            });
            mainSplittableContainer2.SecondContainer.Add(mainSplittableContainer3 = new KCSSplittableContainer(Direction.Vertical)
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
            });
            mainSplittableContainer3.FirstContainer.Add(new BasicButton()
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
                Text = $"TopRight",
                Position = new osuTK.Vector2(0, 0)
            });
            mainSplittableContainer3.SecondContainer.Add(new BasicButton()
            {
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
                Text = $"BottomRight",
                Position = new osuTK.Vector2(0, 0)
            });
        }
    }
}
