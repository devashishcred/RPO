// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-20-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-24-2018
// ***********************************************************************
// <copyright file="TaskHistoryMessages.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task History Messages.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    /// <summary>
    /// Class Task History Messages.
    /// </summary>
    public class TaskHistoryMessages
    {
        public static string CreateTaskHistoryMessage = "Task# ##TaskNumber## created for this service item";

        //public static string TaskStatusChangedHistoryMessage = "Task# ##TaskNumber## status changed to ##NewStatus##";

        public static string TaskCompletedHistoryFixedPriceMessage = "Task# <a id='linktask' data-type='task' data-id='##TaskId##' class='taskHistory_Click' href='javascript:void(0)' rel='noreferrer'>##TaskNumber##</a> was completed and mapped to this service item. Units obtained for the service item - ##QuantityOfTheServiceItem##";

       public static string TaskCompletedHistoryMessage = "##ServiceItemName## has been updated to ##NewServiceStatus## from ##OldServiceStatus## with reference to Task# <a id='linktask' data-type='task' data-id='##TaskId##' class='taskHistory_Click' href='javascript:void(0)' rel='noreferrer'>##TaskNumber##</a> of type ##TaskType##"; //""Task# <a id='linktask' data-type='task' data-id='##TaskId##' class='taskHistory_Click' href='javascript:void(0)' rel='noreferrer'>##TaskNumber##</a> was completed and mapped to this service item.";


        public static string AdditonalTaskCompletedHistoryMessage = "Additional Service item- ##ServiceItemName## has been updated to ##NewServiceStatus## from ##OldServiceStatus## with reference to Task# <a id='linktask' data-type='task' data-id='##TaskId##' class='taskHistory_Click' href=\"##RedirectionTask##\" rel='noreferrer'>##TaskNumber##</a> in job# <a href=\"##RedirectionLinkJob##\">##JobNumber##</a>"; //""Task# <a id='linktask' data-type='task' data-id='##TaskId##' class='taskHistory_Click' href='javascript:void(0)' rel='noreferrer'>##TaskNumber##</a> was completed and mapped to this service item.";

        public static string AdditonalTimenotesCompletedHistoryMessage = "Additional Service item- ##ServiceItemName## has been updated to ##NewServiceStatus## from ##OldServiceStatus## in job# <a href=\"##RedirectionLinkJob##\">##JobNumber##</a>";

        public static string TaskTramittalHistoryMessage = "Transmittal <a id='linktransmittal' data-type='transmittal' class='taskHistory_Click' data-id='##TransmittalId##' href='javascript:void(0)' rel='noreferrer' target='_blank'>##TransmittalNumber##</a> of type ##TransmittalType## was sent for the scope item via Task ##TaskNumber##";

    }
}