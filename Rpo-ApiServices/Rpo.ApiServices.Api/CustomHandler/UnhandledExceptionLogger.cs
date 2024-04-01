using System;
using System.IO;
using System.Web;
using System.Web.Http.ExceptionHandling;
using WebDemoAPI.Models;
using Rpo.Identity.Core;
using Rpo.ApiServices.Api.Tools;
using System.Collections.Generic;
using System.Text;

namespace WebDemoAPI.CustomHandler
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            try
            {

                var ex = context.Exception;

                var requestedURi = (string)context.Request.RequestUri.AbsoluteUri;
                if (requestedURi.Contains("api/JobApplicationWorkPermits/document"))
                {
                    goto endloop;
                }

                string strLogText = "";

                string innerExceptionmessage = string.Empty;
                string message = string.Empty;
                if (ex.InnerException != null)
                {
                    innerExceptionmessage = ex.InnerException.ToString();
                }
                else if (!string.IsNullOrEmpty(ex.Message))
                {
                    message = ex.Message;
                }

                strLogText += Environment.NewLine + "------------------------------------------------------------";
                strLogText += Environment.NewLine + "ErrorMessage ---\n{0}" + message;
                strLogText += Environment.NewLine + "InnerExceptionMessage ---\n{0}" + innerExceptionmessage;
                strLogText += Environment.NewLine + "------------------------------------------------------------";
                strLogText += Environment.NewLine + "Source ---\n{0}" + ex.Source;
                strLogText += Environment.NewLine + "StackTrace ---\n{0}" + ex.StackTrace;
                strLogText += Environment.NewLine + "TargetSite ---\n{0}" + ex.TargetSite;


                var requestMethod = context.Request.Method.ToString();
                var AuthorizationToken = context.Request.Headers.Authorization.ToString();

                var UserName = context.RequestContext.Principal.Identity.Name;
                string requestData = string.Empty;
                if (requestMethod == "POST" || requestMethod == "PUT")
                {
                    byte[] requestMessage = context.Request.Content.ReadAsByteArrayAsync().Result;
                    requestData = Encoding.UTF8.GetString(requestMessage);
                }
                var requestPayLoad = context.Request.RequestUri.PathAndQuery;
                var UserAgent = context.Request.Headers.UserAgent;
                var timeUtc = DateTime.Now;

                string errorLogFilename = "ErrorLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

                string directory = AppDomain.CurrentDomain.BaseDirectory + "Log";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }


                string path = AppDomain.CurrentDomain.BaseDirectory + "Log/" + errorLogFilename;

                if (File.Exists(path))
                {
                    using (StreamWriter stwriter = new StreamWriter(path, true))
                    {
                        stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                        stwriter.WriteLine("Authorization Token :" + AuthorizationToken);
                        stwriter.WriteLine("User Detail :" + UserName);
                        stwriter.WriteLine("Requested URL :" + requestedURi);
                        stwriter.WriteLine("Requested Method :" + requestMethod);
                        stwriter.WriteLine("Requested Parameters :" + requestData);
                        stwriter.WriteLine("User Agent:" + UserAgent);
                        stwriter.WriteLine("Message: " + strLogText.ToString());
                        stwriter.WriteLine("-------------------End----------------------------");
                        stwriter.Close();
                    }
                }
                else
                {
                    StreamWriter stwriter = File.CreateText(path);
                    stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                    stwriter.WriteLine("Authorization Token :" + AuthorizationToken);
                    stwriter.WriteLine("User Detail :" + UserName);
                    stwriter.WriteLine("Requested URL :" + requestedURi);
                    stwriter.WriteLine("Requested Method :" + requestMethod);
                    stwriter.WriteLine("Requested Parameters :" + requestData);
                    stwriter.WriteLine("User Agent:" + UserAgent);
                    stwriter.WriteLine("Message: " + strLogText.ToString());
                    stwriter.WriteLine("-------------------End----------------------------");
                    stwriter.Close();
                }

                var attachments = new List<string>();
                if (File.Exists(path))
                {
                    attachments.Add(path);
                }

                var to = new List<KeyValuePair<string, string>>();

                to.Add(new KeyValuePair<string, string>("meethalal.teli@credencys.com", "RPO Support Group"));
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                var cc = new List<KeyValuePair<string, string>>();

                string emailBody = body;
                emailBody = emailBody.Replace("##EmailBody##", "Please correct the issue.");

                if (Rpo.ApiServices.Api.Properties.Settings.Default.IsSnapcor.Trim().ToLower() == "yes")
                {
                    Mail.Send(
                           new KeyValuePair<string, string>("noreply@rpoinc.com", "RPO Log"),
                           to,
                           cc,
                           "Error Log-[Snapcor]",
                           emailBody,
                           attachments
                       );
                }
                if (Rpo.ApiServices.Api.Properties.Settings.Default.IsUAT.Trim().ToLower() == "yes")
                {
                    Mail.Send(
                           new KeyValuePair<string, string>("noreply@rpoinc.com", "RPO Log"),
                           to,
                           cc,
                           "Error Log-[UAT]",
                           emailBody,
                           attachments
                       );
                }
            endloop:
                string st = "";
            }
            catch (Exception)
            {
            }        
        }

    }
}