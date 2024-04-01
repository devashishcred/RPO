// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-06-2018
// ***********************************************************************
// <copyright file="CompanyItem.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Company Item.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Companies
{
    /// <summary>
    /// Class Company Item.
    /// </summary>
    public class CompanyItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }
    }
}