﻿namespace ProcessingTools.Processors.Processors.Validation
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Harvesters.Contracts.Harvesters.Bio;
    using ProcessingTools.Processors.Contracts.Processors.Validation;
    using ProcessingTools.Services.Validation.Contracts.Services;

    public class TaxaValidator : ITaxaValidator
    {
        private readonly ITaxonNamesHarvester harvester;
        private readonly ITaxaValidationService validationService;

        public TaxaValidator(
            ITaxonNamesHarvester harvester,
            ITaxaValidationService validationService)
        {
            this.harvester = harvester ?? throw new ArgumentNullException(nameof(harvester));
            this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        }

        public async Task<object> Validate(IDocument document, IReporter reporter)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (reporter == null)
            {
                throw new ArgumentNullException(nameof(reporter));
            }

            var data = await this.harvester.Harvest(document.XmlDocument);
            var scientificNames = data?.Distinct().ToArray();

            if (scientificNames == null || scientificNames.Length < 1)
            {
                reporter.AppendContent("Warning: No taxon names found.");
                return false;
            }

            var result = await this.validationService.Validate(scientificNames);

            var nonValidItems = result.Where(r => r.ValidationStatus != ValidationStatus.Valid)
                .Select(r => r.ValidatedObject)
                .OrderBy(i => i);

            reporter.AppendContent("Non-valid taxon names:");
            foreach (var taxonName in nonValidItems)
            {
                reporter.AppendContent(string.Format("\t{0}", taxonName));
            }

            return true;
        }
    }
}
