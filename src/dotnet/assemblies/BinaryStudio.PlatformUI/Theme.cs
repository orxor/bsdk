using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Media;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI
    {
    public partial class Theme : DependencyObject
        {
        #region P:CurrentTheme:Theme
        public static Theme CurrentTheme { get; private set; }
        #endregion
        #region P:Application:Application
        private static Application Application { get {
            return Application.Current ?? new Application{
                ShutdownMode = ShutdownMode.OnExplicitShutdown
                };
            }}
        #endregion
        public ResourceDictionary Source {get; }
        public String Name { get; }

        private static readonly Guid ControlStyleCategory = new Guid("3e635bf2-7305-43dc-b316-26d5cdfe6351");
        private static readonly Guid SystemColorCategory = new Guid("38c7be46-1216-45b8-9070-3ef4332772aa");

        public static Theme[] Themes = {
            new Theme("NormalColor",        "Modern.NormalColor.xaml"),
            new Theme("Dark",               "Modern.Dark.xaml"),
            new Theme("Light",              "Modern.Light.xaml"),
            new Theme("VStudio",            "Modern.VStudio.xaml"),
            };

        private Theme(String name, String source) {
            Name = name;
            Source = LoadResourceDictionary(source);
            }

        static Theme()
            {
            try
                {
                Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"PresentationFramework.Classic.dll"));
                }
            catch
                {
                /* do nothing */
                }
            }

        #region M:Apply
        public static void Apply() {
            Apply(Themes[0]);
            }
        #endregion
        #region M:Apply(Theme)
        public static void Apply(Theme source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            CurrentTheme = source;
            var application = Application;
            if (application != null) {
                application.Resources = source.Source;
                //application.Resources.BeginInit();
                //application.Resources.MergedDictionaries.Clear();
                //application.Resources.Clear();
                //application.Resources.MergedDictionaries.Add(source.Source);
                //Apply(source.Source);
                OnApply(source.Name);
                //application.Resources.EndInit();
                }
            }
        #endregion
        #region M:Apply(String)
        public static void Apply(String source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (String.IsNullOrWhiteSpace(source)) { throw new ArgumentOutOfRangeException(nameof(source)); }
            foreach (var theme in Themes) {
                if (String.Equals(theme.Name, source, StringComparison.OrdinalIgnoreCase)) {
                    Apply(theme);
                    return;
                    }
                }
            throw new ArgumentOutOfRangeException(nameof(source));
            }
        #endregion
        #region M:Apply(ResourceDictionary)
        private static void Apply(ResourceDictionary source) {
            var application = Application;
            if (application != null) {
                foreach (var i in source.MergedDictionaries) {
                    Apply(i);
                    }
                foreach (var i in source.Keys) {
                    application.Resources[i] = null;
                    application.Resources[i] = source[i];
                    }
                }
            return;
            }
        #endregion
        #region M:LoadResourceDictionary(String):ResourceDictionary
        private static ResourceDictionary LoadResourceDictionary(String source) {
            return LoadResourceDictionary("BinaryStudio.PlatformUI", source);
            }
        #endregion
        #region M:LoadResourceDictionary(String,String):ResourceDictionary
        private static ResourceDictionary LoadResourceDictionary(String assembly, String source) {
            var r = (ResourceDictionary)Application.LoadComponent(
                new Uri($"{assembly};component/Themes/{source}",
                UriKind.Relative));
            Debug.Print($"{assembly};component/Themes/{source}");
            return r;
            }
        #endregion
        #region M:OnApply(String)
        private static void OnApply(String source) {
            if (String.Equals(source,"NormalColor",StringComparison.OrdinalIgnoreCase)) {
                var c = new SolidColorBrush(AdjustBrightness(SystemColors.HighlightColor,100));
                Application.Resources[HighlightLightBrushResourceKey] = c;
                }
            return;
            try
                {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies.Where(i => !i.IsDynamic)) {
                    if (!ReferenceEquals(assembly, Assembly.GetExecutingAssembly())) {
                        var resources = assembly.GetManifestResourceNames();
                        if (resources.Length > 0) {
                            foreach (var resource in resources) {
                                if (resource.EndsWith(".g.resources")) {
                                    try
                                        {
                                        var r = LoadResourceDictionary(Path.GetFileNameWithoutExtension(assembly.Location), $"Modern.{source}.xaml");
                                        if (r != null) {
                                            Application.Resources.MergedDictionaries.Add(r);
                                            }
                                        }
                                    catch (IOException) { }
                                    catch (MissingSatelliteAssemblyException) { }
                                    catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            catch (Exception e)
                {
                Debug.Print(Exceptions.ToString(e));
                throw;
                }
            }
        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return Name;
            }

        private static Color AdjustBrightness(Color value, Byte factor) {
            return value + Color.FromRgb(factor,factor,factor);
            }
        }
    }