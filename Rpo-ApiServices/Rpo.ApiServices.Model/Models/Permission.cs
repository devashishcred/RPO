
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Get or Sets the Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the DisplayName
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// Gets or Sets the Group Name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the permission class.
        /// </summary>
        /// <value>The permission class.</value>
        public string PermissionClass { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

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

        /// <summary>
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }
    }
}
