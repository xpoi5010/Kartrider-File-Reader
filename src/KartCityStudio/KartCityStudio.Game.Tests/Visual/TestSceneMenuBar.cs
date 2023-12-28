using KartCityStudio.Game.Graphics.UserInterface;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneMenuBar : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        private KCSMenu mainMenubar;
        public TestSceneMenuBar()
        {
            Add(new Container
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.X,
                Height = 25,
                Child = mainMenubar = new KCSMenu()
                {
                    RelativeSizeAxes = Axes.Both,
                    Items = new[]
                    {
                        new MenuItem("File", () => { })
                        {
                            Items = new[]
                            {
                                new MenuItem("Open File", () =>
                                {
                                    
                                }),
                                new MenuItem("Create File and delete the file that new created", () => { }),
                            },
                        },
                        new MenuItem("Edit", () => { })
                        {
                            Items = new[]
                            {
                                new MenuItem("Add File", () => { }),
                                new MenuItem("Remove All", () => { }),
                            }
                        },
                        new MenuItem("About", () => { })
                        {
                            
                        },
                    }
                }
            });
        }
    }
}
