﻿namespace ProcessingTools.Contracts.Data.Journals.Models
{
    using System.Collections.Generic;
    using ProcessingTools.Contracts.Models;

    public interface IAddressable : IDataModel
    {
        ICollection<IAddress> Addresses { get; }
    }
}
