﻿namespace ProcessingTools.Services.Providers
{
    using System;
    using ProcessingTools.Contracts;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
