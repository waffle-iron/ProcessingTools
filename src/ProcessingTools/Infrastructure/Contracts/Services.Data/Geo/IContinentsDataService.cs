﻿namespace ProcessingTools.Contracts.Services.Data.Geo
{
    using ProcessingTools.Contracts.Filters.Geo;
    using ProcessingTools.Contracts.Models.Geo;

    public interface IContinentsDataService : IDataServiceAsync<IContinent, IContinentsFilter>, IGeoSynonymisableDataService<IContinent, IContinentSynonym, IContinentSynonymsFilter>
    {
    }
}
