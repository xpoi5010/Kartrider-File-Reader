using KartCityStudio.Game.Graphics.UserInterface;
using NUnit.Framework;
using osu.Framework.Graphics;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneTextBox : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        //private KCSScrollBar mainScrollBar;
        private readonly KCSTextbox textbox;
        public TestSceneTextBox()
        {
            //Add(mainScrollBar = new KCSScrollBar());
            Add(textbox = new KCSTextbox()
            {
                RelativeSizeAxes = Axes.X,
                Width = 0.5f,
                Height = 30f,
                X = 30f,
                Y = 20f,
            });
        }
    }
}
