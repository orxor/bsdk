using System;
using System.Collections.Generic;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.Reporting
    {
    internal class HierarchyNodeCER
        {
        public Boolean IsSelfSigned { get;set; }
        public X509Certificate CER { get;set; }
        public IList<HierarchyNodeCER> DescendantsCER { get; }

        public HierarchyNodeCER()
            {
            DescendantsCER = new List<HierarchyNodeCER>();
            }
        }
    }