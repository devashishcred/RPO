
namespace Rpo.ApiServices.Api.Controllers.Dashboards
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Api.Jobs;
    using DataTable;
    using Enums;
    using Filters;
    using Model.Models;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Model.Models.Enums;

    [Authorize]
    public class DashboardHearingDatesController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Get the JobViolations
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get the deshboard JobViolations grater than current date </returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetDashboardHearingDates([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var recordsTotal = rpoContext.JobViolations.Count();
            var recordsFiltered = recordsTotal;

            //IQueryable<JobViolation> jobViolations = rpoContext.JobViolations.Include("Job.RfpAddress.Borough").AsQueryable();
            //IQueryable<JobViolation> jobViolations = rpoContext.JobViolations.Include("RfpAddress").Include("Job").Where(x => x.Job.Status == JobStatus.Active).AsQueryable();

            List <int> all = new List<int>();
            // DateTime monthbefore=  DateTime.Now.AddDays(-30);

            //    var days30 = jobViolations.Where(t=> DbFunctions.TruncateTime(t.CreatedDate) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(t.CreatedDate) > DbFunctions.TruncateTime(monthbefore)).AsQueryable();
            //  var days30 = jobViolations.Where(t => DbFunctions.TruncateTime(t.CreatedDate) <= DbFunctions.TruncateTime(DateTime.Now) && DbFunctions.TruncateTime(t.CreatedDate) > DbFunctions.TruncateTime(monthbefore)).ToList();
            //   var ecb = days30.Where(t => (DbFunctions.TruncateTime(t.HearingDate) >= DbFunctions.TruncateTime(DateTime.Now)) && t.IsFullyResolved == false).Select(x => x.Id).ToList();
            var ecb = rpoContext.JobViolations.Include("RfpAddress").Include("Job").Where(t => (DbFunctions.TruncateTime(t.HearingDate) >= DbFunctions.TruncateTime(DateTime.Now)) && t.IsFullyResolved == false && t.Job.Status==JobStatus.Active).Select(x => x.Id).ToList();

            foreach (var e in ecb)
            {
                all.Add(e);
            }
            //  var dob = days30.Where(t => t.IsFullyResolved == false && t.Type_ECB_DOB== "DOB").Select(x => x.Id).ToList();

            //foreach (var d in dob)
            //{
            //    all.Add(d);
            //}
            //all.Distinct();

            var v = rpoContext.JobViolations.Where(x => all.Contains(x.Id)).ToList();
            //previous  jobViolations = jobViolations.Where(t =>( DbFunctions.TruncateTime(t.HearingDate) >= DbFunctions.TruncateTime(DateTime.Now) && t.IsFullyResolved == false) && t.DateIssued <= DbFunctions.TruncateTime(DateTime.Now));

            // IQueryable<UpcomingHearingDateDTO> upcomingHearingDatelists = jobViolations.AsEnumerable().Select(j => FormatUpcomingHearingDate(j)).AsQueryable();
            IQueryable<UpcomingHearingDateDTO> upcomingHearingDatelists = v.AsEnumerable().Select(j => FormatUpcomingHearingDate(j)).AsQueryable();

            var result = upcomingHearingDatelists
                           .AsEnumerable()
                           .Select(j => j)
                           .AsQueryable()
                           .DataTableParameters(dataTableParameters, out recordsFiltered)
                           .ToList();

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result.OrderBy(x => x.HearingDate)
            });
        }

        private UpcomingHearingDateDTO FormatUpcomingHearingDate(JobViolation jobViolation)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string JobApplicationType = string.Empty;
            if (jobViolation.IdJob != null && jobViolation.Job != null)
            {
                JobApplicationType = string.Join(",", jobViolation.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            string binNumber = rpoContext.JobViolations.Where(i => i.SummonsNumber == jobViolation.SummonsNumber).Select(i => i.BinNumber).FirstOrDefault();
            var address = rpoContext.RfpAddresses.Where(b => b.BinNumber == binNumber).ToList();
            List<int> lstjobids = new List<int>();
            string Projectnumbers = null;
            //foreach (var a in address)
            //{
            //    List<int> jobids = rpoContext.Jobs.Where(j => j.IdRfpAddress == a.Id).Select(i => i.Id).ToList();
            //    lstjobids.AddRange(jobids);

            //}
            String eachaddress = "";
            foreach (var a in address)
            {
                List<int> jobids = rpoContext.Jobs.Where(j => j.IdRfpAddress == a.Id && j.Status==JobStatus.Active).Select(i => i.Id).ToList();
                if (jobids.Count > 0)
                {
                    lstjobids.AddRange(jobids);

                    List<string> alladdresses = new List<string>();
                    var RfpAddress = rpoContext.RfpAddresses.Where(j => j.Id == a.Id).FirstOrDefault();
                    var boroughDescription = rpoContext.Boroughes.Where(x => x.Id == RfpAddress.IdBorough).Select(y => y.Description).FirstOrDefault();
                    //eachaddress += RfpAddress != null ? RfpAddress.HouseNumber + " " + RfpAddress.Street + (RfpAddress.Borough != null ? " " + RfpAddress.Borough.Description +", " : string.Empty) : string.Empty;
                    eachaddress += RfpAddress != null ? RfpAddress.HouseNumber + " " + (RfpAddress.Street != null ? RfpAddress.Street : string.Empty)
                   //+ (RfpAddress.Borough != null ? RfpAddress.Borough.Description : string.Empty)+"), ") : string.Empty;
                   + " " + boroughDescription + ", " : string.Empty;
                }
                // alladdresses.Add(eachaddress);
            }
            if(eachaddress!="")
            eachaddress = eachaddress.Remove(eachaddress.Length - 2, 2);
            foreach (var j in lstjobids)
            {
                if (jobViolation.Type_ECB_DOB == "DOB")
                    Projectnumbers += "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "/job/" + j + "/violation?highlighted=" + jobViolation.SummonsNumber + "&isDob=true" + "\">" + j + "</a>, ";
                else if (jobViolation.Type_ECB_DOB == "ECB")
                    Projectnumbers += "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "/job/" + j + "/violation?highlighted=" + jobViolation.SummonsNumber + "\">" + j + "</a>, ";
            }
            if (Projectnumbers != null)
                Projectnumbers = Projectnumbers.Remove(Projectnumbers.Length - 2, 2);
            return new UpcomingHearingDateDTO
            {

                Id = jobViolation.Id,
                // IdJob = jobViolation.IdJob,
                IdJob = Projectnumbers,
                JobApplicationType = JobApplicationType,
                JobNumber = jobViolation.Job != null ? jobViolation.Job.JobNumber : string.Empty,
                //  Address = jobViolation.Job != null && jobViolation.Job.RfpAddress != null ?
                //     (jobViolation.Job.RfpAddress.HouseNumber + " " + (jobViolation.Job.RfpAddress.Street != null ? jobViolation.Job.RfpAddress.Street + ", " : string.Empty)
                //  + (jobViolation.Job.RfpAddress.Borough != null ? jobViolation.Job.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                Address = eachaddress,
                SummonsNumber = jobViolation.SummonsNumber,
                BalanceDue = jobViolation.BalanceDue,
                DateIssued = jobViolation.DateIssued,
                RespondentName = jobViolation.RespondentName,
                HearingDate = jobViolation.HearingDate,
                CureDate = jobViolation.CureDate,
                HearingLocation = jobViolation.HearingLocation,
                HearingResult = jobViolation.HearingResult,
                InspectionLocation = jobViolation.InspectionLocation,
                IssuingAgency = jobViolation.IssuingAgency,
                RespondentAddress = jobViolation.RespondentAddress,
                StatusOfSummonsNotice = jobViolation.StatusOfSummonsNotice,
                ComplianceOn = jobViolation.ComplianceOn,
                ResolvedDate = jobViolation.ResolvedDate,
                IsFullyResolved = jobViolation.IsFullyResolved,
                CertificationStatus = jobViolation.CertificationStatus,
                Notes = jobViolation.Notes,
                IsCOC = jobViolation.IsCOC,
                COCDate = jobViolation.COCDate,
                PaneltyAmount = jobViolation.PaneltyAmount,
            };
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.rpoContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }


}