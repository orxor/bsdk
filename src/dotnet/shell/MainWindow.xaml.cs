﻿using System;
using System.Collections.Generic;
using System.IO;
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
using BinaryStudio.PlatformUI.Shell;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using log4net;
using Microsoft.Win32;
using BMainWindow = BinaryStudio.PlatformUI.MainWindow;

public partial class MainWindow : BMainWindow
    {
    private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
    private DocumentGroup dockgroup;
    private DocumentManager docmanager;

    public MainWindow()
        {
        InitializeComponent();
        }

    #region M:OnLoaded(Object,RoutedEventArgs)
    private void OnLoaded(Object sender, RoutedEventArgs e)
        {
        log.Debug("OnLoaded");
        Theme.Apply(Theme.Themes[1]);
        UpdateCommandBindings();
        var dockgroupcontainer = (DocumentGroupContainer)Profile.DockRoot.Children.FirstOrDefault(i => i is DocumentGroupContainer);
        if (dockgroupcontainer == null) {
            Profile.DockRoot.Children.Add(new DocumentGroupContainer
                (
                dockgroup = new DocumentGroup())
                );
            }
        else
            {
            dockgroup = (DocumentGroup)dockgroupcontainer.Children.FirstOrDefault(i => i is DocumentGroup);
            if (dockgroup == null)
                {
                dockgroupcontainer.Children.Add(dockgroup = new DocumentGroup());
                }
            }
        ViewManager.GetViewManager(dockgroup);
        docmanager = new DocumentManager(dockgroup,Profile);
        Initialize();
        }
    #endregion
    #region M:Initialize
    private void Initialize() {
        //OpenDocument(@"C:\TFS\bsdk\src\dotnet\tests\UnitTestData\dll\mfc250d.dll");
        //var view = new WebView2 {
        //    CreationProperties = new CoreWebView2CreationProperties {
        //        BrowserExecutableFolder = "Microsoft.WebView2.FixedVersionRuntime.108.0.1462.46.x64"
        //        }
        //    };
        //await view.EnsureCoreWebView2Async(null);
        //LoadFrom(@"C:\TFS\bsdk\mdl\atl30\atl30.emx");
        //ObjectIdentifierInfoExecuted(null,null);
        //OpenRegistryKeyExecuted(Registry.CurrentConfig);
        //LoadFrom(@"C:\TFS\.sqlite3\trace-rtEditor-2022-05-19-18-01-21.db");
        //docmanager.AddCertificateStoreManagement();
        }
    #endregion
    #region M:OpenDocument(String)
    private void OpenDocument(String filename) {
        docmanager.OpenDocument(filename);
        }
    #endregion
    private void UpdateCommandBindings() {
        CommandManager.RegisterClassCommandBinding(GetType(), new CommandBinding(ApplicationCommands.Open, OpenExecuted,CanExecuteAllways));
        //CommandManager.RegisterClassCommandBinding(GetType(), new CommandBinding(DocumentCommands.ConvertToBase64, ConvertToBase64Executed,CanExecuteAllways));
        //CommandManager.RegisterClassCommandBinding(GetType(), new CommandBinding(DocumentCommands.OpenBase64, OpenBase64Executed,CanExecuteAllways));
        //CommandManager.RegisterClassCommandBinding(GetType(), new CommandBinding(DocumentCommands.OpenRegistryKey, OpenRegistryKeyExecuted,CanExecuteAllways));
        //CommandManager.RegisterClassCommandBinding(GetType(), new CommandBinding(DocumentCommands.ObjectIdentifierInfo, ObjectIdentifierInfoExecuted,CanExecuteAllways));
        }

    //private void OpenBase64Executed(Object sender, ExecutedRoutedEventArgs e)
    //    {
    //    var dialog = new OpenFromBase64Dialog
    //        {
    //        Owner = this
    //        };
    //    //Theme.CurrentTheme.ApplyTo(dialog);
    //    if (dialog.ShowDialog() == true)
    //        {
    //        var o = docmanager.LoadView(Asn1Object.Load(new ReadOnlyMemoryMappingStream(dialog.OutputBytes)).FirstOrDefault());
    //        docmanager.Add(o, "?");
    //        }
    //    }
    //private void OpenBinaryDataExecuted(RoutedEventArgs e) {
    //    if (e is OpenBinaryDataEventArgs data) {
    //        var dialog = new OpenFromBase64Dialog{
    //            Owner = this,
    //            InputBytes = data.Data
    //            };
    //        if (dialog.ShowDialog() == true)
    //            {
    //            var o = docmanager.LoadView(Asn1Object.Load(new ReadOnlyMemoryMappingStream(dialog.OutputBytes)).FirstOrDefault());
    //            docmanager.Add(o, "?");
    //            e.Handled = true;
    //            }
    //        }
    //    }

    //private void ConvertToBase64Executed(Object sender, ExecutedRoutedEventArgs e)
    //    {
    //    var dialog = new OpenFileDialog{
    //        Filter = "All Files (*.*)|*.*"
    //        };
    //    if (dialog.ShowDialog(this) == true)
    //        {
    //        var r = File.ReadAllBytes(dialog.FileName);
    //        docmanager.Add(
    //            new[] {
    //                new View<EBase64Edit>(new EBase64Edit {
    //                Text = Convert.ToBase64String(r)
    //                }) },
    //            Path.GetFileNameWithoutExtension(dialog.FileName));
    //        }
    //    }

    private void LoadFrom(String filename) {
        docmanager.OpenDocument(filename);
        }

    private void OpenExecuted(Object sender, ExecutedRoutedEventArgs e) {
        var dialog = new OpenFileDialog{
            Filter = "All Files (*.*)|*.*|SQLite Files (*.db)|*.db|Model Files (*.emx)|*.emx"
            };
        if (dialog.ShowDialog(this) == true) {
            LoadFrom(dialog.FileName);
            }
        }

    private static void CanExecuteAllways(Object sender, CanExecuteRoutedEventArgs e)
        {
        e.CanExecute = true;
        e.Handled = true;
        }
    }
