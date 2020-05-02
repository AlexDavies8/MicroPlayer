using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace MicroPlayer
{
    public class TrackManager
    {
        const float previousTrackThreshold = 5f;

        public AudioManager audioManager;
        public bool playing { get { return audioManager.playing; } }
        public Queue<Track> queue = new Queue<Track>();
        public Stack<Track> history = new Stack<Track>();

        public Action NextTrackCallback;

        public Track currentTrack;

        public TrackManager()
        {
            audioManager = new AudioManager();

            audioManager.TrackEndedCallback += (_a, _b) => NextTrack();
            audioManager.NextTrackCallback += (_a, _b) => NextTrackCallback();
        }

        public void ClearHistory()
        {
            history.Clear();
        }

        public void ClearQueue()
        {
            queue.Clear();
        }

        public void JumpQueue(int count)
        {
            for (int i = 0; i < count; i++)
            {
                history.Push(currentTrack);
                currentTrack = queue.Dequeue();
            }

            audioManager.PlayTrack(currentTrack.path);
        }

        public void JumpHistory(int count)
        {
            for (int i = 0; i < count; i++)
            {
                queue.Enqueue(currentTrack);
                currentTrack = history.Pop();
            }

            audioManager.PlayTrack(currentTrack.path);
        }

        public void LoadTrack(string path)
        {
            Track track = new Track();

            //Get path
            track.path = path;

            //Get Title
            var tfile = TagLib.File.Create(track.path);
            track.title = tfile.Tag.Title;

            //Get Image
            if (tfile.Tag.Pictures.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(tfile.Tag.Pictures[0].Data.Data))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    track.image = bitmap;
                }
            }

            if (track.title == "" || track.title == null)
            {
                track.title = Path.GetFileNameWithoutExtension(track.path);
            }

            queue.Enqueue(track);
            if (currentTrack == null) NextTrack();
        }

        public void PreviousTrack()
        {
            if (audioManager.GetPlaybackTime() >= previousTrackThreshold)
            {
                audioManager.Restart();
                return;
            }

            if (history.Count == 0) return;

            queue.Enqueue(currentTrack);
            currentTrack = history.Pop();
            audioManager.PlayTrack(currentTrack.path);
        }

        public void NextTrack()
        {
            if (queue.Count == 0) return;

            if (currentTrack != null) history.Push(currentTrack);
            currentTrack = queue.Dequeue();
            audioManager.PlayTrack(currentTrack.path);
        }

        public void Play()
        {
            audioManager.Play();
        }

        public void Pause()
        {
            audioManager.Pause();
        }

        public void TogglePlay()
        {
            if (audioManager.playing) audioManager.Pause();
            else audioManager.Play();
        }

        public string GetCurrentTrackName()
        {
            if (currentTrack == null) return null;
            return currentTrack.title;
        }

        public BitmapImage GetCurrentTrackImage()
        {
            if (currentTrack == null) return null;
            return currentTrack.image;
        }

        public class Track
        {
            public string title;
            public string path;
            public BitmapImage image;
        }
    }
}
