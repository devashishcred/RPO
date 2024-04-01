
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.Groups
{
    public class GroupDetail
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public PermissionDetail PermissionDetail { get; set; }
    }

    public class PermissionDetail
    {
        public string GroupName { get; set; }

        public Permission Permissions { get; set; }
    }
}