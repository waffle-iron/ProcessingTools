﻿namespace ProcessingTools.Data.Miners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Contracts;
    using Extensions;
    using Models;
    using Models.Contracts;
    using Nlm.Publishing.Types;

    public class NlmExternalLinksDataMiner : INlmExternalLinksDataMiner
    {
        private const string UriPatternSuffix = @"(?=(?:&gt;|>)?\]?[,;\.]*[\)\s]+|[\.;\)]$|$)";
        private const string IPAddressPattern = @"\b(?:(?:(?:0?0?[0-9]|0?[0-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\.){3,3}(?:0?0?[0-9]|0?[0-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))\b";

        private const string HttpPattern = @"(?i)https?://(?:www)?\S+?" + UriPatternSuffix + @"|" +
            @"(?i)(?<!://)www\S+?" + UriPatternSuffix + @"|" +
            IPAddressPattern + @"(?:\D\S*)" + UriPatternSuffix;

        private const string FtpPattern = @"(?i)s?ftp://(?:www)?\S+?" + UriPatternSuffix;

        private const string DoiPattern = @"(?i)(?<=\bdoi\W{0,3})\d+\S+" + UriPatternSuffix + @"|" +
            @"10\.\d{4,5}/\S+" + UriPatternSuffix;

        private const string PmidPattern = @"(?i)(?<=\bpmid\W?)\d+";

        private const string PmcidPattern = @"(?i)\bpmc\W?\d+|(?i)(?<=\bpmcid\W?)\d+";

        public NlmExternalLinksDataMiner()
        {
        }

        public async Task<IQueryable<INlmExternalLink>> Mine(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException("content");
            }

            var internalMiner = new InternalMiner(content);

            var doiItems = await internalMiner.MineDoi();
            var uriItems = await internalMiner.MineUri();
            var ftpItems = await internalMiner.MineFtp();
            var pmidItems = await internalMiner.MinePmid();
            var pmcidItems = await internalMiner.MinePmcid();

            var items = new List<INlmExternalLink>();
            items.AddRange(pmcidItems);
            items.AddRange(pmidItems);
            items.AddRange(ftpItems);
            items.AddRange(uriItems);
            items.AddRange(doiItems);

            var result = new HashSet<INlmExternalLink>(items);
            return result.AsQueryable();
        }

        private class InternalMiner
        {
            private string content;

            public InternalMiner(string content)
            {
                this.content = content;
            }

            public async Task<IEnumerable<INlmExternalLink>> MineUri()
            {
                var matches = await this.content.GetMatchesAsync(new Regex(HttpPattern));
                return matches.Select(item => new NlmExternalLinkResponseModel
                {
                    Content = item,
                    Type = ExternalLinkType.Uri
                });
            }

            public async Task<IEnumerable<INlmExternalLink>> MineFtp()
            {
                var matches = await this.content.GetMatchesAsync(new Regex(FtpPattern));
                return matches.Select(item => new NlmExternalLinkResponseModel
                {
                    Content = item,
                    Type = ExternalLinkType.Ftp
                });
            }

            public async Task<IEnumerable<INlmExternalLink>> MineDoi()
            {
                var matches = await this.content.GetMatchesAsync(new Regex(DoiPattern));
                return matches.Select(item => new NlmExternalLinkResponseModel
                {
                    Content = item,
                    Type = ExternalLinkType.Doi
                });
            }

            public async Task<IEnumerable<INlmExternalLink>> MinePmid()
            {
                var matches = await this.content.GetMatchesAsync(new Regex(PmidPattern));
                return matches.Select(item => new NlmExternalLinkResponseModel
                {
                    Content = item,
                    Type = ExternalLinkType.Pmid
                });
            }

            public async Task<IEnumerable<INlmExternalLink>> MinePmcid()
            {
                var matches = await this.content.GetMatchesAsync(new Regex(PmcidPattern));
                return matches.Select(item => new NlmExternalLinkResponseModel
                {
                    Content = item,
                    Type = ExternalLinkType.Pmcid
                });
            }
        }
    }
}