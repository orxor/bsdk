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
using BMainWindow = BinaryStudio.PlatformUI.MainWindow;

public partial class MainWindow : BMainWindow
    {
    public MainWindow()
        {
        InitializeComponent();
        }

    #region M:OnLoaded(Object,RoutedEventArgs)
    private void OnLoaded(Object sender, RoutedEventArgs e)
        {
        Theme.Apply(Theme.Themes[3]);
        }
    #endregion
    }
