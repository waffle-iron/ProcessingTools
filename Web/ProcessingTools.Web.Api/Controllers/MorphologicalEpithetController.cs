﻿namespace ProcessingTools.Web.Api.Controllers
{
    using Bio.Services.Data.Contracts;
    using Bio.Services.Data.Models.Contracts;
    using Factories;
    using Models.MorphologicalEpithetModels;

    public class MorphologicalEpithetController : GenericDataServiceControllerFactory<IMorphologicalEpithet, MorphologicalEpithetRequestModel, MorphologicalEpithetResponseModel>
    {
        public MorphologicalEpithetController(IMorphologicalEpithetsDataService service)
            : base(service)
        {
        }
    }
}