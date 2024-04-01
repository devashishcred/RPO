using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Model.Models;
namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class CompositeViolationDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>   
        public int Id { get; set; }

        public int IdCompositeChecklist { get; set; }
        public int IdJobViolations { get; set; }
        public virtual JobViolation JobViolation { get; set; }
       
    }
}