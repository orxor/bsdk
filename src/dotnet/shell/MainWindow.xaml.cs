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
using BinaryStudio.PlatformUI.Shell;
using log4net;
using Microsoft.Web.WebView2.Wpf;
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
    private async void OnLoaded(Object sender, RoutedEventArgs e)
        {
        log.Debug("OnLoaded");
        Theme.Apply(Theme.Themes[1]);
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
    private async void Initialize() {
        OpenDocument(@"C:\TFS\bsdk\src\dotnet\tests\UnitTestData\dll\mfc250d.dll");
        var view = new WebView2 {
            CreationProperties = new CoreWebView2CreationProperties {
                BrowserExecutableFolder = "Microsoft.WebView2.FixedVersionRuntime.108.0.1462.46.x64"
                }
            };
        await view.EnsureCoreWebView2Async(null);
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
    }
