﻿namespace ProcessingTools.MediaType.Services.Data.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;
    using Models;

    public abstract class MediaTypeDataServiceBase : IMediaTypeDataService
    {
        protected const string DefaultMediaType = "unknown/unknown";
        protected const string DefaultMediaTypeOnException = "application/octet-stream";

        public abstract Task<IQueryable<MediaTypeServiceModel>> GetMediaType(string fileExtension);

        protected string GetValidFileExtension(string fileExtension)
        {
            string extension = fileExtension?.TrimStart('.', ' ', '\n', '\r');
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentNullException(nameof(fileExtension));
            }

            return extension;
        }

        protected IQueryable<MediaTypeServiceModel> GetSingleStringMediaTypeResultAsQueryable(string extension, string mediaType)
        {
            var result = new Queue<MediaTypeServiceModel>();

            int slashIndex = mediaType.IndexOf('/');
            result.Enqueue(new MediaTypeServiceModel
            {
                FileExtension = extension,
                MimeType = mediaType.Substring(0, slashIndex),
                MimeSubtype = mediaType.Substring(slashIndex + 1)
            });

            return result.AsQueryable();
        }
    }
}