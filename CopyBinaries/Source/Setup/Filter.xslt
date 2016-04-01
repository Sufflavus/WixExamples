<?xml version="1.0" encoding="UTF-8"?>
<!--This will remove all pdb, xml, dll.config, vshost.exe and the WIXDemo.exe file from the Binaries file. -->

<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:wix="http://schemas.microsoft.com/wix/2006/wi">
  <xsl:strip-space elements="*"/> <!--remove any white-space-only text nodes-->
  <xsl:output method="xml" indent="yes" />
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>
  <xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.pdb')]" use="@Id" />
  <xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.xml')]" use="@Id" />
  <xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.dll.config')]" use="@Id" />
  <xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, '.vshost.exe')]" use="@Id" />
  <xsl:key name="service-search" match="wix:Component[contains(wix:File/@Source, 'app.confing')]" use="@Id" />
  <xsl:template match="wix:Component[key('service-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('service-search', @Id)]" />
</xsl:stylesheet>