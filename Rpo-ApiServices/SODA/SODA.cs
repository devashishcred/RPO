using SODA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SODA
{
    public class SodaClient
    {
        /// <summary>
        /// The url to the Socrata Open Data Portal this client targets.
        /// </summary>
        public readonly string Host;

        /// <summary>
        /// The Socrata application token that this client uses for all requests.
        /// </summary>
        /// <remarks>
        /// Socrata Application Tokens are not required, but are recommended for expanded API quotas.
        /// See https://dev.socrata.com/docs/app-tokens.html for more information.
        /// </remarks>
        public readonly string AppToken;

        /// <summary>
        /// The user account that this client uses for Authentication during each request.
        /// </summary>
        /// <remarks>
        /// Authentication is only necessary when accessing datasets that have been marked as private or when making write requests (PUT, POST, and DELETE).
        /// See http://dev.socrata.com/docs/authentication.html for more information.
        /// </remarks>
        public readonly string Username;

        //not publicly readable, can only be set in a constructor
        private readonly string password;

        /// <summary>
        /// If set, the number of milliseconds to wait before requests to the <see cref="Host"/> timeout and throw a <see cref="System.Net.WebException"/>.
        /// If unset, the default value is that of <see cref="System.Net.HttpWebRequest.Timeout"/>.
        /// </summary>
        public int? RequestTimeout { get; set; }

        /// <summary>
        /// Initialize a new SodaClient for the specified Socrata host, using the specified application token and Authentication credentials.
        /// </summary>
        /// <param name="host">The Socrata Open Data Portal that this client will target.</param>
        /// <param name="appToken">The Socrata application token that this client will use for all requests.</param>
        /// <param name="username">The user account that this client will use for Authentication during each request.</param>
        /// <param name="password">The password for the specified <paramref name="username"/> that this client will use for Authentication during each request.</param>
        /// <exception cref="System.ArgumentException">Thrown if no <paramref name="host"/> is provided.</exception>
        public SodaClient(string host, string appToken, string username, string password)
        {
            if (String.IsNullOrEmpty(host))
                throw new ArgumentException("host", "A host is required");

            Host = SodaUri.enforceHttps(host);
            AppToken = appToken;
            Username = username;
            this.password = password;
        }

        /// <summary>
        /// Initialize a new SodaClient for the specified Socrata host, using the specified Authentication credentials.
        /// </summary>
        /// <param name="host">The Socrata Open Data Portal that this client will target.</param>
        /// <param name="username">The user account that this client will use for Authentication during each request.</param>
        /// <param name="password">The password for the specified <paramref name="username"/> that this client will use for Authentication during each request.</param>
        /// <exception cref="System.ArgumentException">Thrown if no <paramref name="host"/> is provided.</exception>
        public SodaClient(string host, string username, string password)
            : this(host, null, username, password)
        {
        }

        /// <summary>
        /// Initialize a new (anonymous) SodaClient for the specified Socrata host, using the specified application token.
        /// </summary>
        /// <param name="host">The Socrata Open Data Portal that this client will target.</param>
        /// <param name="appToken">The Socrata application token that this client will use for all requests.</param>
        /// <exception cref="System.ArgumentException">Thrown if no <paramref name="host"/> is provided.</exception>
        public SodaClient(string host, string appToken = null)
            : this(host, appToken, null, null)
        {
        }

        /// <summary>
        /// Get a ResourceMetadata object using the specified resource identifier.
        /// </summary>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>
        /// A ResourceMetadata object for the specified resource identifier.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        public ResourceMetadata GetMetadata(string resourceId)
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            var uri = SodaUri.ForMetadata(Host, resourceId);

            var metadata = read<ResourceMetadata>(uri);
            metadata.Client = this;

            return metadata;
        }

        /// <summary>
        /// Get a collection of ResourceMetadata objects on the specified page.
        /// </summary>
        /// <param name="page">The 1-indexed page of the Metadata Catalog on this client's Socrata host.</param>
        /// <returns>A collection of ResourceMetadata objects from the specified page of this client's Socrata host.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="page"/> is zero or negative.</exception>
        public IEnumerable<ResourceMetadata> GetMetadataPage(int page)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException("page", "Resouce metadata catalogs begin on page 1.");

            var catalogUri = SodaUri.ForMetadataList(Host, page);

            //an entry of raw data contains some, but not all, of the fields required to populate a ResourceMetadata
            IEnumerable<dynamic> rawDataList = read<IEnumerable<dynamic>>(catalogUri).ToArray();
            //so loop over the collection, using the identifier to make another call for the "real" metadata
            foreach (var rawData in rawDataList)
            {
                var metadata = GetMetadata((string)rawData.id);
                //yield return here creates an interator - results aren't returned until explicitly requested via foreach
                //or similar interation on the result of the call to GetMetadataPage.
                yield return metadata;
            }
        }

        /// <summary>
        /// Get a Resource object using the specified resource identifier.
        /// </summary>
        /// <typeparam name="TRow">The .NET class that represents the type of the underlying row in the Resource.</typeparam>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A Resource object with an underlying row set of type <typeparamref name="TRow"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        public Resource<TRow> GetResource<TRow>(string resourceId) where TRow : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            return new Resource<TRow>(resourceId, this);
        }

        /// <summary>
        /// Query using the specified <see cref="SoqlQuery"/> against the specified resource identifier.
        /// </summary>
        /// <typeparam name="TRow">The .NET class that represents the type of the underlying rows in the result set of this query.</typeparam>
        /// <param name="soqlQuery">A <see cref="SoqlQuery"/> to execute against the Resource.</param>
        /// <param name="resourceId">The identifier (4x4) for a resource on the Socrata host to target.</param>
        /// <returns>A collection of entities of type <typeparamref name="TRow"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified <paramref name="resourceId"/> does not match the Socrata 4x4 pattern.</exception>
        /// <remarks>
        /// By default, Socrata will only return the first 1000 rows unless otherwise specified in SoQL using the Limit and Offset parameters.
        /// This method checks the specified SoqlQuery object for either the Limit or Offset parameter, and honors those parameters if present.
        /// If both Limit and Offset are not part of the SoqlQuery, this method attempts to retrieve all rows in the dataset across all pages.
        /// In other words, this method hides the fact that Socrata will only return 1000 rows at a time, unless explicity told not to via the SoqlQuery argument.
        /// </remarks>
        public IEnumerable<TRow> Query<TRow>(SoqlQuery soqlQuery, string resourceId) where TRow : class
        {
            if (FourByFour.IsNotValid(resourceId))
                throw new ArgumentOutOfRangeException("resourceId", "The provided resourceId is not a valid Socrata (4x4) resource identifier.");

            //if the query explicitly asks for a limit/offset, honor the ask
            if (soqlQuery.LimitValue > 0 || soqlQuery.OffsetValue > 0)
            {
                var queryUri = SodaUri.ForQuery(Host, resourceId, soqlQuery);
                return read<IEnumerable<TRow>>(queryUri);
            }
            //otherwise, go nuts and get EVERYTHING
            else
            {
                List<TRow> allResults = new List<TRow>();
                int offset = 0;

                soqlQuery = soqlQuery.Limit(SoqlQuery.MaximumLimit).Offset(offset);
                IEnumerable<TRow> offsetResults = read<IEnumerable<TRow>>(SodaUri.ForQuery(Host, resourceId, soqlQuery));

                while (offsetResults.Any())
                {
                    allResults.AddRange(offsetResults);
                    soqlQuery = soqlQuery.Offset(++offset * SoqlQuery.MaximumLimit);
                    offsetResults = read<IEnumerable<TRow>>(SodaUri.ForQuery(Host, resourceId, soqlQuery));
                }

                return allResults;
            }
        }

       
    }
}

