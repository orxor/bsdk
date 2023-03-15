using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using BinaryStudio.IO;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a date-time types:
    /// <table style="font-family: Consolas;width:100%;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="UTCTIME"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1UtcTime"/>
    ///     </td>
    ///   </tr>
    ///   <tr>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see langword="GENERALIZEDTIME"/>
    ///     </td>
    ///     <td style="border:solid windowtext 1.0pt;mso-border-alt:solid windowtext .5pt;padding:0cm 5.4pt 0cm 5.4pt;">
    ///       <see cref="Asn1GeneralTime"/>
    ///     </td>
    ///   </tr>
    /// </table>
    /// </summary>
    public abstract class Asn1Time : Asn1UniversalObject
        {
        public abstract DateTimeKind Kind { get; }
        public DateTimeOffset Value { get;protected set; }

        #region ctor
        protected Asn1Time()
            {
            }
        #endregion
        #region ctor{DateTime}
        protected Asn1Time(DateTime source)
            {
            Value = new DateTimeOffset(source);
            State |= ObjectState.Decoded;
            }
        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            var Toffset = Value.Offset;
            var Boffset = TimeZoneInfo.Local.BaseUtcOffset;
            if (Toffset == Boffset) {
                var LocalTime = Value.LocalDateTime;
                return (LocalTime.Millisecond > 0)
                    ? LocalTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffff")
                    : LocalTime.ToString("yyyy-MM-ddTHH:mm:ss");
                }
            if (Toffset == TimeSpan.Zero) {
                var LocalTime = Value.LocalDateTime;
                return (LocalTime.Millisecond > 0)
                    ? LocalTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ")
                    : LocalTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
            else
                {
                var LocalTime = Value.LocalDateTime;
                #if NET35
                var TimeZone = (Toffset > TimeSpan.Zero)
                    ? ("+" + Toffset.ToString())
                    : ("-" + Toffset.ToString());
                #else
                var TimeZone = (Toffset > TimeSpan.Zero)
                    ? ("+" + Toffset.ToString(@"hh\:mm"))
                    : ("-" + Toffset.ToString(@"hh\:mm"));
                #endif
                return String.Format("{0}{1}",
                    (LocalTime.Millisecond > 0)
                        ? LocalTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffff")
                        : LocalTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    TimeZone);
                }
            }

        public static implicit operator DateTime(Asn1Time source)
            {
            return source.Value.LocalDateTime;
            }

        private static String ToString(Group value) {
            return value.Success
                ? value.Value
                : null;
            }

        #region M:Parse(String,Asn1ObjectType):DateTimeOffset?
        protected static DateTimeOffset? Parse(String value, Asn1ObjectType type) {
            #if NET35
            if (String.IsNullOrEmpty(value)) { return null; }
            #else
            if (String.IsNullOrWhiteSpace(value)) { return null; }
            #endif
            if (type == Asn1ObjectType.UtcTime)
                {
                /*
                 * yyMMddHH[mm[ss[.fff]]]Z
                 * yyMMddHH[mm[ss[.fff]]]
                 * yyMMddHH[mm[ss[.fff]]]+-HHmm
                 */
                var r = Regex.Match(value, @"^(\d{8})(?:(\d{2})(?:(\d{2})?(?:[.,](\d+))?)?)?(Z|(?:[+-]\d{4})|)$");
                if (r.Success) {
                    var gt = ToString(r.Groups[1]);
                    var mm = ToString(r.Groups[2]) ?? "00";
                    var ss = ToString(r.Groups[3]) ?? "00";
                    var ff = ToString(r.Groups[4]) ?? "0";
                    var tz = ToString(r.Groups[5]) ?? "Z";
                    var o = DateTime.SpecifyKind(DateTime.ParseExact(gt, "yyMMddHH", CultureInfo.CurrentCulture), DateTimeKind.Unspecified);
                    if (o.Year < 2000) { o = o.AddYears(100); }
                    o = o.Add(TimeSpan.Parse($"00:{mm}:{ss}.{ff}"));
                    if ((tz != String.Empty) && (tz != "Z")) {
                        var timespan = new TimeSpan(
                            Int32.Parse(tz.Substring(1,2)),
                            Int32.Parse(tz.Substring(3,2)),0);
                        return (new DateTimeOffset(DateTime.SpecifyKind(o,DateTimeKind.Unspecified))).ToOffset(
                            (tz[0] == '-')
                            ? -timespan
                            : +timespan);
                        }
                    return (new DateTimeOffset(DateTime.SpecifyKind(o,DateTimeKind.Unspecified))).ToOffset(TimeSpan.Zero);
                    }
                }
            else
                {
                /*
                 * yyyyMMddHH[mm[ss[.fff]]]Z
                 * yyyyMMddHH[mm[ss[.fff]]]
                 * yyyyMMddHH[mm[ss[.fff]]]+-HHmm
                 */
                var r = Regex.Match(value, @"^(\d{10})(?:(\d{2})(?:(\d{2})?(?:[.,](\d+))?)?)?(Z|(?:[+-]\d{4})|)$");
                if (r.Success) {
                    var gt = ToString(r.Groups[1]);
                    var mm = ToString(r.Groups[2]) ?? "00";
                    var ss = ToString(r.Groups[3]) ?? "00";
                    var ff = ToString(r.Groups[4]) ?? "0";
                    var tz = ToString(r.Groups[5]);
                    var o = DateTime.SpecifyKind(DateTime.ParseExact(gt, "yyyyMMddHH", CultureInfo.CurrentCulture), DateTimeKind.Local);
                    o = o.Add(TimeSpan.Parse($"00:{mm}:{ss}.{ff}"));
                    if ((tz != String.Empty) && (tz != "Z")) {
                        var timespan = new TimeSpan(
                            Int32.Parse(tz.Substring(1,2)),
                            Int32.Parse(tz.Substring(3,2)),0);
                        return (new DateTimeOffset(DateTime.SpecifyKind(o,DateTimeKind.Unspecified))).ToOffset(
                            (tz[0] == '-')
                            ? -timespan
                            : +timespan);
                        }
                    if (tz == "Z")
                        {
                        return (new DateTimeOffset(DateTime.SpecifyKind(o,DateTimeKind.Unspecified))).ToOffset(TimeSpan.Zero);
                        }
                    return new DateTimeOffset(o).ToOffset(TimeZoneInfo.Local.BaseUtcOffset);
                    }
                }
            return null;
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Type), TypeCode);
                if (Offset >= 0) { writer.WriteValue(nameof(Offset), Offset); }
                writer.WriteValue(nameof(Value), ToString());
                }
            }
        #endregion
        }
    }