using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobDocument.Models
{
    public class PullPermitVARPMT
    {
        public int IdJobDocument { get; set; }

        public string DetailUrl { get; set; }

        public string ReferenceNumber { get; set; }
    }
    
    public class PullPermitResultVARPMT
    {
        public string JobDocumentUrl { get; set; }

        public bool IsPdfExist { get; set; }
    }
}