using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedBass.Fx;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSButton : Button
    {
        private readonly Box hoverBox;
        private readonly Box backgroundBox;
        private readonly Container internalContainer;
        private readonly Container contentContainer;

        private bool isHover = false;
        private bool isMouseDown = false;

        protected override Container<Drawable> Content => contentContainer;

        public new bool Masking
        {
            get => internalContainer.Masking;
            set => internalContainer.Masking = value;
        }

        public new float CornerRadius
        {
            get => internalContainer.CornerRadius;
            set => internalContainer.CornerRadius = value;
        }

        public new float CornerExponent
        {
            get => internalContainer.CornerExponent;
            set => internalContainer.CornerExponent = value;
        }

        public bool ScaleWhenButtonDown { get; set; } = true;

        public KCSButton()
        {
            InternalChild = internalContainer = new Container()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1, 1),
                Masking = true,
                CornerRadius = 8f,
                Children = new Drawable[]
                {
                    backgroundBox = new Box()
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.FromHex("427D9D"),
                    },
                    hoverBox = new Box()
                    {
                        Alpha = 0,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.FromHex("164863"),
                    },
                    contentContainer = new Container()
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                }
            };
        }

        public Colour4 BackgroundColour
        {
            get => backgroundBox.Colour;
            set => backgroundBox.Colour = value;
        }

        public Colour4 HoverColour
        {
            get => hoverBox.Colour;
            set => hoverBox.Colour = value;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            isMouseDown = true;
            if (ScaleWhenButtonDown)
                internalContainer.ScaleTo(0.85f, 400, Easing.OutQuint);
            else
                this.FadeTo(0.55f, 200, Easing.OutQuint);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            isMouseDown = false;
            if (ScaleWhenButtonDown)
                internalContainer.ScaleTo(1f, 500, Easing.OutQuint);
            else
                this.FadeTo(1f, 200, Easing.OutQuint);

            if (!isHover)
                hoverBox.FadeOut(500, Easing.OutQuint);
            base.OnMouseUp(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            isHover = true;
            hoverBox.FadeIn(500, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            isHover = false;
            if(!isMouseDown)
                hoverBox.FadeOut(500, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}
