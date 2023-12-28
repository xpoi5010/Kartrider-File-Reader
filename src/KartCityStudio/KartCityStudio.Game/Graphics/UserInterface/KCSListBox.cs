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
    public partial class KCSListBox : ListBox
    {
        
        public KCSListBox()
        {
            ItemsContainer.Padding = new MarginPadding { Horizontal = 10, Vertical = 6 };
        }

        protected override DrawableListBoxItem CreateDrawableListBoxItem(ListBoxItem item) => new KCSListBoxItem(item);

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) => new KCSScrollContainer<Drawable>();
    }
}
