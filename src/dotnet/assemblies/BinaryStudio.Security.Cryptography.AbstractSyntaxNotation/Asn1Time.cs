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
        }
    }