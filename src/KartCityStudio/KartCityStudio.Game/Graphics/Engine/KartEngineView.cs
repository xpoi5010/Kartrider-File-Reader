using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Graphics.Veldrid;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Shaders;
using osuTK;
using KartCityStudio.Game.Graphics.Engine.Render;
using System.IO;

namespace KartCityStudio.Game.Graphics.Engine
{

    public partial class KartEngineView : Drawable
    {
        KartEngineRender render;
        [BackgroundDependencyLoader]
        private async void load(GameHost gamehost, TextureStore textureStore)
        {
            render = new KartEngineRender();
            var screenshot = await gamehost.TakeScreenshotAsync();
            using(MemoryStream ms = new MemoryStream())
            {
                screenshot.Save(ms, new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder());
            }
        }

    }
}
