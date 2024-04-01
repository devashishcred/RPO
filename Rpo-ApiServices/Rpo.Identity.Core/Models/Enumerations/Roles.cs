using System;

namespace Rpo.Identity.Core.Models.Enumerations
{
    [Flags]
    public enum RpoRoles
    {
        Administrator = 1,
        Employee = 2,
        Customer = 4
    }
}
