﻿namespace ProcessingTools.Data.Miners.Contracts.Models.Bio
{
    public interface IBiorepositoriesInstitution
    {
        string InstitutionalCode { get; }

        string NameOfInstitution { get; }

        string Url { get; }
    }
}
