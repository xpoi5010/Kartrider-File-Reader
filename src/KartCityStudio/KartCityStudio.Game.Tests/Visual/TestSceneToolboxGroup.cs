using KartCityStudio.Game.Graphics.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace KartCityStudio.Game.Tests.Visual
{
    public partial class TestSceneToolboxGroup : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        private KCSToolboxGroup toolboxGroup;
        public TestSceneToolboxGroup()
        {
            Add(new Container()
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding() { Horizontal = 7, Vertical = 7 },
                Child = toolboxGroup = new KCSToolboxGroup()
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    GroupText = "TestGroup",
                    Masking = true,
                    CornerRadius = 5,
                    Children = new Drawable[]
                    {
                        new BasicButton()
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 25,
                            Text = "Hello world!"
                        }
                    }
                }
            });
        }
    }
}
