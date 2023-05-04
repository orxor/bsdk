using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public interface IX509CertificateChainStatus
        {
        CertificateChainErrorStatus ErrorStatus { get; }
        CertificateChainInfoStatus  InfoStatus  { get; }
        }
    }
