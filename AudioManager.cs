using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MicroPlayer
{
    public class AudioManager
    {
        MediaPlayer player;
        public bool playing { get; private set; }

        public EventHandler TrackEndedCallback;
        public EventHandler NextTrackCallback;

        public AudioManager()
        {
            player = new MediaPlayer();
            player.MediaOpened += OnOpen;
            player.MediaEnded += (sender, e) => TrackEndedCallback(sender, e);
            player.MediaOpened += (sender, e) => NextTrackCallback(sender, e);
        }

        private void OnOpen(object sender, EventArgs e)
        {
            if (playing) player.Play();
        }

        public void PlayTrack(string pathToFile)
        {
            player.Open(new Uri(pathToFile));
            Play();
        }

        public void Restart()
        {
            player.Position = TimeSpan.FromSeconds(0);
        }

        public void Play()
        {
            if (!playing)
            {
                player.Play();
                playing = true;
            }
        }

        public void Pause()
        {
            if (playing)
            {
                player.Pause();
                playing = false;
            }
        }

        public float GetNormalizedPlaybackTime()
        {
            if (!player.HasAudio || !player.NaturalDuration.HasTimeSpan) return 0.0f;

            return (float)(player.Position.TotalMilliseconds / player.NaturalDuration.TimeSpan.TotalMilliseconds);
        }

        public void SetNormalizedPlaybackTime(float time)
        {
            if (!player.HasAudio) return;

            player.Position = TimeSpan.FromMilliseconds(time * player.NaturalDuration.TimeSpan.TotalMilliseconds);
        }

        public float GetPlaybackTime()
        {
            if (!player.HasAudio) return 0.0f;

            return (float) player.Position.TotalSeconds;
        }

        public void SetVolume(float volume)
        {
            player.Volume = Math.Log(volume);
        }
    }
}
