<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:helix="urn:helix" xmlns:u="http://schemas.helix.global"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
  <xsl:output method="xml" indent="yes"/>
  <msxsl:script language="CSharp" implements-prefix="helix">
    <![CDATA[
    public string str(long value, string format) {
      return value.ToString(format);
      }
    ]]>
  </msxsl:script>
  <xsl:template match="/">
    <Section>
      <xsl:for-each select="/Section">
        <xsl:apply-templates select="."/>
      </xsl:for-each>
    </Section>
  </xsl:template>
  <xsl:template name="FormatLineNumbers">
    <xsl:param name="Offset"/>
    <TableRow>
      <xsl:for-each select="LineNumbers/LineInfo[(position() &gt;= $Offset) and (position() &lt; ($Offset + 4))]">
        <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,0,1,1" Padding="5,0,0,0">
          <Paragraph Margin="0,0,0,0">
            <Run><xsl:value-of select="helix:str(@LineNumber,'d5')"/></Run><Run Text="10" BaselineAlignment="Subscript" FontSize="7"/><Run>:</Run>
            <Run><xsl:value-of select="helix:str(@SegmentOffset,'x8')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/>
          </Paragraph>
        </TableCell>
      </xsl:for-each>
    </TableRow>
    <xsl:if test="($Offset + 4) &lt; count(LineNumbers/LineInfo)">
      <xsl:call-template name="FormatLineNumbers">
        <xsl:with-param name="Offset" select="$Offset + 4"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Section[@Type='SrcModule']">
    <Section>
      <Table BorderThickness="1,0,0,1" CellSpacing="0" Margin="0"
             BorderBrush="{{DynamicResource ControlDarkBrushKey}}"
             u:TextProperties.IsAutoSize="True">
        <Table.Columns>
          <TableColumn u:TextProperties.IsAutoSize="True"/>
          <TableColumn u:TextProperties.IsAutoSize="True"/>
        </Table.Columns>
        <TableRowGroup>
          <TableRow>
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,1,1,0" Padding="5,0,5,0" Background="{{DynamicResource ControlLightBrushKey}}">
              <Paragraph>Offset</Paragraph>
            </TableCell>
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,1,1,0" Padding="5,0,5,0">
              <Paragraph>
                <Run><xsl:value-of select="helix:str(@Offset,'x8')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/>
              </Paragraph>
            </TableCell>
          </TableRow>
          <TableRow>
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,1,1,0" Padding="5,0,5,0" Background="{{DynamicResource ControlLightBrushKey}}">
              <Paragraph>Size</Paragraph>
            </TableCell>
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,1,1,0" Padding="5,0,5,0">
              <Paragraph>
                <Run><xsl:value-of select="helix:str(@Size,'x8')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/>
                <Run Text=" {{"/><Run><xsl:value-of select="@Size"/></Run><Run Text="10" BaselineAlignment="Subscript" FontSize="7"/><Run>}</Run>
              </Paragraph>
            </TableCell>
          </TableRow>
        </TableRowGroup>
      </Table>
    </Section>
    <Section>
      <Paragraph Margin="0,5,0,5">Segments:</Paragraph>
      <Table CellSpacing="0" Margin="0" BorderThickness="0,0,0,1"
             BorderBrush="{{DynamicResource ControlDarkBrushKey}}"
             u:TextProperties.IsAutoSize="True">
        <Table.Columns>
          <TableColumn u:TextProperties.IsAutoSize="True"/>
        </Table.Columns>
        <TableRowGroup>
          <xsl:for-each select="Segments/SegmentInfo">
            <TableRow>
              <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,1,0" Padding="5,0,5,0">
                <Paragraph>
                  <Run><xsl:value-of select="helix:str(@Index,'x4')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/><Run>:</Run>
                  <Run><xsl:value-of select="helix:str(@Offset,'x8')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/><Run>-</Run>
                  <Run><xsl:value-of select="helix:str(@Offset+@Size+1,'x8')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/>
                  <Run Text=" {{"/>
                  <Run><xsl:value-of select="@Size"/></Run><Run Text="10" BaselineAlignment="Subscript" FontSize="7"/>
                  <Run>}</Run>
                </Paragraph>
              </TableCell>
            </TableRow>
          </xsl:for-each>
        </TableRowGroup>
      </Table>
      <Paragraph Margin="0,5,0,5">Files:</Paragraph>
      <Table CellSpacing="0" Margin="0" BorderThickness="0,0,0,1"
             BorderBrush="{{DynamicResource ControlDarkBrushKey}}"
             u:TextProperties.IsAutoSize="True">
        <Table.Columns>
          <TableColumn u:TextProperties.IsAutoSize="True"/>
          <TableColumn u:TextProperties.IsAutoSize="True"/>
        </Table.Columns>
        <TableRowGroup>
          <TableRow Background="{{DynamicResource ControlLightBrushKey}}">
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,0,0" Padding="5,0,5,0"><Paragraph>Name</Paragraph></TableCell>
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,1,0" Padding="5,0,5,0"><Paragraph>Offset</Paragraph></TableCell>
          </TableRow>
          <xsl:for-each select="Files/FileInfo">
            <TableRow>
              <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,0,0" Padding="5,0,5,0">
                <Paragraph><xsl:value-of select="@Name"/></Paragraph>
              </TableCell>
              <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,1,0" Padding="5,0,5,0">
                <Paragraph>
                  <Run><xsl:value-of select="helix:str(@Offset,'x8')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/>
                </Paragraph>
              </TableCell>
            </TableRow>
            <xsl:for-each select="SegmentInfo">
              <TableRow>
                <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,1,0" Padding="5,5,5,5" ColumnSpan="2">
                  <Table BorderBrush="{{DynamicResource ControlDarkBrushKey}}"
                         BorderThickness="1,1,0,0" CellSpacing="0" Margin="0"
                         u:TextProperties.IsAutoSize="True">
                    <Table.Columns>
                      <TableColumn u:TextProperties.IsAutoSize="True"/>
                      <TableColumn u:TextProperties.IsAutoSize="True"/>
                      <TableColumn u:TextProperties.IsAutoSize="True"/>
                      <TableColumn u:TextProperties.IsAutoSize="True"/>
                    </Table.Columns>
                    <TableRowGroup>
                      <xsl:call-template name="FormatLineNumbers">
                        <xsl:with-param name="Offset" select="1"/>
                      </xsl:call-template>
                    </TableRowGroup>
                  </Table>
                </TableCell>
              </TableRow>
            </xsl:for-each>
          </xsl:for-each>
        </TableRowGroup>
      </Table>
    </Section>
  </xsl:template>
</xsl:stylesheet>
