using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartCityStudio.Game.Utilities
{
    public static class UnitUtility
    {
        private static readonly string[] dataSizeSIUnits = new[] { "B ","KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"}; 

        private static readonly string[] dataSizeIECUnits = new[] { "B  ", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"}; 

        public static string FormatDataSize(long dataSize)
        {
            double preciseDataSize = dataSize;
            if(Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                // Use KiB, MiB and so on.
                foreach(string unit in dataSizeIECUnits)
                {
                    if (preciseDataSize < 1024d)
                    {
                        return $"{Math.Round(preciseDataSize, 2)} {unit}";
                    }
                    else
                    {
                        preciseDataSize /= 1024d;
                    }
                }
                return $"{Math.Round(preciseDataSize * 1024d, 2)} {dataSizeIECUnits[^1]}";
            }
            else
            {
                // Use KB, MB and so on.
                foreach (string unit in dataSizeSIUnits)
                {
                    if (preciseDataSize < 1000d)
                    {
                        return $"{Math.Round(preciseDataSize, 2)} {unit}";
                    }
                    else
                    {
                        preciseDataSize /= 1000d;
                    }
                }
                return $"{Math.Round(preciseDataSize * 1000d, 2)} {dataSizeSIUnits[^1]}";
            }
        }
    }
}
