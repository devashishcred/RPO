using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.DocumentMaster.Models
{
    public class DocumentMasterDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        /// <value>The Name.</value>
        public string DocumentName { get; set; }


        /// <summary>
        /// Gets or sets the Item Name
        /// </summary>
        /// <value>The Item Name.</value>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        /// <value>The Code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Path
        /// </summary>
        /// <value>The Path.</value>
        public string Path { get; set; }

        public int DisplayOrder { get; set; }
        public bool? IsDevelopementCompleted { get; set; }
        public bool? IsAddPage { get; set; }
        public bool IsNewDocument { get; set; }
    }
}