<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
														xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
														xmlns:request="urn:user-request"
>
<xsl:output method="html" indent="yes"/>

<xsl:template match="/">
<html>
<body style="background: White;">
	<a href="javascript:history.back();">back</a>
	<br/>
	<img src="/MyComputer/{request:URLEncode(request:getParameter('path'))}" alt="{request:getParameter('path')}"/>
</body>
</html>
</xsl:template>

</xsl:stylesheet>
