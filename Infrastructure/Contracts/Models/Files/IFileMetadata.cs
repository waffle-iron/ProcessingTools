﻿namespace ProcessingTools.Contracts.Models.Files
{
    public interface IFileMetadata : IObjectIdentifiable, IDescribable, IContentTypable, IModelWithUserInformation
    {
        long ContentLength { get; }

        string FileExtension { get; }

        string FileName { get; }

        string FullName { get; }
    }
}