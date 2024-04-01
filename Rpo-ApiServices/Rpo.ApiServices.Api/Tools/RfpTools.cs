// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-16-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-16-2018
// ***********************************************************************
// <copyright file="RfpTools.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Tools.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    using System.Web;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Rfp Tools.
    /// </summary>
    public static class RfpTools
    {
        /// <summary>
        /// Sets the response go next step header.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        public static void SetResponseGoNextStepHeader(this Rfp rfp)
        {
            if (rfp.GoNextStep != null)
            {
                HttpContext.Current.Response.Headers.Add("goNextStep", rfp.GoNextStep.ToString());
            }
        }

        /// <summary>
        /// Processes the go next step header.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        public static void ProcessGoNextStepHeader(this Rfp rfp)
        {
            var goNextStepHeader = HttpContext.Current.Request.Headers["goNextStep"];

            int goNextStep = 0;

            if (!string.IsNullOrWhiteSpace(goNextStepHeader) && int.TryParse(goNextStepHeader, out goNextStep))
            {
                rfp.GoNextStep = goNextStep;
            }
        }
    }
}