using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class PlaylistPage : Page
    {
        App app;

        List<Playlist> playlists = new List<Playlist>();

        Playlist currentPlaylist;

        public PlaylistPage()
        {
            InitializeComponent();

            app = Application.Current as App;

            LoadPlaylists();
            Loaded += (_a, _b) =>
            {
                CreatePlaylistButtons();
                UpdateView();
            };
        }

        public void LoadPlaylists()
        {
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
                            currPlaylist.tracks.Add(app.trackManager.GetTrackFromPath(value));
                            break;
                    }
                }

                playlists.Add(currPlaylist);
            }

            CreatePlaylistButtons();
        }

        private void CreatePlaylistButtons()
        {
            PlaylistContainer.Children.Clear();
            PlaylistContainer.RowDefinitions.Clear();

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

                btn.Foreground = (Brush)FindResource("FontBrush");

                Grid.SetColumn(btn, x);
                Grid.SetRow(btn, y);

                if (playlists[i].name != null)
                {
                    btn.Text = playlists[i].name;
                }

                if (playlists[i].imagePath != null)
                {
                    if (File.Exists(playlists[i].imagePath))
                    {
                        btn.Foreground = Brushes.Black;
                        btn.Source = new BitmapImage(new Uri(playlists[i].imagePath, UriKind.RelativeOrAbsolute));
                    }
                }

                int _i = i;
                btn.Button.Click += (_a, _b) =>
                {
                    currentPlaylist = playlists[_i];
                    UpdateView();
                };

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

            newPlaylistButton.Click += (_a, _b) => AddPlaylist();

            if (playlists.Count / 2 > PlaylistContainer.RowDefinitions.Count - 1)
            {
                PlaylistContainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(140) });
            }

            Grid.SetColumn(newPlaylistButton, playlists.Count % 2);
            Grid.SetRow(newPlaylistButton, playlists.Count / 2);

            PlaylistContainer.Children.Add(newPlaylistButton);
        }

        private void UpdateView()
        {
            var oldTrackList = LogicalTreeHelper.FindLogicalNode(root, "TrackList") as Grid;
            if (oldTrackList != null) root.Children.Remove(oldTrackList);

            if (currentPlaylist == null)
            {
                PlaylistScroller.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                PlaylistScroller.Visibility = Visibility.Collapsed;
            }

            Grid trackListRoot = new Grid() { Name = "TrackList" };
            CustomScrollViewer scrollViewer = new CustomScrollViewer() { SpeedFactor = 0.1, VerticalScrollBarVisibility = ScrollBarVisibility.Hidden, Margin = new Thickness(10, 60, 10, 10), ClipToBounds = true };
            trackListRoot.Children.Add(scrollViewer);
            StackPanel container = new StackPanel();
            scrollViewer.Content = container;
            int i = 0;
            foreach (var track in currentPlaylist.tracks)
            {
                int _i = i;

                Thickness margin = new Thickness(0, 5, 0, 5);
                if (_i == app.trackManager.queue.Count - 1) margin = new Thickness(0, 5, 0, 0);

                TrackButton btn = new TrackButton() { Margin = margin };
                btn.PlayClicked += (_a, _b) =>
                {
                    app.trackManager.LoadTrack(currentPlaylist.tracks[_i].path);
                };
                btn.RemoveClicked += (_a, _b) =>
                {
                    currentPlaylist.tracks.RemoveAt(_i);
                    UpdateView();
                    SavePlaylists();
                };
                btn.Text = track.title;
                container.Children.Add(btn);
                i++;
            }

            Button backButton = new Button() { Content = "\uE751", Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 24, Width = 32, Height = 32, Margin = new Thickness(10, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
            backButton.Click += (_a, _b) =>
            {
                currentPlaylist = null;
                UpdateView();
            };
            trackListRoot.Children.Add(backButton);

            Button playButton = new Button() { Content = "\uE102", Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 24, Width = 32, Height = 32, Margin = new Thickness(68, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
            playButton.Click += (_a, _b) =>
            {
                AddPlaylistToQueue();
            };
            trackListRoot.Children.Add(playButton);

            Button addButton = new Button() { Content = "\uE8E5", Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 24, Width = 32, Height = 32, Margin = new Thickness(126, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
            addButton.Click += LoadTrack;
            trackListRoot.Children.Add(addButton);

            Button imageButton = new Button() { Content = "\uE932", Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 24, Width = 32, Height = 32, Margin = new Thickness(184, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
            imageButton.Click += (_a, _b) =>
            {
                SetPlaylistImage();
                UpdateView();
                CreatePlaylistButtons();
            };
            trackListRoot.Children.Add(imageButton);

            Button renameButton = new Button() { Content = "\uE8AC", Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 24, Width = 32, Height = 32, Margin = new Thickness(221, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
            renameButton.Click += (_a, _b) =>
            {
                SetPlaylistName();
            };
            trackListRoot.Children.Add(renameButton);

            Button removeButton = new Button() { Content = "\uE74D", Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 24, Width = 32, Height = 32, Margin = new Thickness(258, 10, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
            removeButton.Click += (_a, _b) =>
            {
                playlists.Remove(currentPlaylist);
                currentPlaylist = null;
                SavePlaylists();
                UpdateView();
                CreatePlaylistButtons();
            };
            trackListRoot.Children.Add(removeButton);

            root.Children.Add(trackListRoot);
        }

        void AddPlaylistToQueue()
        {
            app.trackManager.currentTrack = null;
            app.trackManager.ClearQueue();
            foreach (var track in currentPlaylist.tracks)
            {
                app.trackManager.LoadTrack(track.path);
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
                    currentPlaylist.tracks.Add(app.trackManager.GetTrackFromPath(path));
                }

                SavePlaylists();
                UpdateView();
            }
        }

        private void AddPlaylist()
        {
            Playlist newPlaylist = new Playlist();
            newPlaylist.name = "New Playlist";

            playlists.Add(newPlaylist);

            SavePlaylists();
            CreatePlaylistButtons();
        }

        private void SetPlaylistName()
        {
            var popupGrid = new Grid();

            var popup = new Popup() { Width = 200, Height = 30, Child = popupGrid, IsOpen = true, Placement = PlacementMode.MousePoint };
            popup.Child = popupGrid;
            popup.IsOpen = true;
            popup.PlacementTarget = root;
            popup.Placement = PlacementMode.MousePoint;

            var nameField = new TextBox() { Width = 140, Height = 30, FontSize = 16, Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left };
            popupGrid.Children.Add(nameField);

            var cancelButton = new Button() { Width = 30, FontSize = 16, Margin = new Thickness(0, 0, 30, 0), Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), Content = "\uE711", FontFamily = new FontFamily("Segoe MDL2 Assets"), BorderThickness = new Thickness(0), HorizontalAlignment = HorizontalAlignment.Right };
            cancelButton.Click += (_a, _b) =>
            {
                root.Children.Remove(popup);
            };
            popupGrid.Children.Add(cancelButton);

            var doneButton = new Button() { Width = 30, FontSize = 16, Background = (Brush)FindResource("ForegroundBrush"), Foreground = (Brush)FindResource("FontBrush"), Content = "\uE73E", FontFamily = new FontFamily("Segoe MDL2 Assets"), BorderThickness = new Thickness(0), HorizontalAlignment = HorizontalAlignment.Right };
            doneButton.Click += (_a, _b) =>
            {
                currentPlaylist.name = nameField.Text;
                SavePlaylists();
                CreatePlaylistButtons();
                root.Children.Remove(popup);
            };
            popupGrid.Children.Add(doneButton);

            root.Children.Add(popup);
        }

        private void SetPlaylistImage()
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter =
                    "Image Files (*.png;*.jpg)|*.png;*.jpg|" +
                    "All files (*.*)|*.*",
                Title = "Select Playlist Image"
            };

            var result = openDialog.ShowDialog();
            if (result != null)
            {
                if (File.Exists(openDialog.FileName))
                {
                    currentPlaylist.imagePath = openDialog.FileName;

                    SavePlaylists();
                }
            }
        }

        private void SavePlaylists()
        {
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string txtPath = System.IO.Path.Combine(baseDir, "resources", "playlists.txt");

            using (StreamWriter writer = new StreamWriter(txtPath))
            {
                foreach (var playlist in playlists)
                {
                    writer.WriteLine("NAME " + playlist.name);
                    writer.WriteLine("IMAGE " + playlist.imagePath);
                    foreach (var track in playlist.tracks)
                    {
                        writer.WriteLine("TRACK " + track.path);
                    }
                }
            }
        }

        class Playlist
        {
            public string name;
            public string imagePath;
            public List<TrackManager.Track> tracks = new List<TrackManager.Track>();
        }
    }
}
