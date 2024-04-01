using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.NewsLetter.Models
{
    public class NewsLetterDTO
    {     
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>    
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the Title.
            /// </summary>
            /// <value>The code.</value>

            public string Title { get; set; }

            /// <summary>
            /// Gets or sets the Image.
            /// </summary>
            /// <value>The code.</value>
            ///  public string Image { get; set; }
            public string NewsImagePath { get; set; }

            public string URL { get; set; }
            /// <summary>
            /// Gets or sets the Title.
            /// </summary>
            /// <value>The code.</value>

            public string Description { get; set; }
            /// <summary>
            /// Gets or sets the created by.
            /// </summary>
            /// <value>The created by.</value>
            public int? CreatedBy { get; set; }
            /// <summary>
            /// Gets or sets the name of the created by employee.
            /// </summary>
            /// <value>The name of the created by employee.</value>
            public string CreatedByEmployeeName { get; set; }

            /// <summary>
            /// Gets or sets the created date.
            /// </summary>
            /// <value>The created date.</value>
            public DateTime? CreatedDate { get; set; }
        }
}