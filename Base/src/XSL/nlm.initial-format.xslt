﻿<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
  xmlns:mml="http://www.w3.org/1998/Math/MathML" xmlns:xlink="http://www.w3.org/1999/xlink"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:tp="http://www.plazi.org/taxpub">

	<xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="no" indent="no"/>
	<xsl:preserve-space elements="*"/>
	<xsl:strip-space elements="addr-line aff"/>

	<xsl:template match="@* | node()" mode="strip-label">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="strip-label"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="label | title" mode="strip-label"/>

	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="sec/@sec-type">
		<xsl:variable name="lower-case" select="translate(string(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')"/>
		<xsl:attribute name="{name()}">
			<xsl:choose>
				<xsl:when test="contains($lower-case, 'material') and contains($lower-case, 'method')">
					<xsl:text>materials|methods</xsl:text>
				</xsl:when>
				<xsl:when test="contains($lower-case, 'method')">
					<xsl:text>methods</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="len" select="string-length($lower-case)"/>
					<xsl:variable name="last-char" select="substring($lower-case, $len - 1, 1)"/>
					<xsl:choose>
						<xsl:when test="$last-char = '.' or $last-char = ',' or $last-char = ';' or $last-char = ';'">
							<xsl:value-of select="substring(string(), 1, $len - 1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="normalize-space()"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>

	<xsl:template match="tp:treatment-sec/@sec-type">
		<xsl:variable name="lower-case" select="translate(string(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')"/>
		<xsl:attribute name="{name()}">
			<xsl:choose>
				<xsl:when test="contains($lower-case, 'material') and contains($lower-case, 'type')">
					<xsl:text>type material</xsl:text>
				</xsl:when>
				<xsl:when test="contains($lower-case, 'material')">
					<xsl:text>material</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="len" select="string-length($lower-case)"/>
					<xsl:variable name="last-char" select="substring($lower-case, $len - 1, 1)"/>
					<xsl:choose>
						<xsl:when test="$last-char = '.' or $last-char = ',' or $last-char = ';' or $last-char = ';'">
							<xsl:value-of select="substring(string(), 1, $len - 1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="normalize-space()"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>

	<xsl:template match="em | i | italic">
		<i>
			<xsl:apply-templates/>
		</i>
	</xsl:template>

	<xsl:template match="strong | b | bold">
		<b>
			<xsl:apply-templates />
		</b>
	</xsl:template>

	<xsl:template match="bold-italic | Bold-Italic">
		<b>
			<i>
				<xsl:apply-templates />
			</i>
		</b>
	</xsl:template>

	<xsl:template match="u | underline">
		<u>
			<xsl:apply-templates />
		</u>
	</xsl:template>

	<xsl:template match="strike | s">
		<s>
			<xsl:apply-templates/>
		</s>
	</xsl:template>

	<xsl:template match="sup">
		<sup>
			<xsl:apply-templates/>
		</sup>
	</xsl:template>

	<xsl:template match="sub">
		<sub>
			<xsl:apply-templates/>
		</sub>
	</xsl:template>

	<xsl:template match="span | div | fake_tag">
		<xsl:apply-templates />
	</xsl:template>

	<xsl:template match="alt-title | article-title | product | subtitle | title | label | trans-subtitle | trans-title">
		<xsl:apply-templates select="." mode="title"/>
	</xsl:template>

	<!--
    <xsl:template match="article-meta/aff">
        <xsl:element name="{name()}">
            <xsl:apply-templates select="label"/>
            <addr-line>
                <xsl:apply-templates mode="strip-label"/>
            </addr-line>
        </xsl:element>
    </xsl:template>
-->

	<!-- Title modes -->
	<xsl:template match="@* | node()" mode="title">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="title"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="em | i | italic" mode="title">
		<i>
			<xsl:apply-templates mode="title"/>
		</i>
	</xsl:template>

	<xsl:template match="strong | b | bold" mode="title">
		<xsl:apply-templates mode="title"/>
	</xsl:template>

	<xsl:template match="bold-italic | Bold-Italic" mode="title">
		<i>
			<xsl:apply-templates mode="title"/>
		</i>
	</xsl:template>

	<xsl:template match="u | underline" mode="title">
		<u>
			<xsl:apply-templates mode="title"/>
		</u>
	</xsl:template>

	<xsl:template match="strike | s" mode="title">
		<s>
			<xsl:apply-templates mode="title"/>
		</s>
	</xsl:template>

	<xsl:template match="sup" mode="title">
		<sup>
			<xsl:apply-templates mode="title"/>
		</sup>
	</xsl:template>

	<xsl:template match="sub" mode="title">
		<sub>
			<xsl:apply-templates mode="title"/>
		</sub>
	</xsl:template>

	<xsl:template match="span | div" mode="title">
		<xsl:apply-templates mode="title"/>
	</xsl:template>

	<xsl:template match="p" mode="title">
		<xsl:if test="position() != 1">
			<break/>
		</xsl:if>
		<xsl:apply-templates mode="title"/>
	</xsl:template>

	<!-- Table modes -->
	<xsl:template match="table-wrap">
		<xsl:element name="{name()}">
			<xsl:if test="string(table/@id)!=''">
				<xsl:attribute name="id">
					<xsl:value-of select="table/@id"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="@*"/>
			<xsl:attribute name="position">
				<xsl:text>float</xsl:text>
			</xsl:attribute>
			<xsl:if test="string(@content-type)='key'">
				<xsl:attribute name="position">
					<xsl:text>anchor</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="orientation">
				<xsl:text>portrait</xsl:text>
			</xsl:attribute>
			<xsl:apply-templates/>
		</xsl:element>
	</xsl:template>

	<xsl:template match="table">
		<xsl:element name="{name()}">
			<xsl:attribute name="id">
				<xsl:text>T</xsl:text>
				<xsl:value-of select="generate-id()"/>
			</xsl:attribute>
			<xsl:apply-templates select="@border | @cellpadding | @cellspacing | @content-type | @frame | @style | @summary | @width"/>
			<xsl:attribute name="rules">
				<xsl:text>all</xsl:text>
			</xsl:attribute>
			<xsl:apply-templates/>
		</xsl:element>
	</xsl:template>

	<xsl:template match="td | th">
		<xsl:element name="{name()}">
			<xsl:apply-templates select="@abbr | @align | @axis | @char | @charoff | @colspan | @content-type | @headers | @id | @rowspan | @scope | @style | @valign"/>
			<xsl:if test="string(@rowspan)=''">
				<xsl:attribute name="rowspan">
					<xsl:text>1</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="string(@colspan)=''">
				<xsl:attribute name="colspan">
					<xsl:text>1</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="count(text()) = count(node())">
					<xsl:value-of select="normalize-space()"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>

	<xsl:template match="th/b | th/bold | th/Bold">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="th/bold-italic | th/Bold-Italic">
		<i>
			<xsl:apply-templates/>
		</i>
	</xsl:template>

	<xsl:template match="td/p | th/p">
		<xsl:if test="count(preceding-sibling::p) != 0">
			<break/>
		</xsl:if>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ref/text()[name()=''][normalize-space()='']"></xsl:template>

	<xsl:template match="kwd/text()[name()=''][normalize-space()='']"></xsl:template>

	<xsl:template match="label[count(text()) = count(node())] | title[count(text()) = count(node())] | p[local-name(..)!='td' and local-name(..)!='th'][count(text()) = count(node())] | license-p[count(text()) = count(node())] | li[count(text()) = count(node())] | attrib[count(text()) = count(node())] | element-citation[count(text()) = count(node())] | nlm-citation[count(text()) = count(node())] | mixed-citation[count(text()) = count(node())] | object-id[count(text()) = count(node())] | xref-group[count(text()) = count(node())] | kwd[count(text()) = count(node())] | tp:nomenclature-citation[count(text()) = count(node())] | article-title[count(text()) = count(node())] | self-uri[count(text()) = count(node())] | given-names[count(text()) = count(node())] | surname[count(text()) = count(node())]">
		<xsl:element name="{name()}">
			<xsl:apply-templates select="@*"/>
			<xsl:value-of select="normalize-space()"/>
		</xsl:element>
	</xsl:template>

	<!-- Replace white-space-tags with pure white space -->
	<xsl:template match="i[normalize-space() = ''][string() != ''][not(comment())] | italic[normalize-space() = ''][string() != ''][not(comment())] | Italic[normalize-space() = ''][string() != ''][not(comment())] | em[normalize-space() = ''][string() != ''][not(comment())] | b[normalize-space() = ''][string() != ''][not(comment())] | bold[normalize-space() = ''][string() != ''][not(comment())] | Bold[normalize-space() = ''][string() != ''][not(comment())] | strong[normalize-space() = ''][string() != ''][not(comment())] | bold-italic[normalize-space() = ''][string() != ''][not(comment())] | Bold-Italic[normalize-space() = ''][string() != ''][not(comment())] | sup[normalize-space() = ''][string() != ''][not(comment())] | sub[normalize-space() = ''][string() != ''][not(comment())] | u[normalize-space() = ''][string() != ''][not(comment())] | underline[normalize-space() = ''][string() != ''][not(comment())] | s[normalize-space() = ''][string() != ''][not(comment())] | strike[normalize-space() = ''][string() != ''][not(comment())]">
		<xsl:text> </xsl:text>
	</xsl:template>

	<!-- Remove empty tags -->
	<xsl:template match="i[normalize-space() = ''][string() = ''][not(comment())] | italic[normalize-space() = ''][string() = ''][not(comment())] | Italic[normalize-space() = ''][string() = ''][not(comment())] | em[normalize-space() = ''][string() = ''][not(comment())] | b[normalize-space() = ''][string() = ''][not(comment())] | bold[normalize-space() = ''][string() = ''][not(comment())] | Bold[normalize-space() = ''][string() = ''][not(comment())] | strong[normalize-space() = ''][string() = ''][not(comment())] | bold-italic[normalize-space() = ''][string() = ''][not(comment())] | Bold-Italic[normalize-space() = ''][string() = ''][not(comment())] | sup[normalize-space() = ''][string() = ''][not(comment())] | sub[normalize-space() = ''][string() = ''][not(comment())] | u[normalize-space() = ''][string() = ''][not(comment())] | underline[normalize-space() = ''][string() = ''][not(comment())]| s[normalize-space() = ''][string() = ''][not(comment())] | strike[normalize-space() = ''][string() = ''][not(comment())]">
	</xsl:template>

	<!-- Copy comments from empty tags -->
	<xsl:template match="i[normalize-space() = ''][comment()] | italic[normalize-space() = ''][comment()] | Italic[normalize-space() = ''][comment()] | em[normalize-space() = ''][comment()] | b[normalize-space() = ''][comment()] | bold[normalize-space() = ''][comment()] | Bold[normalize-space() = ''][comment()] | strong[normalize-space() = ''][comment()] | bold-italic[normalize-space() = ''][comment()] | Bold-Italic[normalize-space() = ''][comment()] | sup[normalize-space() = ''][comment()] | sub[normalize-space() = ''][comment()] | u[normalize-space() = ''][comment()] | underline[normalize-space() = ''][comment()]| s[normalize-space() = ''][comment()] | strike[normalize-space() = ''][comment()] | label[normalize-space() = ''][comment()] | title[normalize-space() = ''][comment()] | p[local-name(..)!='td' and local-name(..)!='th'][normalize-space() = ''][comment()] | license-p[normalize-space() = ''][comment()] | li[normalize-space() = ''][comment()] | attrib[normalize-space() = ''][comment()] | ref[normalize-space() = ''][comment()] | element-citation[normalize-space() = ''][comment()] | nlm-citation[normalize-space() = ''][comment()] | mixed-citation[normalize-space() = ''][comment()] | object-id[normalize-space() = ''][comment()] | xref-group[normalize-space() = ''][comment()] | kwd[normalize-space() = ''][comment()] | tp:nomenclature-citation[normalize-space() = ''][comment()] | article-title[normalize-space() = ''][comment()] | self-uri[normalize-space() = ''][comment()] | given-names[normalize-space() = ''][comment()] | surname[normalize-space() = ''][comment()]">
		<xsl:apply-templates select="comment()"/>
	</xsl:template>

	<xsl:template match="tp:nomenclature/tp:taxon-name">
		<title>
			<xsl:apply-templates/>
		</title>
	</xsl:template>

</xsl:stylesheet>
