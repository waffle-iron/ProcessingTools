﻿namespace ProcessingTools.Bio.Taxonomy.ServiceClient.CatalogueOfLife
{
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Models;
    using ProcessingTools.Infrastructure.Extensions;
    using ProcessingTools.Infrastructure.Net;

    /// <summary>
    /// Implementations of some of the Catalogue Of Life (CoL) API-s.
    /// </summary>
    public class CatalogueOfLifeDataRequester : ICatalogueOfLifeDataRequester
    {
        private const string CatalogueOfLifeBaseAddress = "http://www.catalogueoflife.org";

        /// <summary>
        /// Search scientific name in Catalogue Of Life (CoL).
        /// </summary>
        /// <param name="scientificName">Scientific name of the taxon which rank is searched.</param>
        /// <returns>XmlDocument of the CoL API response.</returns>
        /// <example>http://www.catalogueoflife.org/col/webservice?name=Tara+spinosa&response=full</example>
        public async Task<XmlDocument> RequestXmlFromCatalogueOfLife(string scientificName)
        {
            string url = $"col/webservice?name={scientificName}&response=full";

            try
            {
                var connector = new Connector(CatalogueOfLifeBaseAddress);
                string response = await connector.Get(url, Connector.XmlContentType);
                return response.ToXmlDocument();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Search scientific name in Catalogue Of Life (CoL).
        /// </summary>
        /// <param name="scientificName">Scientific name of the taxon which rank is searched.</param>
        /// <returns>CatalogueOfLifeApiServiceResponse of the CoL API response.</returns>
        /// <example>http://www.catalogueoflife.org/col/webservice?name=Tara+spinosa&response=full</example>
        public async Task<CatalogueOfLifeApiServiceResponse> RequestData(string scientificName)
        {
            string requestName = scientificName.UrlEncode();
            string url = $"/col/webservice?name={requestName}&response=full";

            try
            {
                var connector = new Connector(CatalogueOfLifeBaseAddress);
                var result = await connector.GetAndDeserializeXml<CatalogueOfLifeApiServiceResponse>(url);
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}