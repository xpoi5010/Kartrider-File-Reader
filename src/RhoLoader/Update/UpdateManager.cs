using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Update
{
    public static class UpdateManager
    {
        public static UpdateInfo GetUpdateInfo()
        {
            WebClient wc = new WebClient();
            string updateInfo = wc.DownloadString(AppBaseInfo.UpdateInfoLink);
            dynamic info = JsonConvert.DeserializeObject(updateInfo);
            string DownloadLink = info.DownloadLink;
            string LastVersion = info.LatestVersion;
            wc.Dispose();
            return new UpdateInfo
            {
                DownloadLink = DownloadLink,
                Version = LastVersion
            };
        }

        public static string GetCurrentVersion()
        {
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }

        public static int GetCurrentVersionNumber()
        {
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            return (ver.Major << 16) | (ver.Minor << 8) | (ver.Build);
        }

        public static uint GetUpdateToolIdentifier()
        {
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            int part_1 = ver.Major * 6203;
            int part_2 = ver.Minor * 6763;
            int part_3 = ver.Build * 6221;
            int result = 0x12345678;
            while(part_2 != 0)
            {
                result += (168867) * part_2 * part_3 * part_1 * (30259 << ((part_2 & 1) + 1));
                if ((result & 0b10110100) == 0)
                    result ^= (0x72110385 >> (part_2 & 0b1111)) | (0x39261150 << (part_2 & 0b1111 ^ 0b1111));
                part_2 >>= 1;
                part_1 = (part_1 << 1) | (part_1 >> 31);
                part_3 = (part_3 << 31) | (part_3 >> 1);
            }
            if (result < 0)
                result ^= 0b01010101;
            return (uint)result;
        }

        public static byte[] CalculateProgramHash(bool IsUpdateTool = false)
        {
            string _exec_filename = Application.ExecutablePath;
            using (FileStream file_stream = new FileStream(_exec_filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (file_stream.Length < 4 || file_stream.Length > 0x100000)
                    throw new IOException("exec file length cannot be accepted.");
                byte[] buffer = new byte[file_stream.Length - (IsUpdateTool ?  4 : 0)];
                file_stream.Read(buffer, 0, buffer.Length);
                SHA512 sha512 = SHA512.Create();
                byte[] code_hash = sha512.ComputeHash(buffer);
                //code_hash = sha512.ComputeHash(code_hash);
                return code_hash;
            }
        }

        public static bool HaveNewUpdate()
        {
            var update_info = GetUpdateInfo();
            if (update_info.Version != GetCurrentVersion())
                return true;
            else
                return false;
        }
    }

    public class UpdateInfo
    {
        public string Version { get; set; }
        public string DownloadLink { get; set; }
        public string Checksum { get; set; }
    }
}
