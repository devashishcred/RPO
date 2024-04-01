// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-16-2018
// ***********************************************************************
// <copyright file="Extensions.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Extensions.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    using System;
    using System.Linq;

    /// <summary>
    /// Class Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Clones as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T.</returns>
        public static T CloneAs<T>(this object source)
        {
            T result = (T)Activator.CreateInstance(typeof(T));

            source.CloneTo(result);

            return result;
        }

        /// <summary>
        /// Clones to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="dest">The dest.</param>
        public static void CloneTo<T>(this object source, T dest)
        {
            var ts = source.GetType();
            var td = typeof(T);
            var tdProperties = td.GetProperties();

            ts.GetProperties()
                .Where(p => tdProperties.Any(b => b.Name == p.Name && b.PropertyType == p.PropertyType))
                .ToList()
                .ForEach(p =>
                {
                    td.GetProperty(p.Name).SetValue(dest, ts.GetProperty(p.Name).GetValue(source));
                });
        }
    }
}