using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class MainWindow : Window
    {
        PlaybackPage playbackPage;
        PlaylistPage playlistPage;
        SettingsPage settingsPage;
        QueuePage queuePage;

        public MainWindow()
        {
            InitializeComponent();

            playbackPage = new PlaybackPage();
            playlistPage = new PlaylistPage();
            settingsPage = new SettingsPage();
            queuePage = new QueuePage();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void NavigateToPlaybackPage(object sender, RoutedEventArgs e)
        {
            pageFrame.Navigate(playbackPage);
        }

        private void NavigateToQueuePage(object sender, RoutedEventArgs e)
        {
            pageFrame.Navigate(queuePage);
        }

        private void NavigateToPlaylistPage(object sender, RoutedEventArgs e)
        {
            pageFrame.Navigate(playlistPage);
        }

        private void NavigateToSettingsPage(object sender, RoutedEventArgs e)
        {
            pageFrame.Navigate(settingsPage);
        }

        private void QuitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
