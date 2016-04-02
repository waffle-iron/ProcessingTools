﻿namespace ProcessingTools.Bio.Taxonomy.Data.Repositories
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Contracts;
    using Models;
    using ProcessingTools.Configurator;

    public class TaxaRepository : ITaxaRepository
    {
        public TaxaRepository()
        {
            this.Config = ConfigBuilder.Create();
            this.TaxaWhiteListed = new ConcurrentDictionary<string, bool>();
            this.TaxaRanks = new ConcurrentDictionary<string, HashSet<string>>();
        }

        private Config Config { get; set; }

        private ConcurrentDictionary<string, bool> TaxaWhiteListed { get; set; }

        private ConcurrentDictionary<string, HashSet<string>> TaxaRanks { get; set; }

        public Task Add(Taxon entity)
        {
            return Task.Run(() => this.AddTaxon(entity));
        }

        public Task<IQueryable<Taxon>> All()
        {
            return Task.Run(() =>
            {
                this.ReadTaxaFromFile();

                return this.TaxaRanks
                    .Select(t => new Taxon
                    {
                        Name = t.Key,
                        Ranks = t.Value,
                        IsWhiteListed = this.TaxaWhiteListed.GetOrAdd(t.Key, false)
                    })
                    .AsQueryable();
            });
        }

        public Task<IQueryable<Taxon>> All(int skip, int take)
        {
            return Task.Run(() =>
            {
                this.ReadTaxaFromFile();
                return this.TaxaRanks
                    .OrderBy(t => t.Key)
                    .Skip(skip)
                    .Take(take)
                    .Select(t => new Taxon
                    {
                        Name = t.Key,
                        Ranks = t.Value,
                        IsWhiteListed = this.TaxaWhiteListed.GetOrAdd(t.Key, false)
                    })
                    .AsQueryable();
            });
        }

        public Task Delete(object id)
        {
            return Task.Run(() =>
            {
                this.ReadTaxaFromFile();
                var value = new HashSet<string>();
                this.TaxaRanks.TryRemove(id.ToString(), out value);
            });
        }

        public Task Delete(Taxon entity)
        {
            return this.Delete(entity.Name);
        }

        public Task<Taxon> Get(object id)
        {
            return Task.Run(() =>
            {
                this.ReadTaxaFromFile();
                var taxon = this.TaxaRanks
                    .Where(t => t.Key == id.ToString())
                    .Select(t => new Taxon
                    {
                        Name = t.Key,
                        Ranks = t.Value,
                        IsWhiteListed = this.TaxaWhiteListed.GetOrAdd(t.Key, false)
                    })
                    .FirstOrDefault();
                return taxon;
            });
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(this.WriteTaxaToFile());
        }

        public Task Update(Taxon entity)
        {
            return Task.Run(() =>
            {
                this.ReadTaxaFromFile();

                string name = entity.Name;

                this.TaxaWhiteListed.AddOrUpdate(name, entity.IsWhiteListed, (k, b) => entity.IsWhiteListed);

                var ranks = new HashSet<string>(entity.Ranks);
                if (this.TaxaRanks.ContainsKey(name))
                {
                    this.TaxaRanks[name] = ranks;
                }
                else
                {
                    this.TaxaRanks.GetOrAdd(name, ranks);
                }
            });
        }

        private void AddTaxon(Taxon taxon)
        {
            string name = taxon.Name;

            this.TaxaWhiteListed.GetOrAdd(name, taxon.IsWhiteListed);

            var ranksToAdd = new HashSet<string>(taxon.Ranks.Where(r => !string.IsNullOrWhiteSpace(r)));
            if (this.TaxaRanks.ContainsKey(name))
            {
                foreach (var rank in ranksToAdd)
                {
                    this.TaxaRanks[name].Add(rank);
                }
            }
            else
            {
                this.TaxaRanks.GetOrAdd(name, ranksToAdd);
            }
        }

        private void ReadTaxaFromFile()
        {
            XElement taxaList = XElement.Load(this.Config.RankListXmlFilePath);

            foreach (var taxon in taxaList.Descendants("taxon"))
            {
                string name = taxon.Element("part").Element("value").Value;

                string[] ranks = taxon.Element("part")
                    .Element("rank")
                    .Elements("value")
                    .Select(i => i.Value)
                    .ToArray();

                var taxonToAdd = new Taxon
                {
                    Name = name,
                    Ranks = ranks,
                    IsWhiteListed = false
                };

                string whiteListedAttribute = taxon.Attribute("white-listed")?.Value;

                bool whiteListed;
                if (bool.TryParse(whiteListedAttribute, out whiteListed))
                {
                    taxonToAdd.IsWhiteListed = whiteListed;
                }

                this.AddTaxon(taxonToAdd);
            }
        }

        private int WriteTaxaToFile()
        {
            this.ReadTaxaFromFile();

            var taxa = this.TaxaRanks
                .Select(pair =>
                {
                    var ranks = pair.Value.Select(r => new XElement("value", r)).ToArray();

                    XElement rank = new XElement("rank", ranks);
                    XElement name = new XElement("value", pair.Key);
                    XElement part = new XElement("part", name, rank);

                    XAttribute whiteListed = new XAttribute(
                        "white-listed",
                        this.TaxaWhiteListed.GetOrAdd(pair.Key, false));

                    return new XElement("taxon", whiteListed, part);
                })
                .ToArray();

            XElement taxaList = new XElement("taxa", taxa);

            taxaList.Save(this.Config.RankListXmlFilePath, SaveOptions.None);

            return taxaList.Elements().Count();
        }
    }
}