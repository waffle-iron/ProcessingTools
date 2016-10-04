﻿namespace ProcessingTools.Bio.Taxonomy.Data.Xml.Contracts
{
    using ProcessingTools.Bio.Taxonomy.Data.Common.Models.Contracts;
    using ProcessingTools.Data.Common.Contracts;
    using ProcessingTools.Data.Common.File.Contracts;

    public interface IXmlBiotaxonomicBlackListContextProvider : IFileDbContextProvider<IXmlBiotaxonomicBlackListContext, IBlackListEntity>, IDatabaseProvider<IXmlBiotaxonomicBlackListContext>
    {
    }
}