﻿using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) subjectAltName(17)}
     * 2.5.29.17
     * /Joint-ISO-ITU-T/5/29/17
     * Subject alternative name ("subjectAltName" extension)
     * IETF RFC 5280
     * */
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.NSS_OID_X509_SUBJECT_ALT_NAME)]
    public class SubjectAlternativeName : IssuerAlternativeName
        {
        #region ctor{CertificateExtension}
        internal SubjectAlternativeName(CertificateExtension source)
            : base(source)
            {
            }
        #endregion
        }
    }