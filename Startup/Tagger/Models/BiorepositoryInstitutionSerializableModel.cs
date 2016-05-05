﻿namespace ProcessingTools.Tagger.Models
{
    using System.Xml.Serialization;
    using ProcessingTools.Nlm.Publishing.Constants;

    [XmlType(AnonymousType = true)]
    [XmlRoot(NodeNames.NamedContent, Namespace = "", IsNullable = false)]
    public class BiorepositoryInstitutionSerializableModel : NamedContentSerializableModel
    {
        [XmlAttribute(AttributeNames.ContentType)]
        public override string ContentType
        {
            get
            {
                return XmlNodesSettings.Default.BiorepositoriesInstitutionContentType;
            }

            set
            {
            }
        }
    }
}
