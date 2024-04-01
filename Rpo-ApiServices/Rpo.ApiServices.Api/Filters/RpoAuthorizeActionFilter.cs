// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="RpoAuthorizeActionFilter.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class RPO Authorize Action Filter.</summary>
// ***********************************************************************

/// <summary>
/// The Filters namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using Tools;

    /// <summary>
    /// Class RPO Authorize Action Filter.
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.IActionFilter" />
    public class RpoAuthorizeActionFilter : IActionFilter
    {
        /// <summary>
        /// Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.
        /// </summary>
        /// <value><c>true</c> if [allow multiple]; otherwise, <c>false</c>.</value>
        public bool AllowMultiple => true;

        /// <summary>
        /// Executes the filter action asynchronously.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="cancellationToken">The cancellation token assigned for this task.</param>
        /// <param name="continuation">The delegate function to continue after the action method is invoked.</param>
        /// <returns>The newly created task for this operation.</returns>
        /// <exception cref="System.Exception">
        /// User not found as Employee!
        /// or
        /// Controller and Action authorization not set as required
        /// or
        /// Controller and Action authorization not set as required
        /// or
        /// Controller and Action authorization not set as required
        /// </exception>
        /// <exception cref="HttpResponseException"></exception>
        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                IEnumerable<RpoAuthorizeAttribute> actionAutorizes = actionContext.ActionDescriptor
                        .GetCustomAttributes<RpoAuthorizeAttribute>(true);
                IEnumerable<RpoAuthorizeAttribute> controllerAutorizes = actionContext.ControllerContext.ControllerDescriptor
                        .GetCustomAttributes<RpoAuthorizeAttribute>(true);
              
               
                   
                if (actionAutorizes.Any() || controllerAutorizes.Any())
                {
                    string userName = actionContext.RequestContext.Principal.Identity.Name;

                    RpoContext db = new RpoContext();
                    Employee employee;
                    Customer customer;
                    customer = db.Customers.FirstOrDefault(e => e.EmailAddress == userName);
                    employee = db.Employees.FirstOrDefault(e => e.Email == userName);
                    if(customer==null && employee==null)
                    {
                        throw new Exception("User not found");
                    }
                    else if(customer != null)
                    {
                        if (!customer.IsActive)
                        {
                            throw new RpoUnAuthorizedException(StaticMessages.EmployeeIsSetInactiveTokenMessage);
                        }
                    }
                    else if (employee != null)
                    {
                        if (!employee.IsActive)
                        {
                            throw new RpoUnAuthorizedException(StaticMessages.EmployeeIsSetInactiveTokenMessage);
                        }
                        if (employee.IsArchive)
                        {
                            throw new RpoUnAuthorizedException(StaticMessages.EmployeeIsDeletedTokenMessage);
                        }
                    }
                    //if (actionContext.Request.RequestUri.ToString().ToLower().Contains("customer"))
                    //{
                    //    customer = db.Customers.FirstOrDefault(e => e.EmailAddress == userName);

                    //    if (customer == null)
                    //    {
                    //        throw new Exception("customer not found");
                    //    }
                    //    if (!customer.IsActive)
                    //    {
                    //        throw new RpoUnAuthorizedException(StaticMessages.EmployeeIsSetInactiveTokenMessage);
                    //    }
                    //}
                    //else
                    //{ 
                    // employee = db.Employees.FirstOrDefault(e => e.Email == userName);
                    //    if (employee == null)
                    //    {
                    //        throw new Exception("User not found as Employee!");
                    //    }

                    //    if (!employee.IsActive)
                    //    {
                    //        throw new RpoUnAuthorizedException(StaticMessages.EmployeeIsSetInactiveTokenMessage);
                    //    }

                    //    if (employee.IsArchive)
                    //    {
                    //        throw new RpoUnAuthorizedException(StaticMessages.EmployeeIsDeletedTokenMessage);
                    //    }
                    //}


                    return continuation();
                    /*
                    if (actionAutorizes.Any())
                    {
                        foreach (var actionAutorize in actionAutorizes)
                        {
                            if (actionAutorize.FunctionGrantType != null && actionAutorize.GrantType != null)
                            {
                                if (isAutorized(employee, actionAutorize.FunctionGrantType.Value, actionAutorize.GrantType.Value))
                                {
                                    return continuation();
                                }
                            }
                            else if (actionAutorize.FunctionGrantType == null)
                            {
                                if (controllerAutorizes.Where(ca => ca.GrantType == null).Any())
                                {
                                    foreach (var controllerAutorize in controllerAutorizes.Where(ca => ca.GrantType == null))
                                    {
                                        if (isAutorized(employee, controllerAutorize.FunctionGrantType.Value, actionAutorize.GrantType.Value))
                                        {
                                            return continuation();
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("Controller and Action authorization not set as required");
                                }
                            }
                            else
                            {
                                if (controllerAutorizes.Where(ca => ca.FunctionGrantType == null).Any())
                                {
                                    foreach (var controllerAutorize in controllerAutorizes.Where(ca => ca.GrantType == null))
                                    {
                                        if (isAutorized(employee, actionAutorize.FunctionGrantType.Value, controllerAutorize.GrantType.Value))
                                        {
                                            return continuation();
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("Controller and Action authorization not set as required");
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var controllerAutorize in controllerAutorizes)
                        {
                            if (controllerAutorize.FunctionGrantType == null)
                            {
                                throw new Exception("Controller and Action authorization not set as required");
                            }

                            if (isAutorized(employee, controllerAutorize.FunctionGrantType.Value, controllerAutorize.GrantType ?? defaultGrantypeForAction(actionContext.ActionDescriptor)))
                            {
                                return continuation();
                            }
                        }
                    }
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                    */
                }
                else
                {
                    return continuation();
                }
            }
            else
            {
                return continuation();
            }
        }
        
        /// <summary>
        /// Defaults the grant type for action.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>Grant Type.</returns>
        /// <exception cref="System.Exception">Controller and Action authorization not set as required! Is not possible determine default Grant Type for action.</exception>
        /// <exception cref="System.ArgumentException"></exception>
        private GrantType DefaultGrantTypeForAction(HttpActionDescriptor descriptor)
        {
            if (descriptor.SupportedHttpMethods.Count != 1)
            {
                throw new Exception("Controller and Action authorization not set as required! Is not possible determine default Grant Type for action.");
            }

            var httpMethod = descriptor.SupportedHttpMethods.First().Method;

            switch (httpMethod)
            {
                case "GET": return GrantType.View;
                case "POST":
                case "PUT": return GrantType.CreateEdit;
                case "DELETE": return GrantType.Delete;
                default: throw new ArgumentException();
            }
        }
    }
}