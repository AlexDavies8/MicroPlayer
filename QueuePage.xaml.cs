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
    public partial class QueuePage : Page
    {
        App app;

        bool showQueue = true;

        public QueuePage()
        {
            InitializeComponent();

            app = (App)Application.Current;

            Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            UpdateDisplay();
        }

        void UpdateDisplay()
        {
            if (showQueue)
            {
                QueueButton.Background = (Brush)FindResource("AccentBrush");
                HistoryButton.Background = (Brush)FindResource("ForegroundBrush");
                SetQueueDisplay();
            }
            else
            {
                QueueButton.Background = (Brush)FindResource("ForegroundBrush");
                HistoryButton.Background = (Brush)FindResource("AccentBrush");
                SetHistoryDisplay();
            }
        }

        void SetQueueDisplay()
        {
            EmptyLabel.Visibility = (app.trackManager.queue.Count == 0 && app.trackManager.currentTrack == null) ? Visibility.Visible : Visibility.Collapsed;

            Container.Children.Clear();

            if (app.trackManager.currentTrack != null)
            {
                Button currBtn = new Button() { BorderThickness = new Thickness(0), Height = 30, FontSize = 16, Margin = new Thickness(0, 5, 0, 5), Background = (Brush)FindResource("AccentBrush"), Foreground = (Brush)FindResource("FontBrush") };
                currBtn.Click += (_a, _b) =>
                {
                    app.trackManager.audioManager.Restart();
                };
                currBtn.Content = app.trackManager.currentTrack.title;
                Container.Children.Add(currBtn);
            }

            int i = 1;
            foreach(var track in app.trackManager.queue)
            {
                int _i = i;

                Button btn = new Button() { BorderThickness = new Thickness(0), Height = 30, FontSize = 16, Margin = new Thickness(0, 5, 0, 5), Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush") };
                btn.Click += (_a, _b) =>
                {
                    app.trackManager.JumpQueue(_i);
                    UpdateDisplay();
                };
                btn.Content = track.title;
                Container.Children.Add(btn);
                i++;
            }
        }

        void SetHistoryDisplay()
        {
            EmptyLabel.Visibility = (app.trackManager.history.Count == 0 && app.trackManager.currentTrack == null) ? Visibility.Visible : Visibility.Collapsed;

            Container.Children.Clear();

            if (app.trackManager.currentTrack != null)
            {
                Button currBtn = new Button() { BorderThickness = new Thickness(0), Height = 30, FontSize = 16, Margin = new Thickness(0, 5, 0, 5), Background = (Brush)FindResource("AccentBrush"), Foreground = (Brush)FindResource("FontBrush") };
                currBtn.Click += (_a, _b) =>
                {
                    app.trackManager.audioManager.Restart();
                };
                currBtn.Content = app.trackManager.currentTrack.title;
                Container.Children.Add(currBtn);
            }

            int i = 1;
            foreach (var track in app.trackManager.history)
            {
                int _i = i;

                Button btn = new Button() { BorderThickness = new Thickness(0), Height = 30, FontSize = 16, Margin = new Thickness(0, 5, 0, 5), Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush") };
                btn.Click += (_a, _b) =>
                {
                    app.trackManager.JumpHistory(_i);
                    UpdateDisplay();
                };
                btn.Content = track.title;
                Container.Children.Add(btn);
                i++;
            }
        }

        private void PlayPrevious(object sender, RoutedEventArgs e)
        {
            app.trackManager.PreviousTrack();
        }

        private void PlayCurrent(object sender, RoutedEventArgs e)
        {
            app.trackManager.audioManager.Restart();
        }

        private void PlayNext(object sender, RoutedEventArgs e)
        {
            app.trackManager.NextTrack();
        }

        private void ShowQueue(object sender, RoutedEventArgs e)
        {
            showQueue = true;
            UpdateDisplay();
        }
        private void ShowHistory(object sender, RoutedEventArgs e)
        {
            showQueue = false;
            UpdateDisplay();
        }
    }
}
