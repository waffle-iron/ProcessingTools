﻿namespace ProcessingTools.Web.Api.Controllers
{
    using Factories;
    using Models.InstitutionModels;
    using Services.Data.Contracts;
    using Services.Data.Models.Contracts;

    public class InstitutionController : GenericDataServiceControllerFactory<IInstitution, InstitutionRequestModel, InstitutionResponseModel>
    {
        public InstitutionController(IInstitutionsDataService service)
        {
            this.Service = service;
        }
    }
}