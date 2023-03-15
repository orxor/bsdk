using System.Runtime.CompilerServices;
#if UseWPF
using System.Runtime.InteropServices;
using System.Windows.Markup;
#endif

[assembly: InternalsVisibleTo("BinaryStudio.Security.Cryptography")]
[assembly: InternalsVisibleTo("BinaryStudio.Security.Cryptography.CryptographicMessageSyntax")]
[assembly: InternalsVisibleTo("UnitTests.BinaryStudio.Security.Cryptography.Generator")]

#if UseWPF
[assembly: XmlnsPrefix("http://schemas.helix.global", "u")]
[assembly: XmlnsDefinition("http://schemas.helix.global", "BinaryStudio.Security.Cryptography.Certificates")]
#endif
