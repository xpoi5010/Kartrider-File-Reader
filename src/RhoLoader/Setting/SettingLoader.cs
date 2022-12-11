using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RhoLoader.Setting
{
    public class SettingLoader
    {
        public Setting Setting { get; set; } = new Setting();
        public SettingLoader() 
        { 
                
        }

        public void LoadSetting(string FilePath)
        {
            FileStream file_stream = new FileStream(FilePath, FileMode.Open);
            this.Setting = JsonSerializer.Deserialize<Setting>(file_stream);
            file_stream.Close();
        }

        public void SaveSetting(string FilePath)
        {
            FileStream fileStream = new FileStream(FilePath, FileMode.Create);
            JsonSerializer.Serialize<Setting>(fileStream, Setting);
            fileStream.Close();
        }

        public void Reset()
        {
            Setting = new Setting();
        }
    }
}
