using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Layout;
using osuTK;

namespace KartCityStudio.Game.Graphics.Containers
{
    public partial class FlexibleFillFlowContainer<T> : FlowContainer<T> where T: Drawable
    {
        public FlexibleFillFlowContainer()
        {
            
        }

        protected override IEnumerable<Vector2> ComputeLayoutPositions()
        {
            throw new NotImplementedException();
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {

            return true;
        }
    }
}
