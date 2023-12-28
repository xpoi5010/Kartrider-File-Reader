using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Primitives;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Localisation;
using osu.Framework.IO.Stores;
using System.Runtime.CompilerServices;
using osu.Framework.Platform;
using osu.Framework.Platform.Windows;
using System.Reflection;
using Veldrid;
using NUnit.Framework.Internal;
using osu.Framework.Logging;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSSubMenuItem: Menu.DrawableMenuItem
    {
        #region Members
        private KCSSubMenuItemTextContainer text;
        #endregion
        #region Properies
        
        #endregion
        #region Constructors
        public KCSSubMenuItem(MenuItem item): base(item)
        {
            Margin = new MarginPadding { Vertical = 2 };
        }
        #endregion

        [BackgroundDependencyLoader]
        private void load()
        {
            BackgroundColour = Colour4.Transparent;
            BackgroundColourHover = Colour4.FromHex("569DAA");
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

        protected partial class KCSSubMenuItemTextContainer: Container, IHasText
        {
            private readonly SpriteIcon menuItemIcon;
            private readonly SpriteText menuItemText;
            private LocalisableString text;

            public LocalisableString Text
            {
                get => text;
                set
                {
                    text = value;
                    menuItemText.Text = value;
                }
            }

            public KCSSubMenuItemTextContainer()
            {
                Anchor = Anchor.CentreLeft;
                Origin = Anchor.CentreLeft;
                AutoSizeAxes = Axes.Both;
                Children = new Drawable[]
                {
                    menuItemText = new SpriteText()
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
