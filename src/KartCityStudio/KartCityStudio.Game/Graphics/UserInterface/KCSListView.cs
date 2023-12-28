using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartCityStudio.Game.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSListView : ListView
    {

        public KCSListView()
        {
            ItemsContainer.Padding = new MarginPadding { Horizontal = 10, Vertical = 1 };
            HeadersContainer.Padding = new MarginPadding { Horizontal = 10, Vertical = 0 };
        }

        protected override DrawableListViewItem CreateDrawableListViewItem(ListViewItem item) => new KCSListViewItem(item);

        protected override DrawableListViewHeaderItem CreateDrawableListViewHeaderItem(ListViewHeaderItem header) => new KCSListViewHeaderItem(header);

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) => new KCSScrollContainer<Drawable>();
    }
}
