using System;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    public class DistributionPoint : Asn1LinkObject
        {
        private Boolean IsLinked;
        public DistributionPointName Point { get; }
        public ReasonFlags ReasonFlags { get; }
        public IX509GeneralName CRLIssuer { get; }

        #region ctor{Asn1Sequence}
        internal DistributionPoint(Asn1Object source)
            : base(source)
            {
            IsLinked = true;
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
        #endregion
        #region ctor{Uri}
        public DistributionPoint(Uri url)
            :base(new Asn1PrivateObject(0))
            {
            IsLinked = false;
            ReasonFlags = ReasonFlags.Unused;
            Point = new DistributionPointName(
                new Asn1ContextSpecificObject(0,
                new Asn1ContextSpecificObject(0,
                new Asn1ContextSpecificObject(6,Encoding.UTF8.GetBytes(url.AbsoluteUri)))));
            }
        #endregion

        public override void WriteTo(Stream target, Boolean force = false) {
            if (IsLinked) {
                base.WriteTo(target,force);
                return;
                }
            var r = new Asn1Sequence {
                IsExplicitConstructed = true
                };
            r.Add(Point);
            r.WriteTo(target, true);
            }

        private static ReasonFlags ToReasonFlags(Asn1ContextSpecificObject source) {
            if (source == null) { return ReasonFlags.Unused; }
            var r = source.Content.ToArray();
            return (ReasonFlags)r[0];
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (ReasonFlags != ReasonFlags.Unused) {
                using (writer.Object()) {
                    writer.WriteValue(nameof(ReasonFlags), ReasonFlags.ToString());
                    if (Point != null) {
                        writer.WriteValue("DistributionPoint", Point);
                        }
                    }
                }
            else
                {
                if (Point != null)
                    {
                    Point.WriteTo(writer);
                    }
                }
            }
        }
    }