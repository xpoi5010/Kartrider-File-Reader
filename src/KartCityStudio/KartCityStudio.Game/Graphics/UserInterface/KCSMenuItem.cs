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
    public partial class DrawableKCSMenuItem: Menu.DrawableMenuItem
    {
        #region Members
        private KCSMenuItemTextContainer text;
        #endregion
        #region Properies
        
        #endregion
        #region Constructors
        public DrawableKCSMenuItem(MenuItem item): base(item)
        {
            
        }
        #endregion

        [BackgroundDependencyLoader]
        private void load()
        {
            BackgroundColour = Colour4.Transparent;
            BackgroundColourHover = Colour4.FromHex("0F6292");
            BorderColour = Colour4.FromHex("577D86");
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

        protected virtual KCSMenuItemTextContainer CreateTextContainer() => new KCSMenuItemTextContainer();

        protected override void UpdateBackgroundColour()
        {
            if (State == MenuItemState.Selected)
                Background.FadeColour(BackgroundColourHover);
            else
                base.UpdateBackgroundColour();
        }

        protected partial class KCSMenuItemTextContainer: Container, IHasText
        {
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

            public KCSMenuItemTextContainer()
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
                        Margin = new MarginPadding { Horizontal = 11 }
                    }
                };
            }
        }
    }

    public class KCSMenuItemSpacer : MenuItem
    {
        public KCSMenuItemSpacer() : base("")
        {

        }
    }

    public partial class DrawableKCSMenuItemSpacer: DrawableKCSMenuItem
    {
        public DrawableKCSMenuItemSpacer(MenuItem item): base(item)
        {
            AddInternal(new Box()
            {
                Anchor = Anchor.Centre,
                Origin= Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                Colour = Colour4.FromHex("6F6F6F"),
                Size = new Vector2(1, 2)
            });
        }
    }
}
