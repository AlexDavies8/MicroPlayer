using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class PlaylistPage : Page
    {
        App app;

        List<Playlist> playlists = new List<Playlist>();

        public PlaylistPage()
        {
            InitializeComponent();

            app = Application.Current as App;

            LoadPlaylists();
            Loaded += (_a, _b) => CreatePlaylistButtons();
        }

        public void LoadPlaylists()
        {
            PlaylistContainer.Children.Clear();

            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string txtPath = System.IO.Path.Combine(baseDir, "resources", "playlists.txt");

            using (StreamReader reader = new StreamReader(txtPath))
            {
                string line;
                Playlist currPlaylist = new Playlist();

                while ((line = reader.ReadLine()) != null)
                {
                    string key = line.Substring(0, line.IndexOf(' ') + 1).Trim();
                    string value = line.Substring(line.IndexOf(' ') + 1).Trim();

                    switch (key)
                    {
                        case "NAME":
                            if (currPlaylist.name == null) 
                                currPlaylist.name = value;
                            else
                            {
                                playlists.Add(currPlaylist);
                                currPlaylist = new Playlist();
                                currPlaylist.name = value;
                            }
                            break;
                        case "IMAGE":
                            currPlaylist.imagePath = value;
                            break;
                        case "TRACK":
                            currPlaylist.trackPaths.Add(value);
                            break;
                    }
                }

                playlists.Add(currPlaylist);
            }

            CreatePlaylistButtons();
        }

        private void CreatePlaylistButtons()
        {

            for (int i = 0; i < playlists.Count; i++)
            {
                int x = i % 2;
                int y = i / 2;

                if (x == 0)
                {
                    PlaylistContainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(140) });
                }

                var btn = new PlaylistButton();
                btn.Margin = new Thickness(0, 0, 20, 20);

                btn.Width = 120;
                btn.Height = 120;
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.VerticalAlignment = VerticalAlignment.Top;

                Grid.SetColumn(btn, x);
                Grid.SetRow(btn, y);

                if (playlists[i].name != null)
                {
                    btn.Text = playlists[i].name;
                }

                if (playlists[i].imagePath != null)
                {
                    btn.Source = new BitmapImage(new Uri(playlists[i].imagePath, UriKind.RelativeOrAbsolute));
                }

                PlaylistContainer.Children.Add(btn);
            }

            var newPlaylistButton = new Button()
            {
                Width = 120,
                Height = 120,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Content = "\uE109",
                FontSize = 32,
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Background = (Brush)FindResource("ForegroundBrush"),
                Foreground = (Brush)FindResource("FontBrush")
            };

            newPlaylistButton.Click += LoadTrack;

            Grid.SetColumn(newPlaylistButton, playlists.Count % 2);
            Grid.SetRow(newPlaylistButton, playlists.Count / 2);

            PlaylistContainer.Children.Add(newPlaylistButton);
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
        }

        class Playlist
        {
            public string name;
            public string imagePath;
            public List<string> trackPaths = new List<string>();
        }
    }
}
