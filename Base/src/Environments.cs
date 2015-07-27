﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ProcessingTools.Base
{
    public class Environments : Base
    {
        public Environments(string xml)
            : base(xml)
        {
        }

        public Environments(Config config, string xml)
            : base(config, xml)
        {
        }

        public void TagEnvironmentsRecords()
        {
            this.xml = Base.NormalizeNlmToSystemXml(this.Config, this.xml);
            this.ParseXmlStringToXmlDocument();

            XmlElement testXmlNode = this.xmlDocument.CreateElement("test");

            // Set the XPath string to select all nodes which may contain environments’ strings
            string xpath = string.Empty;
            if (this.Config.NlmStyle)
            {
                xpath = "//p|//title|//label|//tp:nomenclature-citation";
            }
            else
            {
                // TODO
                xpath = "//p";
            }

            try
            {
                // Select all nodes which potentioaly contains environments’ records
                XmlNodeList nodeList = this.xmlDocument.SelectNodes(xpath, this.NamespaceManager);

                // Connect to Environments database and use its records to tag Xml
                using (SqlConnection connection = new SqlConnection(this.Config.environmentsDataSourceString))
                {
                    connection.Open();
                    string query = @"select
                                 [dbo].[environments_names].[Content] as content,
                                 [dbo].[environments_names].[ContentId] as id,
                                 [dbo].[environments_entities].[EnvoId] as envoId
                                from [dbo].[environments_names]
                                inner join [dbo].[environments_entities]
                                on [dbo].[environments_names].[ContentId]=[dbo].[environments_entities].[Id]
                                where content not like 'ENVO%'
                                order by len(content) desc;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // For each environments’ record in database try to tag the Xml content
                                foreach (XmlNode node in nodeList)
                                {
                                    testXmlNode.InnerText = reader.GetString(0);
                                    string contentString = testXmlNode.InnerText;
                                    string envoId = "ENVO_" + reader.GetString(2).Substring(5);
                                    string envoTagName = "envo";
                                    string envoOpenTag = "<" + envoTagName +
                                        ////@" EnvoTermUri=""http://purl.obolibrary.org/obo/" + envoId + @"""" + 
                                        @" EnvoTermUri=""" + envoId + @"""" +
                                        @" ID=""" + reader.GetInt32(1) + @"""" +
                                        @" EnvoID=""" + envoId + @"""" +
                                        @" VerbatimTerm=""" + reader.GetString(0) + @"""" +
                                        @">";

                                    string pattern = "\\b((?i)" + Regex.Replace(Regex.Escape(contentString), "'", "\\W") + ")\\b";
                                    if (Regex.Match(node.InnerText, pattern).Success)
                                    {
                                        // The text content of the current node contains this environment string
                                        pattern = "(?<!<[^>]+)" + pattern + "(?![^<>]*>)";
                                        string replace = node.InnerXml;

                                        if (!Regex.Match(node.InnerXml, pattern).Success)
                                        {
                                            // The xml-as-string content of the current node soes not contain this environment string
                                            // Here we suppose that there is some tag inside the environment-string in the xml node.

                                            // Tag the xml-node-content using non-regex skip-tag matches
                                            replace = this.TagNodeContent(node.InnerXml, contentString, envoOpenTag);
                                            replace = this.TagOrderNormalizer(replace, envoTagName);
                                        }
                                        else
                                        {
                                            replace = Regex.Replace(node.InnerXml, pattern, envoOpenTag + "$1</" + envoTagName + ">");
                                        }

                                        node.InnerXml = replace;
                                    }
                                }
                            }
                        }
                    }

                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 1);
            }

            this.xmlDocument.InnerXml = Regex.Replace(this.xmlDocument.InnerXml, @"(?<=\sEnvoTermUri="")", "http://purl.obolibrary.org/obo/");
            this.xml = this.xmlDocument.OuterXml;
            if (this.Config.NlmStyle)
            {
                this.xml = Base.NormalizeSystemToNlmXml(this.Config, this.xml);
            }
        }
    }
}
