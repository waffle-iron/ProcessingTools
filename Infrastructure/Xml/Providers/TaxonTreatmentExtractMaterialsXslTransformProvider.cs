﻿namespace ProcessingTools.Xml.Providers
{
    using System.Configuration;

    using Contracts.Cache;
    using Contracts.Providers;
    using Abstracts;

    using ProcessingTools.Constants.Configuration;

    public class TaxonTreatmentExtractMaterialsXslTransformProvider : AbstractXslTransformProvider, ITaxonTreatmentExtractMaterialsXslTransformProvider
    {
        public TaxonTreatmentExtractMaterialsXslTransformProvider(IXslTransformCache cache)
            : base(cache)
        {
        }

        protected override string XslFileName => ConfigurationManager.AppSettings[AppSettingsKeys.TaxonTreatmentExtractMaterialsXslPathKey];
    }
}
