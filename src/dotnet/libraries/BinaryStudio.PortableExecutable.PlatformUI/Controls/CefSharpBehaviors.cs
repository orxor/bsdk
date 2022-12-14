using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using Microsoft.Web.WebView2.Wpf;

namespace BinaryStudio.PortableExecutable.PlatformUI.Controls
    {
    public class CefSharpBehaviors
        {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(Object), typeof(CefSharpBehaviors), new PropertyMetadata(default(Object),OnSourceChanged));
        private static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            //if (sender is ChromiumWebBrowser browser) {
            //    if (e.NewValue is String String) {
            //        var filename = Path.GetTempFileName() + ".html";
            //        File.WriteAllText(filename,String);
            //        browser.LoadingStateChanged += OnLoadingStateChanged;
            //        browser.Load($"file://{filename}");
            //        return;
            //        }
            //    }
            if (sender is WebView2 browser) {
                if (e.NewValue is String String) {
                    var filename = Path.GetTempFileName() + ".html";
                    File.WriteAllText(filename,String);
                    //browser.EnsureCoreWebView2Async(null).Wait();
                    browser.Source = new Uri($"file://{filename}");
                    //browser.CoreWebView2.NavigateToString(String);
                    return;
                    }
                }
            }

        private static async void OnLoadingStateChanged(Object sender,LoadingStateChangedEventArgs e) {
            if (sender is ChromiumWebBrowser browser) {
                if (!e.IsLoading) {
                    browser.LoadingStateChanged -= OnLoadingStateChanged;
                    var response = await browser.EvaluateScriptAsync(
                        // GET HEIGHT OF CONTENT
                        "(function() {                       " +
                        "  var _docHeight =                  " +
                        "    (document.height !== undefined) " +
                        "    ? document.height               " +
                        "    : document.body.offsetHeight;   " +
                        "                                    " +
                        "  return _docHeight;                " +
                        "}                                   " +
                        ")();");
                    var docHeight = (Int32)response.Result;
                    response = await browser.EvaluateScriptAsync(
                        // GET HEIGHT OF CONTENT
                        "(function() {                       " +
                        "  var _docWidth =                  " +
                        "    (document.width !== undefined) " +
                        "    ? document.width               " +
                        "    : document.body.offsetWidth;   " +
                        "                                    " +
                        "  return _docWidth;                " +
                        "}                                   " +
                        ")();");
                    var docWidth = (Int32)response.Result;
                    browser.Dispatcher.Invoke(() => {
                        browser.Height = docHeight + 10;
                        browser.UpdateLayout();
                        //browser.Width = docWidth + 10;
                        });
                    }
                }
            }

        public static void SetSource(DependencyObject element, Object value)
            {
            element.SetValue(SourceProperty, value);
            }

        public static Object GetSource(DependencyObject element)
            {
            return (Object)element.GetValue(SourceProperty);
            }
        }
    }