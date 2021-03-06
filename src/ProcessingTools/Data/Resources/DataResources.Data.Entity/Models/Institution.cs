﻿namespace ProcessingTools.DataResources.Data.Entity.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ProcessingTools.Constants.Data.DataResources;
    using ProcessingTools.Contracts.Data.DataResources.Models;

    public class Institution : EntityWithSources, IInstitutionEntity
    {
        private string name;

        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(ValidationConstants.InstitutionNameMaximalLength)]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value.Trim(' ', ',', ';', ':', '/', '\\');
            }
        }
    }
}
