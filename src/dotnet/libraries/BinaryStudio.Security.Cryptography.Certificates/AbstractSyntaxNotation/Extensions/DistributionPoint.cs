using System.Linq;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    public class DistributionPoint : Asn1LinkObject<Asn1Sequence>
        {
        public DistributionPointName Point { get; }
        public ReasonFlags ReasonFlags { get; }
        public IX509GeneralName CRLIssuer { get; }

        public DistributionPoint(Asn1Sequence source)
            : base(source)
            {
            var values = source.OfType<Asn1ContextSpecificObject>().ToArray();
            var c = values.FirstOrDefault(i => i.Type == 0);
            if (c != null)
                {
                Point = new DistributionPointName(c);
                }
            ReasonFlags = ToReasonFlags(values.FirstOrDefault(i => i.Type == 1));
            c = values.FirstOrDefault(i => i.Type == 2);
            if (c != null)
                {
                CRLIssuer = X509GeneralName.From((Asn1ContextSpecificObject)c[0]);
                }
            }

        private static ReasonFlags ToReasonFlags(Asn1ContextSpecificObject source) {
            if (source == null) { return ReasonFlags.Unused; }
            var r = source.Content.ToArray();
            return (ReasonFlags)r[0];
            }
        }
    }