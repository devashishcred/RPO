// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-26-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-16-2018
// ***********************************************************************
// <copyright file="EmployeeDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Employee Detail.</summary>
// ***********************************************************************

/// <summary>
/// The Employees namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Employees
{
    /// <summary>
    /// Class Employee Detail.
    /// </summary>
    public class EmployeeDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the employee.
        /// </summary>
        /// <value>The name of the employee.</value>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public int IdEmployee { get; set; }

    }
}