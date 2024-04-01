using System;

namespace Rpo.Identity.Core.Models.Enumerations
{
    [Flags]
    public enum RpoClaimTypes
    {
        /// <summary>
        /// Full Time Employee, for any user who has worked for more than 90 days.
        /// </summary>
        FTE = 1
    }
}
