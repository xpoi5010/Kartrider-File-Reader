using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

namespace KartCityStudio.Game.Graphics.Containers
{
    public partial class KCSSplittableContainer: SplittableContainer
    {
        public KCSSplittableContainer(Direction direction): base(direction)
        {

        }

        protected override SplitterBarContainer CreateSplitterBar(Direction direction) => new KCSSplitterBarContainer(direction);

        protected partial class KCSSplitterBarContainer: SplitterBarContainer
        {
            private const float splitterWidth = 6;

            private const float hovingAlpha = 0.3f;
            private const float clickedAlpha = 0.75f;
            private const float inactiveAlpha = 0;

            private readonly Box background;

            private bool isHover = false;
            private bool isMouseDown = false;
            private bool isDragging = false;

            

            public KCSSplitterBarContainer(Direction splitDirection): base(splitDirection)
            {
                RelativeSizeAxes = splitDirection == Direction.Horizontal ? Axes.Y : Axes.X;
                if (splitDirection == Direction.Horizontal)
                    Size = new osuTK.Vector2(splitterWidth, 1);
                else
                    Size = new osuTK.Vector2(1, splitterWidth);
                Blending = BlendingParameters.Additive;
                Children = new Drawable[]
                {
                    background = new Box()
                    {
                        Alpha = 0f,
                        Anchor = Anchor.TopLeft,
                        Origin = Anchor.TopLeft,
                        RelativeSizeAxes = Axes.Both,
                        Size = new osuTK.Vector2(1f, 1f),
                        Colour = Colour4.FromHex("164863"),
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load()
            {

            }

            protected override bool OnHover(HoverEvent e)
            {
                isHover = true;
                if(!isMouseDown && !isDragging)
                    background.FadeTo(0.3f, 245, Easing.OutQuint);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                isHover = false;
                if (!isMouseDown && !isDragging)
                    background.FadeTo(inactiveAlpha, 245, Easing.OutQuint);
                base.OnHoverLost(e);
            }

            protected override bool OnMouseDown(MouseDownEvent e)
            {
                if(e.Button != osuTK.Input.MouseButton.Left) return base.OnMouseDown(e);
                isMouseDown = true;
                if(!isDragging)
                    background.FadeTo(clickedAlpha, 245, Easing.OutQuint);
                return base.OnMouseDown(e);
            }

            protected override void OnMouseUp(MouseUpEvent e)
            {
                if (e.Button != osuTK.Input.MouseButton.Left)
                    return;
                isMouseDown = false;
                if(!isDragging)
                    if (isHover)
                        background.FadeTo(hovingAlpha, 245, Easing.OutQuint);
                    else
                        background.FadeTo(inactiveAlpha, 245, Easing.OutQuint);
                base.OnMouseUp(e);
            }

            protected override bool OnDragStart(DragStartEvent e)
            {
                isDragging = true;
                if(!isMouseDown)
                    background.FadeTo(clickedAlpha, 245, Easing.OutQuint);
                return base.OnDragStart(e);
            }

            protected override void OnDragEnd(DragEndEvent e)
            {
                isDragging = false;
                if (!isMouseDown)
                    if(isHover)
                        background.FadeTo(hovingAlpha, 245, Easing.OutQuint);
                    else
                        background.FadeTo(inactiveAlpha, 245, Easing.OutQuint);
                base.OnDragEnd(e);
            }

        }
    }
}
