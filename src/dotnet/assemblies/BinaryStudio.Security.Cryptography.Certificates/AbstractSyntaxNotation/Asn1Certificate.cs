using System;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.Certificates.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents ASN.1 certificate structure.
    /// </summary>
    /// <remarks>
    /// <pre style="font-family: Consolas">
    /// Certificate  ::=  SEQUENCE  {
    ///   tbsCertificate       TBSCertificate,
    ///   signatureAlgorithm   AlgorithmIdentifier,
    ///   signatureValue       BIT STRING
    ///   }
    ///
    /// TBSCertificate  ::=  SEQUENCE  {
    ///   version         [0]  EXPLICIT Version DEFAULT v1,
    ///   serialNumber         CertificateSerialNumber,
    ///   signature            AlgorithmIdentifier,
    ///   issuer               Name,
    ///   validity             Validity,
    ///   subject              Name,
    ///   subjectPublicKeyInfo SubjectPublicKeyInfo,
    ///   issuerUniqueID  [1]  IMPLICIT UniqueIdentifier OPTIONAL, # If present, version MUST be v2 or v3
    ///   subjectUniqueID [2]  IMPLICIT UniqueIdentifier OPTIONAL, # If present, version MUST be v2 or v3
    ///   extensions      [3]  EXPLICIT Extensions OPTIONAL        # If present, version MUST be v3
    ///   }
    /// Version  ::=  INTEGER  {  v1(0), v2(1), v3(2)  }
    /// CertificateSerialNumber  ::=  INTEGER
    /// Validity ::= SEQUENCE {
    ///   notBefore      Time,
    ///   notAfter       Time
    ///   }
    /// 
    /// Time ::= CHOICE {
    ///   utcTime        UTCTime,
    ///   generalTime    GeneralizedTime
    ///   }
    /// 
    /// UniqueIdentifier  ::=  BIT STRING
    /// 
    /// SubjectPublicKeyInfo  ::=  SEQUENCE  {
    ///   algorithm            AlgorithmIdentifier,
    ///   subjectPublicKey     BIT STRING
    ///   }
    /// 
    /// Extensions  ::=  SEQUENCE SIZE (1..MAX) OF Extension
    /// 
    /// Extension  ::=  SEQUENCE  {
    ///   extnID      OBJECT IDENTIFIER,
    ///   critical    BOOLEAN DEFAULT FALSE,
    ///   extnValue   OCTET STRING
    ///                  -- contains the DER encoding of an ASN.1 value
    ///                  -- corresponding to the extension type identified
    ///                  -- by extnID
    ///   }
    /// 
    /// AlgorithmIdentifier  ::=  SEQUENCE  {
    ///   algorithm               OBJECT IDENTIFIER,
    ///   parameters              ANY DEFINED BY algorithm OPTIONAL
    ///   }
    /// </pre>
    /// </remarks>
    public class Asn1Certificate : Asn1LinkObject
        {
        public Int32 Version { get; }

        public Asn1Certificate(Asn1Object o)
            : base(o)
            {
            State |= ObjectState.Failed;
            if (o is Asn1Sequence U) {
                if ((U[0] is Asn1Sequence) &&
                    (U[1] is Asn1Sequence) &&
                    (U[2] is Asn1BitString))
                    {
                    }
                }
            }
        }
    }