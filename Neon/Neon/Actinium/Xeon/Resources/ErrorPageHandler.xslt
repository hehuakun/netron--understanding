<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes"/>
	
	<xsl:template match="/">
		<a href="javascript:history.back()">back</a>
		<br/>
		<font size="3">
		This is a custom error page
		</font>
		<br/>
		Exception information:
		<br/>
		
		<xsl:for-each select="page/exceptions/exception">
			<xsl:value-of select="message"/>
			<br/>
			<pre><xsl:value-of select="stack-trace"/>	</pre>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
  