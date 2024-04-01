// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 02-10-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 02-10-2023
// ***********************************************************************
// <copyright file="NewsController.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class News Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.News
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Permissions;
    using Rpo.ApiServices.Api.Controllers.News;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using Tools;
    /// <summary>
    /// Class Groups Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    public class News : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        ///// <summary>
        ///// Puts the News.
        ///// </summary>
        ///// <param name="id">The identifier.</param>
        ///// <param name="NewsCreateOrUpdateDTO">The News create or update dto.</param>
        ///// <returns>IHttpActionResult.</returns>
        //[Authorize]
        //[RpoAuthorize]
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutNews(int id, NewsCreateOrUpdateDTO NewsCreateOrUpdateDTO)
        //{
        //    var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
        //   // if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserNews))
        //   // {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        News News = NewsCreateOrUpdateDTO.CloneAs<News>();

        //        if (id != NewsCreateOrUpdateDTO.Id)
        //        {
        //            return BadRequest();
        //        }           
        //        if(News!=null)
        //        rpoContext.Entry(News).State = EntityState.Modified;

        //        try
        //        {
        //            rpoContext.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!NewsExists(id))
        //            {
        //                return this.NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return StatusCode(HttpStatusCode.NoContent);
        //    //}
        //    //else
        //    //{
        //    //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
        //    //}
        //}

        ///// <summary>
        ///// Posts the News.
        ///// </summary>
        ///// <param name="NewsCreateOrUpdateDTO">The News create or update dto.</param>
        ///// <returns>IHttpActionResult.</returns>
        //[Authorize]
        //[RpoAuthorize]
        //[ResponseType(typeof(News))]
        //public IHttpActionResult PostNews(NewsCreateOrUpdateDTO NewsCreateOrUpdateDTO)
        //{
        //    var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
        //   // if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.add))
        //   // {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        News news = NewsCreateOrUpdateDTO.CloneAs<News>();
            
        //        //if (NewsCreateOrUpdateDTO.Permissions != null && NewsCreateOrUpdateDTO.Permissions.Count() > 0)
        //        //{
        //        //    News.Permissions = string.Join(",", NewsCreateOrUpdateDTO.Permissions.Select(x => x.ToString()));
        //        //}
        //        //else
        //        //{
        //        //    News.Permissions = string.Empty;
        //        //}


        //    rpoContext.News.Add(news);
        //        rpoContext.SaveChanges();

        //        return this.CreatedAtRoute("DefaultApi", new { id = News.Id }, News);
        //    //}
        //    //else
        //    //{
        //    //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
        //    //}
        //}

        /// <summary>
        /// Newss the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool NewsExists(int id)        {
            return rpoContext.News.Count(e => e.Id == id) > 0;
        }
    }
}