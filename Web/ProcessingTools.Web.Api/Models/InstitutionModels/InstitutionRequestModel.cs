﻿namespace ProcessingTools.Web.Api.Models.InstitutionModels
{
    using System.ComponentModel.DataAnnotations;

    using Contracts.Mapping;
    using Services.Data.Models.Contracts;

    public class InstitutionRequestModel : IMapFrom<IInstitution>
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
    }
}