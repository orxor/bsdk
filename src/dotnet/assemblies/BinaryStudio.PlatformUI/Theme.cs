using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI
    {
    public class Theme : DependencyObject
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

        public static Theme[] Themes = {
            new Theme("NormalColor",        "Modern.NormalColor.xaml"),
            new Theme("Dark",               "Modern.Dark.xaml"),
            new Theme("Light",              "Modern.Light.xaml"),
            new Theme("VStudio",            "Modern.VStudio.xaml"),
            };

        private Theme(String name, String source) {
            Name = name;
            Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"PresentationFramework.Classic.dll"));
            Source = LoadResourceDictionary(source);
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
            if (Application != null) {
                Application.Resources.MergedDictionaries.Add(source.Source);
                OnApply(source.Name);
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

        private static void OnApply(String source) {
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
        }
    }