
// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 08-09-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 08-09-2023
// ***********************************************************************
// <copyright file="CustomerDetails.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class Employee Detail.</summary>
// ***********************************************************************

using System;
/// <summary>
/// The Employees namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Customer
    {
        /// <summary>
        /// Class Employee Detail.
        /// </summary>
        public class CustomerDetails
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
           // public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the name of the address1.
        /// </summary>
        /// <value>The name of the address1.</value>
        public string Address1 { get; set; }
        /// <summary>
        /// Gets or sets the name of the address2.
        /// </summary>
        /// <value>The name of the address2.</value>
        public string Address2 { get; set; }
        /// <summary>
        /// Gets or sets the name of the City.
        /// </summary>
        /// <value>The name of the City.</value>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the name of the IdState.
        /// </summary>
        /// <value>The name of the IdState.</value>
        public int IdState { get; set; }
        /// <summary>
        /// Gets or sets the name of the IdState.
        /// </summary>
        /// <value>The name of the IdState.</value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string LoginPassword { get; set; }

        /// <summary>
        /// Gets or sets the identifier IdCOntact.
        /// </summary>
        /// <value>The identifier IdCOntact.</value>
        public int IdContact{ get; set; }
        public string WorkPhone { get; set; }
        public string WorkPhoneExt { get; set; }
        public string MobilePhone { get; set; }
        public DateTime RenewalDate { get; set; }
        /// <summary>
        /// Gets or sets the contact image path.
        /// </summary>
        /// <value>The contact image path.</value>
  
        public string ContactImagePath { get; set; }

        /// <summary>
        /// Gets or sets the contact image thumb path.
        /// </summary>
        /// <value>The contact image thumb path.</value>
        
        public string ContactImageThumbPath { get; set; }
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public byte[] Image { get; set; }

    }
    }
