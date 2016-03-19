﻿namespace ProcessingTools.Documents.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class Address
    {
        private ICollection<Institution> institutions;
        private ICollection<Affiliation> affiliations;

        public Address()
        {
            this.institutions = new HashSet<Institution>();
            this.affiliations = new HashSet<Affiliation>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.MaximalLengthOfAddressString)]
        public string AddressString { get; set; }

        public virtual int CityId { get; set; }

        public virtual City City { get; set; }

        public virtual int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Institution> Institutions
        {
            get
            {
                return this.institutions;
            }

            set
            {
                this.institutions = value;
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
    }
}