using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.NotificationSettings.Models
{
    public class CustomerNotificationDetails
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Id Customer.
        /// </summary>
        /// <value>The code.</value>

        public int IdCustomer { get; set; }

        /// <summary>
        /// Gets or sets the Project Access Email.
        /// </summary>
        /// <value>The code.</value>
        public bool ProjectAccessEmail { get; set; }
        /// <summary>
        /// Gets or sets the Project Access InApp.
        /// </summary>
        /// <value>The code.</value>
        public bool ProjectAccessInApp { get; set; }
        /// <summary>
        /// Gets or sets the Violation Email.
        /// </summary>
        /// <value>The code.</value>

        public bool ViolationEmail { get; set; }
        /// <summary>
        /// Gets or sets the Violation InApp by.
        /// </summary>
        /// <value>The created by.</value>
        public bool ViolationInApp { get; set; }

    }
}