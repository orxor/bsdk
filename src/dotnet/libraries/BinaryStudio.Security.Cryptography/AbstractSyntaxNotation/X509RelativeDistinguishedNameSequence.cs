using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class X509RelativeDistinguishedNameSequence :
        ReadOnlyCollection<KeyValuePair<Asn1ObjectIdentifier,String>>,
        IJsonSerializable,IX509GeneralName
        {
        public X509RelativeDistinguishedNameSequence(IList<KeyValuePair<Asn1ObjectIdentifier,String>> source)
            : base(source)
            {
            }

        public X509RelativeDistinguishedNameSequence(IEnumerable<KeyValuePair<Asn1ObjectIdentifier,String>> source)
            : base(source.ToArray())
            {
            }

        public Object this[Asn1ObjectIdentifier key] { get {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            foreach (var item in Items) {
                if (item.Key.Equals(key)) { return item.Value; }
                }
            throw new ArgumentOutOfRangeException(nameof(key));
            }}

        public Object this[String key] { get {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            foreach (var item in Items) {
                if (item.Key.Equals(key)) {
                    return item.Value;
                    }
                }
            return null;
            }}

        #region M:ToString(Object):String
        internal static String ToString(Object source) {
            if (source == null) { return String.Empty; }
            if (source is String) {
                var value = (String)source;
                if (value.IndexOf("\"") != -1) {
                    return $"\"{value.Replace("\"", "\"\"")}\"";
                    }
                }
            return source.ToString();
            }
        #endregion
        #region M:ToString:String
        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return String.Join(", ", Items.Select(i => $"{new Oid(i.Key.ToString()).FriendlyName}={ToString(i.Value)}"));
            }
        #endregion
        #region M:Contains(String,Func<String,Boolean>):Boolean
        public Boolean Contains(String key, Func<String,Boolean> comparer) {
            if (comparer == null) { throw new ArgumentNullException(nameof(comparer)); }
            foreach (var item in Items) {
                if (item.Key.Equals(key)) {
                    return comparer.Invoke(ToString(item.Value));
                    }
                }
            return false;
            }
        #endregion
        #region M:TryGetValue(String,{out}Object):Boolean
        public Boolean TryGetValue(String key, out Object r) {
            r = null;
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            foreach (var item in Items) {
                if (item.Key.Equals(key)) {
                    r = item.Value;
                    return true;
                    }
                }
            return false;
            }
        #endregion
        #region M:Build(Asn1Object):X509RelativeDistinguishedNameSequence
        internal static X509RelativeDistinguishedNameSequence Build(Asn1Object source) {
            var r = new List<KeyValuePair<Asn1ObjectIdentifier,String>>();
            foreach (var i in source) {
                if (i.Count > 0) {
                    var j = i[0];
                    if (j.Count > 1) {
                        r.Add(new KeyValuePair<Asn1ObjectIdentifier,String>((Asn1ObjectIdentifier)
                            j[0],
                            j[1].ToString()));
                        continue;
                        }
                    if (j.Count == 1)
                        {
                        r.Add(new KeyValuePair<Asn1ObjectIdentifier,String>((Asn1ObjectIdentifier)
                            j[0],
                            String.Empty));
                        continue;
                        }
                    if (j is Asn1ObjectIdentifier) {
                        r.Add(new KeyValuePair<Asn1ObjectIdentifier,String>((Asn1ObjectIdentifier)
                            j,
                            i[1].ToString()));
                        }
                    }
                else
                    {
                    break;
                    }
                }
            return new X509RelativeDistinguishedNameSequence(r);
            }
        #endregion
        #region M:BuildSequence(X509RelativeDistinguishedNameSequence):Asn1Sequence
        internal static Asn1Sequence BuildSequence(X509RelativeDistinguishedNameSequence source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            return BuildSequence((IEnumerable<KeyValuePair<Asn1ObjectIdentifier,String>>)source);
            }
        #endregion
        #region M:BuildSequence(IEnumerable<KeyValuePair<Asn1ObjectIdentifier,String>>):Asn1Sequence
        internal static Asn1Sequence BuildSequence(IEnumerable<KeyValuePair<Asn1ObjectIdentifier,String>> source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            return new Asn1Sequence(source.
                Select(i => (Asn1Object)new Asn1Set(new Asn1Sequence(i.Key,new Asn1Utf8String(i.Value)))).
                ToArray());
            }
        #endregion
        #region M:BuildSequence():Asn1Sequence
        internal Asn1Sequence BuildSequence() {
            return BuildSequence(this);
            }
        #endregion

        Boolean IX509GeneralName.IsEmpty { get {
            return Count == 0;
            }}

        public X509GeneralNameType Type { get { return X509GeneralNameType.Directory; }}

        #region M:WriteTo(IJsonWriter)
        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                var c = Count;
                writer.WriteValue(nameof(Count),c);
                writer.WriteValue("(Self)", ToString());
                if (c > 0) {
                    writer.WritePropertyName("{Self}");
                    using (writer.Array()) {
                        var values = Items.ToArray();
                        var l = values.Select(j => j.Key.ToString().Length).Max();
                        var f = writer.Formatting;
                        writer.Formatting = Formatting.None;
                        foreach (var item in values) {
                            writer.Formatting = Formatting.Indented;
                            using (writer.Object()) {
                                writer.Formatting = Formatting.None;
                                writer.WriteRawString($@"""{item.Key}""{new String(' ',l-item.Key.ToString().Length)}: ""{item.Value}""");
                                }
                            }
                        writer.Formatting = f;
                        }
                    }
                }
            }
        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        void IX509GeneralName.WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue("Type", Type.ToString());
                writer.WriteValue("Value", this);
                }
            }
        #endregion
        }
    }