using osu.Framework;
using osu.Framework.Platform;

namespace KartCityStudio.Game.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost("visual-tests", new HostOptions()))
            using (var game = new KartCityStudioTestBrowser())
                host.Run(game);
        }
    }
}
