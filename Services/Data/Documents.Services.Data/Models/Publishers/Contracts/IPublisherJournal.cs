﻿namespace ProcessingTools.Documents.Services.Data.Models.Publishers.Contracts
{
    using ProcessingTools.Contracts.Models;

    public interface IPublisherJournal : IGuidIdentifiable
    {
        string Name { get; }
    }
}
