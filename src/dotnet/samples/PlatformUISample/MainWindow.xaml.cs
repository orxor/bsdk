using System;
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
using BinaryStudio.IO;
using BinaryStudio.PlatformUI;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

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
            Theme.Apply(Theme.Themes[0]);
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
            TabItemAsn1.DataContext = Asn1Object.Load(new ReadOnlyMemoryMappingStream(Convert.FromBase64String("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"))).FirstOrDefault();
            }

        private void ThemeChange(Object sender, RoutedEventArgs e)
            {
            Theme.Apply((Theme)((MenuItem)e.OriginalSource).Header);
            }
        }
    }
