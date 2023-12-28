using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Bindables;
using osu.Framework.Localisation;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public class ListBoxItem
    {
        public readonly Bindable<LocalisableString> Text = new Bindable<LocalisableString>(string.Empty);

        public readonly Bindable<Action?> ClickAction = new Bindable<Action?>();

        public readonly Bindable<Action?> DoubleClickAction = new Bindable<Action?>();

        public ListBoxItem(LocalisableString text, Action? clickAction = null, Action? doubleClickAction = null)
        {
            Text.Value = text;
            ClickAction.Value = clickAction;
            DoubleClickAction.Value = doubleClickAction;
        }
    }
}
