﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xs="http://www.w3.org/2001/XMLSchema"
  xmlns:mml="http://www.w3.org/1998/Math/MathML"
  xmlns:xlink="http://www.w3.org/1999/xlink"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xmlns:tp="http://www.plazi.org/taxpub"
  exclude-result-prefixes="xs">

  <xsl:include href="../geo/geo-json.xsl" />
  <xsl:include href="../Floats/format-tables.xsl" />

  <xsl:variable name="invalid-tag-name" select="'INVALID-TAG'" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="p//break" />

  <xsl:template match="article_figs_and_tables[not(*)]" />

  <xsl:template match="*[@delete='true']">
    <xsl:apply-templates />
  </xsl:template>

  <!-- Clear double-format tags -->
  <!-- Italics -->
  <xsl:template match="i//i | i//em | i//italic | i//Italic">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="em//i | em//em | em//italic | em//Italic">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="italic//i | italic//em | italic//italic | italic//Italic">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="Italic//i | Italic//em | Italic//italic | Italic//Italic">
    <xsl:apply-templates />
  </xsl:template>

  <!-- Bolds -->
  <xsl:template match="b//b | b//strong | b//bold | b//Bold">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="strong//b | strong//strong | strong//bold | strong//Bold">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="bold//b | bold//strong | bold//bold | bold//Bold">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="Bold//b | Bold//strong | Bold//bold | Bold//Bold">
    <xsl:apply-templates />
  </xsl:template>

  <!-- Remove empty format tags-->
  <!-- Italics -->
  <xsl:template match="i[not(node())] | em[not(node())] | italic[not(node())] | Italic[not(node())]" />

  <!-- Bolds -->
  <xsl:template match="b[not(node())] | strong[not(node())] | bold[not(node())] | Bold[not(node())]" />

  <!-- Text-lines -->
  <xsl:template match="u[not(node())] | underline[not(node())] | overline[not(node())] | s[not(node())] | strike[not(node())]" />

  <!---->

  <xsl:template match="named-content[@content-type='voucher code'][ancestor::named-content[@content-type='voucher code']]">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="named-content[@content-type='voucher code'][@xlink:title='OSUC'][not(ancestor::named-content[@content-type='voucher code'])][not(ext-link)]">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <ext-link xlink:type="simple" ext-link-type="uri">
        <xsl:attribute name="xlink:href">
          <xsl:text>http://hol.osu.edu/spmInfo.html?id=OSUC%20</xsl:text>
          <xsl:value-of select="normalize-space(translate(string(), 'QWERTYUIOPASDFGHJKLZXCVBNM', ''))" />
        </xsl:attribute>
        <xsl:apply-templates />
      </ext-link>
    </xsl:element>
  </xsl:template>

  <xsl:template match="named-content[@content-type='voucher code'][@xlink:title='CASENT'][not(ancestor::named-content[@content-type='voucher code'])][not(ext-link)]">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <ext-link xlink:type="simple" ext-link-type="uri">
        <xsl:attribute name="xlink:href">
          <xsl:text>http://data.antweb.org/specimen/CASENT</xsl:text>
          <xsl:value-of select="normalize-space(translate(string(), 'QWERTYUIOPASDFGHJKLZXCVBNM', ''))" />
        </xsl:attribute>
        <xsl:apply-templates />
      </ext-link>
    </xsl:element>
  </xsl:template>

  <xsl:template match="named-content[@content-type='voucher code'][@xlink:title='USNMENT'][not(ancestor::named-content[@content-type='voucher code'])][not(ext-link)]">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <ext-link xlink:type="simple" ext-link-type="uri">
        <xsl:attribute name="xlink:href">
          <xsl:text>http://hol.osu.edu/spmInfo.html?id=USNMENT</xsl:text>
          <xsl:value-of select="normalize-space(translate(string(), 'QWERTYUIOPASDFGHJKLZXCVBNM', ''))" />
        </xsl:attribute>
        <xsl:apply-templates />
      </ext-link>
    </xsl:element>
  </xsl:template>

  <xsl:template match="named-content">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:call-template name="generate-id">
        <xsl:with-param name="prefix">
          <xsl:text>NC</xsl:text>
        </xsl:with-param>
      </xsl:call-template>
      <xsl:choose>
        <xsl:when test="@xlink:title='BOLD BIN' and not(ext-link)">
          <ext-link
            ext-link-type="uri"
            xlink:href="http://www.boldsystems.org/index.php/Public_BarcodeCluster?clusteruri={normalize-space(.)}">
            <xsl:apply-templates select="node()" />
          </ext-link>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="node()" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

  <xsl:template match="article-meta/article-id">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:value-of select="normalize-space()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="envo[@EnvoID]">
    <xsl:element name="{name()}">
      <xsl:attribute name="EnvoTermUri">
        <xsl:text>http://purl.obolibrary.org/obo/</xsl:text>
        <xsl:value-of select="normalize-space(@EnvoID)" />
      </xsl:attribute>
      <xsl:apply-templates select="@* | node()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="tn//tn | tp:taxon-name//tn | a//tn | *[@object_id='82']//tn | *[@id='41']//tn | *[@id='236' or @id='436' or @id='435' or @id='418' or @id='49' or @id='417' or @id='48' or @id='434' or @id='433' or @id='432' or @id='431' or @id='430' or @id='429' or @id='428' or @id='427' or @id='426' or @id='425' or @id='424' or @id='423' or @id='422' or @id='421' or @id='420' or @id='419' or @id='475' or @id='414']/value//tn | label//xref | fig/title//xref | table-wrap/title//xref | xref//tn | tn//xref | tn//abbrev | abbrev//abbrev | tp:taxon-name//abbrev | xref//abbrev | element-citation//abbrev | nlm-citation//abbrev | mixed-citation//abbrev | aff/abbrev | institution//abbrev | source//abbrev | kwd//abbrev">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="tn//tp:taxon-name | tp:taxon-name//tp:taxon-name | a//tp:* | *[@object_id='82']//tp:* | *[@id='41']//tp:* | *[@id='236' or @id='436' or @id='435' or @id='418' or @id='49' or @id='417' or @id='48' or @id='434' or @id='433' or @id='432' or @id='431' or @id='430' or @id='429' or @id='428' or @id='427' or @id='426' or @id='425' or @id='424' or @id='423' or @id='422' or @id='421' or @id='420' or @id='419' or @id='475' or @id='414']/value//tp:* | xref//tp:* | tp:taxon-name//xref | named-content[@content-type='date']//institutional_code | named-content[@content-type='date']//named-content[@content-type='date'] | ref//named-content[@content-type='institution'] | named-content[@content-type='institution']//named-content[@content-type='institution'] | addr-line//named-content[@content-type='institution']">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="tp:taxon-treatment/@id" />

  <xsl:template match="tp:treatment-meta/kwd-group/kwd/named-content">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:value-of select="string()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="tp:nomenclature-citation[not(comment)]">
    <xsl:element name="{name()}">
      <comment>
        <xsl:apply-templates />
      </comment>
    </xsl:element>
  </xsl:template>

  <xsl:template match="tn-part[name(..)!='tn'][name(..)!='tp:taxon-name']|tp:taxon-name-part[name(..)!='tn'][name(..)!='tp:taxon-name']|tn-part//tn-part|tn-part//tp:*|tp:taxon-name-part//tp:*">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="tn-part/*[name(.)!='i' and name(.)!='b']">
    <xsl:value-of select="." />
  </xsl:template>

  <xsl:template match="tn-part[@type='authority'][not(@full-name)][normalize-space(.)='']"></xsl:template>

  <xsl:template match="tn/genus-authority | tn/authority | tn/basionym-authority[not(contains(string(), '(') or contains(string(), ')'))]">
    <tn-part type="authority">
      <xsl:apply-templates />
    </tn-part>
  </xsl:template>

  <xsl:template match="tn/infraspecific-rank | tn/infraspecific">
    <tn-part type="{name()}">
      <xsl:apply-templates />
    </tn-part>
  </xsl:template>

  <xsl:template match="tn/basionym | tn/specific | tn/genus | tn/species">
    <tn-part type="x-rank">
      <xsl:apply-templates />
    </tn-part>
  </xsl:template>

  <xsl:template match="tn/sensu">
    <tn-part type="sensu">
      <xsl:apply-templates />
    </tn-part>
  </xsl:template>

  <xsl:template match="xref/institutional_code">
    <xsl:value-of select="." />
  </xsl:template>

  <xsl:template match="a//a|a//ext-link|a//xref|xref//xref|xref//a|xref//ext-link|ext-link//ext-link|xref//named-content">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="date//institutional_code | quantity//quantity | //abbrev[normalize-space(@content-type)!='institution' and normalize-space(@content-type)!='collection']//institutional_code[name(..)!='p']">
    <xsl:apply-templates />
  </xsl:template>

  <!-- Remove xref/bibr from ref. In some cases this is not needed. -->
  <xsl:template match="ref//xref[@ref-type='bibr' or @ref-type='fig' or @ref-type='table' or @ref-type='table-fn']">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="ref//institutional_code | ref//institution[@url]">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="person-group">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@* | comment() | anonymous | collab | name | aff | etal | string-name" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="person-group/name">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@* | comment() | surname | given-names | prefix | suffix | anonymous | etal" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="mixed-citation/person-group/name">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:for-each select="comment() | surname | given-names | prefix | suffix | anonymous | etal">
        <xsl:apply-templates select="." />
        <xsl:if test="position()!= last() and name()!=''">
          <xsl:text> </xsl:text>
        </xsl:if>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>

  <xsl:template match="surname | given-names | prefix | suffix | anonymous | etal">
    <xsl:element name="{name()}">
      <xsl:value-of select="normalize-space()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="conf-loc[not(@field_name)] | conf-date[not(@field_name)] | edition[not(@field_name)]">
    <xsl:element name="{name()}">
      <xsl:value-of select="string()" />
    </xsl:element>
  </xsl:template>

  <!-- author-notes/fn/p model -->

  <xsl:template match="author-notes/fn/p">
    <p>
      <xsl:apply-templates mode="author-notes-fn-p" />
    </p>
  </xsl:template>

  <xsl:template match="@*" mode="author-notes-fn-p">
    <xsl:copy>
      <xsl:apply-templates select="@*" mode="author-notes-fn-p" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="text()" mode="author-notes-fn-p">
    <xsl:copy-of select="." />
  </xsl:template>

  <xsl:template match="email | ext-link" mode="author-notes-fn-p">
    <xsl:apply-templates select="." />
  </xsl:template>

  <xsl:template match="*" mode="author-notes-fn-p">
    <xsl:apply-templates mode="author-notes-fn-p" />
  </xsl:template>

  <!-- locality-coordinates model -->

  <xsl:template match="locality-coordinates">
    <xsl:element name="{name()}">
      <xsl:attribute name="latitude" />
      <xsl:attribute name="longitude" />
      <xsl:apply-templates select="@* | node()" mode="locality-coordinates" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="@*" mode="locality-coordinates">
    <xsl:copy>
      <xsl:apply-templates select="@*" mode="locality-coordinates" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="text() | comment()" mode="locality-coordinates">
    <xsl:copy-of select="." />
  </xsl:template>

  <xsl:template match="bold | Bold | b | italic | Italic | i | bold-italic | monospace | overline | roman | sans-serif | sc | strike | s | underline | u | named-content | styled-content | sub | sup" mode="locality-coordinates">
    <xsl:apply-templates select="." />
  </xsl:template>

  <xsl:template match="*" mode="locality-coordinates">
    <xsl:apply-templates mode="locality-coordinates" />
  </xsl:template>

  <!-- Links and ids -->

  <xsl:template match="object-id">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:value-of select="normalize-space()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="email | self-uri | uri">
    <xsl:element name="{name()}">
      <xsl:attribute name="xlink:type">
        <xsl:text>simple</xsl:text>
      </xsl:attribute>
      <xsl:apply-templates select="@*" />
      <xsl:value-of select="normalize-space()" />
    </xsl:element>
  </xsl:template>

  <!-- ext-link model -->

  <xsl:template match="ext-link[not(ancestor::ext-link)]">
    <xsl:element name="{name()}">
      <xsl:attribute name="xlink:type">
        <xsl:text>simple</xsl:text>
      </xsl:attribute>
      <xsl:apply-templates select="@* | node()" mode="ext-link" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="ext-link[@ext-link-type][not(@xlink:href)]">
    <xsl:element name="{name()}">
      <xsl:attribute name="xlink:type">
        <xsl:text>simple</xsl:text>
      </xsl:attribute>

      <xsl:apply-templates select="@*" mode="ext-link" />

      <xsl:attribute name="xlink:href">
        <xsl:variable name="content" select="normalize-space(.)" />
        <xsl:choose>
          <xsl:when test="@ext-link-type='uri'">
            <xsl:choose>
              <xsl:when test="translate(substring($content, 1, 4), 'HTP', 'htp') = 'http'">
                <xsl:value-of select="$content" />
              </xsl:when>
              <xsl:when test="contains($content,'://')">
                <xsl:text>http://</xsl:text>
                <xsl:value-of select="normalize-space(substring-after($content,'://'))" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>http://</xsl:text>
                <xsl:value-of select="$content" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@ext-link-type='ftp'">
            <xsl:choose>
              <xsl:when test="contains(translate(substring($content, 1, 4), 'FTP', 'ftp'), 'ftp')">
                <xsl:value-of select="$content" />
              </xsl:when>
              <xsl:when test="contains($content,'://')">
                <xsl:text>ftp://</xsl:text>
                <xsl:value-of select="normalize-space(substring-after($content,'://'))" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>ftp://</xsl:text>
                <xsl:value-of select="$content" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@ext-link-type='doi'">
            <xsl:choose>
              <xsl:when test="contains($content,'10.')">
                <xsl:text>10.</xsl:text>
                <xsl:value-of select="normalize-space(substring-after($content,'10.'))" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:message terminate="no">
                  <xsl:text>INVALID DOI: </xsl:text>
                  <xsl:value-of select="$content" />
                </xsl:message>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@ext-link-type='pmid'">
            <xsl:call-template name="get-number-content">
              <xsl:with-param name="content" select="$content" />
            </xsl:call-template>
          </xsl:when>
          <xsl:when test="@ext-link-type='pmcid'">
            <xsl:text>PMC</xsl:text>
            <xsl:call-template name="get-number-content">
              <xsl:with-param name="content" select="$content" />
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$content" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>

      <xsl:apply-templates select="node()" mode="ext-link" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="@* | text() | comment()" mode="ext-link">
    <xsl:copy-of select="." />
  </xsl:template>

  <xsl:template match="bold | Bold | b | italic | Italic | i | bold-italic | monospace | overline | roman | sans-serif | sc | strike | s | underline | u | named-content | styled-content | sub | sup" mode="ext-link">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@* | node()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="*" mode="ext-link">
    <xsl:apply-templates mode="ext-link" />
  </xsl:template>

  <!-- license-p model -->

  <xsl:template match="license-p">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@* | node()" mode="license-p" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="@* | text() | comment()" mode="license-p">
    <xsl:copy-of select="." />
  </xsl:template>

  <xsl:template match="email | ext-link | uri | inline-supplementary-material | related-article | related-object | address | alternatives | array | boxed-text | chem-struct-wrap | fig | fig-group | graphic | media | preformat | supplementary-material | table-wrap | table-wrap-group | disp-formula | disp-formula-group | element-citation | mixed-citation | nlm-citation | bold | b | italic | i | monospace | overline | roman | sans-serif | sc | strike | s | underline | u | award-id | funding-source | open-access | chem-struct | inline-formula | inline-graphic | private-char | def-list | list | ol | ul | tex-math | mml:math | abbrev | milestone-end | milestone-start | named-content | styled-content | disp-quote | speech | statement | verse-group | fn | target | xref | sub | sup | price" mode="license-p">
    <xsl:element name="{name()}">
      <!-- Apply templates not in node license-p -->
      <xsl:apply-templates select="@* | node()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="*" mode="license-p">
    <xsl:apply-templates mode="license-p" />
  </xsl:template>

  <!-- other -->
  <xsl:template match="person-group/name[not(@name-style)]">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:attribute name="name-style">
        <xsl:text>western</xsl:text>
      </xsl:attribute>
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="size[normalize-space(@units)='']">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:attribute name="units">
        <xsl:choose>
          <xsl:when test="contains(string(.), 'pp') or contains(string(.), 'Pp') or contains(string(.), 'PP')">
            <xsl:text>page</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <!-- Unknown units type -->
            <xsl:text></xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="trans-source[not(@xml:lang)] | trans-title[not(@xml:lang)]">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:attribute name="xml:lang">
        <xsl:text>en</xsl:text>
      </xsl:attribute>
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="date-in-citation[normalize-space(@content-type)='']">
    <xsl:variable name="content">
      <xsl:call-template name="to-lower-case">
        <xsl:with-param name="string" select="string(.)" />
      </xsl:call-template>
    </xsl:variable>
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:attribute name="content-type">
        <xsl:choose>
          <xsl:when test="contains($content, 'update')">
            <xsl:text>update-date</xsl:text>
          </xsl:when>
          <xsl:when test="contains($content, 'publish') or contains($content, 'publication')">
            <xsl:text>publish-date</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>access-date</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="td[count(node()) = 1 and count(text()) = 1] | th[count(node()) = 1 and count(text()) = 1] | title[count(node()) = 1 and count(text()) = 1] | label[count(node()) = 1 and count(text()) = 1] | p[count(node()) = 1 and count(text()) = 1] | article-title[count(node()) = 1 and count(text()) = 1] | li[count(node()) = 1 and count(text()) = 1] | kwd[count(node()) = 1 and count(text()) = 1] | xref-group[count(node()) = 1 and count(text()) = 1]">
    <xsl:element name="{name()}">
      <xsl:apply-templates select="@*" />
      <xsl:value-of select="normalize-space()" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="tex-math">
    <xsl:element name="{name()}">
      <xsl:attribute name="id">
        <xsl:text>Math</xsl:text>
        <xsl:value-of select="generate-id()" />
      </xsl:attribute>
      <xsl:apply-templates select="@* | node()" />
    </xsl:element>
  </xsl:template>

  <xsl:template name="get-number-content">
    <xsl:param name="content" />
    <xsl:variable name="masked-number" select="translate($content, '0123456789', '¶¶¶¶¶¶¶¶¶¶')" />
    <xsl:variable name="prefix" select="substring-before($content, '¶')" />
    <xsl:choose>
      <xsl:when test="string-length($prefix) = 0">
        <xsl:value-of select="$content" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring-after($content, $prefix)" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="generate-id">
    <xsl:param name="prefix" select="''" />
    <xsl:attribute name="id">
      <xsl:value-of select="$prefix" />
      <xsl:value-of select="generate-id()" />
    </xsl:attribute>
  </xsl:template>

  <xsl:template name="to-lower-case">
    <xsl:param name="string" />
    <xsl:value-of select="translate($string, 'QWERTYUIOPASDFGHJKLZXCVBNM', 'qwertyuiopasdfghjklzxcvbnm')" />
  </xsl:template>
</xsl:stylesheet>
