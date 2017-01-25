﻿namespace ProcessingTools.Services.Validation.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Models;
    using Models.Contracts;
    using ProcessingTools.Common.Collections;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Services.Cache.Contracts.Validation;
    using ProcessingTools.Services.Cache.Models.Validation;

    public abstract class AbstractValidationService<TValidatedObject, TItemToCheck> : IValidationService<TValidatedObject>
    {
        private readonly IValidationCacheService cacheService;

        public AbstractValidationService(IValidationCacheService cacheService)
        {
            if (cacheService == null)
            {
                throw new ArgumentNullException(nameof(cacheService));
            }

            this.cacheService = cacheService;
            this.CacheServiceIsUsable = true;
        }

        protected IValidationCacheService CacheService => this.cacheService;

        protected bool CacheServiceIsUsable { get; private set; }

        protected abstract Func<TItemToCheck, string> GetContextKey { get; }

        protected abstract Func<TValidatedObject, TItemToCheck> GetItemToCheck { get; }

        protected abstract Func<TItemToCheck, TValidatedObject> GetValidatedObject { get; }

        protected Func<TItemToCheck, IValidationServiceModel<TValidatedObject>> MapToValidationServiceModel => item => new ValidationServiceModel<TValidatedObject>
        {
            ValidatedObject = this.GetValidatedObject.Invoke(item),
            ValidationException = null,
            ValidationStatus = ValidationStatus.Valid
        };

        public async Task<IEnumerable<IValidationServiceModel<TValidatedObject>>> Validate(params TValidatedObject[] items)
        {
            var validatedItems = new ConcurrentQueueCollection<IValidationServiceModel<TValidatedObject>>();

            if (items == null || items.Length < 1)
            {
                return validatedItems;
            }

            var itemsToCheck = await this.TryValidationWithCache(items, validatedItems);
            if (itemsToCheck.Count() > 0)
            {
                await this.Validate(itemsToCheck, validatedItems);
            }

            return validatedItems.ToArray();
        }

        private async Task<IEnumerable<TItemToCheck>> TryValidationWithCache(
            IEnumerable<TValidatedObject> items,
            ICollection<IValidationServiceModel<TValidatedObject>> validatedItems)
        {
            var itemsToCheck = new ConcurrentQueueCollection<TItemToCheck>();
            try
            {
                await this.ValidateItemsFromCache(items, validatedItems, itemsToCheck);
                this.CacheServiceIsUsable = true;
            }
            catch
            {
                itemsToCheck = new ConcurrentQueueCollection<TItemToCheck>(items.Select(this.GetItemToCheck));
                this.CacheServiceIsUsable = false;
            }

            return itemsToCheck.ToArray();
        }

        protected virtual async Task AddItemToCache(IValidationServiceModel<TValidatedObject> item)
        {
            if (!this.CacheServiceIsUsable)
            {
                return;
            }

            if (item == null)
            {
                return;
            }

            string contextKey = this.GetContextKey(this.GetItemToCheck(item.ValidatedObject));
            if (string.IsNullOrWhiteSpace(contextKey))
            {
                return;
            }

            try
            {
                // TODO: DateTime.UtcNow
                var model = new ValidationCacheServiceModel
                {
                    Status = item.ValidationStatus,
                    LastUpdate = DateTime.UtcNow
                };

                await this.CacheService.Add(contextKey, model);
            }
            catch
            {
                this.CacheServiceIsUsable = false;
            }
        }

        protected abstract Task Validate(IEnumerable<TItemToCheck> items, ICollection<IValidationServiceModel<TValidatedObject>> validatedItems);

        private async Task ValidateItemsFromCache(
            IEnumerable<TValidatedObject> items,
            ICollection<IValidationServiceModel<TValidatedObject>> validatedItems,
            ICollection<TItemToCheck> itemsToCheck)
        {
            if (!this.CacheServiceIsUsable)
            {
                return;
            }

            if (items == null)
            {
                return;
            }

            var tasks = items.Select(this.GetItemToCheck)
                .Select(item => this.ValidateSingleItem(item, validatedItems, itemsToCheck))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private async Task ValidateSingleItem(
            TItemToCheck item,
            ICollection<IValidationServiceModel<TValidatedObject>> validatedItems,
            ICollection<TItemToCheck> itemsToCheck)
        {
            if (!this.CacheServiceIsUsable)
            {
                return;
            }

            string contextKey = this.GetContextKey(item);
            if (string.IsNullOrWhiteSpace(contextKey))
            {
                return;
            }

            var cachedItem = await cacheService.Get(contextKey);
            if (cachedItem != null && cachedItem.Status == ValidationStatus.Valid)
            {
                var model = this.MapToValidationServiceModel(item);
                validatedItems.Add(model);
                return;
            }

            itemsToCheck.Add(item);
        }
    }
}