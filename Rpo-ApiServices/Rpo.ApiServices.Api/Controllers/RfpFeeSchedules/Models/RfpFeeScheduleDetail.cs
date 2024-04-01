

namespace Rpo.ApiServices.Api.Controllers.RfpFeeSchedules
{
    public class RfpFeeScheduleDetail
    {
        public int IdRfpFeeSchedule { get; set; }

        public string ItemName { get; set; }

        public int? IdPartof { get; set; }

        public int IdRFPWorkType { get; set; }

        public string RFPName { get; set; }
    }
}