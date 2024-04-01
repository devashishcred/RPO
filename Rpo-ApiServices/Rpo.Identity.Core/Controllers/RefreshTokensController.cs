// ***********************************************************************
// Assembly         : Rpo.Identity.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="RefreshTokensController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Refresh Tokens Controller.</summary>
// ***********************************************************************

namespace Rpo.Identity.Api.Controllers
{
    using Rpo.Identity.Core.Repositories;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Class Refresh Tokens Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RefreshTokensController : ApiController
    {
        /// <summary>
        /// The repository
        /// </summary>
        private AuthenticationRepository _repository = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokensController"/> class.
        /// </summary>
        public RefreshTokensController()
        {
            _repository = new AuthenticationRepository();
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [Authorize(Roles = "Administrator")]
        [ActionName("")]
        public IHttpActionResult Get()
        {
            return Ok(_repository.GetAllRefreshTokens());
        }

        /// <summary>
        /// Deletes the specified token identifier.
        /// </summary>
        /// <param name="tokenId">The token identifier.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [AllowAnonymous]
        [ActionName("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await _repository.RemoveRefreshToken(tokenId);
            if (result)
                return Ok();
            
            return BadRequest("Token Id does not exist");
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repository.Dispose();

            base.Dispose(disposing);
        }
    }
}