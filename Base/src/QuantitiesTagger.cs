﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProcessingTools.Base
{
    public class QuantitiesTagger : TaggerBase
    {
        public QuantitiesTagger(Config config, string xml)
            : base(config, xml)
        {
        }

        public QuantitiesTagger(TaggerBase baseObject)
            : base(baseObject)
        {
        }

        public void TagQuantities(IXPathProvider xpathProvider)
        {
            string xpath = string.Format(xpathProvider.SelectContentNodesXPathTemplate, "normalize-space(.)!=''");
            XmlNodeList nodeList = this.XmlDocument.SelectNodes(xpath, this.NamespaceManager);

            List<string> quantities = new List<string>();

            // 28.4–30.0 °C
            // 30.1–31.2 ppt
            // 1,500 m × 200 m
            // 15 mM
            // 15 units/ µl
            // 30 s
            // 5 min
            // 8 minutes
            // between 432 and 887 bp
            // 0.6–1.9 mm, 1.1–1.7 × 0.5–0.8 mm
            // 3.5 cm × 3 cm
            // 11 cm x 8 cm
            // 59 kV
            // 167 µA
            // 2–4 mm
            {
                string pattern = @"(?<!<[^>]+)((?:(?:[\(\)\[\]\{\}–—−‒-]\s*)??\d+(?:[,\.]\d+)?(?:\s*[\(\)\[\]\{\}×\*])?\s*)+?(?:[kdcmµ]m|meters?|[kmµnp]g|[º°˚]\s*[FC]|[M]?bp|ft|m|[kdcmµ]M|[dcmµ][lL]|[kdcmµ]mol|mile|mi|min(?:ute)|\%)\b)(?![^<>]*>)";
                Regex matchQuantities = new Regex(pattern);
                quantities = nodeList.GetMatchesInXmlText(matchQuantities, true);
            }

            TagContent quantityTag = new TagContent("quantity");
            foreach (string quantity in quantities)
            {
                Alert.Log(quantity);
                TagTextInXmlDocument(quantity, quantityTag, xpathProvider.SelectContentNodesXPathTemplate, true);
            }
        }

        public void TagDirections(IXPathProvider xpathProvider)
        {
            foreach (XmlNode node in this.XmlDocument.SelectNodes(xpathProvider.SelectContentNodesXPath, this.NamespaceManager))
            {
                string replace = node.InnerXml;

                // 24 km W
                string pattern = @"(<quantity>.*?</quantity>\W{0,4}(?:[NSEW][NSEW\s\.-]{0,5}(?!\w)|(?i)(?:east|west|south|notrh)+))";
                Match m = Regex.Match(replace, pattern);
                if (m.Success)
                {
                    // Alert.Message(m.Value);
                    replace = Regex.Replace(replace, pattern, "<direction>$1</direction>");
                    node.InnerXml = replace;
                }
            }
        }

        public void TagAltitude(IXPathProvider xpathProvider)
        {
            // 58 m alt.

            foreach (XmlNode node in this.XmlDocument.SelectNodes(xpathProvider.SelectContentNodesXPath, this.NamespaceManager))
            {
                string replace = node.InnerXml;

                // 510–650 m a.s.l.
                string pattern = @"(<quantity>.*?</quantity>\W{0,4}(?:(?i)(?:<[^>]*>)*a\W*(?:<[^>]*>)*s\W*(?:<[^>]*>)*l\W?))";
                Match m = Regex.Match(replace, pattern);
                if (m.Success)
                {
                    // Alert.Message(m.Value);
                    replace = Regex.Replace(replace, pattern, "<altitude>$1</altitude>");
                    node.InnerXml = replace;
                }
            }
        }
    }
}
