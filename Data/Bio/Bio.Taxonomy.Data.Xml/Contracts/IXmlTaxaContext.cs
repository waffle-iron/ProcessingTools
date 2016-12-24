﻿namespace ProcessingTools.Bio.Taxonomy.Data.Xml.Contracts
{
    using ProcessingTools.Bio.Taxonomy.Data.Common.Contracts.Models;
    using ProcessingTools.Data.Common.File.Contracts;

    public interface IXmlTaxaContext : IFileDbContext<ITaxonRankEntity>
    {
    }
}