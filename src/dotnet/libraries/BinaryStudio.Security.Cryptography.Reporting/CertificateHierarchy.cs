using System.Collections.Generic;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.Reporting
    {
    internal class CertificateHierarchy : HierarchyNodeCER
        {
        public CertificateHierarchy(X509Certificate certificate)
            {
            CER = certificate;
            }
        }
    }