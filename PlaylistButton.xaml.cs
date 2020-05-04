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
    public partial class PlaylistButton : UserControl
    {
        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(ImageSource),
            typeof(PlaylistButton), new PropertyMetadata(null));

        public ImageSource Source
        {
            get { return GetValue(SourceProperty) as ImageSource; }
            set { SetValue(SourceProperty, value); UpdateSource(); }
        }

        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register("Text", typeof(string),
            typeof(PlaylistButton), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Button Button
        {
            get { return button; }
        }

        public PlaylistButton()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        void UpdateSource()
        {
            if (Source != null) Icon.Visibility = Visibility.Hidden;
            else Icon.Visibility = Visibility.Visible;
        }
    }
}
