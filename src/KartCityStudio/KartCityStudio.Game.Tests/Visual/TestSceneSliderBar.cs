using KartCityStudio.Game.Graphics.UserInterface;
using NUnit.Framework;
using osu.Framework.Graphics;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneSliderBar : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        //private KCSScrollBar mainScrollBar;
        private readonly KCSSliderBar<double> sliderBar;
        public TestSceneSliderBar()
        {
            //Add(mainScrollBar = new KCSScrollBar());
            Add(sliderBar = new KCSSliderBar<double>()
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                RelativePositionAxes = Axes.X,
                Width = 0.7f,
                Height =10f,
                Y = 20f,
                MinValue = 0,
                MaxValue = 100
            });
        }
    }
}
