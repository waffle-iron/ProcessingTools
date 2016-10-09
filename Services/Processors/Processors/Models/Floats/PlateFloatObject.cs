﻿namespace ProcessingTools.Processors.Models.Floats
{
    using ProcessingTools.Nlm.Publishing.Constants;
    using Types;

    /// <summary>
    /// Plate.
    /// </summary>
    internal class PlateFloatObject : IFloatObject
    {
        public string FloatObjectXPath => $".//fig[contains(string(label),'{this.FloatTypeNameInLabel}')]";

        public FloatsReferenceType FloatReferenceType => FloatsReferenceType.Figure;

        public string FloatTypeNameInLabel => "Plate";

        public string MatchCitationPattern => @"(?:Plates?)";

        public string InternalReferenceType => "plate";

        public string ResultantReferenceType => RefTypeAttributeValues.Figure;

        public string Description => "Plate";
    }
}