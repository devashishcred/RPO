using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using Model.Models.Enums;

namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown
{
    public class checklitItemTransmittalDTO
    {
        public int Id { get; set; }
        public int IdJobChecklistGroup { get; set; } 
        public int IdChecklistItem { get; set; }       
        public int Displayorder { get; set; }
        public int Status { get; set; }  
        public int? PartyResponsible1 { get; set; }
        public string ManualPartyResponsible { get; set; }        
        public int IdContact { get; set; }
        public virtual Contact Contacts { get; set; }
        public int IdCompany { get; set; }
        public virtual Contact Company { get; set; }

        public int? IdDesignApplicant { get; set; }

        public virtual Contact DesignApplicantContact { get; set; }
        public int? IdInspector { get; set; }

        public virtual Contact InspectorContact { get; set; }
        public string Stage { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public string ContactName { get; set; }
        public List<string> CompanyName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }


    }
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public int? IdCompany { get; set; }

        public virtual Company Company { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact title.
        /// </summary>
        /// <value>The identifier contact title.</value>
        public virtual int? IdContactTitle { get; set; }
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the work phone.
        /// </summary>
        /// <value>The work phone.</value>
        [MaxLength(15)]
        public string WorkPhone { get; set; }

        /// <summary>
        /// Gets or sets the work phone ext.
        /// </summary>
        /// <value>The work phone ext.</value>
        [MaxLength(5)]
        public string WorkPhoneExt { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone.
        /// </summary>
        /// <value>The mobile phone.</value>
        [MaxLength(15)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the other phone.
        /// </summary>
        /// <value>The other phone.</value>
        [MaxLength(15)]
        public string OtherPhone { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the contact image path.
        /// </summary>
        /// <value>The contact image path.</value>
        [MaxLength(200)]
        public string ContactImagePath { get; set; }

        /// <summary>
        /// Gets or sets the contact image thumb path.
        /// </summary>
        /// <value>The contact image thumb path.</value>
        [MaxLength(200)]
        public string ContactImageThumbPath { get; set; }


        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }


        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the fax number.
        /// </summary>
        /// <value>The fax number.</value>
        public string FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the PrimaryCompanyAddressId number.
        /// </summary>
        /// <value>The PrimaryCompanyAddressId number.</value>
        /// 
        public int? IdPrimaryCompanyAddress { get; set; }
        /// <summary>
        /// Gets or sets the IsPrimaryCompanyAddress number.
        /// </summary>
        /// <value>The PrimaryCompanyAddressId number.</value>
        /// 
        public bool? IsPrimaryCompanyAddress { get; set; }

        public bool IsActive { get; set; }
    }

    public class Company {
        public int Id { get; set; }

        public string Name { get; set; }
    }
    public class CheckListGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        public int? Displayorder { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }
         public DateTime? LastModifiedDate { get; set; }
    }

    public class JobApplicationType
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int? IdParent { get; set; }
        public JobApplicationType Parent { get; set; }       
        
    }
    public class JobWorkType
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Code { get; set; }

        public double? Cost { get; set; }

        public int IdJobApplicationType { get; set; }

       public JobApplicationType JobApplicationType { get; set; }

        
    }
    public class WorkPermitTypeDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }
    }

    public class ReferenceDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string ContentPath { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
    public class DocumentMaster
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string Code { get; set; }
        public string Path { get; set; }
        public int DisplayOrder { get; set; }
        public bool? IsDevelopementCompleted { get; set; }
        public bool? IsAddPage { get; set; }
        public bool IsNewDocument { get; set; }
    }


}

    