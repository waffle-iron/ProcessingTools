﻿namespace ProcessingTools.Bio.Taxonomy.Processors.Models.Parsers
{
    using System;
    using System.Linq;
    using System.Xml;

    using ProcessingTools.Bio.Taxonomy.Constants;
    using ProcessingTools.Bio.Taxonomy.Extensions;
    using ProcessingTools.Bio.Taxonomy.Types;

    internal class TaxonName : ITaxonName
    {
        private const long PositionDefaultValue = 0L;

        public TaxonName(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            this.Id = node.Attributes[XmlInternalSchemaConstants.IdAttributeName]?.InnerText ?? string.Empty;

            var typeAttribute = node.Attributes[XmlInternalSchemaConstants.TypeAttributeName];
            if (typeAttribute != null)
            {
                this.Type = typeAttribute.InnerText.MapTaxonTypeStringToTaxonType();
            }
            else
            {
                this.Type = TaxonType.Undefined;
            }

            var positionAttribute = node.Attributes[XmlInternalSchemaConstants.PositionAttributeName];
            long position = PositionDefaultValue;
            if (positionAttribute != null && long.TryParse(positionAttribute.InnerText, out position))
            {
                this.Position = position;
            }
            else
            {
                this.Position = PositionDefaultValue;
            }

            var parts = node.SelectNodes(XmlInternalSchemaConstants.TaxonNamePartElementName)
                .Cast<XmlNode>()
                .Select(n => new TaxonNamePart(n))
                .ToArray<ITaxonNamePart>();

            this.Parts = parts.AsQueryable();
        }

        public string Id { get; set; }

        public long Position { get; set; }

        public TaxonType Type { get; set; }

        public IQueryable<ITaxonNamePart> Parts { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} | {3}", this.Id, this.Type, this.Position, string.Join(" / ", this.Parts));
        }
    }
}
