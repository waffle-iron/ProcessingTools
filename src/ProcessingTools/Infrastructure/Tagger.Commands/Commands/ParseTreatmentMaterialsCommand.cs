﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using Contracts.Commands;
    using Generics;
    using ProcessingTools.Attributes;
    using ProcessingTools.Processors.Contracts.Processors.Bio.Materials;

    [Description("Parse treatment materials.")]
    public class ParseTreatmentMaterialsCommand : GenericDocumentParserCommand<ITreatmentMaterialsParser>, IParseTreatmentMaterialsCommand
    {
        public ParseTreatmentMaterialsCommand(ITreatmentMaterialsParser parser)
            : base(parser)
        {
        }
    }
}