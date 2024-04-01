using Microsoft.AspNet.Identity;
using Rpo.Identity.Core.BindingModels.Account;
using Rpo.Identity.Core.Filters;
using Rpo.Identity.Core.Infrastructure;
using Rpo.Identity.Core.Models;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Rpo.Identity.Core.Controllers
{
    public class AccountController : RpoIdentityApiController
    {
        [RpoValidation]
        [AllowAnonymous]
        [ActionName("ActivateAccount")]
        [HttpPost]
        public async Task<IHttpActionResult> ActivateAccount(ActivateAccountBindingModel model)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                if (await UserManager.FindByIdAsync(model.UserId) == null)
                    return BadRequest("Error");

                if (await UserManager.IsEmailConfirmedAsync(model.UserId))
                    return BadRequest("Your e-mail is already confirmed.");

                IdentityResult confirmEmailResult = await UserManager.ConfirmEmailAsync(model.UserId, model.Token);

                if (confirmEmailResult.Succeeded)
                {
                    IdentityResult addPasswordResult = await UserManager.AddPasswordAsync(model.UserId, model.Password);

                    if (addPasswordResult.Succeeded)
                    {
                        dbContextTransaction.Commit();
                        return Ok();
                    }
                    else
                        return BadRequest(string.Join("<br />", addPasswordResult.Errors));
                }
                else
                    return BadRequest(string.Join("<br />", confirmEmailResult.Errors));
            }
        }

        [RpoValidation]
        [AllowAnonymous]
        [HttpPost]
        [ActionName("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordBindingModel model)
        {
            RpoIdentityUser user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok();
            }

            string token = HttpUtility.UrlEncode(await UserManager.GeneratePasswordResetTokenAsync(user.Id));
            string url = string.Concat(model.ExternalRoute, user.Id, '/', token);

            IdentityMessage message = new IdentityMessage();

            message.Destination = user.Email;
            message.Subject = "Change Password";
            message.Body = "Please change your password by clicking here: <a href=\"" + url + "\">Change password</a>";

            await UserManager.EmailService.SendAsync(message);

            return Ok();
        }

        [RpoValidation]
        [Authorize]
        [ActionName("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            string userId = User.Identity.GetUserId();

            IdentityResult result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
                return BadRequest(string.Join("<br />", result.Errors));
        }

        [RpoValidation]
        [AllowAnonymous]
        [HttpPost]
        [ActionName("ExternalChangePassword")]
        public async Task<IHttpActionResult> ExternalChangePassword(ExternalChangePasswordBindingModel model)
        {
            IdentityResult result = await UserManager.ResetPasswordAsync(model.UserId, model.Token, model.Password);

            if (result.Succeeded)
                return Ok();

            return BadRequest(string.Join("<br />", result.Errors));
        }

        [RpoValidation]
        [Authorize]
        [HttpPost]
        [ActionName("GetUserName")]
        public async Task<IHttpActionResult> GetUserName()
        {
            try
            {
                return Ok((await IdentityUser).UserName);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
