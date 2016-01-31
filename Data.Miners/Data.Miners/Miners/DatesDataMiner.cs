﻿/*
 01. Oct. 1930
 24.- 29.09.1929
 19/August/2002
 2012/12/10
 15th October 2014
 2nd March 2015
 July 01.2015
 26–30. June, 2014
 29th of April, 2015
 13-III-08-V-1998
 30Aug1923
*/

namespace ProcessingTools.Data.Miners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Extensions;
    using ProcessingTools.Data.Miners.Contracts;

    public class DatesDataMiner : IDatesDataMiner
    {
        public async Task<IQueryable<string>> Mine(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException("content");
            }

            var internalMiner = new InternalMiner(content);

            var itemsDayMonthNumberYear = await internalMiner.MineDayMonthNumberYear();
            var itemsMonthStringDayYear = await internalMiner.MineMonthStringDayYear();
            var itemsDayMonthRomanYear = await internalMiner.MineDayMonthRomanYear();
            var itemsDayMonthStringYear = await internalMiner.MineDayMonthStringYear();

            var items = new List<string>();
            items.AddRange(itemsDayMonthNumberYear);
            items.AddRange(itemsMonthStringDayYear);
            items.AddRange(itemsDayMonthRomanYear);
            items.AddRange(itemsDayMonthStringYear);

            var result = new HashSet<string>(items);
            return result.AsQueryable();
        }

        private class InternalMiner
        {
            private string content;

            public InternalMiner(string content)
            {
                this.content = content;
            }

            /// <summary>
            /// Finds dates of format DD [mounth as arabic number] YYYY in text and adds them in List dates.
            /// </summary>
            /// <returns>IEnumerable of matches.</returns>
            /// <example>16.6.2013</example>
            public async Task<IEnumerable<string>> MineDayMonthNumberYear()
            {
                const string Pattern = @"((?i)(?:(?:(?:(?:[1-2][0-9]|3[0-1]|0?[1-9])(?:\s*[–—−‒-]\s*))+|(?<![^\s–—−‒-])(?:0[1-9]|[1-2][0-9]|3[0-1]|[1-9]))[^\w<>]{0,4})?\b(?:1[0-2]|0[1-9]|[1-9])\b[^\w<>]{0,4}(?:1[6-9][0-9]|20[0-9])[0-9](?![0-9]))";

                return await this.content.GetMatchesAsync(new Regex(Pattern));
            }

            /// <summary>
            /// Finds dates of format [mounth string] DD YYYY in text and adds them in List dates.
            /// </summary>
            /// <returns>IEnumerable of matches.</returns>
            /// <example>March 12.2014</example>
            public async Task<IEnumerable<string>> MineMonthStringDayYear()
            {
                const string Pattern = @"((?i)(?:(?:Jan(?:uary)?|Febr?(?:uary)?|Mar(?:ch)?|Apr(?:il)?|May|June?|July?|Aug(?:ust)?|Sept?(?:ember)?|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?)\s*(?:[–—−‒-]|to)\s*)+(?:(?:(?:(?:[1-2][0-9]|3[0-1]|0?[1-9])(?:\s*[–—−‒-]\s*))+|(?<!\S)(?:[1-2][0-9]|3[0-1]|0?[1-9]))[^\w<>]{0,4})?[^\w<>]{0,4}(?:1[6-9][0-9]|20[0-9])[0-9](?![0-9]))";

                return await this.content.GetMatchesAsync(new Regex(Pattern));
            }

            /// <summary>
            /// Finds dates of format DD [mounth in roman] YYYY in text and adds them in List dates.
            /// </summary>
            /// <returns>IEnumerable of matches.</returns>
            /// <example>22–25.I.2007</example>
            public async Task<IEnumerable<string>> MineDayMonthRomanYear()
            {
                ////const string Pattern = @"((?i)(?:(?:(?:(?:[1-2][0-9]|3[0-1]|0?[1-9])(?:\s*[–—−‒-]\s*))+|(?<!\S)(?:[1-2][0-9]|3[0-1]|0?[1-9]))[^\w<>]{0,4})?(?<![a-z])(?:I|II|III|IV|V|VI|VII|VIII|IX|X|XI|XII)[^\w<>]{0,4}(?:1[6-9][0-9]|20[0-9])[0-9](?![0-9]))";
                const string Pattern = @"((?i)(?:(?:(?:(?:[1-2][0-9]|3[0-1]|0?[1-9])(?:\s*[–—−‒-]\s*))+|(?<![^\s–—−‒-])(?:[1-2][0-9]|3[0-1]|0?[1-9]))[^\w<>]{0,4})?\b(?:I|II|III|IV|V|VI|VII|VIII|IX|X|XI|XII)\b[^\w<>]{0,4}(?:1[6-9][0-9]|20[0-9])[0-9](?![0-9]))";

                return await this.content.GetMatchesAsync(new Regex(Pattern));
            }

            /// <summary>
            /// Finds dates of format DD [mounth string] YYYY in text and adds them in List dates.
            /// </summary>
            /// <returns>IEnumerable of matches.</returns>
            /// <example>24–30 March 2013</example>
            /// <example>18 Jan 2008</example>
            public async Task<IEnumerable<string>> MineDayMonthStringYear()
            {
                const string Pattern = @"((?i)(?:(?:(?:(?:[1-2][0-9]|3[0-1]|0?[1-9])(?:\s*[–—−‒-]\s*))+|(?<!\S)(?:[1-2][0-9]|3[0-1]|0?[1-9]))[^\w<>]{0,4})?(?:(?:Jan(?:uary)?|Febr?(?:uary)?|Mar(?:ch)?|Apr(?:il)?|May|June?|July?|Aug(?:ust)?|Sept?(?:ember)?|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?)\s*(?:[–—−‒-]|to)\s*)+[^\w<>]{0,4}(?:1[6-9][0-9]|20[0-9])[0-9](?![0-9]))";

                return await this.content.GetMatchesAsync(new Regex(Pattern));
            }
        }
    }
}