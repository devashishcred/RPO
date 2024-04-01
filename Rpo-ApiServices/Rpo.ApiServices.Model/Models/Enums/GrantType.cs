using System;

namespace Rpo.ApiServices.Model.Models.Enums
{
    [Flags]
    public enum GrantType
    {
        CreateEdit = 1,

        View = 2,

        Delete = 4
    }
}
