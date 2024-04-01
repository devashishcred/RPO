// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 06-05-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 06-05-2018
// ***********************************************************************
// <copyright file="JobWorkPermitHistoryController.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.JobWorkPermitHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Model;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class JobWorkPermitHistoryController.
    /// </summary>
    public class JobWorkPermitHistoryController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job work permit histories.
        /// </summary>
        /// <returns>IQueryable&lt;JobWorkPermitHistory&gt;.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<JobWorkPermitHistory> GetJobWorkPermitHistories()
        {
            return rpoContext.JobWorkPermitHistories.Include("CreatedByEmployee")
                .Include("JobApplicationWorkPermitTypes")
                .Include("JobApplications")
                .OrderByDescending(x => x.CreatedDate);
        }

    }
}
