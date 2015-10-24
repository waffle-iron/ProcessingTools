﻿namespace ProcessingTools.BaseLibrary
{
    using System;
    using System.Data.SqlClient;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using Configurator;
    using Globals.Extensions;
    using Globals.Loggers;

    public class DataProvider : TaggerBase, IDataProvider
    {
        private ILogger logger;

        public DataProvider(Config config, string xml, ILogger logger)
            : base(config, xml)
        {
            this.logger = logger;
        }

        public DataProvider(IBase baseObject, ILogger logger)
            : base(baseObject)
        {
            this.logger = logger;
        }

        public void ExecuteSimpleReplaceUsingDatabase(string xpath, string query, TagContent tag, bool caseSensitive = false)
        {
            string patternTemplate = string.Empty;
            if (caseSensitive)
            {
                patternTemplate = "(?<!<[^>]+)\\b({0})\\b(?![^<>]*>)";
            }
            else
            {
                patternTemplate = "(?<!<[^>]+)\\b((?i){0})\\b(?![^<>]*>)";
            }

            try
            {
                string connectionString = this.Config.MainDictionaryDataSourceString;
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                XmlNodeList nodeList = this.XmlDocument.SelectNodes(xpath, this.NamespaceManager);
                XmlDocumentFragment fragment = this.XmlDocument.CreateDocumentFragment();

                using (connection)
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    SetTagAttributes(tag, reader);

                                    string replacement = tag.OpenTag + "$1" + tag.CloseTag;

                                    fragment.InnerText = reader.GetString(0);
                                    string contentString = Regex.Escape(fragment.InnerXml).RegexReplace("'", "\\W");

                                    string pattern = string.Format(patternTemplate, contentString);

                                    Regex matchEntry = new Regex(pattern);
                                    foreach (XmlNode node in nodeList)
                                    {
                                        if (matchEntry.IsMatch(node.InnerText))
                                        {
                                            node.InnerXml = matchEntry.Replace(node.InnerXml, replacement);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            this.logger?.Log(e, string.Empty);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void ExecuteSimpleReplaceUsingDatabase(string xpath, string query, string tagName)
        {
            TagContent tag = new TagContent(tagName);
            this.ExecuteSimpleReplaceUsingDatabase(xpath, query, tag);
        }

        private static void SetTagAttributes(TagContent tag, SqlDataReader reader)
        {
            if (reader.FieldCount > 1)
            {
                StringBuilder attributes = new StringBuilder();
                for (int i = 1, len = reader.FieldCount; i < len; ++i)
                {
                    string xmlEscapedContent = Regex.Replace(Regex.Replace(reader.GetString(i), "&", "&amp;"), @"""", "&quot;");
                    attributes.Append(string.Format(@" {0}=""{1}""", reader.GetName(i), xmlEscapedContent));
                }

                tag.Attributes = attributes.ToString();
            }
            else
            {
                tag.Attributes = string.Empty;
            }
        }
    }
}
