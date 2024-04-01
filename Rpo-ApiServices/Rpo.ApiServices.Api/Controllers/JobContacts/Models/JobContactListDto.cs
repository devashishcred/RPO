// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="JobContactListDto.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Contact List DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobContacts
{
    /// <summary>
    /// Class Job Contact List DTO.
    /// </summary>
    public class JobContactListDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? idContact { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? idJob { get; set; }

        /// <summary>
        /// Gets or sets the type of the jobcontact.
        /// </summary>
        /// <value>The type of the jobcontact.</value>
        public string jobcontactType { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        /// <value>The name of the contact.</value>
        public string contactName { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string companyName { get; set; }

        /// <summary>
        /// Gets or sets the work phone.
        /// </summary>
        /// <value>The work phone.</value>
        public string workPhone { get; set; }
        

        /// <summary>
        /// Gets or sets the ext.
        /// </summary>
        /// <value>The ext.</value>
        public string ext { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string email { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is billing.
        /// </summary>
        /// <value><c>true</c> if this instance is billing; otherwise, <c>false</c>.</value>
        public bool IsBilling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is main company.
        /// </summary>
        /// <value><c>true</c> if this instance is main company; otherwise, <c>false</c>.</value>
        public bool IsMainCompany { get; set; }

        public string GroupName { get; set; }

        public string itemName { get; set; }

        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
        public string IsAuthorized { get; set; }
        /// <summary>
        /// Gets or sets the work phone.
        /// </summary>
        /// <value>The work phone.</value>
        public string mobilePhone { get; set; }


    }
}