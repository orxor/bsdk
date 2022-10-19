using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BinaryStudio.IO;
using BinaryStudio.PlatformUI;
using BinaryStudio.PlatformUI.Media;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using ColorConverter = BinaryStudio.PlatformUI.ColorConverter;

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
            RGBTextBox.Text = SystemColors.HighlightColor.ToString();
            }

        private void ThemeChange(Object sender, RoutedEventArgs e)
            {
            Theme.Apply((Theme)((MenuItem)e.OriginalSource).Header);
            }

        private IList<ColorInfo> MarkSame(IList<ColorInfo> colors, Color color) {
            foreach (var info in colors) {
                var r = ((SolidColorBrush)info.Color).Color;
                var ScR = Math.Abs(r.ScR - color.ScR);
                var ScG = Math.Abs(r.ScG - color.ScG);
                var ScB = Math.Abs(r.ScB - color.ScB);
                if ((ScR < 0.2) && (ScG < 0.2) && (ScB < 0.2)) {
                    info.BorderThickness = new Thickness(2);
                    }
                }
            return colors;
            }

        private void RGBTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
            var rgb = (new ColorConverter()).Convert(RGBTextBox.Text,null,CultureInfo.CurrentUICulture);
            var hsl = new HSLColor(rgb);
            var hsv = new HSVColor(hsl);
            var HSLAxisHValues = new List<ColorInfo>();
            var HSLAxisSValues = new List<ColorInfo>();
            var HSLAxisLValues = new List<ColorInfo>();
            var HSVAxisHValues = new List<ColorInfo>();
            var HSVAxisSValues = new List<ColorInfo>();
            var HSVAxisVValues = new List<ColorInfo>();
            var RGBAxisOValues = new List<ColorInfo>();
            var RGBAxisPValues = new List<ColorInfo>();
            var RGBAxisBValues = new List<ColorInfo>();
            var Count = 100;
            var Step = 1f/Count;
            for (var i = 0; i <= Count; i++) {
                HSLAxisHValues.Add(new ColorInfo(new HSLColor{
                    ScH = i*Step,
                    ScS = hsl.ScS,
                    ScL = hsl.ScL
                    }));
                HSLAxisSValues.Add(new ColorInfo(new HSLColor{
                    ScH = hsl.ScH,
                    ScS = i*Step,
                    ScL = hsl.ScL
                    }));
                HSLAxisLValues.Add(new ColorInfo(new HSLColor{
                    ScH = hsl.ScH,
                    ScS = hsl.ScS,
                    ScL = i*Step
                    }));
                HSVAxisHValues.Add(new ColorInfo(new HSVColor{
                    ScH = i*Step,
                    ScS = hsv.ScS,
                    ScV = hsv.ScV
                    }));
                HSVAxisSValues.Add(new ColorInfo(new HSVColor{
                    ScH = hsv.ScH,
                    ScS = i*Step,
                    ScV = hsv.ScV
                    }));
                HSVAxisVValues.Add(new ColorInfo(new HSVColor{
                    ScH = hsv.ScH,
                    ScS = hsv.ScS,
                    ScV = i*Step
                    }));
                RGBAxisOValues.Add(new ColorInfo(ColorConverter.PerformOpacity(rgb,i*Step)));
                RGBAxisBValues.Add(new ColorInfo(ColorConverter.AdjustBrightness(rgb,-1f + 2*i*Step)));
                RGBAxisPValues.Add(new ColorInfo(new Color{
                    A = 0xff,
                    ScR = rgb.ScR*i*Step,
                    ScG = rgb.ScG*i*Step,
                    ScB = rgb.ScB*i*Step
                    }));
                }
            HSLAxisH.ItemsSource = MarkSame(HSLAxisHValues,rgb);
            HSLAxisS.ItemsSource = MarkSame(HSLAxisSValues,rgb);
            HSLAxisL.ItemsSource = MarkSame(HSLAxisLValues,rgb);
            HSVAxisH.ItemsSource = MarkSame(HSVAxisHValues,rgb);
            HSVAxisS.ItemsSource = MarkSame(HSVAxisSValues,rgb);
            HSVAxisV.ItemsSource = MarkSame(HSVAxisVValues,rgb);
            RGBAxisO.ItemsSource = MarkSame(RGBAxisOValues,rgb);
            RGBAxisP.ItemsSource = MarkSame(RGBAxisPValues,rgb);
            RGBAxisB.ItemsSource = MarkSame(RGBAxisBValues,rgb);
            }
        }
    }
