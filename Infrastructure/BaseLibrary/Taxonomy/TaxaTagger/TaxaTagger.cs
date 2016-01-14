﻿namespace ProcessingTools.BaseLibrary.Taxonomy
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;

    using Bio.Taxonomy.Services.Data.Contracts;
    using Configurator;
    using Contracts;
    using Extensions;
    using ProcessingTools.Contracts;

    public abstract class TaxaTagger : HarvestableDocument, ITagger
    {
        protected const string HigherTaxaReplacePattern = "<tn type=\"higher\">$1</tn>";
        protected const string LowerRaxaReplacePattern = "<tn type=\"lower\">$1</tn>";

        private IRepositoryDataService<string> blackList;

        public TaxaTagger(Config config, string xml, IRepositoryDataService<string> blackList)
            : base(config, xml)
        {
            this.BlackList = blackList;
        }

        public TaxaTagger(IBase baseObject, IRepositoryDataService<string> blackList)
            : base(baseObject)
        {
            this.BlackList = blackList;
        }

        protected IRepositoryDataService<string> BlackList
        {
            get
            {
                return this.blackList;
            }

            private set
            {
                this.blackList = value;
            }
        }

        public abstract void Tag();

        protected IEnumerable<string> ClearFakeTaxaNames(IEnumerable<string> taxaNames)
        {
            var result = taxaNames;

            try
            {
                var taxaNamesFirstWord = new HashSet<string>(result
                        .GetFirstWord()
                        .Select(Regex.Escape));

                result = this.ClearFakeTaxaNamesLikePersonNamesInArticle(result, taxaNamesFirstWord);
                result = this.ClearFakeTaxaNamesUsingBlackList(result, taxaNamesFirstWord);
            }
            catch
            {
                throw;
            }

            return new HashSet<string>(result);
        }

        private IEnumerable<string> ClearFakeTaxaNamesUsingBlackList(IEnumerable<string> taxaNames, HashSet<string> taxaNamesFirstWord)
        {
            var blackListedNames = new HashSet<string>(taxaNamesFirstWord
                .MatchWithStringList(this.BlackList.All().ToList(), true, false, true));

            var result = taxaNames
                .Where(name => !blackListedNames.Contains(name.GetFirstWord()));

            return new HashSet<string>(result);
        }

        private IEnumerable<string> ClearFakeTaxaNamesLikePersonNamesInArticle(IEnumerable<string> taxaNames, IEnumerable<string> taxaNamesFirstWord)
        {
            var taxaLikePersonNameParts = new HashSet<string>(this.XmlDocument
                .SelectNodes("//surname[string-length(normalize-space(.)) > 2]|//given-names[string-length(normalize-space(.)) > 2]")
                .Cast<XmlNode>()
                .Select(s => s.InnerText)
                .MatchWithStringList(taxaNamesFirstWord, false, true, true)
                .Select(Regex.Escape));

            var result = new HashSet<string>(taxaNames
                .DistinctWithStringList(taxaLikePersonNameParts, true, false, true));

            return result;
        }
    }
}