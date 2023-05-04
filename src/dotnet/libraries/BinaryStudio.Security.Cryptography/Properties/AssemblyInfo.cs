using System.Runtime.CompilerServices;
#if UseWPF
using System.Runtime.InteropServices;
using System.Windows.Markup;
#endif

[assembly: InternalsVisibleTo("BinaryStudio.Security.Cryptography.CryptographyServiceProvider")]
[assembly: InternalsVisibleTo("BinaryStudio.Security.Cryptography.CryptographicMessageSyntax")]
[assembly: InternalsVisibleTo("BinaryStudio.Security.Cryptography.Certificates")]
[assembly: InternalsVisibleTo("BinaryStudio.Security.Cryptography.PlatformUI")]

#if UseWPF
[assembly: XmlnsPrefix("http://schemas.helix.global", "u")]
[assembly: XmlnsDefinition("http://schemas.helix.global", "BinaryStudio.Security.Cryptography.Certificates")]
#endif
