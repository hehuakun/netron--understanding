<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:request="urn:user-request">
	<xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:variable name="lastString" select="request:getParameter('lastString')"/>
	<xsl:template match="/">
		<xsl:variable name="depth">
			<xsl:choose>
				<xsl:when test="request:getParameter('depth') = ''">0</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="request:getParameter('depth')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<html>
			<head>
				<link href="css/TreePane.css" rel="stylesheet" type="text/css"/>
			</head>
			<body onload="bodyLoad();" style="width:100%;">
				<div id="MainDiv" style="width:100px">
					<form name="dirData">
						<input type="hidden" value="{request:getParameter('currentNode')}" id="idxRoot"/>
						<xsl:for-each select="page/objects/directory">
							<input type="hidden" value="{@name}" id="idx{position()}"/>
						</xsl:for-each>
					</form>
					<xsl:for-each select="page/objects/directory">
						<div nowrap="true" id="d{position()}" class="rowDiv">
							<span style="width:{16 * $depth}px; height:16px"/>
							<xsl:choose>
								<xsl:when test="@hasChildren = 'true'">
									<a id="aTreeCtrl" href="javascript:LoadData(d{position()})">
										<img id="iSignCtrl" src="Resources/plus.ico" border="0" align="middle"/>
									</a>
								</xsl:when>
								<xsl:otherwise>
									<img src="Resources/empty.ico" border="0" align="middle"/>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:variable name="full-path">
								<xsl:choose>
									<xsl:when test="request:getParameter('currentNode') = ''">
										<xsl:value-of select="@name"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="request:getParameter('currentNode')"/>
										<xsl:text>\</xsl:text>
										<xsl:value-of select="@name"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<a id="aNode" href="FilePane.xsp?path={request:URLEncode(string($full-path))}" target="FilePane">
								<xsl:value-of select="@name"/>
							</a>
						</div>
					</xsl:for-each>
				</div>
				<style>
				body
{
	padding-right: 0px;
	padding-left: 0px;
	font-size: 10pt;
	padding-bottom: 0px;
	margin: 0px;
	border-top-style: none;
	padding-top: 0px;
	font-family: Tahoma;
	border-right-style: none;
	border-left-style: none;
	border-bottom-style: none;
	background: White;
}

a
{
	font-size: 9pt;
	font-family: Verdana;
	text-decoration: none;
}

.TreeImages
{
	position: relative;
}

.rowDiv
{
	font-size: 10pt;
	font-family: Tahoma;
	position: relative;
	height: 16px;
}
.simg
{
	background-color: Yellow;
}
				</style>
				<script language="javascript">
				function locateParentIFrame()
				{
					<xsl:choose>
						<xsl:when test="request:getParameter('parIFrame') = ''">
							return null;
						</xsl:when>
						<xsl:otherwise>
							return parent.d<xsl:value-of select="request:getParameter('parIFrame')"/>.all("ifrm");
						</xsl:otherwise>
					</xsl:choose>
				}
				var depth = <xsl:value-of select="$depth"/>;
				var nodeCount = <xsl:value-of select="count(page/objects/directory)"/>;
				
				<![CDATA[
				function isInnerFrameLoaded(obj)
				{
					try
					{
						var tmp = obj.all('ifrm');
						return tmp != null;
					}
					catch(e)
					{
						return false;
					}
				}
				
				function LoadData(obj)
				{
					if(isInnerFrameLoaded(obj))
						return;
					
					var sName, sNumber;
					sName = obj.id.substring(1);
					sNumber = sName;
					sName = dirData.all("idx" + sName).value;
					if(dirData.idxRoot.value != "")
						sName = dirData.idxRoot.value + "\\" + sName;
						
					var sNewHTML;
					sNewHTML = "<br id='br'/><iframe id='ifrm' style='height:5px;width=5px' scrolling='no'";
					sNewHTML += " frameborder='0'";
					sNewHTML += " src=\"DirectoryTree.xsp?currentNode=";
					sNewHTML += encodeURI(sName);
					sNewHTML += "&parIFrame=" + encodeURI(sNumber);
					sNewHTML += "&depth=" + encodeURI(depth + 1);
					sNewHTML += "\"></iframe>";
					obj.innerHTML += sNewHTML;
					
					obj.all("aTreeCtrl").href = "javascript:UnloadData(" + obj.id + ")";
					obj.all("iSignCtrl").src = "Resources/minus.ico";

					fixSizes();
				}
				
				
				function UnloadData(obj)
				{
					if(!isInnerFrameLoaded(obj))
						return;
						
					//alert(obj.all("ifrm"));
					obj.removeChild(obj.all("ifrm"));
					obj.removeChild(obj.all("br"));
					
					obj.all("aTreeCtrl").href = "javascript:LoadData(" + obj.id + ")";
					obj.all("iSignCtrl").src = "Resources/plus.ico";
					
					fixSizes();
				}
				
				
				function fixSizes()
				{
					if(window == window.top)
						return;
						
					var realHeight = MainDiv.clientHeight;
					var currentHeight = document.body.clientHeight;
					var deltaHeight = realHeight - currentHeight;

					var realWidth = MainDiv.clientWidth;
					var currentWidth = document.body.clientWidth;
					var deltaWidth = realWidth - currentWidth;
					
					try
					{
						var curFrame = locateParentIFrame();
					
						curFrame.style.pixelHeight += deltaHeight;
						curFrame.style.pixelWidth += deltaWidth;
					
						window.parent.fixSizes();
					}
					catch(e)
					{}
				}

				function bodyLoad()
				{
					fixSizes();
				}
				]]></script>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
