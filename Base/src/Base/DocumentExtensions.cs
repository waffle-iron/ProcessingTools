﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace ProcessingTools.Base
{
    public static class DocumentExtensions
    {
        /// <summary>
        /// Converts XDocument to XmlDocument.
        /// Original source: <see cref="http://stackoverflow.com/questions/1508572/converting-xdocument-to-xmldocument-and-vice-versa"/>
        /// </summary>
        /// <param name="xDocument">XDocument instance to be converted.</param>
        /// <returns></returns>
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (XmlReader xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }

            return xmlDocument;
        }

        /// <summary>
        /// Converts XmlDocument to XDocument.
        /// Original source: <see cref="http://stackoverflow.com/questions/1508572/converting-xdocument-to-xmldocument-and-vice-versa"/>
        /// </summary>
        /// <param name="xmlDocument">XmlDocument instance to be converted.</param>
        /// <returns></returns>
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (XmlNodeReader nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static List<string> GetMatchesInXmlText(this XmlNodeList nodeList, Regex search, bool clearList = true)
        {
            List<string> result = new List<string>();

            foreach (XmlNode node in nodeList)
            {
                result.AddRange(GetMatchesInXmlText(node, search, false));
            }

            if (clearList)
            {
                result = result.Distinct().ToList();
                result.Sort();
            }

            return result;
        }

        public static List<string> GetMatchesInXmlText(this XmlNode node, Regex search, bool clearList = true)
        {
            return node.InnerText.GetMatchesInText(search, clearList);
        }

        public static List<string> GetMatchesInText(this string text, Regex search, bool clearList = false)
        {
            List<string> result = new List<string>();

            for (Match m = search.Match(text); m.Success; m = m.NextMatch())
            {
                result.Add(m.Value);
            }

            if (clearList)
            {
                result = result.Distinct().ToList();
                result.Sort();
            }

            return result;
        }

        public static HashSet<string> ExtractWordsFromString(this string text, bool distinctWords = true)
        {
            List<string> result = new List<string>();

            Regex matchWords = new Regex(@"\b\w+\b");
            for (Match word = matchWords.Match(text); word.Success; word = word.NextMatch())
            {
                result.Add(word.Value);
            }

            if (distinctWords)
            {
                result = result.Distinct().ToList();
                result.Sort();
            }

            return new HashSet<string>(result);
        }

        public static HashSet<string> ExtractWordsFromXml(this XmlDocument xml)
        {
            return ExtractWordsFromString(xml.InnerText);
        }

        public static List<string> GetStringListOfUniqueXmlNodes(this XmlNode xml, string xpath, XmlNamespaceManager namespaceManager = null)
        {
            List<string> result = new List<string>();
            try
            {
                XmlNodeList nodeList;
                if (namespaceManager == null)
                {
                    nodeList = xml.SelectNodes(xpath);
                }
                else
                {
                    nodeList = xml.SelectNodes(xpath, namespaceManager);
                }

                result = nodeList.GetStringListOfUniqueXmlNodes();
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0, 1);
            }

            return result;
        }

        public static List<string> GetStringListOfUniqueXmlNodes(this IEnumerable xmlNodeList)
        {
            List<string> result = new List<string>();
            try
            {
                result = xmlNodeList.Cast<XmlNode>().Select(c => c.InnerXml).Distinct().ToList();
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0, 1);
            }

            return result;
        }

        public static List<string> GetStringListOfUniqueXmlNodeContent(this XmlNode xml, string xpath, XmlNamespaceManager namespaceManager = null)
        {
            List<string> result = new List<string>();
            try
            {
                XmlNodeList nodeList;
                if (namespaceManager == null)
                {
                    nodeList = xml.SelectNodes(xpath);
                }
                else
                {
                    nodeList = xml.SelectNodes(xpath, namespaceManager);
                }

                result = nodeList.GetStringListOfUniqueXmlNodeContent();
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0, 1);
            }

            return result;
        }

        public static List<string> GetStringListOfUniqueXmlNodeContent(this IEnumerable xmlNodeList)
        {
            List<string> result = new List<string>();
            try
            {
                result = xmlNodeList.Cast<XmlNode>().Select(c => c.InnerText).Distinct().ToList();
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0, 1);
            }

            return result;
        }

        /// <summary>
        /// Given an input list, returns the sublist of not-included-in-xdoc words
        /// </summary>
        /// <param name="wordList">Input list to be parsed.</param>
        /// <param name="xdoc">XDocument to parse with.</param>
        /// <returns>Xdoc-clean sublist of the wordList.</returns>
        public static IEnumerable<string> ClearListWithXDocument(this IEnumerable<string> wordList, XElement xdoc)
        {
            IEnumerable<string> result = null;

            try
            {
                result = from word in wordList
                         where (from item in xdoc.Elements()
                                where Regex.Match(word, "\\A(?i)" + item.Value + "\\Z").Success
                                select item).Count() == 0
                         select word;
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0, 1);
            }

            return result;
        }

        /// <summary>
        /// Given an input list, returns the sublist of included-in-xdoc words
        /// </summary>
        /// <param name="wordList">Input list to be parsed.</param>
        /// <param name="xdoc">XDocument to parse with.</param>
        /// <returns>Xdoc-selected sublist of the wordList.</returns>
        public static IEnumerable<string> SelectListWithXDocument(this IEnumerable<string> wordList, XElement xdoc)
        {
            IEnumerable<string> result = null;

            try
            {
                result = from word in wordList
                         where (from item in xdoc.Elements()
                                where Regex.Match(word, "\\A(?i)" + item.Value + "\\Z").Success
                                select item).Count() > 0
                         select word;
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0, 1);
            }

            return result;
        }

        /// <summary>
        /// Creates XmlReader object from a text content.
        /// </summary>
        /// <param name="text">Valid XML node as text.</param>
        /// <returns>XmlReader object.</returns>
        /// <exception cref="System.Text.EncoderFallbackException">Input document string schould be UFT8 encoded.</exception>
        public static XmlReader ToXmlReader(this string text)
        {
            XmlReader xmlReader = null;
            try
            {
                byte[] bytesContent = Encoding.UTF8.GetBytes(text);
                xmlReader = XmlReader.Create(new MemoryStream(bytesContent));
            }
            catch (EncoderFallbackException e)
            {
                throw new EncoderFallbackException("Input document string schould be UFT8 encoded.", e);
            }
            catch
            {
                throw;
            }

            return xmlReader;
        }

        /// <summary>
        /// Executes XSL transform using the input document specified by the System.Xml object and returns the result as a string.
        /// </summary>
        /// <param name="xmlDocument">Input document to be transformed.</param>
        /// <param name="xslFileName">File name path of the XSL file.</param>
        /// <returns>Transformed document as string.</returns>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Xml.Xsl.XsltException"></exception>
        /// <exception cref="System.Xml.XmlException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static string ApplyXslTransform(this XmlDocument xmlDocument, string xslFileName)
        {
            return xmlDocument.OuterXml.ApplyXslTransform(xslFileName);
        }

        /// <summary>
        /// Executes XSL transform using the input document specified by the System.Xml object and returns the result as a string.
        /// </summary>
        /// <param name="xmlDocument">Input document to be transformed.</param>
        /// <param name="xslTransform">XslCompiledTransform object.</param>
        /// <returns>Transformed document as string.</returns>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Xml.Xsl.XsltException"></exception>
        /// <exception cref="System.Xml.XmlException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static string ApplyXslTransform(this XmlDocument xmlDocument, XslCompiledTransform xslTransform)
        {
            return xmlDocument.OuterXml.ApplyXslTransform(xslTransform);
        }

        /// <summary>
        /// Executes XSL transform using the input document specified by the string object and returns the result as a string.
        /// </summary>
        /// <param name="xml">Input document to be transformed.</param>
        /// <param name="xslFileName">File name path of the XSL file.</param>
        /// <returns>Transformed document as string.</returns>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Xml.Xsl.XsltException"></exception>
        /// <exception cref="System.Xml.XmlException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static string ApplyXslTransform(this string xml, string xslFileName)
        {
            string result = string.Empty;
            try
            {
                using (XmlReader xmlReader = xml.ToXmlReader())
                {
                    result = xmlReader.ApplyXslTransform(xslFileName);
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Executes XSL transform using the input document specified by the string object and returns the result as a string.
        /// </summary>
        /// <param name="xml">Input document to be transformed.</param>
        /// <param name="xslTransform">XslCompiledTransform object.</param>
        /// <returns>Transformed document as string.</returns>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Xml.Xsl.XsltException"></exception>
        /// <exception cref="System.Xml.XmlException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static string ApplyXslTransform(this string xml, XslCompiledTransform xslTransform)
        {
            string result = string.Empty;
            try
            {
                byte[] bytesContent = Encoding.UTF8.GetBytes(xml);
                using (XmlReader xmlReader = XmlReader.Create(new MemoryStream(bytesContent)))
                {
                    result = xmlReader.ApplyXslTransform(xslTransform);
                }
            }
            catch (EncoderFallbackException e)
            {
                throw new EncoderFallbackException("Input document string must be UFT8 encoded.", e);
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Executes XSL transform using the input document specified by the System.Xml.XmlReader object and returns the result as a string.
        /// </summary>
        /// <param name="xmlReader">Input document to be transformed.</param>
        /// <param name="xslFileName">File name path of the XSL file.</param>
        /// <returns>Transformed document as string.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Xml.Xsl.XsltException"></exception>
        /// <exception cref="System.Xml.XmlException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static string ApplyXslTransform(this XmlReader xmlReader, string xslFileName)
        {
            string result = string.Empty;
            if (xslFileName == null || xslFileName.Length < 1)
            {
                throw new ArgumentNullException("XSL file name is invalid.");
            }

            try
            {
                XslCompiledTransform xslTransform = new XslCompiledTransform();
                xslTransform.Load(xslFileName);
                result = xmlReader.ApplyXslTransform(xslTransform);
            }
            catch (IOException e)
            {
                throw new IOException("Cannot read XSL file.", e);
            }
            catch (XsltException e)
            {
                throw new XsltException("Invalid XSL file.", e);
            }
            catch (XmlException e)
            {
                throw new System.Xml.XmlException("Invalid XML file.", e);
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Executes XSL transform using the input document specified by the System.Xml.XmlReader object and returns the result as a string.
        /// </summary>
        /// <param name="xmlReader">Input document to be transformed.</param>
        /// <param name="xslTransform">XslCompiledTransform object.</param>
        /// <returns>Transformed document as string.</returns>
        /// <exception cref="System.Xml.Xsl.XsltException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static string ApplyXslTransform(this XmlReader xmlReader, XslCompiledTransform xslTransform)
        {
            string result = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                StreamReader streamReader = null;
                try
                {
                    xslTransform.Transform(xmlReader, null, memoryStream);
                    memoryStream.Position = 0;
                    streamReader = new StreamReader(memoryStream);
                }
                catch (XsltException e)
                {
                    throw new XsltException("Invalid XSL file.", e);
                }
                catch (Exception e)
                {
                    throw new System.Exception("General exception.", e);
                }
                finally
                {
                    if (streamReader != null)
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}