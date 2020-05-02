using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MicroPlayer
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        Dictionary<string, string> themes = new Dictionary<string, string>()
        {
            {"Light",  "resources/themes/Light.xaml"},
            {"Night",  "resources/themes/Night.xaml"},
            {"Dark Accent",  "resources/themes/DarkAccent.xaml"},
            {"Seaboard",  "resources/themes/SunsetBeach.xaml"},
            {"Nior",  "resources/themes/Nior.xaml"},
            {"Embers",  "resources/themes/Embers.xaml"}
        };

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ThemeChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string themeName = (string)(comboBox.SelectedItem as ComboBoxItem).Content;

            if (themeName == null) return;

            if (themes.ContainsKey(themeName))
            {
                string themePath = themes[themeName];
                Application.Current.Resources.MergedDictionaries[0].Source = new Uri(themePath, UriKind.RelativeOrAbsolute);
            }
        }
    }
}
