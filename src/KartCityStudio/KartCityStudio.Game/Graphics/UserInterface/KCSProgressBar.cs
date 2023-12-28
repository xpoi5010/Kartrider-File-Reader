using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSProgressBar: Container
    {
        // Members
        private readonly Box background;
        private readonly Box progress;

        private float progressValue = 0.0f;
        private float minValue = 0.0f;
        private float maxValue = 100.0f;
        // Properities

        public KCSProgressBar()
        {
            InternalChildren = new Drawable[]
            {

            };
        }

        private void setProgressValue(float value)
        {

        }

        private void seMinimumValue(float value)
        {

        }

        private void seMaximumValue(float value)
        {

        }

    }
}
