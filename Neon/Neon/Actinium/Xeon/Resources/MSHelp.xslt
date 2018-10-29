<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema">
	<xsl:output version="1.0" encoding="utf-8" omit-xml-declaration="no" indent="no" media-type="text/html"/>
	<xsl:template match="/">
		<html>
			<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
			<head>
				<style>
				<![CDATA[


				body /* This body tag requires the use of one of the sets of banner and/or text div ids */
	{
	margin: 0px 0px 0px 0px;
	padding: 0px 0px 0px 0px;
	background: #ffffff; 
	color: #000000;
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 70%;
	width: 100%;
	}
div#scrollyes /* Allows topic to scroll with correct margins. Cannot be used with running head banner */
	{     /* Must immediately follow body. */
	padding: 2px 15px 2px 22px;
	width: 100%;
	}
div#nsbanner /* Creates Nonscrolling banner region */
	{
	position: relative;
	left: 0px;
	padding: 0px 0px 0px 0px;
	border-bottom: 1px solid #999999;
	}
div#nstext /* Creates the scrolling text area for Nonscrolling region topic */
	{
	padding: 5px 10px 0px 22px; 
	}
div#scrbanner /* Creates the running head bar in a full-scroll topic */
	{     /* Allows topic to scroll. */
	margin: 0px 0px 0px 0px;
	padding: 0px 0px 0px 0px;
	border-bottom: 1px solid #999999;
	}
div#scrtext /* Creates the text area in a full-scroll topic */
	{   /* Allows topic to scroll. */
	padding: 0px 10px 0px 22px; 
	}
div#bannerrow1 /* provides full-width color to top row in running head (requires script) */
	{
	background-color: #99ccff;
	}
div#titlerow /* provides non-scroll topic title area (requires script) */
	{
	width: 100%; /* Forces tables to have correct right margin */
	padding: 0px 10px 0px 22px; 
	background-color: #99ccff;        
	}

h1, h2, h3, h4
	{
	font-family: Verdana, Arial, Helvetica, sans-serif;
	margin-bottom: .4em; 
	margin-top: 1em;
	font-weight: bold;
	}
h1
	{
	font-size: 120%;
	margin-top: 0em;
	}
div#scrollyes h1 /* Changes font size for full-scrolling topic */
	{
	font-size: 150%;
	}
h2
	{
	font-size: 130%;
	}
h3
	{
	font-size: 115%;
	}
h4
	{
	font-size: 100%;
	}
.dtH1, .dtH2, .dtH3, .dtH4
	{
	margin-left: -1px;
	}
div#titlerow h1
	{
	margin-bottom: .2em
	}

table.bannerparthead, table.bannertitle /* General values for the Running Head tables */
	{
	position: relative;
	left: 0px;
	top: 0px;
	padding: 0px 0px 0px 0px;
	margin: 0px 0px 0px 0px;
	width: 100%;
	height: 21px; 
	border-collapse: collapse;
	border-style: solid;
	border-width: 0px;
	background-color: #99ccff; 
	font-size: 100%;
	}
table.bannerparthead td /* General Values for cells in the top row of running head */
	{
	margin: 0px 0px 0px 0px;
	padding: 2px 0px 0px 4px;
	vertical-align: middle;
	border-width: 0px;
	border-style: solid;
	border-color: #999999;
	background: transparent; 
	font-style: italic;
	font-weight: normal;
	}
table.bannerparthead td.product /* Values for top right cell in running head */
	{                       /* Allows for a second text block in the running head */
	text-align: right;
	padding: 2px 5px 0px 5px;
	}
table.bannertitle td /* General Values for cells in the bottom row of running head */
	{
	margin: 0px 0px 0px 0px;
	padding: 0px 0px 0px 3px;
	vertical-align: middle;
	border-width: 0px 0px 1px 0px;
	border-style: solid;
	border-color: #999999;
	background: transparent;
	font-weight: bold;
	}
td.button1 /* Values for button cells */
	{
	width: 14px;
	cursor: hand;
	}

p
	{
	margin: .5em 0em .5em 0em;
	}
blockquote.dtBlock
	{
	margin: .5em 1.5em .5em 1.5em;
	}
div#dtHoverText
	{
	color: #000066;
	}
.normal
	{
	margin: .5em 0em .5em 0em;
	font-size:8pt;
	}
.fineprint
	{
	font-size: 90%; /* 90% of 70% */
	}
.indent
	{
	margin: .5em 1.5em .5em 1.5em;
	}
.topicstatus /* Topic Status Boilerplate class */
	{
	display: block;
	color: red;
	}
p.label
	{
	margin-top: 1em;
	}
p.labelproc
	{
	margin-top: 1em;
	color: #000066;
	}

div.tablediv
	{
	width: 100%; /* Forces tables to have correct right margins and top spacing */
	margin-top: -.4em;
	}
ol div.tablediv, ul div.tablediv, ol div.HxLinkTable, ul div.HxLinkTable
	{
	margin-top: 0em; /* Forces tables to have correct right margins and top spacing */
	}
table.table.dtTABLE
	{
	width: 100%; /* Forces tables to have correct right margin */
	margin-top: .6em;
	margin-bottom: .3em;
	border-width: 1px 1px 0px 0px;
	border-style: solid;
	border-color: #999999;
	background-color: #999999; 
	font-size: 100%; /* Text in Table is same size as text outside table */
	width:80%;
	}
table.dtTABLE th, table.dtTABLE td
	{ 
	border-style: solid; /* Creates the cell border and color */
	border-width: 0px 0px 1px 1px;
	border-style: solid;
	border-color: #999999;
	padding: 4px 6px;
	text-align: left;
	}
table.dtTABLE th	
	{ 
	background: #cccccc; /* Creates the shaded table header row */
	vertical-align: bottom;
	}
table.dtTABLE td	
	{
	background: #ffffff;
	vertical-align: top;
	}

MSHelp\:ktable
	{
	disambiguator: span;
	separator: &nbsp;&#32;
	prefix: |&#32;
	postfix: &nbsp;
	filterString: ;
	}
div.HxLinkTable
	{
	width: auto; /* Forces tables to have correct right margins and top spacing */
	margin-top: -.4em;
	visibility: visible;
	}
ol div.HxLinkTable, ul div.HxLinkTable
	{
	margin-top: 0em; /* Forces tables to have correct right margins and top spacing */
	}
table.HxLinkTable /* Keep in sync with general table settings below */
	{
	width: auto;
	margin-top: 1.5em;
	margin-bottom: .3em;
	margin-left: -1em;
	border-width: 1px 1px 0px 0px;
	border-style: solid;
	border-color: #999999;
	background-color: #999999; 
	font-size: 100%; /* Text in Table is same size as text outside table */
	behavior:url(hxlinktable.htc); /* Attach the behavior to link elements. */
	}
table.HxLinkTable th, table.HxLinkTable td /* Keep in sync with general table settings below */
	{ 
	border-style: solid; /* Creates the cell border and color */
	border-width: 0px 0px 1px 1px;
	border-style: solid;
	border-color: #999999;
	padding: 4px 6px;
	text-align: left;
	}
table.HxLinkTable th /* Keep in sync with general table settings below */
	{ 
	background: #cccccc; /* Creates the shaded table header row */
	vertical-align: bottom;
	}
table.HxLinkTable td /* Keep in sync with general table settings below */
	{
	background: #ffffff;
	vertical-align: top;
	}

pre, div.syntax
	{
	margin-top: .5em;
	margin-bottom: .5em; 
	}
pre, code, .code, div.syntax
	{
	font: 100% Monospace, Courier New, Courier; /* This is 100% of 70% */
	color: #000066;
	}
pre b, code b
	{
	letter-spacing: .1em; /* opens kerning on bold in Syntax/Code */
	}
pre.syntax, div.syntax
	{
	background: #cccccc;
	padding: 4px 8px;
	cursor: text;
	margin-top: 1em;
	margin-bottom: 1em; 
	margin-left: 30px;
	color: #000000;
	border-width: 1px;
	border-style: solid;
	border-color: #999999;
	width:80%;
/* ------------------------------------- */
/* BEGIN changes to dtue.css conventions */
	font-weight: bolder;
	letter-spacing: .1em;
	}
.syntax span.lang
	{
	margin: 0px;
	font-weight: normal;
	}
.syntax span.meta
	{
	margin: 0;
	font-weight: normal;
	font-style: italic;
	}
.syntax a
	{
	margin: 0;
	font-weight: normal;
	}
/* END changes to dtue.css conventions */
/* ----------------------------------- */

.syntax div
	{
	padding-left: 24px;
	text-indent: -24px;
	}

.syntax .attribute
	{
		font-weight: normal;
	}
div.footer
	{
	font-style: italic;
	}
div.footer hr
	{
	color: #999999;
	height: 1px;
	}

ol, ul
	{
	margin: .5em 0em 0em 4em; 
	}
li
	{
	margin-bottom: .5em;
	}
ul p, ol p, dl p
	{
	margin-left: 0em;
	}
ul p.label, ol p.label
	{
	margin-top: .5em;
	}

dl
	{
	margin-top: 0em; 
	padding-left: 1px; /* Prevents italic-letter descenders from being cut off */
	}
dd
	{
	margin-bottom: 0em;  
	margin-left: 1.5em; 
	}
dt
	{
	margin-top: .5em;
	}

a:link
	{
	color: #0000ff;
	}
a:visited
	{
	color: #0000ff;
	}
a:hover
	{
	color: #3366ff;
	}

img
	{
	border: none; 
	}

/* Not in dtue.css. Used by NDoc's "ShowMissing..." options. */
.missing
	{
	color: Red;
	font-weight: bold;
	}
]]>
				</style>
			</head>
			<body bgcolor="#FFFFFF" class="dtBODY" id="bodyID">
				<div id="nsbanner">
				
					<table border="0" width="100%" class="bannerparthead" cellspacing="0">
						<tbody>
							<tr>
								<td class="runninghead">
									<xsl:for-each select="MSHelp">
										<xsl:for-each select="Head">
											<xsl:for-each select="runninghead">
												<xsl:apply-templates/>
											</xsl:for-each>
										</xsl:for-each>
									</xsl:for-each>
								</td>
								<td/>
								<td class="product">
									<xsl:for-each select="MSHelp">
										<xsl:for-each select="Head">
											<xsl:for-each select="product">
												<xsl:apply-templates/>
											</xsl:for-each>
										</xsl:for-each>
									</xsl:for-each>
								</td>
							</tr>
							<tr>
								<td>
									<h1 class="dtH1">
										<xsl:for-each select="MSHelp">
											<xsl:for-each select="Head">
												<xsl:for-each select="Title">
													<xsl:apply-templates/>
												</xsl:for-each>
											</xsl:for-each>
										</xsl:for-each>
									</h1>
								</td>
								<td/>
								<td/>
							</tr>
						</tbody>
					</table>
				</div>
				<table border="0" width="100%" >
					<tbody>
						<tr>
						<td width="10px"	>
						</td>
							<td class="normal"	>
				<p>
					<xsl:for-each select="MSHelp">
						<xsl:for-each select="nstext">
							<xsl:for-each select="preamble">
								<xsl:apply-templates/>
							</xsl:for-each>
						</xsl:for-each>
					</xsl:for-each>
				</p>
				<br/>
				<br/>
				<div class="normal">
				<xsl:for-each select="MSHelp">
					<xsl:for-each select="nstext">
						<xsl:for-each select="syntax">
							<div class="syntax">
								<span class="lang">
									<xsl:for-each select="language">
										<xsl:apply-templates/>
									</xsl:for-each>
								</span>
								<br/>
								<xsl:for-each select="definition">
									<xsl:apply-templates/>
								</xsl:for-each>
							</div>
							<br/>
						</xsl:for-each>
					</xsl:for-each>
				</xsl:for-each>
				<br/>
				<br/>
				<br/>
				<xsl:for-each select="MSHelp">
					<xsl:for-each select="nstext">
						<xsl:for-each select="blocks">
							<xsl:for-each select="block">
								<h4 class="dtH4">
									<xsl:for-each select="title">
										<xsl:apply-templates/>
									</xsl:for-each>
								</h4>
								<xsl:for-each select="members">
									<blockquote>
										<table class="dtTABLE">
											<thead>
												<tr>
													<td>type</td>
													<td>name</td>
													<td>description</td>
												</tr>
											</thead>
											<tbody>
												<xsl:for-each select="member">
													<tr>
														<td>
															<xsl:for-each select="type">
																<xsl:apply-templates/>
															</xsl:for-each>
														</td>
														<td>
															<xsl:for-each select="name">
																<xsl:apply-templates/>
															</xsl:for-each>
														</td>
														<td>
															<xsl:for-each select="description">
																<xsl:apply-templates/>
															</xsl:for-each>
														</td>
													</tr>
												</xsl:for-each>
											</tbody>
										</table>
									</blockquote>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</xsl:for-each>
				</xsl:for-each>
				<br/>
				<br/>
				<xsl:for-each select="MSHelp">
					<xsl:for-each select="Footer">
						<hr width="90%" align="left"		/>
						<div id="footer">
							<xsl:apply-templates/>
						</div>
					</xsl:for-each>
				</xsl:for-each>
				</div>
				</td>
						</tr>
					</tbody>
				</table>
			</body>
		</html>
	</xsl:template>
	<xsl:template match="a">
		<xsl:element name="a">
			<xsl:attribute name="href"><xsl:value-of select="@href"/></xsl:attribute>
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="br">
		<xsl:element name="br">        
    </xsl:element>
	</xsl:template>
</xsl:stylesheet>
