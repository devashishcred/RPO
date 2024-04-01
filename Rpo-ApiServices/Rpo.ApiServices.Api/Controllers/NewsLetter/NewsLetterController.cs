// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 02-10-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 02-10-2023
// ***********************************************************************
// <copyright file="NewsLetterController.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class News Controller.</summary>
// ***********************************************************************
namespace Rpo.ApiServices.Api.Controllers.NewsLetter
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Permissions;   
    using Rpo.ApiServices.Api.Controllers.NewsLetter.Models;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using Tools;
    /// <summary>
    /// Class News Letter Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class NewsLetterController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        //[Route("api/News/GetNewsLetterList")]
        [HttpGet]
        public IHttpActionResult GetNewsLetter([FromUri] DataTableParameters dataTableParameters)
        {
            var NewsLetterList = rpoContext.News.Include("CreatedByEmployee").AsQueryable();

            var recordsTotal = NewsLetterList.Count();
            var recordsFiltered = recordsTotal;

            var result = NewsLetterList
                .AsEnumerable()
                .Select(c => Format(c))
                .AsQueryable()
                 .DataTableParameters(dataTableParameters, out recordsFiltered)
                .ToArray();

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result
            });
          //  return Ok(result);
        }
        /// <summary>
        /// Gets the NewsLetter
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the News in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(NewsLetterDetail))]
        [HttpGet]
        [Route("api/News/GetNewsLetter")]
        public IHttpActionResult GetNewsLetter()
        {
            var news = rpoContext.News.Include("CreatedByEmployee").ToList().OrderByDescending(x=>x.Id).FirstOrDefault();

            if (news == null)
            {
                return this.NotFound();
            }
            else
            return Ok(FormatDetails(news));
           
        }
        private NewsLetterDetail FormatDetails(News news)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new NewsLetterDetail
            {
                Id = news.Id,
                Title = news.Title,
                NewsImagePath = news.NewsImagePath,
                URL = news.URL,
                Description = news.Description,
                CreatedBy = news.CreatedBy,
                CreatedByEmployeeName = news.CreatedByEmployee != null ? news.CreatedByEmployee.FirstName + " " + news.CreatedByEmployee.LastName : string.Empty,
                CreatedDate = news.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(news.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : news.CreatedDate,

            };
        }
        /// <summary>
        /// Gets the borough.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the News in detail.</returns>
        /// 
        /// <summary>
        /// Gets the News.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
    

        /// <summary>
        /// Posts the state.
        /// </summary>
        /// <param name="Newsletter">The News.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        ///
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(NewsLetterDetail))]
        [HttpPost]
       // [Route("api/News/PostNewsLetter")]
        public IHttpActionResult PostNewsLetter(News Newsletter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            Newsletter.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                Newsletter.CreatedBy = employee.Id;
            }

            rpoContext.News.Add(Newsletter);
            rpoContext.SaveChanges();

            News NewsResponse = rpoContext.News.Include("CreatedByEmployee").FirstOrDefault(x => x.Id == Newsletter.Id);
            return Ok(Format(NewsResponse));
        }

        /// <summary>
        /// Deletes the News.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [Authorize]
        [RpoAuthorize]
       // [Route("api/News/DeleteNews/{id}")]
       [HttpDelete]
        public IHttpActionResult DeleteNewsLetter(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            string isdelete = " ";
            News News = rpoContext.News.Find(id);           
            if (News!= null)
            {
                rpoContext.News.Remove(News);
                isdelete = "News Deleted Successfully";
            }        
            try
            {
                rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (News==null)
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(isdelete);
        }

        private NewsLetterDTO Format(News NewsLetter)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new NewsLetterDTO
            {
                Id = NewsLetter.Id,
                Title = NewsLetter.Title,
                URL = NewsLetter.URL,
                NewsImagePath = NewsLetter.NewsImagePath,
                Description = NewsLetter.Description,
                CreatedBy = NewsLetter.CreatedBy,               
                CreatedByEmployeeName = NewsLetter.CreatedByEmployee != null ? NewsLetter.CreatedByEmployee.FirstName + " " + NewsLetter.CreatedByEmployee.LastName : string.Empty,               
                CreatedDate = NewsLetter.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(NewsLetter.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : NewsLetter.CreatedDate,
                
            };
        }


    }
}