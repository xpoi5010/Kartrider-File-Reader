using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veldrid;

namespace KartCityStudio.Game.Graphics.Engine.Render
{
    public class KartEngineRender
    {
        GraphicsDevice renderDevice;

        Texture framebuffer;

        public KartEngineRender()
        {
            renderDevice = GraphicsDevice.CreateD3D11(new GraphicsDeviceOptions()
            {
                
            });
            initizationRenderDevice();
        }

        private void initizationRenderDevice()
        {
            
        }
    }
}
