﻿namespace ProcessingTools.BaseLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class DocumentExtensions
    {
        /// <summary>
        /// Gets list of first words of a given list of strings.
        /// </summary>
        /// <param name="list">IEnumerable&lt;string&gt; object from which to extract first words.</param>
        /// <returns>IEnumerable&lt;string&gt; object containing every first word in the input list.</returns>
        public static IEnumerable<string> GetFirstWord(this IEnumerable<string> list)
        {
            return new HashSet<string>(list.Select(phrase => phrase.GetFirstWord()));
        }

        /// <summary>
        /// Gets the first word of a given string.
        /// </summary>
        /// <param name="phrase">String from which to extract the first word.</param>
        /// <returns>String of the first word.</returns>
        public static string GetFirstWord(this string phrase)
        {
            Regex matchWord = new Regex(@"\A(?:[^\W\d_]{1,3}\.|[^\W\d_]{2,})");
            return matchWord.Match(phrase).Value;
        }
    }
}
