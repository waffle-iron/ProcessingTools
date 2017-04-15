﻿namespace ProcessingTools.Contracts.Services.Data.Geo.Models
{
    using ProcessingTools.Contracts.Models;
    using System.Collections.Generic;

    public interface ICity : ISynonymisable<ICitySynonym>, INameableIntegerIdentifiable, IServiceModel
    {
        int CountryId { get; }

        int? StateId { get; }

        int? ProvinceId { get; }

        int? RegionId { get; }

        int? DistrictId { get; }

        int? MunicipalityId { get; }

        int? CountyId { get; }

        ICollection<IPostCode> PostCodes { get; }
    }
}
