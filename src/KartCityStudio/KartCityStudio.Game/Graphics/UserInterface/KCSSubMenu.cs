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
using osu.Framework.Platform;
using osu.Framework.Logging;
using osuTK.Graphics.ES20;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSSubMenu : Menu
    {
        public KCSSubMenu(): base(Direction.Vertical, false)
        {
            ItemsContainer.Padding = new MarginPadding()
            {
                Top = 5,
                Bottom = 5,
                Left = 5,
                Right = 5,
            };
            BackgroundColour = new Color4(0x1A, 0x1A, 0x1A, 0xFF);
            MaskingContainer.CornerRadius = 7.5f;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            //AutoSizeAxes = Axes.X;
        }

        protected override Menu CreateSubMenu() => new KCSSubMenu();

        protected override DrawableMenuItem CreateDrawableMenuItem(MenuItem item)
        {
            switch (item)
            {
                case KCSMenuItemSpacer spacer:
                    return new DrawableKCSMenuItemSpacer(spacer);
                default:
                    return new KCSSubMenuItem(item);
            }
        }

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction)
        {
            return new BasicScrollContainer<Drawable>(scrollDirection: Direction.Vertical)
            {
                ClampExtension = 0,
                ScrollbarVisible = false,
            };
        }

        protected override void UpdateSize(Vector2 newSize)
        {
            base.UpdateSize(newSize);
            if (Direction == Direction.Vertical)
            {
                Width = newSize.X + ItemsContainer.Padding.Left + ItemsContainer.Padding.Right;
                if(Width < 220)
                    Width = 220;
                this.ResizeHeightTo(newSize.Y, 300, Easing.OutQuint);
            }
            else
            {
                Height = newSize.Y;
                this.ResizeWidthTo(newSize.X, 300, Easing.OutQuint);
            }
        }

        protected override void AnimateOpen()
        {
            this.FadeIn(300, Easing.OutPow10);
        }

        protected override void AnimateClose()
        {
            this.FadeOut();
        }
    }
}
