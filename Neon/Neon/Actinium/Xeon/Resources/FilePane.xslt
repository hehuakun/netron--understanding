<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:request="urn:user-request"
>
<xsl:output method="html" indent="yes"/>

<xsl:template match="/">
<html>
<body style="background: White;">

	<xsl:for-each select="page/objects/picture">
		<a href="ShowPicture.xsp?path={request:URLEncode(@path)}">
			<img border="0" src="ImageCropper.xsp?path={request:URLEncode(@path)}&amp;width=80&amp;height=80" alt="{@path}" />
		</a>
		<xsl:if test="position() mod 5 = 0">
			<br/>
		</xsl:if>
	</xsl:for-each>
	
</body>	
</html>
</xsl:template>

</xsl:stylesheet>
