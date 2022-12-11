using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace RhoLoader
{
    public static class LanguageManager
    {
        private static List<Language> langs = new List<Language>();

        //likes en-us zh-tw

        private static string ln = "";

        private static Language nowLang = new Language();
        public static Language CurrentLanguage => nowLang;
        public static Language[] ListLanguages()
        {
            return langs.ToArray();
        }
        
        public static bool SetLanguage(string DisplayName = "",string LanguageName = "")
        {
            Predicate<Language> compareFunc;
            if (DisplayName != "" && LanguageName != "")
                compareFunc = x => x.DisplayName == DisplayName && x.LanguageName == LanguageName;
            else if (DisplayName.Length + LanguageName.Length != 0)
                compareFunc = x => x.DisplayName == DisplayName || x.LanguageName == LanguageName;
            else
                return false;
            nowLang = langs.Find(compareFunc);
            return nowLang is null;
        }

        public static Language FindLanguage(string DisplayName = "", string LanguageName = "")
        {
            Predicate<Language> compareFunc;
            if (DisplayName != "" && LanguageName != "")
                compareFunc = x => x.DisplayName == DisplayName && x.LanguageName == LanguageName;
            else if (DisplayName.Length + LanguageName.Length != 0)
                compareFunc = x => x.DisplayName == DisplayName || x.LanguageName == LanguageName;
            else
                return null;
            return langs.Find(compareFunc);
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
                return nowLang.LanguageName;
            }
            set
            {
                nowLang = langs.Find(x => x.LanguageName == value);
            }
        }


        
        private static string sb(string name)
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

        public static string GetLangFont()
        {
            string Font_name = nowLang.GetFontName();
            return Font_name;
        }

        public static Font GetLangFontWithBase(Font base_font)
        {
            return nowLang.GetLangFontWithBase(base_font);
        }
    }

    public class Language
    {
        public string LanguageName = "";

        public string DisplayName = "";

        public string[] Font = new string[0];
        
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

        public string GetFontName()
        {
            string outFont = "";
            foreach(string sub_font in Font)
            {
                try
                {
                    FontFamily fm = new FontFamily(sub_font);
                    outFont = sub_font;
                    break;
                }
                catch
                {

                }
            }
            return outFont;
        }

        public Font GetLangFontWithBase(Font base_font)
        {
            string Font_name =this.GetFontName();
            if (Font_name == "")
                return base_font;
            FontFamily fm = new FontFamily(Font_name);
            return new Font(fm, base_font.Size, base_font.Style);
        }
    }

    public class StringBag
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
