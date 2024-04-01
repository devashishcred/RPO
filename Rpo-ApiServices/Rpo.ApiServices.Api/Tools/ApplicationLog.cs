// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-01-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-16-2018
// ***********************************************************************
// <copyright file="ApplicationLog.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Application Log.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    using System;
    using System.IO;

    /// <summary>
    /// Class Application Log.
    /// </summary>
    class ApplicationLog
    {
        /// <summary>
        /// Writes the error log.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErrorLogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Writes the information log.
        /// </summary>
        /// <param name="Message">The message.</param>
        public static void WriteInformationLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErrorLogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }
}
