using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    using CRYPT_DATA_BLOB = CRYPT_BLOB;

    public interface CMSG_KEY_TRANS_RECIPIENT_INFO
        {
        CERT_ID RecipientId { get; }
        }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CMSG_KEY_TRANS_RECIPIENT_INFO32 : CMSG_KEY_TRANS_RECIPIENT_INFO
        {
        public readonly CMSG_KEY_RECIPIENT_VERSION Version;
        public readonly CERT_ID32 RecipientId;
        public readonly CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;
        public readonly CRYPT_DATA_BLOB EncryptedKey;
        CERT_ID CMSG_KEY_TRANS_RECIPIENT_INFO.RecipientId { get { return RecipientId; }}
        }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CMSG_KEY_TRANS_RECIPIENT_INFO64 : CMSG_KEY_TRANS_RECIPIENT_INFO
        {
        public readonly CMSG_KEY_RECIPIENT_VERSION Version;
        public readonly CERT_ID64 RecipientId;
        public readonly CRYPT_ALGORITHM_IDENTIFIER KeyEncryptionAlgorithm;
        public readonly CRYPT_DATA_BLOB EncryptedKey;
        CERT_ID CMSG_KEY_TRANS_RECIPIENT_INFO.RecipientId { get { return RecipientId; }}
        }
    }
