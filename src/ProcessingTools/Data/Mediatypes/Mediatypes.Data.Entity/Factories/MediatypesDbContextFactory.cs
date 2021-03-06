﻿namespace ProcessingTools.Mediatypes.Data.Entity.Factories
{
    using System;
    using Contracts;
    using ProcessingTools.Constants.Configuration;

    public class MediatypesDbContextFactory : IMediatypesDbContextFactory
    {
        private string connectionString;

        public MediatypesDbContextFactory()
        {
            this.ConnectionString = ConnectionStringsKeys.MediatypesDatabaseConnection;
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(this.ConnectionString));
                }

                this.connectionString = value;
            }
        }

        public MediatypesDbContext Create()
        {
            return new MediatypesDbContext(this.ConnectionString);
        }
    }
}
