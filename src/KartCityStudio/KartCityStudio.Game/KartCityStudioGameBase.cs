using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using KartCityStudio.Resources;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osuTK;

namespace KartCityStudio.Game
{
    public partial class KartCityStudioGameBase : osu.Framework.Game
    {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        protected override Container<Drawable> Content { get; }

        protected KartCityStudioGameBase()
        {
             // Ensure game and tests scale with window size and screen DPI.
            base.Content.Add(Content = new Container<Drawable>
            {
                // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
                RelativeSizeAxes = Axes.Both,
            });
        }

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager frameworkConfig)
        {
            frameworkConfig.GetBindable<ExecutionMode>(FrameworkSetting.ExecutionMode).Value = ExecutionMode.MultiThreaded;
            Resources.AddStore(new DllResourceStore(typeof(KartCityStudioResources).Assembly));
            // Add Fonts
            AddFont(Resources, @"Fonts/Noto_Sans_CJK_TC/_100/NotoSansTC-Bold");
            AddFont(Resources, @"Fonts/Noto_Sans_CJK_TC/_100/NotoSansTC-Regular");
            AddFont(Resources, @"Fonts/Noto_Sans_CJK_TC/_100/NotoSansTC-Medium");
            AddFont(Resources, @"Fonts/RedHat_Display_Medium/_100/RedHatDisplay");
            AddFont(Resources, @"Fonts/RedHat_Display_Medium/_100/RedHatDisplay-Black");
            AddFont(Resources, @"Fonts/RedHat_Display_Medium/_100/RedHatDisplay-ExtraBold");
            AddFont(Resources, @"Fonts/RedHat_Display_Medium/_100/RedHatDisplay-Light");
            AddFont(Resources, @"Fonts/RedHat_Display_Medium/_100/RedHatDisplay-Medium");
            AddFont(Resources, @"Fonts/RedHat_Display_Medium/_100/RedHatDisplay-SemiBold");
        }
    }
}
