﻿namespace ProcessingTools.Processors.Common.Bio.Taxonomy
{
    using System.Collections.Generic;
    using System.Linq;
    using ProcessingTools.Enumerations;

    internal static class SpeciesPartsPrefixesResolver
    {
        public static readonly IDictionary<string, SpeciesPartType> SpeciesPartsRanks = new Dictionary<string, SpeciesPartType>
        {
                { "?", SpeciesPartType.Species },
                { "a", SpeciesPartType.Aberration },
                { "ab", SpeciesPartType.Aberration },
                { "aff", SpeciesPartType.Species },
                { "afn", SpeciesPartType.Species },
                { "cf", SpeciesPartType.Species },
                { "f", SpeciesPartType.Form },
                { "fo", SpeciesPartType.Form },
                { "fa", SpeciesPartType.Form },
                { "form", SpeciesPartType.Form },
                { "forma", SpeciesPartType.Form },
                { "mod", SpeciesPartType.Form },
                { "gr", SpeciesPartType.Species },
                { "near", SpeciesPartType.Species },
                { "nr", SpeciesPartType.Species },
                { "prope", SpeciesPartType.Species },
                { "r", SpeciesPartType.Race },
                { "race", SpeciesPartType.Race },
                { "rassa", SpeciesPartType.Race },
                { "sec", SpeciesPartType.Section },
                { "secc", SpeciesPartType.Section },
                { "sect", SpeciesPartType.Section },
                { "section", SpeciesPartType.Section },
                { "ser", SpeciesPartType.Series },
                { "series", SpeciesPartType.Series },
                { "sf", SpeciesPartType.Subform },
                { "subf", SpeciesPartType.Subform },
                { "subfo", SpeciesPartType.Subform },
                { "subfa", SpeciesPartType.Subform },
                { "subform", SpeciesPartType.Subform },
                { "subforma", SpeciesPartType.Subform },
                { "sg", SpeciesPartType.Subgenus },
                { "sp", SpeciesPartType.Species },
                { "sp cf", SpeciesPartType.Species },
                { "sp. cf", SpeciesPartType.Species },
                { "sp aff", SpeciesPartType.Species },
                { "sp. aff", SpeciesPartType.Species },
                { "sp nr", SpeciesPartType.Species },
                { "sp. nr", SpeciesPartType.Species },
                { "sp near", SpeciesPartType.Species },
                { "sp. near", SpeciesPartType.Species },
                { "ssp", SpeciesPartType.Subspecies },
                { "sbsp", SpeciesPartType.Subspecies },
                { "st", SpeciesPartType.Stage },
                { "subg", SpeciesPartType.Subgenus },
                { "subgen", SpeciesPartType.Subgenus },
                { "subgenus", SpeciesPartType.Subgenus },
                { "subsec", SpeciesPartType.Subsection },
                { "subsecc", SpeciesPartType.Subsection },
                { "subsect", SpeciesPartType.Subsection },
                { "subsection", SpeciesPartType.Subsection },
                { "supersec", SpeciesPartType.Supersection },
                { "supersecc", SpeciesPartType.Supersection },
                { "supersect", SpeciesPartType.Supersection },
                { "supersection", SpeciesPartType.Supersection },
                { "subser", SpeciesPartType.Subseries },
                { "subseries", SpeciesPartType.Subseries },
                { "subsp", SpeciesPartType.Subspecies },
                { "subspec", SpeciesPartType.Subspecies },
                { "subspecies", SpeciesPartType.Subspecies },
                { "subvar", SpeciesPartType.Subvariety },
                { "sv", SpeciesPartType.Subvariety },
                { "trib", SpeciesPartType.Tribe },
                { "tribe", SpeciesPartType.Tribe },
                { "v", SpeciesPartType.Variety },
                { "var", SpeciesPartType.Variety },
                { "×", SpeciesPartType.Species }
            };

        public static readonly IEnumerable<string> UncertaintyPrefixes = new HashSet<string>
        {
            "?",
            "aff",
            "afn",
            "cf",
            "near",
            "nr",
            "sp aff",
            "sp cf",
            "sp near",
            "sp nr",
            "sp",
            "sp. aff",
            "sp. cf",
            "sp. near",
            "sp. nr"
        };

        private const string DefaultRank = "species";

        public static IEnumerable<KeyValuePair<string, SpeciesPartType>> NonAmbiguousSpeciesPartsRanks => SpeciesPartsRanks.Where(p => p.Key.Length > 1 && p.Key.IndexOf("trib") < 0 && p.Key != "near");

        public static string Resolve(string infraSpecificRank)
        {
            string rank;
            try
            {
                rank = SpeciesPartsRanks[infraSpecificRank.ToLower()].ToString().ToLower();
            }
            catch
            {
                rank = DefaultRank;
            }

            return rank;
        }
    }
}
