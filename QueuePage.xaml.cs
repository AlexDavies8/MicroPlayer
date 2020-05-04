using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            Container.Children.Clear();

            if (app.trackManager.currentTrack != null)
            {
                TrackButton currBtn = new TrackButton() { Margin = new Thickness(0, 0, 0, 15), Background = (Brush)FindResource("AccentBrush"), ForegroundButton = true };
                currBtn.Text = app.trackManager.currentTrack.title;
                currBtn.PlayClicked += (_a, _b) =>
                {
                    app.trackManager.audioManager.Restart();
                    app.trackManager.Play();
                };
                currBtn.RemoveClicked += (_a, _b) =>
                {
                    app.trackManager.currentTrack = null;
                    app.trackManager.NextTrack();
                    UpdateDisplay();
                };
                Container.Children.Add(currBtn);
            }

            if (app.trackManager.queue.Count == 0)
            {
                Container.Children.Add(new Label() { FontSize = 30, Foreground = (Brush)FindResource("FontBrush"), Content = "Queue Empty", HorizontalAlignment = HorizontalAlignment.Center });
            }

            int i = 1;
            foreach(var track in app.trackManager.queue)
            {
                int _i = i;

                Thickness margin = new Thickness(0, 5, 0, 5);
                if (i == app.trackManager.queue.Count) margin = new Thickness(0, 5, 0, 0);

                TrackButton btn = new TrackButton() { Margin = margin };
                btn.PlayClicked += (_a, _b) =>
                {
                    app.trackManager.JumpQueue(_i);
                    UpdateDisplay();
                };
                btn.RemoveClicked += (_a, _b) =>
                {
                    app.trackManager.RemoveFromQueue(_i);
                    UpdateDisplay();
                };
                btn.Text = track.title;
                Container.Children.Add(btn);
                i++;
            }
        }

        void SetHistoryDisplay()
        {
            Container.Children.Clear();

            if (app.trackManager.currentTrack != null)
            {
                TrackButton currBtn = new TrackButton() { Margin = new Thickness(0, 0, 0, 15), Background = (Brush)FindResource("AccentBrush"), ForegroundButton = true };
                currBtn.Text = app.trackManager.currentTrack.title;
                currBtn.PlayClicked += (_a, _b) =>
                {
                    app.trackManager.audioManager.Restart();
                    app.trackManager.Play();
                };
                currBtn.RemoveClicked += (_a, _b) =>
                {
                    app.trackManager.currentTrack = null;
                    app.trackManager.NextTrack();
                    UpdateDisplay();
                };
                Container.Children.Add(currBtn);
            }

            if (app.trackManager.history.Count == 0)
            {
                Container.Children.Add(new Label() { FontSize = 30, Foreground = (Brush)FindResource("FontBrush"), Content = "History Empty", HorizontalAlignment = HorizontalAlignment.Center });
            }

            int i = 1;
            foreach (var track in app.trackManager.history)
            {
                int _i = i;

                Thickness margin = new Thickness(0, 5, 0, 5);
                if (i == app.trackManager.queue.Count) margin = new Thickness(0, 5, 0, 0);

                TrackButton btn = new TrackButton() { Margin = margin };
                btn.PlayClicked += (_a, _b) =>
                {
                    app.trackManager.JumpHistory(_i);
                    UpdateDisplay();
                };
                btn.RemoveClicked += (_a, _b) =>
                {
                    app.trackManager.RemoveFromHistory(_i);
                    UpdateDisplay();
                };
                btn.Text = track.title;
                Container.Children.Add(btn);
                i++;
            }
        }
        private void LoadTrack(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Multiselect = true,
                Filter =
                    "Audio Files (*.wav;*.mp3)|*.wav;*.mp3|" +
                    "All files (*.*)|*.*",
                Title = "Open Tracks"
            };

            var result = openDialog.ShowDialog();
            if (result != null)
            {
                foreach (string path in openDialog.FileNames)
                {
                    app.trackManager.LoadTrack(path);
                }
            }

            UpdateDisplay();
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
