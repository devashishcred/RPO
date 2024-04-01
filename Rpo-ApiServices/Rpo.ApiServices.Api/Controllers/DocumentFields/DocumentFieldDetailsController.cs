// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 04-18-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="DocumentFieldDetailsController.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.DocumentFields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Filters;
    using Model;
    using Model.Models.Enums;
    using Rpo.ApiServices.Model.Models;


    /// <summary>
    /// Class DocumentFieldDetailsController.
    /// </summary>
    public class DocumentFieldDetailsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the document field details.
        /// </summary>
        /// <param name="DocumentId">The document identifier.</param>
        /// <returns>IHttpActionResult. Get the list of documentfileds against document</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetDocumentFieldDetails(int DocumentId)
        {
            var documentFields = db.DocumentFields.Include("Document").Include("Field").Where(s => s.IdDocument == DocumentId);

           // var result = Format(documentFields);
            var result = documentFields.AsEnumerable().
                Select(c => Format(c)).OrderBy(x=>x.DisplayOrder).ToArray();

            return Ok(result);
        }
        private DocumentFieldDTO Format(DocumentField documentField)
        {
            return new DocumentFieldDTO
            {
                Id = documentField.Id,
                FieldName = documentField.Field != null ? documentField.Field.FieldName : string.Empty,
                ControlType = documentField.Field != null ? documentField.Field.ControlType : 0,
                DataType = documentField.Field != null ? documentField.Field.DataType : 0,
                IdDocument = documentField.IdDocument != null ? documentField.IdDocument : 0,
                IdField = documentField.IdField != null ? documentField.IdField : 0,
                IsRequired = documentField != null ? documentField.IsRequired : false,
                Length = documentField != null ? documentField.Length : 0,
                APIUrl = documentField != null ? documentField.APIUrl : string.Empty,
                DocumentName = documentField.Document != null ? documentField.Document.DocumentName : string.Empty,
                Code = documentField.Document != null ? documentField.Document.Code : string.Empty,
                Path = documentField.Document != null ? documentField.Document.Path : string.Empty,
                Field = documentField.Field != null ? documentField.Field : null,
                Value = documentField.DefaultValue,
                DefaultValue = documentField.DefaultValue,
                IdParentField = documentField.IdParentField,
                StaticDescription = documentField.StaticDescription,
                DisplayOrder = documentField.DisplayOrder,
                DisplayFieldName = documentField.Field != null ? documentField.Field.DisplayFieldName : string.Empty,
            };
        }

    }
}
