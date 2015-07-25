﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ProcessingTools.Base
{
    public class Test : Base
    {
        public Test()
            : base()
        {
        }

        public Test(string xml)
            : base(xml)
        {
        }

        public void ExtractSystemChecklistAuthority1()
        {
            this.ParseXmlStringToXmlDocument();
            XmlDocument newXml = new XmlDocument(this.NamespaceManager.NameTable);
            XmlElement root = newXml.CreateElement("root");

            foreach (XmlNode node in this.xmlDocument.SelectNodes("//fields[taxon_authors_and_year[normalize-space(.)!='']]", this.NamespaceManager))
            {
                XmlElement newNode = newXml.CreateElement("node");
                newNode.InnerXml = node["taxon_authors_and_year"].OuterXml;
                root.AppendChild(newNode);
            }

            newXml.AppendChild(root);

            this.xml = newXml.OuterXml;
        }

        public void ExtractSystemChecklistAuthority()
        {
            this.ParseXmlStringToXmlDocument();

            foreach (XmlNode node in xmlDocument.SelectNodes("//fields/taxon_authors_and_year/value[normalize-space(.)!='']", this.NamespaceManager))
            {
                node.InnerText = Regex.Replace(node.InnerText, @"\s+and\s+", " &amp; ");
                node.InnerText = Regex.Replace(node.InnerText, @"(?<=[^,])\s+(?=\d)", ", ");
            }

            this.xml = this.xmlDocument.OuterXml;
        }

        public void SqlSelect()
        {
            using (SqlConnection connection = new SqlConnection(this.Config.environmentsDataSourceString))
            {
                Alert.Log(connection.ConnectionString);
                Alert.Log(connection.Database);
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
                            Alert.Log(reader.GetString(0) + " " + reader.GetInt32(1));
                        }
                    }
                }

                connection.Dispose();
                connection.Close();
            }
        }
    }
}
