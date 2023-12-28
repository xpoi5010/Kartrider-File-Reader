using System.Threading.Tasks;
using KartCityStudio.Game.Graphics.UserInterface;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneListBox : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        private KCSListBox listbox;
        public TestSceneListBox()
        {
            Add(listbox = new KCSListBox()
            {
                RelativeSizeAxes = Axes.Both,
                BackgroundColour = Colour4.Black,
            });
            AddStep("Add 100 items to ListBox.", () =>
            {
                for (int i = 0; i < 100; i++)
                    listbox.Items.Add(new ListBoxItem($"TestItem{listbox.Items.Count + 1}"));
            });
            listbox.Items.Add(new ListBoxItem("TestItem1!"));
            listbox.Items.Add(new ListBoxItem("TestItem2!"));
        }
    }
}
