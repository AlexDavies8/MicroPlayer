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
    public partial class TrackButton : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(TrackButton), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ForegroundButtonProperty = DependencyProperty.Register(
            "ForegroundButton", typeof(bool), typeof(TrackButton), new PropertyMetadata(false));
        public bool ForegroundButton
        {
            get { return (bool)GetValue(ForegroundButtonProperty); }
            set { SetValue(ForegroundButtonProperty, value); }
        }

        public event EventHandler PlayClicked;
        public event EventHandler RemoveClicked;

        public TrackButton()
        {
            InitializeComponent();

            this.DataContext = this;

            HideButtons(null, null);
        }

        private void ShowButtons(object sender, MouseEventArgs e)
        {
            Buttons.Visibility = Visibility.Visible;

            if (ForegroundButton)
            {
                foreach (var b in Buttons.Children)
                {
                    ((Button)b).Style = (Style)FindResource("ForegroundButton");
                }
            }
        }

        private void HideButtons(object sender, MouseEventArgs e)
        {
            Buttons.Visibility = Visibility.Hidden;
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            PlayClicked(sender, e);
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            RemoveClicked(sender, e);
        }
    }
}
