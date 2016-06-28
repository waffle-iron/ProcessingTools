﻿namespace ProcessingTools.Documents.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ProcessingTools.Common.Models;
    using ProcessingTools.Data.Common.Entity.Models.Contracts;
    using ProcessingTools.Documents.Data.Common.Constants;

    public class Institution : ModelWithUserInformation, IEntityWithPreJoinedFields
    {
        private ICollection<Address> addresses;
        private ICollection<Affiliation> affiliations;

        public Institution()
        {
            this.Id = Guid.NewGuid();
            this.addresses = new HashSet<Address>();
            this.affiliations = new HashSet<Affiliation>();
            this.PreJoinFieldNames = new string[] { nameof(this.Addresses), nameof(this.Affiliations) };
        }

        [Key]
        public Guid Id { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(ValidationConstants.MaximalLengthOfInstitutionName)]
        public string Name { get; set; }

        [MaxLength(ValidationConstants.MaximalLengthOfAbbreviatedInstitutionName)]
        public string AbbreviatedName { get; set; }

        public virtual ICollection<Address> Addresses
        {
            get
            {
                return this.addresses;
            }

            set
            {
                this.addresses = value;
            }
        }

        public virtual ICollection<Affiliation> Affiliations
        {
            get
            {
                return this.affiliations;
            }

            set
            {
                this.affiliations = value;
            }
        }

        [NotMapped]
        public IEnumerable<string> PreJoinFieldNames { get; private set; }
    }
}