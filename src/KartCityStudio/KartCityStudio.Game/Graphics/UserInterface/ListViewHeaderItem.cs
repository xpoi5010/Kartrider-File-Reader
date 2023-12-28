using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Bindables;
using osu.Framework.Localisation;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public class ListViewHeaderItem
    {
        public readonly Bindable<LocalisableString> Text = new Bindable<LocalisableString>();

        public readonly string Name = "";

        public readonly Bindable<float> FieldWidth = new Bindable<float>();
        
        public ListViewHeaderItem(string name, LocalisableString text, float fieldWidth)
        {
            Text.Value = text;
            Name = name;
            FieldWidth.Value = fieldWidth;
        }
    }
}
