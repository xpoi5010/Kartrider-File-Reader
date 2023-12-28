using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Framework.Graphics.Containers;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSListViewItem: ListView.DrawableListViewItem
    {
        #region Members
        private KCSSubMenuItemTextContainer text;
        #endregion
        #region Properies

        #endregion
        #region Constructors
        public KCSListViewItem(ListViewItem item) : base(item)
        {
            Margin = new MarginPadding { Vertical = 2 };
        }
        #endregion

        [BackgroundDependencyLoader]
        private void load()
        {
            BackgroundColour = Colour4.Transparent;
            BackgroundHoverColour = Colour4.FromHex("1A1A1A");
            BackgroundSelectedColour = Colour4.FromHex("3887BE");
            BorderColour = Colour4.Transparent;
            Masking = true;
            CornerRadius = 5f;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Foreground.Anchor = Anchor.CentreLeft;
            Foreground.Origin = Anchor.CentreLeft;
        }

        protected sealed override Drawable CreateContent() => text = CreateTextContainer();

        protected virtual KCSSubMenuItemTextContainer CreateTextContainer() => new KCSSubMenuItemTextContainer();

        protected partial class KCSSubMenuItemTextContainer : Container, IHasText
        {
            private readonly SpriteIcon listBoxItemIcon;
            private readonly SpriteText listBoxItemText;
            private LocalisableString text;

            public LocalisableString Text
            {
                get => text;
                set
                {
                    text = value;
                    listBoxItemText.Text = value;
                }
            }

            public KCSSubMenuItemTextContainer()
            {
                Anchor = Anchor.CentreLeft;
                Origin = Anchor.CentreLeft;
                AutoSizeAxes = Axes.Y;
                Children = new Drawable[]
                {
                    listBoxItemText = new SpriteText()
                    {
                        AlwaysPresent = true,
                        Font = KCSFont.Default.With(size : 17f),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Shadow = true,
                        Margin = new MarginPadding { Horizontal = 22, Vertical = 4 }
                    }
                };
            }
        }
    }

    
}
