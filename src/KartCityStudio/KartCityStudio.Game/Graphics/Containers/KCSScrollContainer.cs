using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartCityStudio.Game.Graphics.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
namespace KartCityStudio.Game.Graphics.Containers
{
    public partial class KCSScrollContainer<T> : ScrollContainer<T> where T : Drawable
    {
        private readonly KCSScrollBar scrollBar;
        private readonly Container scrollBase;
        private readonly FlowContainer<Drawable> scrollContent;


        public Direction ScrollDirection => Direction.Vertical;

        public KCSScrollContainer()
        {
            ClampExtension = 10;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            
        }

        protected override ScrollbarContainer CreateScrollbar(Direction direction)
        {
            return new KCSScrollBar();
        }

        protected partial class KCSScrollBar : ScrollbarContainer
        {
            private readonly Box scroller;
            private float scrollerWidth = 9f;
            private float scrollerDelta = 0.07f;
            private float scrollerPos = 0f;
            private float scrollerHideAlpha = 0.2f;
            private bool isHover = false;
            private bool isDragging = false;
            public KCSScrollBar(): base(Direction.Vertical)
            {
                Anchor = Anchor.TopRight;
                Origin = Anchor.TopRight;
                Size = new osuTK.Vector2(scrollerWidth, 1f);
                Blending = BlendingParameters.Additive;
                Children = new Drawable[]
                {
                    scroller = new Box()
                    {
                        Alpha = scrollerHideAlpha,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Size = new Vector2(1f, 1f),
                        Position = new Vector2(0f, 0f),
                        Colour = Colour4.White,
                    },
                };
                Masking = true;
                CornerRadius = 5f;
            }

            protected override bool OnHover(HoverEvent e)
            {
                scroller
                    .FadeTo(1, duration: 239, easing: Easing.InOutQuint);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                scroller
                    .FadeTo(scrollerHideAlpha, duration: 500, easing: Easing.OutExpo);
                base.OnHoverLost(e);
            }

            protected override bool OnScroll(ScrollEvent e)
            {
                scroller
                    .FadeTo(1, duration: 239, easing: Easing.InOutQuint);
                return base.OnScroll(e);
            }

            public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
            {
                this.ResizeTo(new Vector2(scrollerWidth)
                {
                    [(int)ScrollDirection] = val
                }, 320, Easing.OutExpo);
            }
        }
    }
}
