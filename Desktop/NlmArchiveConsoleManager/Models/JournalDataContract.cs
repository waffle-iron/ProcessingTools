﻿namespace ProcessingTools.NlmArchiveConsoleManager.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class JournalDataContract
    {
        [DataMember(Name = "journalId")]
        public string JournalId { get; set; }

        [DataMember(Name = "journalTitle")]
        public string JournalTitle { get; set; }

        [DataMember(Name = "abbreviatedJournalTitle")]
        public string AbbreviatedJournalTitle { get; set; }

        [DataMember(Name = "issnPPub")]
        public string IssnPPub { get; set; }

        [DataMember(Name = "issnEPub")]
        public string IssnEPub { get; set; }

        [DataMember(Name = "publisherName")]
        public string PublisherName { get; set; }

        [DataMember(Name = "fileNamePattern")]
        public string FileNamePattern { get; set; }
    }
}