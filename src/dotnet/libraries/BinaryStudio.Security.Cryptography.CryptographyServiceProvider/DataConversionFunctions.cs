using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    internal interface DataConversionFunctions
        {
        ALG_ID CertOIDToAlgId(String Id);
        IntPtr CertAlgIdToOID(ALG_ID Id);
        Int32 CertNameToStrA(Int32 CertEncodingType,ref CRYPT_BLOB Name,Int32 StrType,IntPtr psz,Int32 csz);
        Int32 CertNameToStrW(Int32 CertEncodingType,ref CRYPT_BLOB Name,Int32 StrType,IntPtr psz,Int32 csz);
        Boolean CertStrToName(Int32 CertEncodingType,String Name,Int32 StrType,IntPtr Reserved,Byte[] EncodedBytes,ref Int32 EncodedLength,IntPtr Error);
        }
    }