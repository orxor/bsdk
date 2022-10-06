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
using BinaryStudio.PlatformUI;
using BinaryStudio.PortableExecutable;
using BinaryStudio.PortableExecutable.PlatformUI.Models;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace PEView
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
            {
            Theme.Apply();
            var Scope = new MetadataScope();
            var o = Scope.Load(@"C:\TFS\mir\crypto\.intermediate\csecapi32.dll");
            DataContext = new EMZMetadataObject((MZMetadataObject)o);
            }
        }
    }
