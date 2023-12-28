using KartCityStudio.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace KartCityStudio.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"KartCityStudio"))
            using (osu.Framework.Game game = new KartCityStudioGame())
            {
                host.Run(game);
            }
        }
    }
}
