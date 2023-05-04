using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace BinaryStudio.Modeling.VSShellPackage.Controls
    {
    /// <summary>
    /// Interaction logic for MetadataScopeBrowserControl.xaml
    /// </summary>
    public partial class MetadataScopeBrowserControl : UserControl
        {
        public MetadataScopeBrowserControl()
            {
            InitializeComponent();
            }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
            {
            VSColorTheme.ThemeChanged += OnThemeChanged;
            OnThemeChanged(null);
            }

        private void OnThemeChanged(ThemeChangedEventArgs e) {
            var Shell = Package.GetGlobalService(typeof(SVsUIShell)) as IVsUIShell5;
            Box.Text = String.Empty;
            var r = new StringBuilder();
            foreach (var i in VsColors.GetCurrentThemedColorValues().OrderBy(i => i.Key.Name)) {
                r.AppendLine($"Category={i.Key.Category:B}:Name={{{i.Key.Name}}}:Type:{{{i.Key.KeyType}}}:{Shell.GetThemedWPFColor(i.Key).ToString().ToLower()}");
                }
            Box.Text = r.ToString();
            Colors.ItemsSource = VsColors.GetCurrentThemedColorValues().Keys.OrderBy(i=>i.Name).Select(i => new ColorInfo(i, Shell.GetThemedWPFColor(i))).ToArray();
            }

        private static void BuildReport(StringBuilder Target,Type Source) {
            Target.AppendLine($"Type:{{{Source.GetType().FullName}}}");
            Target.AppendLine("------------------------------------------------");
            foreach (var pi in Source.GetProperties(BindingFlags.Public|BindingFlags.Static)) {
                if (pi.Name.EndsWith("Key")) {
                    var r = pi.GetValue(null);
                    if (r != null) {
                        Target.Append($"{pi.Name}=");
                        }
                    }
                }
            }
        }

    public class ColorInfo
        {
        public String Name { get;set; }
        public Color Color { get; }
        public String Key { get;set; }
        public Guid Category { get;set; }
        public String Type { get;set; }
        public ColorInfo(ThemeResourceKey key, Color color)
            {
            Name = key.Name;
            Category = key.Category;
            Key = color.ToString().ToLower();
            Type = key.KeyType.ToString();
            Color = color;
            }
        }
    }
