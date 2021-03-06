﻿namespace ProcessingTools.Special.Processors.Contracts.Processors
{
    using ProcessingTools.Contracts;

    public interface ITestFeaturesProvider
    {
        void ExtractSystemChecklistAuthority(IDocument document);

        void MoveAuthorityTaxonNamePartToTaxonAuthorityTagInTaxPubTpNomenclaure(IDocument document);

        void RenumerateFootNotes(IDocument document);

        void WrapEmptySuperscriptsInFootnoteXrefTag(IDocument document);
    }
}
