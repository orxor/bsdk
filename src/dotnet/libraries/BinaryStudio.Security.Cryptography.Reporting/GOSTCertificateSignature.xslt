<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <meta content="text/html; charset=utf-8" http-equiv="Content-Type"/>
        <title>GOSTCertificateSignature</title>
        <style type="text/css">
          td
            {
            border: solid lightgray 1.0pt;
            mso-border-alt: solid windowtext .5pt;
            padding: 0cm 5.4pt 0cm 5.4pt;
            }
         </style>
      </head>
      <body>
        <table style="font-family: Consolas;border-collapse:collapSe;border:none;mso-border-alt:solid windowtext .5pt;mso-padding-alt:0cm 5.4pt 0cm 5.4pt; background-color: white;">
          <xsl:for-each select="/Report/Page[@Name='Certificates']/Chain">
            <xsl:apply-templates select="."/>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="Chain">
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>NotBefore</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="@NotBefore"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>NotAfter</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="@NotAfter"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>SerialNumber</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="@SerialNumber"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>Subject</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="@Subject"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>Issuer</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="@Issuer"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>SKI</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="SKI/@KeyIdentifier"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>AKI</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="AKI/@KeyIdentifier"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>AKI{SerialNumber}</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="AKI/@SerialNumber"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td>AKI{Issuer}</td>
      <xsl:for-each select="Certificate">
        <td>
          <xsl:value-of select="AKI/@CertificateIssuer"/>
        </td>
      </xsl:for-each>
    </tr>
    <tr xmlns="http://www.w3.org/1999/xhtml">
      <td style="border: solid gray 1.0pt;"/>
      <xsl:for-each select="Certificate">
        <td style="border: solid gray 1.0pt;"/>
      </xsl:for-each>
    </tr>
  </xsl:template>
    
  <!--<xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>-->
</xsl:stylesheet>
