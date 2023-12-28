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

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSMenu : Menu
    {
        public KCSMenu(): base(Direction.Horizontal, true)
        {
            ItemsContainer.Padding = new MarginPadding()
            {
                Top = 3,
                Bottom = 3,
                Left = 8
            };
            BackgroundColour = new Color4(0x10, 0x10, 0x10, 0xFF);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            
        }

        protected override Menu CreateSubMenu() => new KCSSubMenu();

        protected override DrawableMenuItem CreateDrawableMenuItem(MenuItem item)
        {
            return new DrawableKCSMenuItem(item);
        }

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction)
        {
            return new BasicScrollContainer<Drawable>(scrollDirection: Direction.Horizontal)
            {
                ClampExtension = 0,
                ScrollbarVisible = false,
            };
        }

        protected override void UpdateSize(Vector2 newSize)
        {
            if (Direction == Direction.Vertical)
            {
                Width = newSize.X;
                this.ResizeHeightTo(newSize.Y, 300, Easing.OutQuint);
            }
            else
            {
                Height = newSize.Y;
                this.ResizeWidthTo(newSize.X, 300, Easing.OutQuint);
            }
        }
    }
}
