﻿namespace ProcessingTools.Processors.Models.Floats
{
    using ProcessingTools.Nlm.Publishing.Constants;
    using Types;

    /// <summary>
    /// Appendix.
    /// </summary>
    internal class AppendixFloatObject : IFloatObject
    {
        public string FloatObjectXPath => $".//app[contains(string(title),'{this.FloatTypeNameInLabel}')]";

        public FloatsReferenceType FloatReferenceType => FloatsReferenceType.Appendix;

        public string FloatTypeNameInLabel => "Append";

        public string MatchCitationPattern => @"(?:Append(?:\.|[a-z]+))";

        public string InternalReferenceType => "app";

        public string ResultantReferenceType => RefTypeAttributeValues.Appendix;

        public string Description => "Appendix";
    }
}