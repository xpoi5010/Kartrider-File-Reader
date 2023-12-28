using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSToolboxGroup: Container, IStateful<KCSToolBoxGroupState>
    {
        private readonly KCSToolboxGroupHeader groupHeader;
        private readonly FillFlowContainer contentContainer;
        private readonly Container childContainer;
        private readonly Box background;
        private KCSToolBoxGroupState state;

        public event Action<KCSToolBoxGroupState> StateChanged;

        public LocalisableString GroupText
        {
            get => groupHeader.Text;
            set => groupHeader.Text = value;
        }

        public KCSToolBoxGroupState State
        {
            get => state;
            set
            {
                state = value;
                groupHeader.State = state;
                Scheduler.AddOnce(updateState, true);
                StateChanged?.Invoke(state);
            }
        }

        protected override Container<Drawable> Content => childContainer;

        public KCSToolboxGroup()
        {
            InternalChildren = new Drawable[]
            {
                background = new Box()
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.FromHex("3A3A3A")
                },
                contentContainer = new FillFlowContainer()
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new Drawable[]
                    {
                        groupHeader = new KCSToolboxGroupHeader()
                        {
                            RelativeSizeAxes = Axes.X,
                            Clicked = onHeaderClicked,
                            Height = 30,
                        },
                        childContainer = new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            Masking = true,
                            Padding = new MarginPadding() { Top = 0, Bottom = 7, Horizontal = 13}
                        }
                    }
                }
            };

            Scheduler.AddOnce(updateState, false);
        }

        private void updateState(bool animated)
        {
            childContainer.ClearTransforms();

            if (state == KCSToolBoxGroupState.NotExpanded)
            {
                childContainer.AutoSizeAxes = Axes.None;
                childContainer.ResizeHeightTo(0, animated ? 320: 0, Easing.OutQuint);
                childContainer.Delay(100).FadeOut(animated ? 220 : 0, Easing.OutQuint);
            }
            else
            {
                childContainer.AutoSizeAxes = Axes.Y;
                childContainer.AutoSizeDuration = 320;
                childContainer.AutoSizeEasing = Easing.OutQuint;
                childContainer.FadeIn(animated ? 220 : 0, Easing.OutQuint);
            }
        }

        private void onHeaderClicked()
        {
            if (State == KCSToolBoxGroupState.Expanded)
                State = KCSToolBoxGroupState.NotExpanded;
            else if (State == KCSToolBoxGroupState.NotExpanded)
                State = KCSToolBoxGroupState.Expanded;
        }
    }

    internal partial class KCSToolboxGroupHeader : CompositeDrawable, IStateful<KCSToolBoxGroupState>, IHasText
    {
        private readonly SpriteIcon expandStateIcon;
        private readonly SpriteText headerText;
        private KCSToolBoxGroupState state;

        internal Action? Clicked;

        public event Action<KCSToolBoxGroupState> StateChanged;

        public LocalisableString Text
        {
            get => headerText.Text;
            set => headerText.Text = value;
        }

        public KCSToolBoxGroupState State
        {
            get => state;
            set
            {
                state = value;
                Scheduler.AddOnce(updateState);
                StateChanged?.Invoke(state);
            }
        }

        public KCSToolboxGroupHeader()
        {
            Padding = new MarginPadding() { Horizontal = 10, Vertical = 7 };
            InternalChildren = new Drawable[]
            {
                expandStateIcon = new SpriteIcon()
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.None,
                    Width = 10,
                    Height = 10,
                    X = 7.5f,
                    Icon = FontAwesome.Solid.ChevronRight,
                    Colour = Colour4.White
                },
                headerText = new SpriteText()
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.None,
                    RelativePositionAxes = Axes.None,
                    X = 20,
                    Y = 0,
                    Font = KCSFont.Default.With(size: 17f),
                    Colour = Colour4.White
                }
            };
        }

        protected override bool OnClick(ClickEvent e)
        {
            Clicked?.Invoke();
            return true;
        }

        private void updateState()
        {
            if (state == KCSToolBoxGroupState.Expanded)
                expandStateIcon.RotateTo(90, 320, Easing.OutQuint);
            else
                expandStateIcon.RotateTo(0, 320, Easing.OutQuint);
        }
    }

    public enum KCSToolBoxGroupState
    {
        NotExpanded,
        Expanded,
    }
}
