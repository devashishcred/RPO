// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 04-25-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-25-2018
// ***********************************************************************
// <copyright file="OpenMapAddress.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Open Map Address.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class Open Map Address.
    /// </summary>
    public class OpenMapAddress
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the borough.
        /// </summary>
        /// <value>The borough.</value>
        public string Borough { get; set; }

        /// <summary>
        /// Gets or sets the house number.
        /// </summary>
        /// <value>The house number.</value>
        public string HouseNumber_Street { get; set; }

        /// <summary>
        /// Gets or sets the zone district.
        /// </summary>
        /// <value>The zone district.</value>
        public string ZoneDistrict { get; set; }

        /// <summary>
        /// Gets or sets the overlay.
        /// </summary>
        /// <value>The overlay.</value>
        public string Overlay { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <value>The map.</value>
        public string Map { get; set; }

        /// <summary>
        /// Gets or sets the stories.
        /// </summary>
        /// <value>The stories.</value>
        public string Stories { get; set; }

        /// <summary>
        /// Gets or sets the dwelling units.
        /// </summary>
        /// <value>The dwelling units.</value>
        public string DwellingUnits { get; set; }

        /// <summary>
        /// Gets or sets the gross area.
        /// </summary>
        /// <value>The gross area.</value>
        public string GrossArea { get; set; }

        public string Block { get; set; }

        public string Lot { get; set; }
    }
}
