using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSSliderBar<T> : SliderBar<T> where T : struct, IComparable<T>, IConvertible, IEquatable<T>
    {
        private readonly Container contentContainer;
        private readonly Box foregroundBox;
        private readonly Box backgroundBox;

        private bool isHover = false;
        private bool isDrag = false;
        private bool isMouseDown = false;
        private bool isLocked = false; // Lock for adject the value of this slider.

        public Action<T> OnUserChangeValue;

        public Colour4 ForegroundColour
        {
            get => foregroundBox.Colour;
            set => foregroundBox.Colour = value;
        }

        public Colour4 BackgroundColour
        {
            get => backgroundBox.Colour;
            set => backgroundBox.Colour = value;
        }

        public T MinValue
        {
            get => CurrentNumber.MinValue;
            set => CurrentNumber.MinValue = value;
        }

        public T MaxValue
        {
            get => CurrentNumber.MaxValue;
            set => CurrentNumber.MaxValue = value;
        }

        public T Value
        {
            get => CurrentNumber.Value;
            set
            {
                if (!isLocked)
                    CurrentNumber.Value = value;
            }
        }

        private Anchor childAnchor => Origin & ((Anchor)0b111) | Anchor.x0;

        public KCSSliderBar()
        {
            Children = new Drawable[]
            {
                contentContainer = new Container()
                {
                    Anchor = childAnchor,
                    Origin = childAnchor,
                    RelativeSizeAxes = Axes.Both,
                    Height = 1f,
                    Masking = true,
                    CornerRadius = DrawSize.Y / 2f,
                    Children = new Drawable[]
                    {
                        backgroundBox = new Box()
                        {
                            Anchor = childAnchor,
                            Origin = childAnchor,
                            RelativeSizeAxes = Axes.Both,
                            Height = 1f,
                            Colour = Colour4.FromHex("6DB9EF")
                        },
                        foregroundBox = new Box()
                        {
                            Anchor = childAnchor,
                            Origin = childAnchor,
                            RelativeSizeAxes = Axes.Both,
                            Height = 1f,
                            Colour = Colour4.FromHex("3081D0")
                        },
                    }
                },
            };
        }

        protected override void UpdateValue(float value)
        {
            foregroundBox.ResizeWidthTo(value, 270, Easing.OutQuint);
        }

        protected override void Update()
        {
            base.Update();
            foregroundBox.Anchor = childAnchor;
            backgroundBox.Anchor = childAnchor;
            foregroundBox.Origin = childAnchor;
            backgroundBox.Origin = childAnchor;
            contentContainer.Anchor = childAnchor;
            contentContainer.Origin = childAnchor;
            contentContainer.CornerRadius = contentContainer.DrawSize.Y / 2f;
        }

        protected override bool OnHover(HoverEvent e)
        {
            isHover = true;
            contentContainer.ResizeHeightTo(1.5f, 270, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            isHover = false;
            if(!isMouseDown)
                contentContainer.ResizeHeightTo(1f, 270, Easing.OutQuint);
            base.OnHoverLost(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            isMouseDown = true;
            isLocked = true;
            if (!isHover)
                contentContainer.ResizeHeightTo(1.5f, 270, Easing.OutQuint);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            isMouseDown = false;
            isLocked = false;
            if (!isHover)
                contentContainer.ResizeHeightTo(1f, 270, Easing.OutQuint);
            base.OnMouseUp(e);
        }

        protected override void OnUserChange(T value)
        {
            OnUserChangeValue?.Invoke(Value);
        }


    }
}
