using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1CertificateRevocationList : Asn1LinkObject
        {
        public Int32 Version { get; }
        public DateTime  EffectiveDate { get; }
        public DateTime? NextUpdate { get; }
        public X509RelativeDistinguishedNameSequence Issuer { get;private set; }
        public Asn1CertificateExtensionCollection Extensions { get; }
        public String Country { get; }

        public Asn1CertificateRevocationList(Asn1Object o) :
            base(o)
            {
            State |= ObjectState.Failed;
            if (o is Asn1Sequence u) {
                if (u.Count == 3) {
                    if ((u[0] is Asn1Sequence) &&
                        (u[1] is Asn1Sequence) &&
                        (u[2] is Asn1BitString))
                        {
                        Version = (Int32)(Asn1Integer)u[0][0];
                        Issuer = new X509RelativeDistinguishedNameSequence(o[0][2].
                            Select(j => new KeyValuePair<Asn1ObjectIdentifier,String>(
                                (Asn1ObjectIdentifier)j[0][0], j[0][1].ToString())));
                        EffectiveDate = (Asn1Time)o[0][3];
                        var i = 4;
                        if (o[0][i] is Asn1Time) {
                            NextUpdate = (Asn1Time)o[0][4];
                            i++;
                            }
                        Country = GetCountry(Issuer);
                        State &= ~ObjectState.Failed;
                        }
                    }
                }
            }

        private static String GetCountry(X509RelativeDistinguishedNameSequence source) {
            return source.TryGetValue("2.5.4.6", out var r)
                ? r.ToString().ToLower()
                : null;
            }
        }
    }