using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography
    {
    public interface RequestSecureString
        {
        HRESULT GetSecureString(CryptographicContext Context, RequestSecureStringEventArgs e);
        }
    }