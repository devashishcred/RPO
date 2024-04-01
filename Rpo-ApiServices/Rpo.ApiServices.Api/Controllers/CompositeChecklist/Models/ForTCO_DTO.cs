using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist
{
    public class ForTCO_DTO
    {
        public bool IsRequiredForTCO { get; set; }
        public bool isplumbingitem { get; set; }
        public int IdJobchecklistitemdetail { get; set; }
    }

    public class ForViolationTCO_DTO
    {
        public bool IsRequiredForTCO { get; set; }       
        public int IdJobViolation { get; set; }
    }
    public class ForViolationStatus_DTO
    {
        public int Status { get; set; }
        public int IdJobViolation { get; set; }
    }
}