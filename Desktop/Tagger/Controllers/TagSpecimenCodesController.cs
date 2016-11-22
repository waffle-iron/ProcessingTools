﻿namespace ProcessingTools.Tagger.Controllers
{
    using Contracts.Controllers;
    using Generics;

    using ProcessingTools.Attributes;
    using ProcessingTools.Processors.Contracts.Bio.Codes;

    [Description("Tag specimen codes.")]
    public class TagSpecimenCodesController : GenericDocumentTaggerController<ISpecimenCodesByPatternTagger>, ITagSpecimenCodesController
    {
        public TagSpecimenCodesController(ISpecimenCodesByPatternTagger tagger)
            : base(tagger)
        {
        }
    }
}