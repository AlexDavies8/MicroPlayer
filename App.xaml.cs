﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MicroPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public TrackManager trackManager;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            trackManager = new TrackManager();
        }
    }
}
