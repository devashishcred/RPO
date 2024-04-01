
/// <summary>
/// The Utilities namespace.
/// </summary>
namespace Rpo.QuickBook.WebServices.Utilities
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Class AppSettings.
    /// </summary>
    public static class AppSettings
    {
        ///// <summary>
        ///// Gets the connection string.
        ///// </summary>
        ///// <value>The connection string.</value>
        //public static string ConnectionString
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings["connectionString"] != null)
        //        {
        //            return Convert.ToString(ConfigurationManager.AppSettings["connectionString"]);
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //}

        /// <summary>
        /// Gets the future energy solutions contracts no one LLLP.
        /// </summary>
        /// <value>The future energy solutions contracts no one LLLP.</value>
        public static string RPOTimeEntrySystem
        {
            get
            {
                if (ConfigurationManager.AppSettings["RPOTimeEntrySystem"] != null)
                {
                    return Convert.ToString(ConfigurationManager.AppSettings["RPOTimeEntrySystem"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}