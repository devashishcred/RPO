namespace Rpo.ApiServices.Api.Controllers.Contacts
{
    public class ContactDataTableParameters: DataTable.DataTableParameters
    {
        /// <summary>
        /// Gets or sets the type of the identifier contact license.
        /// </summary>
        /// <value>The type of the identifier contact license.</value>
        public int? IdContactLicenseType { get; set; }

        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the type of the global search.
        /// </summary>
        /// <value>The type of the global search.</value>
        public int? GlobalSearchType { get; set; }

        /// <summary>
        /// Gets or sets the global search text.
        /// </summary>
        /// <value>The global search text.</value>
        public string GlobalSearchText { get; set; }

        public int? Individual { get; set; }

    }
}