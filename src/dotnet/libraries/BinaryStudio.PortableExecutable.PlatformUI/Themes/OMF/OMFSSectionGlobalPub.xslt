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
  <xsl:template name="S_DATASYM16_16">
    <Table CellSpacing="0" Margin="0" u:TextProperties.IsAutoSize="True">
      <Table.Columns>
        <TableColumn u:TextProperties.SharedSizeGroup="SharedSize0"/>
        <TableColumn u:TextProperties.SharedSizeGroup="SharedSize1"/>
      </Table.Columns>
      <TableRowGroup>
        <TableRow>
          <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,0,1,1" Padding="5,0,5,0">
            <Paragraph>TypeIndex</Paragraph>
          </TableCell>
          <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,0,0,1" Padding="5,0,5,0">
            <Paragraph>
              <Run><xsl:value-of select="helix:str(@TypeIndex,'x4')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/>
            </Paragraph>
          </TableCell>
        </TableRow>
        <TableRow>
          <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,0,1,1" Padding="5,0,5,0">
            <Paragraph>Name</Paragraph>
          </TableCell>
          <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,0,0,1" Padding="5,0,5,0">
            <Paragraph><xsl:value-of select="@Name"/></Paragraph>
          </TableCell>
        </TableRow>
        <TableRow>
          <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,0,1,0" Padding="5,0,5,0">
            <Paragraph>Location</Paragraph>
          </TableCell>
          <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="0,0,0,0" Padding="5,0,5,0">
            <Paragraph>
              <Run><xsl:value-of select="helix:str(@SegmentIndex,'x4')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/>:<Run><xsl:value-of select="helix:str(@SymbolOffset,'x4')"/></Run><Run Text="16" BaselineAlignment="Subscript" FontSize="7"/>
            </Paragraph>
          </TableCell>
        </TableRow>
      </TableRowGroup>
    </Table>
  </xsl:template>
  <xsl:template match="SymbolInfo[@Type='S_PUB16']">
    <xsl:call-template name="S_DATASYM16_16"/>
  </xsl:template>
  <xsl:template match="SymbolInfo">
    <Paragraph Foreground="{{DynamicResource AccentRBrushKey}}"><xsl:value-of select="@Type"/></Paragraph>
  </xsl:template>
  <xsl:template match="Section[@Type='GlobalPub']">
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
      <Paragraph Margin="0,5,0,5">Symbols:</Paragraph>
      <Table CellSpacing="0" Margin="0" BorderThickness="0,0,0,1"
             BorderBrush="{{DynamicResource ControlDarkBrushKey}}"
             u:TextProperties.IsAutoSize="True"
             u:TextProperties.IsSharedSizeScope="True">
        <Table.Columns>
          <TableColumn u:TextProperties.IsAutoSize="True"/>
          <TableColumn u:TextProperties.IsAutoSize="True"/>
          <TableColumn u:TextProperties.SharedSizeGroup="SharedSize0"/>
          <TableColumn u:TextProperties.SharedSizeGroup="SharedSize1"/>
        </Table.Columns>
        <TableRowGroup>
          <TableRow Background="{{DynamicResource ControlLightBrushKey}}">
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,0,0" Padding="5,0,5,0">
              <Paragraph>Offset</Paragraph>
            </TableCell>
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,0,0" Padding="5,0,5,0">
              <Paragraph>Symbol</Paragraph>
            </TableCell>
            <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,1,0" Padding="5,0,5,0" ColumnSpan="2">
              <Paragraph>Details</Paragraph>
            </TableCell>
          </TableRow>
          <xsl:for-each select="Symbols/SymbolInfo">
            <TableRow>
              <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,0,0" Padding="5,0,5,0">
                <Paragraph><xsl:value-of select="helix:str(@Offset,'x8')"/></Paragraph>
              </TableCell>
              <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,0,0" Padding="5,0,5,0">
                <Paragraph><xsl:value-of select="@Type"/></Paragraph>
              </TableCell>
              <TableCell BorderBrush="{{DynamicResource ControlDarkBrushKey}}" BorderThickness="1,1,1,0" Padding="0" ColumnSpan="2">
                <xsl:apply-templates select="."/>
              </TableCell>
            </TableRow>
          </xsl:for-each>
        </TableRowGroup>
      </Table>
    </Section>
  </xsl:template>
</xsl:stylesheet>
