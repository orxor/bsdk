using System;
using System.Text;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a string types:
    /// <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="GENERALSTRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1GeneralString"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="GRAPHICSTRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1GraphicString"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="IA5STRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1IA5String"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="NUMERICSTRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1NumericString"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="PRINTABLESTRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1PrintableString"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="T61STRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1TeletexString"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="BMPSTRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1UnicodeString"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="UNIVERSALSTRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1UniversalString"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="UTF8STRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1Utf8String"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="VIDEOTEXSTRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1VideotexString"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="VISIBLESTRING"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1VisibleString"/>
    ///     </td>
    ///   </tr>
    /// </table>
    /// </summary>
    public abstract class Asn1String : Asn1UniversalObject
        {
        public abstract Encoding Encoding { get; }
        public String Value { get;protected set; }

        #region M:ToString:String
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return Value;
            }
        #endregion
        #region M:Decode:Boolean
        protected override Boolean Decode()
            {
            if (IsDecoded) { return true; }
            if (IsIndefiniteLength) { return base.Decode(); }
            var r = new Byte[Length];
            Content.Read(r, 0, r.Length);
            Value = Encoding.GetString(r);
            State |= ObjectState.Decoded;
            return true;
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Type), TypeCode);
                if (Offset >= 0) { writer.WriteValue(nameof(Offset), Offset); }
                writer.WriteValue(nameof(Value), Value);
                }
            }
        #endregion

        public static implicit operator String(Asn1String source) { return source.Value; }
        }
    }