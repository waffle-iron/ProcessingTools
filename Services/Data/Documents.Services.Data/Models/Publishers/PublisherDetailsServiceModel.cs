﻿namespace ProcessingTools.Documents.Services.Data.Models.Publishers
{
    using System.Collections.Generic;

    public class PublisherDetailsServiceModel : PublisherServiceModel
    {
        public PublisherDetailsServiceModel()
            : base()
        {
            this.Addresses = new HashSet<IPublisherAddress>();
            this.Journals = new HashSet<JournalServiceModel>();
        }

        public ICollection<IPublisherAddress> Addresses { get; set; }

        public ICollection<JournalServiceModel> Journals { get; set; }
    }
}
