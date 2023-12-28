using System;
using System.Threading.Tasks;
using KartCityStudio.Game.Graphics.UserInterface;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

namespace KartCityStudio.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneListView : KartCityStudioTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.
        private KCSListView listview;

        private ListViewHeaderItem nameHeader;
        private ListViewHeaderItem idHeader;
        private ListViewHeaderItem scoreHeader;

        public TestSceneListView()
        {
            Add(listview = new KCSListView()
            {
                RelativeSizeAxes = Axes.Both,
                BackgroundColour = Colour4.Black,
            });
            listview.Headers.Add(nameHeader = new ListViewHeaderItem("Name", "Name", 0.7f));
            listview.Headers.Add(idHeader = new ListViewHeaderItem("ID", "ID", 0.3f));

            AddStep("Add 100 items to ListView.", () =>
            {
                for (int i = 0; i < 100; i++)
                    listview.Items.Add(new ListViewItem(new(string, LocalisableString)[]
                    {
                        ("Name", $"Item{listview.Items.Count}"),
                        ("ID", $"{listview.Items.Count}"),
                        ("Score", $"{(((i + 0x12345678) * 0xFC905B) % 100)}"),
                    }));
            });
            AddStep("Adjust field size", () =>
            {
                nameHeader.FieldWidth.Value = 0.5f;
                idHeader.FieldWidth.Value = 0.5f;
            });
            AddStep("Add a new field: Score.", () =>
            {
                listview.Headers.Add(scoreHeader = new ListViewHeaderItem("Score", "Score", 0.3f));
                nameHeader.FieldWidth.Value = 0.4f;
                idHeader.FieldWidth.Value = 0.3f;
            });
            AddStep("Change header to chinese name.", () =>
            {
                nameHeader.Text.Value = "名字";
                idHeader.Text.Value = "編號";
                scoreHeader.Text.Value = "分數";
            });
            AddStep("Remove a field: ID", () =>
            {
                listview.Headers.Remove(idHeader);
            });
            AddStep("Modify one of texts of first ListViewItem.", () =>
            {
                ListViewItem item = listview.Items[0];
                item.Texts["Name"] = $"Modified!{DateTime.Now.Microsecond}";
            });
            AddStep("Remove one of texts of first ListViewItem.", () =>
            {
                ListViewItem item = listview.Items[0];
                if(item.Texts.ContainsKey("Name"))
                    item.Texts.Remove("Name");
            });
            AddStep("Add 10000 items to ListView.", () =>
            {
                for (int i = 0; i < 10000; i++)
                    listview.Items.Add(new ListViewItem(new (string, LocalisableString)[]
                    {
                        ("Name", $"Item{listview.Items.Count}"),
                        ("ID", $"{listview.Items.Count}"),
                        ("Score", $"{(((i + 0x12345678) * 0xFC905B) % 100)}"),
                    }));
            });
        }
    }
}
