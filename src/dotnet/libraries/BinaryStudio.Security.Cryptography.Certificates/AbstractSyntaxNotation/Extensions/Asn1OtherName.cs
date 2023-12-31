﻿using System;
using System.Text;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    internal sealed class Asn1OtherName : Asn1GeneralNameObject
        {
        public Asn1ObjectIdentifier Type { get; }
        public Object Value { get; }
        public Asn1OtherName(Asn1ContextSpecificObject source)
            : base(source)
            {
            Type = (Asn1ObjectIdentifier)source[0];
            if (source[1] is Asn1ContextSpecificObject) {
                if (source[1].Count == 1) {
                    if (source[1][0] is Asn1OctetString) {
                        Value = source[1][0].Content.ToArray();
                        Value = Encoding.UTF8.GetString((Byte[])Value);
                        return;
                        }
                    if (source[1][0] is Asn1String value) {
                        Value = value.Value;
                        return;
                        }
                    }
                }
            Value = source[1].Content.ToArray();
            }

        public override Boolean IsEmpty { get {
            return (Value == null) ||
                   (Value is String r) &&
                   String.IsNullOrEmpty(r);
            }}

        public override String ToString()
            {
            return (Value != null)
                ? Value.ToString()
                : "{none}";
            }

        protected override X509GeneralNameType InternalType { get { return X509GeneralNameType.Other; }}
        }
    }