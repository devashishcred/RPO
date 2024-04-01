
namespace Rpo.ApiServices.Api.Controllers.RfpAddresses
{
    /// <summary>
    /// Class BIS Address Request DTO.
    /// </summary>
    public class BisAddressRequestDTO
    {
        /// <summary>
        /// Gets or sets the house number.
        /// </summary>
        /// <value>The house number.</value>
        public string houseNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the street.
        /// </summary>
        /// <value>The name of the street.</value>
        public string streetName { get; set; }

        /// <summary>
        /// Gets or sets the borough.
        /// </summary>
        /// <value>The borough.</value>
        public string borough { get; set; }

        public bool isExactMatch { get; set; }
    }
}