﻿namespace ProcessingTools.Documents.Services.Data
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Models;

    using ProcessingTools.Common;
    using ProcessingTools.Common.Constants;
    using ProcessingTools.Common.Exceptions;
    using ProcessingTools.Data.Common.Repositories.Contracts;
    using ProcessingTools.Documents.Data.Common.Constants;
    using ProcessingTools.Documents.Data.Models;
    using ProcessingTools.Documents.Data.Repositories.Contracts;
    using ProcessingTools.Extensions;
    using ProcessingTools.FileSystem.Contracts;

    public class DocumentsDataService : IDocumentsDataService
    {
        private const string DefaultDataFilesDirectoryKey = "DefaultDataFilesDirectory";

        private readonly IDocumentsRepositoryProvider<Document> repositoryProvider;
        private readonly IXmlFileReaderWriter xmlFileReaderWriter;

        public DocumentsDataService(IDocumentsRepositoryProvider<Document> repositoryProvider, IXmlFileReaderWriter xmlFileReaderWriter)
        {
            if (repositoryProvider == null)
            {
                throw new ArgumentNullException(nameof(repositoryProvider));
            }

            if (xmlFileReaderWriter == null)
            {
                throw new ArgumentNullException(nameof(xmlFileReaderWriter));
            }

            this.repositoryProvider = repositoryProvider;
            this.xmlFileReaderWriter = xmlFileReaderWriter;
        }

        private string DataDirectory
        {
            get
            {
                string path = ConfigurationManager.AppSettings[DefaultDataFilesDirectoryKey];
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        public async Task<IQueryable<DocumentServiceModel>> All(object userId, object articleId, int pageNumber, int itemsPerPage)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (articleId == null)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            if (pageNumber < 0)
            {
                throw new InvalidPageNumberException();
            }

            if (1 > itemsPerPage || itemsPerPage > PagingConstants.MaximalItemsPerPageAllowed)
            {
                throw new InvalidItemsPerPageException();
            }

            var repository = this.repositoryProvider.Create();

            var documents = (await repository.All())
                .Where(d => d.CreatedByUser == userId.ToString())
                //// TODO // .Where(d => d.Article.Id.ToString() == articleId.ToString())
                .OrderByDescending(d => d.DateModified)
                .Skip(pageNumber * itemsPerPage)
                .Take(itemsPerPage)
                .Select(d => new DocumentServiceModel
                {
                    Id = d.Id.ToString(),
                    FileName = d.FileName,
                    FileExtension = d.FileExtension,
                    Comment = d.Comment,
                    ContentType = d.ContentType,
                    ContentLength = d.ContentLength,
                    DateCreated = d.DateCreated,
                    DateModified = d.DateModified
                })
                .ToList();

            repository.TryDispose();

            return documents.AsQueryable();
        }

        public async Task<long> Count(object userId, object articleId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (articleId == null)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            var repository = this.repositoryProvider.Create();

            long count = (await repository.All())
                .Where(d => d.CreatedByUser == userId.ToString())
                //// TODO // .Where(d => d.Article.Id.ToString() == articleId.ToString())
                .LongCount();

            repository.TryDispose();

            return count;
        }

        public async Task<object> Create(object userId, object articleId, DocumentServiceModel document, Stream inputStream)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (articleId == null)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            string path = await this.xmlFileReaderWriter.GetNewFilePath(document.FileName, this.DataDirectory, ValidationConstants.LengthOfDocumentFileName);

            var entity = new Document
            {
                FileName = document.FileName,
                OriginalFileName = document.FileName,

                ContentLength = document.ContentLength,
                OriginalContentLength = document.ContentLength,

                ContentType = document.ContentType,
                OriginalContentType = document.ContentType,

                FileExtension = document.FileExtension,
                OriginalFileExtension = document.FileExtension,

                Comment = document.Comment,

                CreatedByUser = userId.ToString(),
                ModifiedByUser = userId.ToString()
            };

            var repository = this.repositoryProvider.Create();

            entity.FileName = Path.GetFileNameWithoutExtension(path);
            entity.ContentLength = await this.xmlFileReaderWriter.Write(inputStream, entity.FileName, this.DataDirectory);

            await repository.Add(entity);
            await repository.SaveChanges();

            repository.TryDispose();

            return entity.ContentLength;
        }

        public async Task<object> Delete(object userId, object articleId, object documentId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (articleId == null)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            if (documentId == null)
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            var repository = this.repositoryProvider.Create();

            var entity = await this.GetEntity(userId, articleId, documentId, repository);

            await this.xmlFileReaderWriter.Delete(entity.FileName, this.DataDirectory);

            await repository.Delete(entity.Id);
            await repository.SaveChanges();

            repository.TryDispose();

            return documentId;
        }

        public async Task<DocumentServiceModel> Get(object userId, object articleId, object documentId)
        {
            var entity = await this.GetDocument(userId, articleId, documentId);
            return new DocumentServiceModel
            {
                Id = entity.Id.ToString(),
                Comment = entity.Comment,
                ContentLength = entity.ContentLength,
                ContentType = entity.ContentType,
                DateCreated = entity.DateCreated,
                DateModified = entity.DateModified,
                FileExtension = entity.FileExtension.Trim('.'),
                FileName = entity.OriginalFileName
            };
        }

        public async Task<XmlReader> GetReader(object userId, object articleId, object documentId)
        {
            var entity = await this.GetDocument(userId, articleId, documentId);
            return this.xmlFileReaderWriter.GetXmlReader(entity.FileName, this.DataDirectory);
        }

        public async Task<Stream> GetStream(object userId, object articleId, object documentId)
        {
            var entity = await this.GetDocument(userId, articleId, documentId);
            return this.xmlFileReaderWriter.ReadToStream(entity.FileName, this.DataDirectory);
        }

        public async Task<object> UpdateMeta(object userId, object articleId, DocumentServiceModel document)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (articleId == null)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var repository = this.repositoryProvider.Create();

            var entity = await this.GetEntity(userId, articleId, document.Id, repository);

            entity.FileExtension = document.FileExtension;
            entity.Comment = document.Comment;
            entity.ModifiedByUser = userId.ToString();
            entity.DateModified = DateTime.UtcNow;
            entity.ContentType = document.ContentType;

            await repository.Update(entity: entity);
            await repository.SaveChanges();

            repository.TryDispose();

            return entity.ContentLength;
        }

        public async Task<object> UpdateContent(object userId, object articleId, DocumentServiceModel document, string content)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (articleId == null)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var repository = this.repositoryProvider.Create();

            var entity = await this.GetEntity(userId, articleId, document.Id, repository);

            using (var stream = new MemoryStream(Defaults.DefaultEncoding.GetBytes(content)))
            {
                entity.ContentLength = await this.xmlFileReaderWriter.Write(stream, entity.FileName, this.DataDirectory);
                entity.ModifiedByUser = userId.ToString();
                entity.DateModified = DateTime.UtcNow;
                entity.ContentType = document.ContentType;
                stream.Close();
            }

            await repository.Update(entity: entity);
            await repository.SaveChanges();

            repository.TryDispose();

            return entity.ContentLength;
        }

        private async Task<Document> GetDocument(object userId, object articleId, object documentId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (articleId == null)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            if (documentId == null)
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            var repository = this.repositoryProvider.Create();

            var entity = await this.GetEntity(userId, articleId, documentId, repository);

            repository.TryDispose();

            return entity;
        }

        private async Task<Document> GetEntity(object userId, object articleId, object documentId, IGenericRepository<Document> repository)
        {
            var entity = (await repository.All())
                .Where(d => d.CreatedByUser == userId.ToString())
                //// TODO: // .Where(d => d.Article.Id.ToString() == articleId.ToString())
                .FirstOrDefault(d => d.Id.ToString() == documentId.ToString());
            if (entity == null)
            {
                repository.TryDispose();
                throw new EntityNotFoundException();
            }

            return entity;
        }
    }
}