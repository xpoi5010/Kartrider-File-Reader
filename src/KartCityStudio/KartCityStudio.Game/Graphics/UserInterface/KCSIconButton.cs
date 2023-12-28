using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSIconButton: KCSButton
    {
        private readonly SpriteIcon icon;

        public Vector2 IconRelativeSize
        {
            get => icon.Size;
            set => icon.Size = value;
        }

        public IconUsage Icon
        {
            get => icon.Icon;
            set => icon.Icon = value;
        }

        public KCSIconButton()
        {
            Add(icon = new SpriteIcon()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(0.6f, 0.6f),
            });
        }
    }
}
