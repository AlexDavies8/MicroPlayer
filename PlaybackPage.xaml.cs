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
using System.Windows.Threading;

namespace MicroPlayer
{
    public partial class PlaybackPage : Page
    {
        App app;

        public PlaybackPage()
        {
            InitializeComponent();

            app = Application.Current as App;

            app.trackManager.NextTrackCallback += () =>
            {
                UpdatePlayButton();
                UpdateTrackInfo();
            };

            Loaded += OnLoaded;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (_a, _b) =>
            {
                progressRect.Width = 300 * app.trackManager.audioManager.GetNormalizedPlaybackTime();
            };
            timer.Start();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdatePlayButton();
            UpdateTrackInfo();
        }

        private void TogglePlay(object sender, RoutedEventArgs e)
        {
            if (app.trackManager.playing)
            {
                app.trackManager.Pause();
            }
            else
            {
                app.trackManager.Play();
            }

            UpdatePlayButton();
        }

        void UpdatePlayButton()
        {
            if (app.trackManager.playing) PlayButton.Content = "\uE103";
            else PlayButton.Content = "\uE102";
        }

        private void PreviousTrack(object sender, RoutedEventArgs e)
        {
            app.trackManager.PreviousTrack();

            UpdateTrackInfo();
        }

        private void NextTrack(object sender, RoutedEventArgs e)
        {
            app.trackManager.NextTrack();

            UpdateTrackInfo();
        }

        private void UpdateTrackInfo()
        {
            TrackName.Content = app.trackManager.GetCurrentTrackName();

            var img = app.trackManager.GetCurrentTrackImage();

            CoverImage.Source = img;
        }

        bool draggingProgress;
        float dragOffset;

        const float dragThreshold = 0.05f;

        bool play;

        private void ProgressRectDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(this);

            float oldX = app.trackManager.audioManager.GetNormalizedPlaybackTime();
            float newX = (float)(mousePos.X / 300);

            float dist = oldX - newX;
            if (Math.Abs(dist) <= dragThreshold)
            {
                draggingProgress = true;
                dragOffset = dist;

                e.Handled = true;

                play = app.trackManager.playing;
                if (app.trackManager.playing)
                {
                    app.trackManager.Pause();
                }
            }
        }

        private void ProgressRectDrag(object sender, MouseEventArgs e)
        {
            if (draggingProgress)
            {
                var mousePos = e.GetPosition(this);

                float newX = (float)Math.Clamp(mousePos.X / 300 + dragOffset, 0, 1);

                app.trackManager.audioManager.SetNormalizedPlaybackTime(newX);

                e.Handled = true;
            }
        }

        private void ProgressRectUp(object sender, MouseEventArgs e)
        {
            if (draggingProgress)
            {
                draggingProgress = false;
                e.Handled = true;

                if (play)
                {
                    app.trackManager.Play();
                }
            }
        }
    }
}
