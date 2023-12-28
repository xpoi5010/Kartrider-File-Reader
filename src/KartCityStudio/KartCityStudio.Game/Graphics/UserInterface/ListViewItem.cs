using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Packaging;
using osu.Framework.Bindables;
using osu.Framework.Localisation;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public class ListViewItem
    {
        public readonly BindableDictionary<string, LocalisableString> Texts = new BindableDictionary<string, LocalisableString>();

        internal readonly Bindable<Action<ListViewItem>?> ClickAction = new Bindable<Action<ListViewItem>?>();

        internal readonly Bindable<Action<ListViewItem>?> DoubleClickAction = new Bindable<Action<ListViewItem>?>();

        public ListViewItem((string key, LocalisableString value)[] texts)
        {
            Texts.AddRange(texts.Select(x => new KeyValuePair<string, LocalisableString>(x.key, x.value)));
        }

        public ListViewItem(KeyValuePair<string, LocalisableString>[] texts)
        {
            Texts.AddRange(texts);
        }
    }
}
