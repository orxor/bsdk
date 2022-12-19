using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    internal class Asn1CertificateAuthorityInformationAccessCollection : ReadOnlyCollection<Asn1CertificateAuthorityInformationAccess>
        {
        public Asn1CertificateAuthorityInformationAccessCollection(IList<Asn1CertificateAuthorityInformationAccess> source)
            : base(source)
            {
            }

        public Asn1CertificateAuthorityInformationAccessCollection(IEnumerable<Asn1CertificateAuthorityInformationAccess> source)
            : this(source.ToList())
            {
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return $"Count = {Count}";
            }
        }
    }