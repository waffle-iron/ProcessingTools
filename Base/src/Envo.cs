﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProcessingTools.Base
{
    public class Envo : Base
    {
        public Envo(string xml)
            :base(xml)
        {
        }

        public Envo(Config config, string xml)
            : base(config, xml)
        {
        }

        public Envo(Base baseObject)
            : base(baseObject)
        {
        }

        public void Tag()
        {
            XmlDocument envoTermsTagSet = new XmlDocument();
            {
                XmlDocument envoTermsResponse = Net.UseGreekTagger(this.TextContent);

                try
                {
                    envoTermsResponse.Save(this.Config.EnvoResponseOutputXmlFileName);
                }
                catch (Exception e)
                {
                    Alert.RaiseExceptionForMethod(
                        e,
                        this.GetType().Name,
                        0,
                        string.Format(
                            "Cannot write envoTermsResponse XML document to file '{0}'",
                            this.Config.EnvoResponseOutputXmlFileName));
                }

                string envoTermsResponseString = Regex.Replace(
                    envoTermsResponse.OuterXml,
                    @"\sxmlns=""[^<>""]*""",
                    string.Empty);

                string tagSetString = XsltOnString.ApplyTransform(
                    this.Config.envoTermsWebServiceTransformXslPath,
                    envoTermsResponseString);

                envoTermsTagSet.LoadXml(tagSetString);
            }

            {
                ParseXmlStringToXmlDocument();

                const string XPathTemplate = "//p[{0}]|//license-p[{0}]|//li[{0}]|//th[{0}]|//td[{0}]|//mixed-citation[{0}]|//element-citation[{0}]|//nlm-citation[{0}]|//tp:nomenclature-citation[{0}]";
                string xpath = string.Format(XPathTemplate, "normalize-space(.)!=''");
                XmlNodeList nodeList = this.xmlDocument.SelectNodes(xpath, this.NamespaceManager);

                TagTextInXmlDocument(envoTermsTagSet, nodeList, true, false);

                ParseXmlDocumentToXmlString();
            }
        }
    }
}
