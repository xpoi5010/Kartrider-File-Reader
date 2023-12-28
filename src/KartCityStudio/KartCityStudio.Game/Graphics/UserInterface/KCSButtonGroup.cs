using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSButtonGroup: Container<KCSButton>
    {
        private readonly FillFlowContainer<KCSButton> btnsFlowContainer;
        private readonly FillDirection fillDirection;

        protected override Container<KCSButton> Content => btnsFlowContainer;

        public new bool Masking
        {
            get => btnsFlowContainer.Masking;
            set => btnsFlowContainer.Masking = value;
        }

        public new float CornerRadius
        {
            get => btnsFlowContainer.CornerRadius;
            set => btnsFlowContainer.CornerRadius = value;
        }

        public new float CornerExponent
        {
            get => btnsFlowContainer.CornerExponent;
            set => btnsFlowContainer.CornerExponent = value;
        }

        public KCSButtonGroup(FillDirection fillDirection)
        {
            this.fillDirection = fillDirection;
            AutoSizeAxes = fillDirection == FillDirection.Horizontal ? Axes.X : Axes.Y;
            InternalChild = btnsFlowContainer = new FillFlowContainer<KCSButton>()
            {
                RelativeSizeAxes = fillDirection == FillDirection.Horizontal ? Axes.Y : Axes.X,
                AutoSizeAxes = fillDirection == FillDirection.Horizontal ? Axes.X : Axes.Y,
            };
        }

        public override void Add(KCSButton button)
        {
            btnsFlowContainer.Add(button.With(d =>
            {
                d.Anchor = Anchor.TopLeft;
                d.Origin = Anchor.TopLeft;
                d.RelativeSizeAxes &= fillDirection == FillDirection.Horizontal ? Axes.Y : Axes.X;
                d.ScaleWhenButtonDown = false;
                d.Masking = false;
            }));
        }
    }
}
