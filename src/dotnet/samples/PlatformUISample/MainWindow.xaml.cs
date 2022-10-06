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
using BinaryStudio.PlatformUI;

namespace PlatformUISample
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            }

        private void MainWindow_OnLoaded(Object sender, RoutedEventArgs e) {
            Theme.Apply();
            //Colors.ItemsSource = (new[] {
            //    Theme.ActiveBorderBrushKey,
            //    Theme.ActiveCaptionBrushKey,
            //    Theme.ActiveCaptionTextBrushKey,
            //    Theme.AppWorkspaceBrushKey,
            //    Theme.ControlBrushKey,
            //    Theme.ControlDarkBrushKey,
            //    Theme.ControlDarkDarkBrushKey,
            //    Theme.ControlLightBrushKey,
            //    Theme.ControlLightLightBrushKey,
            //    Theme.ControlTextBrushKey,
            //    Theme.DesktopBrushKey,
            //    Theme.GradientActiveCaptionBrushKey,
            //    Theme.GradientInactiveCaptionBrushKey,
            //    Theme.GrayTextBrushKey,
            //    Theme.HighlightBrushKey,
            //    Theme.HighlightTextBrushKey,
            //    Theme.HotTrackBrushKey,
            //    Theme.InactiveBorderBrushKey,
            //    Theme.InactiveCaptionBrushKey,
            //    Theme.InactiveCaptionTextBrushKey,
            //    Theme.InfoBrushKey,
            //    Theme.InfoTextBrushKey,
            //    Theme.MenuBrushKey,
            //    Theme.MenuBarBrushKey,
            //    Theme.MenuHighlightBrushKey,
            //    Theme.MenuTextBrushKey,
            //    Theme.ScrollBarBrushKey,
            //    Theme.WindowBrushKey,
            //    Theme.WindowFrameBrushKey,
            //    Theme.WindowTextBrushKey
            //    }).Select(i => new ColorInfo((ThemeResourceKey)i)).ToArray();
            foreach (var theme in Theme.Themes) {
                MenuItem i;
                ThemeMenuItem.Items.Add(i = new MenuItem
                    {
                    Header = theme,
                    });
                i.Click += ThemeChange;
                }
            }

        private void ThemeChange(Object sender, RoutedEventArgs e)
            {
            Theme.Apply((Theme)((MenuItem)e.OriginalSource).Header);
            }
        }
    }
