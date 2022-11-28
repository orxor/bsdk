using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using BinaryStudio.PlatformUI.Shell;
using BinaryStudio.PlatformUI.Shell.Controls;

namespace BinaryStudio.PlatformUI
    {
    public class MainWindow : CustomChromeWindow
        {
        static MainWindow()
            {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(typeof(MainWindow)));
            }

        public MainWindow()
            {
            UpdateCommandBindings();
            }

        #region P:VectorIcon:DrawingBrush
        public static readonly DependencyProperty VectorIconProperty = DependencyProperty.Register("VectorIcon", typeof(DrawingBrush), typeof(MainWindow), new PropertyMetadata(default(DrawingBrush)));
        public DrawingBrush VectorIcon {
            get { return (DrawingBrush)GetValue(VectorIconProperty); }
            set { SetValue(VectorIconProperty, value); }
            }
        #endregion
        #region P:Menu:Menu
        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register("Menu", typeof(Menu), typeof(MainWindow), new PropertyMetadata(default(Menu), OnMenuChanged));
        private static void OnMenuChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {
            var source = (sender as MainWindow);
            if (source != null)
                {
                source.OnMenuChanged();
                }
            }

        private void OnMenuChanged()
            {
            //throw new NotImplementedException();
            }

        public Menu Menu {
            get { return (Menu)GetValue(MenuProperty); }
            set { SetValue(MenuProperty, value); }
            }
        #endregion
        #region P:MainSite:MainSite
        public MainSite MainSite { get {
            EnsureMainSite();
            return mainsite;
            }}
        #endregion
        #region P:Profile:WindowProfile
        public WindowProfile Profile { get {
            EnsureWindowProfile();
            return profile;
            }}
        #endregion

        public void Connect(Int32 connectionId, Object target)
            {
            //throw new NotImplementedException();

            }

        public void InitializeComponent()
            {
            //throw new NotImplementedException();
            }

        #region M:EnsureMainSite
        private MainSite mainsite;
        private void EnsureMainSite() {
            if (mainsite == null) {
                mainsite = Profile.MainSite;
                }
            }
        #endregion
        #region M:EnsureWindowProfile
        private WindowProfile profile;
        private void EnsureWindowProfile() {
            if (profile == null) {
                profile = WindowProfile.Create("Default");
                ViewManager.Instance.Initialize(this);
                ViewManager.Instance.WindowProfile = profile;
                }
            }
        #endregion

        private void UpdateCommandBindings() {
            CommandManager.RegisterClassCommandBinding(GetType(), new CommandBinding(PlatformCommands.CopyToXaml, CopyToXamlExecuted,CanExecuteCopyToXaml));
            }

        #region M:CopyToXamlExecuted(Object,ExecutedRoutedEventArgs)
        private static void CopyToXamlExecuted(Object sender, ExecutedRoutedEventArgs e) {
            if (e.OriginalSource is RichTextBox RichTextBox) {
                using (var memory = new MemoryStream()) {
                    RichTextBox.Selection.Save(memory,DataFormats.Xaml);
                    var builder = new StringBuilder();
                    using (var writer = XmlWriter.Create(new StringWriter(builder),new XmlWriterSettings{
                        Indent = true,
                        IndentChars = "  "
                        })) {
                        using (var reader = XmlReader.Create(new StreamReader(new MemoryStream(memory.ToArray())))) {
                            writer.WriteNode(reader,false);
                            }
                        }
                    Clipboard.SetText(builder.ToString());
                    }
                }
            }
        #endregion
        #region M:CanExecuteCopyToXaml(Object,CanExecuteRoutedEventArgs)
        private static void CanExecuteCopyToXaml(Object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = false;
            if (e.OriginalSource is RichTextBox RichTextBox) {
                e.CanExecute = !RichTextBox.Selection.IsEmpty;
                e.Handled = true;
                }
            }
        #endregion
        }
    }