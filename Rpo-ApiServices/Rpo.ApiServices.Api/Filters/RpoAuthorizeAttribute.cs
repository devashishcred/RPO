// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-23-2018
// ***********************************************************************
// <copyright file="RpoAuthorizeAttribute.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class RPO Authorize Attribute.</summary>
// ***********************************************************************

/// <summary>
/// The Filters namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Filters
{
    using System;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class RPO Authorize Attribute.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class RpoAuthorizeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RpoAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="functionGrantType">Type of the function grant.</param>
        /// <param name="grantType">Type of the grant.</param>
        public RpoAuthorizeAttribute(FunctionGrantType functionGrantType, GrantType grantType)
        {
            this.FunctionGrantType = functionGrantType;
            this.GrantType = grantType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpoAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="functionGrantType">Type of the function grant.</param>
        public RpoAuthorizeAttribute(FunctionGrantType functionGrantType)
        {
            this.FunctionGrantType = functionGrantType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpoAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="grantType">Type of the grant.</param>
        public RpoAuthorizeAttribute(GrantType grantType)
        {
            this.GrantType = grantType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpoAuthorizeAttribute"/> class.
        /// </summary>
        public RpoAuthorizeAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the type of the function grant.
        /// </summary>
        /// <value>The type of the function grant.</value>
        public FunctionGrantType? FunctionGrantType { get; set; }

        /// <summary>
        /// Gets or sets the type of the grant.
        /// </summary>
        /// <value>The type of the grant.</value>
        public GrantType? GrantType { get; set; }
    }
}