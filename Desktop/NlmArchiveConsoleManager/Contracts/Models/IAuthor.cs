﻿namespace ProcessingTools.NlmArchiveConsoleManager.Contracts.Models
{
    public interface IAuthor
    {
        string Surname { get; set; }

        string GivenNames { get; set; }

        string Prefix { get; set; }

        string Suffix { get; set; }
    }
}