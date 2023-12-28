using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSMenuBackgroundMask : Box
    {
        public KCSMenuBackgroundMask()
        {
            Colour = new Colour4(0x00, 0x00, 0x00, 0x00);
        }

        public void Active()
        {
            this.FadeIn(0);
        }

        public void Deactive()
        {
            this.FadeOut(0);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            MenuBackgroundFocus?.Invoke(this);
            return base.OnMouseDown(e);
        }
        internal event MenuBackgroundFocusDelegate MenuBackgroundFocus;
    }

    internal delegate void MenuBackgroundFocusDelegate(KCSMenuBackgroundMask sender);
}
