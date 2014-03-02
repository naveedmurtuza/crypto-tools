using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace CryptoTools.Helpers
{
    public interface IContentSwitcher
    {

        void SwitchContent(UserControl control, params object[] args);
    }

    public static class Switcher
    {
        public static IContentSwitcher pageSwitcher;

        public static void Switch(UserControl newPage)
        {
            pageSwitcher.SwitchContent(newPage);
        }

        public static void Switch(UserControl newPage, object state)
        {
            pageSwitcher.SwitchContent(newPage, state);
        }
    }
}
