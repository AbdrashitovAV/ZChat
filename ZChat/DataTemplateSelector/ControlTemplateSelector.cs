using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WCFChat.DataTemplateSelector
{
    public class ControlTemplateSelector : System.Windows.Controls.DataTemplateSelector
    {
        public DataTemplate SettingsTemplate { get; set; }
        public DataTemplate ChatTemplate { get; set; }

        public DataTemplate Empty { get; set; } = new DataTemplate();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return Empty;

            var a = (bool)item;

            if (a)
                return ChatTemplate;

            return SettingsTemplate;


        }
    }
}
