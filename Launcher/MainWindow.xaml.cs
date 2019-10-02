﻿using System;
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
using Launcher.Core;

namespace Launcher
{
    public partial class MainWindow : Window
    {
        readonly DataManager<Config> ConfigManager = new DataManager<Config>("Launcher.json");
        readonly Config Config;

        public MainWindow()
        {
            InitializeComponent();

            Config = ConfigManager.Load(ThrowException);

            Title = Config.Title ?? App.Title;

        }

        // Alert windows
        public void Alert(string text, string title = "Alert") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        public void Warning(string text, string title = "Warning") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        public void Error(string text, string title = "Error") => MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        public void ThrowException(Exception exception) => Error(exception.ToString(), exception.GetType().Name);
    }
}
