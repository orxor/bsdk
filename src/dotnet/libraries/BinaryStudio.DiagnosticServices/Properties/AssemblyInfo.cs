using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if UseWPF
using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(ResourceDictionaryLocation.SourceAssembly, ResourceDictionaryLocation.SourceAssembly)]
[assembly: XmlnsPrefix("http://schemas.helix.global", "u")]
[assembly: XmlnsDefinition("http://schemas.helix.global", "BinaryStudio.DiagnosticServices")]
#endif
