using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace RhoLoader
{
    public static class LanguageManager
    {
        private static List<Language> langs = new List<Language>();

        //likes en-us zh-tw

        private static string ln = "";

        private static Language nowLang;

        public static string[] ListLanguages()
        {
            return langs.ConvertAll(x => x.DisplayName).ToArray();
        }
        
        public static bool SetLanguage(string DisplayName = "",string LanguageName = "")
        {
            nowLang = langs.Find(x => x.DisplayName == DisplayName || x.LanguageName == LanguageName);
            return nowLang is null;
        }

        public static void LoadLang()
        {
            DirectoryInfo di = new DirectoryInfo("lang\\");
            if (!di.Exists)
                return;
            FileInfo[] fis = di.GetFiles();
            foreach(FileInfo fi in fis)
            {
                if (fi.Extension != ".json")
                    continue;
                using (FileStream fs = new FileStream(fi.FullName, FileMode.Open))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    Language lang = JsonConvert.DeserializeObject<Language>(Encoding.UTF8.GetString(data));
                    langs.Add(lang);
                }
            }
        }

        public static string LanguageName
        {
            get
            {
                return ln;
            }
            set
            {
                ln = value;
                nowLang = langs.Find(x => x.LanguageName == value);
            }
        }


        
        public static string sb(string name)
        {
            if (nowLang is null)
                return $"!sb({name})";
            if(!nowLang.ContainStringBag(name))
                return $"!sb({name})";
            return nowLang.GetStringBag(name);
        }

        public static string GetStringBag(this string str)
        {
            return sb(str);
        }
    }

    public class Language
    {
        public string LanguageName;

        public string DisplayName;

        public StringBag[] StringBags;

        public bool ContainStringBag(string Name)
        {
            return Array.Exists(StringBags, x => x.Name == Name);
        }

        public string GetStringBag(string Name)
        {
            StringBag sb = Array.Find(StringBags, x => x.Name == Name);
            if (sb is null)
                return $"!sb({Name})";
            return sb.Value;
        }
    }

    public class StringBag
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
