﻿namespace ProcessingTools.Xml.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml;

    using ProcessingTools.Contracts;

    public static class XmlNodeExtensions
    {
        /// <summary>
        /// Checks the type of a given XmlNode object.
        /// Returns true if the XmlNode is a named XmlNode (*) or a text node.
        /// Returns false if the node is a comment, proceeding instruction, DOCTYPE or CDATA element.
        /// </summary>
        /// <param name="node">XmlNode object to check.</param>
        /// <returns>Returns true if the XmlNode is a named XmlNode (*) or a text node. Returns false if the node is a comment, proceeding instruction, DOCTYPE or CDATA element.</returns>
        public static bool CheckIfIsPossibleToPerformReplaceInXmlNode(this XmlNode node)
        {
            bool performReplace = false;
            switch (node.NodeType)
            {
                case XmlNodeType.None:
                    performReplace = false;
                    break;

                case XmlNodeType.Element:
                    performReplace = false;
                    break;

                case XmlNodeType.Attribute:
                    performReplace = false;
                    break;

                case XmlNodeType.Text:
                    performReplace = true;
                    break;

                case XmlNodeType.CDATA:
                    performReplace = false;
                    break;

                case XmlNodeType.EntityReference:
                    performReplace = false;
                    break;

                case XmlNodeType.Entity:
                    performReplace = false;
                    break;

                case XmlNodeType.ProcessingInstruction:
                    performReplace = false;
                    break;

                case XmlNodeType.Comment:
                    performReplace = false;
                    break;

                case XmlNodeType.Document:
                    performReplace = true;
                    break;

                case XmlNodeType.DocumentType:
                    performReplace = false;
                    break;

                case XmlNodeType.DocumentFragment:
                    performReplace = true;
                    break;

                case XmlNodeType.Notation:
                    performReplace = false;
                    break;

                case XmlNodeType.Whitespace:
                    performReplace = false;
                    break;

                case XmlNodeType.SignificantWhitespace:
                    performReplace = false;
                    break;

                case XmlNodeType.EndElement:
                    performReplace = false;
                    break;

                case XmlNodeType.EndEntity:
                    performReplace = false;
                    break;

                case XmlNodeType.XmlDeclaration:
                    performReplace = false;
                    break;

                default:
                    performReplace = false;
                    break;
            }

            return performReplace;
        }

        public static IEnumerable<string> GetStringListOfUniqueXmlNodeContent(this XmlNode xml, string xpath)
        {
            XmlNodeList nodeList = xml.SelectNodes(xpath);
            var result = nodeList.GetStringListOfUniqueXmlNodeContent();
            return new HashSet<string>(result);
        }

        public static IEnumerable<string> GetStringListOfUniqueXmlNodeContent(this XmlNode xml, string xpath, XmlNamespaceManager namespaceManager)
        {
            XmlNodeList nodeList = xml.SelectNodes(xpath, namespaceManager);
            var result = nodeList.GetStringListOfUniqueXmlNodeContent();
            return new HashSet<string>(result);
        }

        public static IEnumerable<string> GetStringListOfUniqueXmlNodeContent(this IEnumerable xmlNodeList)
        {
            var result = xmlNodeList.Cast<XmlNode>().Select(c => c.InnerText);
            return new HashSet<string>(result);
        }

        public static IEnumerable<string> GetStringListOfUniqueXmlNodes(this XmlNode xml, string xpath)
        {
            XmlNodeList nodeList = xml.SelectNodes(xpath);
            var result = nodeList.GetStringListOfUniqueXmlNodes();
            return new HashSet<string>(result);
        }

        public static IEnumerable<string> GetStringListOfUniqueXmlNodes(this XmlNode xml, string xpath, XmlNamespaceManager namespaceManager)
        {
            XmlNodeList nodeList = xml.SelectNodes(xpath, namespaceManager);
            var result = nodeList.GetStringListOfUniqueXmlNodes();
            return new HashSet<string>(result);
        }

        public static IEnumerable<string> GetStringListOfUniqueXmlNodes(this IEnumerable xmlNodeList)
        {
            var result = xmlNodeList.Cast<XmlNode>().Select(c => c.InnerXml);
            return new HashSet<string>(result);
        }

        /// <summary>
        /// Replaces the whole XmlNode object by a XmlDocumentFragment, generated by Regex.Replace.
        /// </summary>
        /// <param name="node">XmlNode object to be replaced.</param>
        /// <param name="regexPattern">Regex pattern string to be executed.</param>
        /// <param name="regexReplacement">Regex replacement string to build the XmlDocumentFragment object.</param>
        public static void ReplaceWholeXmlNodeByRegexPattern(this XmlNode node, string regexPattern, string regexReplacement)
        {
            XmlDocumentFragment nodeFragment = node.OwnerDocument.CreateDocumentFragment();
            nodeFragment.InnerXml = Regex.Replace(node.OuterXml, regexPattern, regexReplacement);
            node.ParentNode.ReplaceChild(nodeFragment, node);
        }

        /// <summary>
        /// Replaces the whole XmlNode object by a XmlDocumentFragment, generated by Regex.Replace.
        /// </summary>
        /// <param name="node">XmlNode object to be replaced.</param>
        /// <param name="regex">Regex object to be executed.</param>
        /// <param name="regexReplacement">Regex replacement string to build the XmlDocumentFragment object.</param>
        public static void ReplaceWholeXmlNodeByRegexPattern(this XmlNode node, Regex regex, string regexReplacement)
        {
            XmlDocumentFragment nodeFragment = node.OwnerDocument.CreateDocumentFragment();
            nodeFragment.InnerXml = regex.Replace(node.OuterXml, regexReplacement);
            node.ParentNode.ReplaceChild(nodeFragment, node);
        }

        /// <summary>
        /// Replaces safely the InnerXml of a given XmlNode. If the replace string is not a valid Xml fragment, replacement will not be done.
        /// </summary>
        /// <param name="node">XmlNode which content would be replaced.</param>
        /// <param name="replace">Replacement string.</param>
        /// <param name="logger">ILogger object to log exceptions.</param>
        /// <returns>Status value: is the replacement performed or not.</returns>
        public static Task<bool> SafeReplaceInnerXml(this XmlNode node, string replace, ILogger logger)
        {
            string nodeInnerXml = node.InnerXml;
            bool reset = false;
            try
            {
                reset = true;
                node.InnerXml = replace;
                reset = false;
            }
            catch (Exception e)
            {
                logger?.Log(e, "\nInvalid replacement string:\n{0}\n\n", replace.Substring(0, Math.Min(replace.Length, 300)));
            }
            finally
            {
                if (reset)
                {
                    node.InnerXml = nodeInnerXml;
                }
            }

            return Task.FromResult(!reset);
        }

        /// <summary>
        /// Sets or updates attribute with given value of an XmlNode object.
        /// </summary>
        /// <param name="node">XmlNode object to be changed.</param>
        /// <param name="attributeName">Name of the attribute to be created of updated.</param>
        /// <param name="attributeValue">Value of the attribute.</param>
        /// <returns>The same XmlNode object. Used for chaining.</returns>
        public static XmlNode SetOrUpdateAttribute(this XmlNode node, string attributeName, string attributeValue)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var attribute = node.Attributes[attributeName];
            if (attribute == null)
            {
                var a = node.OwnerDocument.CreateAttribute(attributeName);
                node.Attributes.Append(a);
                attribute = a;
            }

            attribute.InnerText = attributeValue;

            return node;
        }
    }
}