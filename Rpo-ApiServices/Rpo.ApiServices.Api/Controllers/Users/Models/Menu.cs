// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="Menu.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Menu.</summary>
// ***********************************************************************

/// <summary>
/// The Users namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Users
{
    using System.Collections.Generic;

    /// <summary>
    /// Class Menu.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the router link.
        /// </summary>
        /// <value>The router link.</value>
        public string RouterLink { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<Menu> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public Menu(string text)
        {
            this.Text = text;
            this.Items = new List<Menu>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="routerLink">The router link.</param>
        public Menu(string text, string routerLink)
        {
            this.Text = text;
            this.RouterLink = routerLink;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="routerLink">The router link.</param>
        public Menu(string text, string routerLink, string Icon)
        {
            this.Text = text;
            this.RouterLink = routerLink;
            this.Icon = Icon;
            this.Items = new List<Menu>();
        }
    }
}