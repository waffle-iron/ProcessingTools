﻿namespace ProcessingTools.Geo.Data.Seed.Seeders
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Constants.Configuration;
    using ProcessingTools.Data.Common.Entity.Seed;
    using ProcessingTools.Geo.Data.Entity;
    using ProcessingTools.Geo.Data.Entity.Contracts;
    using ProcessingTools.Geo.Data.Entity.Models;
    using ProcessingTools.Geo.Data.Seed.Contracts;

    public class GeoDataSeeder : IGeoDataSeeder
    {
        private const string UserName = "system";
        private static readonly DateTime Now = DateTime.UtcNow;

        private readonly IGeoDbContextFactory contextFactory;
        private readonly Type stringType = typeof(string);

        private FileByLineDbContextSeeder<GeoDbContext> seeder;
        private string dataFilesDirectoryPath;
        private ConcurrentQueue<Exception> exceptions;

        public GeoDataSeeder(IGeoDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            this.seeder = new FileByLineDbContextSeeder<GeoDbContext>(this.contextFactory);

            this.dataFilesDirectoryPath = ConfigurationManager.AppSettings[AppSettingsKeys.DataFilesDirectoryName];
            this.exceptions = new ConcurrentQueue<Exception>();
        }

        // TODO: Link countries and continents
        public async Task<object> Seed()
        {
            this.exceptions = new ConcurrentQueue<Exception>();

            var tasks = new Task[]
            {
                this.SeedGeoNames(ConfigurationManager.AppSettings[AppSettingsKeys.GeoNamesSeedFileName]),
                this.SeedGeoEpithets(ConfigurationManager.AppSettings[AppSettingsKeys.GeoEpithetsSeedFileName]),
                this.SeedContinents(ConfigurationManager.AppSettings[AppSettingsKeys.ContinentsCodesSeedFileName]),
                this.SeedCountryCodes(ConfigurationManager.AppSettings[AppSettingsKeys.CountryCodesSeedFileName])
            };

            await Task.WhenAll(tasks);

            if (this.exceptions.Count > 0)
            {
                throw new AggregateException(this.exceptions);
            }

            return true;
        }

        private async Task SeedGeoNames(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            try
            {
                await this.seeder.ImportSingleLineTextObjectsFromFile(
                    $"{this.dataFilesDirectoryPath}/{fileName}",
                    (context, line) =>
                    {
                        context.GeoNames.AddOrUpdate(new GeoName
                        {
                            Name = line,
                            CreatedBy = UserName,
                            CreatedOn = Now,
                            ModifiedBy = UserName,
                            ModifiedOn = Now
                        });
                    });
            }
            catch (Exception e)
            {
                this.exceptions.Enqueue(e);
            }
        }

        private async Task SeedGeoEpithets(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            try
            {
                await this.seeder.ImportSingleLineTextObjectsFromFile(
                    $"{this.dataFilesDirectoryPath}/{fileName}",
                    (context, line) =>
                    {
                        context.GeoEpithets.AddOrUpdate(new GeoEpithet
                        {
                            Name = line,
                            CreatedBy = UserName,
                            CreatedOn = Now,
                            ModifiedBy = UserName,
                            ModifiedOn = Now
                        });
                    });
            }
            catch (Exception e)
            {
                this.exceptions.Enqueue(e);
            }
        }

        private async Task SeedContinents(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            try
            {
                await this.seeder.ImportSingleLineTextObjectsFromFile(
                    $"{this.dataFilesDirectoryPath}/{fileName}",
                    (context, line) =>
                    {
                        var data = line.Split('\t');
                        if (data.Length > 0)
                        {
                            context.Continents.AddOrUpdate(new Continent
                            {
                                Name = data[0],
                                CreatedBy = UserName,
                                CreatedOn = Now,
                                ModifiedBy = UserName,
                                ModifiedOn = Now
                            });
                        }
                    });
            }
            catch (Exception e)
            {
                this.exceptions.Enqueue(e);
            }
        }

        private async Task SeedCountryCodes(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            try
            {
                await this.seeder.ImportSingleLineTextObjectsFromFile(
                    $"{this.dataFilesDirectoryPath}/{fileName}",
                    (context, line) =>
                    {
                        var data = line.Split('\t');
                        if (data.Length > 2 && data[0].Trim().Length > 1)
                        {
                            var languageCodes = data[2].Split('/').Select(l => l.Trim()).ToArray();
                            context.Countries.AddOrUpdate(new Country
                            {
                                Name = data[0],
                                CallingCode = data[1],
                                LanguageCode = languageCodes[0],
                                Iso639xCode = languageCodes[1],
                                CreatedBy = UserName,
                                CreatedOn = Now,
                                ModifiedBy = UserName,
                                ModifiedOn = Now
                            });
                        }
                    });
            }
            catch (Exception e)
            {
                this.exceptions.Enqueue(e);
            }
        }
    }
}