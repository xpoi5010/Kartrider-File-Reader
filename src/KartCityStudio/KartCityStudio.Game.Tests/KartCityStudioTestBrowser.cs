using KartCityStudio.Game.IO.Stores;
using KartLibrary.File;
using osu.Framework.Audio.Track;
using osu.Framework.Audio;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Audio;
using KartCityStudio.Resources;
using KartLibrary.Consts;
using osu.Framework.Logging;
using System;

namespace KartCityStudio.Game.Tests
{
    public partial class KartCityStudioTestBrowser : KartCityStudioGameBase
    {
        [Cached]
        private KartStorageSystem storageSystem;

        private KartStorageResourceStore kartResourcesStore;

        public KartCityStudioTestBrowser()
        {
            KartStorageSystemBuilder kartStorageSystemBuilder = new KartStorageSystemBuilder();
            storageSystem =
                kartStorageSystemBuilder
                    .UseRho()
                    .UseRho5()
                    //.UsePackFolderListFile()
                    .SetDataPath(@"H:/game/KartRider/Data")
                    .SetClientRegion(CountryCode.KR)
                    .Build();

        }

        [BackgroundDependencyLoader]
        private async Task load()
        {
            Logger.Log("Initializing KartStorageSystem.");
            DateTime beginTime = DateTime.Now;
            await storageSystem.Initialize();
            TimeSpan duration = DateTime.Now - beginTime;
            Logger.Log($"Finsh Initialize KartStorageSystem. Spends {duration.TotalMilliseconds} ms.");

        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddRange(new Drawable[]
            {   new TestBrowser("KartCityStudio"),
                new CursorContainer()
            });
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }

        protected override void Dispose(bool isDisposing)
        {
            storageSystem.Dispose();
            base.Dispose(isDisposing);
        }
    }
}
