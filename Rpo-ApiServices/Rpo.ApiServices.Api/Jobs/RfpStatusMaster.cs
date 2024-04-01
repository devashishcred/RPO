using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Jobs
{
    public enum RfpStatusMaster
    {
        In_Draft = 1,

        Pending_Review_By_RPO = 2,

        Submitted_To_Client = 3,

        Cancelled = 4,

        Lost = 5,

        On_Hold = 6,

        Accepted_By_Client_Pending_Retainer = 7,

        Reviewed_At_RPO = 8,

        Retained_Active = 9



        //In_Draft = 1,

        //Pending_Review = 2,

        //Submitted_To_Client = 3,

        //Cancelle = 4,

        //Lost = 5,

        //On_Hold = 6,

        //Accepted = 7,

        //Accepted_With_Changes = 8,
    }
}