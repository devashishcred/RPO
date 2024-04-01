// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 04-17-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-17-2018
// ***********************************************************************
// <copyright file="DocumentsController.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Documents namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Documents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Filters;
    using Rpo.ApiServices.Model;

    /// <summary>
    /// Class Documents Controller.
    /// </summary>
    public class DocumentsController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the document drop down.
        /// </summary>
        /// <returns>IHttpActionResult. get the documentlist for dropdown</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/document/dropdown")]
        public IHttpActionResult GetDocumentDropdown()
        {
            var result = rpoContext.DocumentMasters.Where(x => x.IsDevelopementCompleted == true).AsEnumerable().
                Select(c => new DocumentsDTO
                {
                    Id = c.Id,
                    DocumentName = c.DocumentName,
                    Code = c.Code,
                    ItemName = "[" + c.Code + "] " + c.DocumentName,
                    DisplayOrder = c.DisplayOrder

                }).ToArray().OrderBy(c=>c.Code);

            return Ok(result);
        }
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/document/DocumentDropdownUploadForm")]
        public IHttpActionResult GetDocumentDropdownUploadForm()
        {
            //as manish sir suggested no need to filter data according to "click to browse option so commenting this code"
            //var Documentdropdown = (from dm in rpoContext.DocumentMasters
            //        join d in rpoContext.DocumentFields
            //        on dm.Id equals d.IdDocument 
            //        join f in rpoContext.Fields
            //        on d.IdField equals f.Id 
            //        where f.ControlType == Model.Models.Enums.ControlType.FileType
            //        select new DocumentsDTO
            //        {
            //            Id = dm.Id,
            //            DocumentName = dm.DocumentName,
            //            Code = dm.Code,
            //            ItemName = "[" + dm.Code + "] " + dm.DocumentName,
            //            DisplayOrder = dm.DisplayOrder
            //        }).ToList();
            var Documentdropdown = rpoContext.DocumentMasters.Where(x => x.IsDevelopementCompleted == true).AsEnumerable().
               Select(c => new DocumentsDTO
               {
                   Id = c.Id,
                   DocumentName = c.DocumentName,
                   Code = c.Code,
                   ItemName = "[" + c.Code + "] " + c.DocumentName,
                   DisplayOrder = c.DisplayOrder

               }).ToArray().OrderBy(c => c.Code);
            return Ok(Documentdropdown);
        }
       
    }
}
