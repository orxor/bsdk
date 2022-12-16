using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: InternalsVisibleTo("BinaryStudio.PortableExecutable.PlatformUI")]
#if UseWPF
[assembly: XmlnsPrefix("http://schemas.helix.global", "u")]
[assembly: XmlnsDefinition("http://schemas.helix.global", "BinaryStudio.PortableExecutable")]
[assembly: XmlnsDefinition("http://schemas.helix.global", "BinaryStudio.PortableExecutable.CodeView")]
#endif
