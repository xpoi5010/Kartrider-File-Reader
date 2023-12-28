using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Layout;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace KartCityStudio.Game.Graphics.Containers
{
    public abstract partial class SplittableContainer: CompositeDrawable
    {
        private const float min_container_size = 15;
        private readonly SplitterBarContainer splitterBar;
        private float splitterBarRelativePos = 0.5f;
        public Container FirstContainer { get; }
        public Container SecondContainer { get; }
        public Direction SplitDirection { get; }

        public float FirstContainerSize { get; set; }

        protected SplittableContainer(Direction splitDirection)
        {
            SplitDirection = splitDirection;

            Axes splitAxes = SplitDirection == Direction.Horizontal ? Axes.X : Axes.Y;
            splitterBar = CreateSplitterBar(splitDirection);
            AddRangeInternal(new Drawable[]
            {
                FirstContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both & ~splitAxes,
                    Masking = true,
                },
                splitterBar,
                SecondContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both & ~splitAxes,
                    Masking = true,
                }
            });

            splitterBar.Dragged = onSplitterBarMovement;

        }
        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

        }

        protected abstract SplitterBarContainer CreateSplitterBar(Direction direction);

        protected partial class SplitterBarContainer: Container
        {
            private float dragOffset;

            internal Action<float> Dragged;
            internal Action<float> WidthChanged;
            protected readonly Direction Direction;

            public SplitterBarContainer(Direction direction)
            {
                Direction = direction;
            }

            protected override bool OnClick(ClickEvent e) => true;

            protected override bool OnMouseDown(MouseDownEvent e)
            {
                if(e.Button != osuTK.Input.MouseButton.Left) return false;

                dragOffset = Position[(int)Direction];
                Dragged?.Invoke(dragOffset);

                return true;
            }

            protected override bool OnDragStart(DragStartEvent e)
            {
                if (e.Button != osuTK.Input.MouseButton.Left) return false;

                dragOffset = e.MousePosition[(int)Direction] - Position[(int)Direction];

                return true;
            }

            protected override void OnDrag(DragEvent e)
            {
                Dragged?.Invoke(e.MousePosition[(int)Direction] - dragOffset);
            }

            protected override bool OnHover(HoverEvent e)
            {
                return base.OnHover(e);
            }
        }

        private void onSplitterBarMovement(float value)
        {
            float availableSize = DrawSize[(int)SplitDirection];
            float splitterSize = splitterBar.DrawSize[(int)SplitDirection];
            float maxSplitterPosition = availableSize - min_container_size - splitterSize;
            value = Math.Clamp(value, min_container_size, maxSplitterPosition);
            splitterBarRelativePos = value / (availableSize - splitterSize);
            Scheduler.AddOnce(updateSize);
        }

        private void updateSize()
        {
            float availableSize = DrawSize[(int)SplitDirection];
            float splitterSize = splitterBar.DrawSize[(int)SplitDirection];
            float value = splitterBarRelativePos * (availableSize - splitterSize);
            splitterBar.Position = SplitDirection == Direction.Horizontal ? new osuTK.Vector2(value, 0) : new osuTK.Vector2(0, value);
            FirstContainer.Size = SplitDirection == Direction.Horizontal ? new osuTK.Vector2(value, 1) : new osuTK.Vector2(1, value);
            SecondContainer.Position = SplitDirection == Direction.Horizontal ? new osuTK.Vector2(value + splitterSize, 0) : new osuTK.Vector2(0, value + splitterSize);
            SecondContainer.Size = SplitDirection == Direction.Horizontal ? new osuTK.Vector2(availableSize - value - splitterSize, 1) : new osuTK.Vector2(1, availableSize - value - splitterSize);
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            if((invalidation & Invalidation.DrawSize) != Invalidation.None)
                Scheduler.AddOnce(updateSize);
            return base.OnInvalidate(invalidation, source);
        }
    }
}
