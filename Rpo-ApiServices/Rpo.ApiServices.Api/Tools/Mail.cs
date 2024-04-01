// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-02-2018
// ***********************************************************************
// <copyright file="Mail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Mail.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Mail;

    /// <summary>
    /// Class Mail.
    /// </summary>
    public static class Mail
    {
        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        public static void Send(KeyValuePair<string, string> from, List<KeyValuePair<string, string>> to, string subject, string body, List<KeyValuePair<string, byte[]>> attachments = null, bool isBodyHtml = true)
        {
            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(from.Key, from.Value);

            to.ForEach(t =>
            {
                mailMessage.To.Add(new MailAddress(t.Key, t.Value));
            });

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = isBodyHtml;

            List<MemoryStream> memoryStreams = null;
            try
            {
                if (attachments != null)
                {
                    memoryStreams = new List<MemoryStream>();

                    attachments.ForEach(a =>
                    {
                        var ms = new MemoryStream(a.Value);

                        memoryStreams.Add(ms);

                        mailMessage.Attachments.Add(new Attachment(ms, a.Key));
                    });
                }


                var smtpClient = new SmtpClient();

                smtpClient.Port = Properties.Settings.Default.SmtpPort;

                smtpClient.EnableSsl = Properties.Settings.Default.SmtpUseSSl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = Properties.Settings.Default.SmtpHost;

                if (Properties.Settings.Default.SmtpAuthenticationEnabled)
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.SmtpUserName, Properties.Settings.Default.SmtpPassword);

                smtpClient.Send(mailMessage);

            }
            finally
            {
                if (memoryStreams != null)
                    memoryStreams.ForEach(ms =>
                    {
                        ms.Dispose();
                    });
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        public static void Send(KeyValuePair<string, string> from, List<KeyValuePair<string, string>> to, string subject, string body, bool isBodyHtml = true)
        {
            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(from.Key, from.Value);

            to.ForEach(t =>
            {
                mailMessage.To.Add(new MailAddress(t.Key, t.Value));
            });

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = isBodyHtml;

            List<MemoryStream> memoryStreams = null;
            try
            {
                var smtpClient = new SmtpClient();

                smtpClient.Port = Properties.Settings.Default.SmtpPort;

                smtpClient.EnableSsl = Properties.Settings.Default.SmtpUseSSl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = Properties.Settings.Default.SmtpHost;

                if (Properties.Settings.Default.SmtpAuthenticationEnabled)
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.SmtpUserName, Properties.Settings.Default.SmtpPassword);

                smtpClient.Send(mailMessage);

            }
            finally
            {
                if (memoryStreams != null)
                    memoryStreams.ForEach(ms =>
                    {
                        ms.Dispose();
                    });
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        public static void Send(KeyValuePair<string, string> from, List<KeyValuePair<string, string>> to, List<KeyValuePair<string, string>> cc, string subject, string body, bool isBodyHtml = true)
        {
            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(from.Key, from.Value);
            //mailMessage.From = new MailAddress("qa1@credencys.net", from.Value);


            to.ForEach(t =>
            {
                mailMessage.To.Add(new MailAddress(t.Key, t.Value));
            });

            cc.ForEach(t =>
            {
                mailMessage.CC.Add(new MailAddress(t.Key, t.Value));
            });

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = isBodyHtml;

            List<MemoryStream> memoryStreams = null;
            try
            {
                var smtpClient = new SmtpClient();

                smtpClient.Port = Properties.Settings.Default.SmtpPort;

                smtpClient.EnableSsl = Properties.Settings.Default.SmtpUseSSl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = Properties.Settings.Default.SmtpHost;

                if (Properties.Settings.Default.SmtpAuthenticationEnabled)
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.SmtpUserName, Properties.Settings.Default.SmtpPassword);

                smtpClient.Send(mailMessage);

            }
            finally
            {
                if (memoryStreams != null)
                    memoryStreams.ForEach(ms =>
                    {
                        ms.Dispose();
                    });
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        public static void SendWithImage(MailMessage mailMessage, KeyValuePair<string, string> from, List<KeyValuePair<string, string>> to, List<KeyValuePair<string, string>> cc, string subject, string body, bool isBodyHtml = true)
        {
            //var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(from.Key, from.Value);
            to.ForEach(t =>
            {
                mailMessage.To.Add(new MailAddress(t.Key, t.Value));
            });

            cc.ForEach(t =>
            {
                mailMessage.CC.Add(new MailAddress(t.Key, t.Value));
            });

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = isBodyHtml;

            List<MemoryStream> memoryStreams = null;
            try
            {
                var smtpClient = new SmtpClient();

                smtpClient.Port = Properties.Settings.Default.SmtpPort;

                smtpClient.EnableSsl = Properties.Settings.Default.SmtpUseSSl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = Properties.Settings.Default.SmtpHost;

                if (Properties.Settings.Default.SmtpAuthenticationEnabled)
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.SmtpUserName, Properties.Settings.Default.SmtpPassword);

                smtpClient.Send(mailMessage);

            }
            finally
            {
                if (memoryStreams != null)
                    memoryStreams.ForEach(ms =>
                    {
                        ms.Dispose();
                    });
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        public static void Send(KeyValuePair<string, string> from, KeyValuePair<string, string> to, string subject, string body, bool isBodyHtml = true)
        {
            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(from.Key, from.Value);

            mailMessage.To.Add(new MailAddress(to.Key, to.Value));

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = isBodyHtml;

            List<MemoryStream> memoryStreams = null;
            try
            {
                var smtpClient = new SmtpClient();

                smtpClient.Port = Properties.Settings.Default.SmtpPort;

                smtpClient.EnableSsl = Properties.Settings.Default.SmtpUseSSl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = Properties.Settings.Default.SmtpHost;

                if (Properties.Settings.Default.SmtpAuthenticationEnabled)
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.SmtpUserName, Properties.Settings.Default.SmtpPassword);

                smtpClient.Send(mailMessage);

            }
            finally
            {
                if (memoryStreams != null)
                    memoryStreams.ForEach(ms =>
                    {
                        ms.Dispose();
                    });
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        public static void Send(KeyValuePair<string, string> from, KeyValuePair<string, string> to, List<KeyValuePair<string, string>> cc, string subject, string body, bool isBodyHtml = true)
        {
            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(from.Key, from.Value);

            mailMessage.To.Add(new MailAddress(to.Key, to.Value));

            cc.ForEach(t =>
            {
                mailMessage.CC.Add(new MailAddress(t.Key, t.Value));
            });

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = isBodyHtml;

            List<MemoryStream> memoryStreams = null;
            try
            {
                var smtpClient = new SmtpClient();

                smtpClient.Port = Properties.Settings.Default.SmtpPort;

                smtpClient.EnableSsl = Properties.Settings.Default.SmtpUseSSl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = Properties.Settings.Default.SmtpHost;

                if (Properties.Settings.Default.SmtpAuthenticationEnabled)
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.SmtpUserName, Properties.Settings.Default.SmtpPassword);

                smtpClient.Send(mailMessage);

            }
            finally
            {
                if (memoryStreams != null)
                    memoryStreams.ForEach(ms =>
                    {
                        ms.Dispose();
                    });
            }
        }
        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        public static void Send(KeyValuePair<string, string> from, List<KeyValuePair<string, string>> to, List<KeyValuePair<string, string>> cc, string subject, string body, List<string> attachments = null, bool isBodyHtml = true)
        {
            var mailMessage = new MailMessage();



            to.ForEach(t =>
            {
                mailMessage.To.Add(new MailAddress(t.Key, t.Value));
            });

            cc.ForEach(t =>
            {
                mailMessage.CC.Add(new MailAddress(t.Key, t.Value));
            });

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = isBodyHtml;

            try
            {
                if (attachments != null)
                {
                    foreach (var item in attachments)
                    {
                        mailMessage.Attachments.Add(new Attachment(item));
                    }
                }

                var smtpClient = new SmtpClient();

                smtpClient.Port = Properties.Settings.Default.SmtpPort;

                smtpClient.EnableSsl = Properties.Settings.Default.SmtpUseSSl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = Properties.Settings.Default.SmtpHost;

                if (Properties.Settings.Default.SmtpAuthenticationEnabled)
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.SmtpUserName, Properties.Settings.Default.SmtpPassword);


                mailMessage.Sender = new MailAddress(Properties.Settings.Default.SmtpUserName, from.Value);
                mailMessage.From = new MailAddress(Properties.Settings.Default.SmtpUserName, from.Value);
                mailMessage.ReplyTo = new MailAddress(from.Key, from.Value);

                smtpClient.Send(mailMessage);

            }
            finally
            {
                mailMessage.Dispose();
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        public static string SendTransmittal(KeyValuePair<string, string> from, List<KeyValuePair<string, string>> to, List<KeyValuePair<string, string>> cc, string subject, string body, List<string> attachments = null, bool isBodyHtml = true)
        {
            var mailMessage = new MailMessage();
            string msg = string.Empty;


            to.ForEach(t =>
            {
                mailMessage.To.Add(new MailAddress(t.Key, t.Value));
            });

            cc.ForEach(t =>
            {
                mailMessage.CC.Add(new MailAddress(t.Key, t.Value));
            });

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = isBodyHtml;

            try
            {
                if (attachments != null)
                {
                    foreach (var item in attachments)
                    {
                        mailMessage.Attachments.Add(new Attachment(item));
                    }
                }

                var smtpClient = new SmtpClient();

                smtpClient.Port = Properties.Settings.Default.SmtpPort;

                smtpClient.EnableSsl = Properties.Settings.Default.SmtpUseSSl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Host = Properties.Settings.Default.SmtpHost;

                if (Properties.Settings.Default.SmtpAuthenticationEnabled)
                    smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.SmtpUserName, Properties.Settings.Default.SmtpPassword);


                mailMessage.Sender = new MailAddress(Properties.Settings.Default.SmtpUserName, from.Value);
                mailMessage.From = new MailAddress(Properties.Settings.Default.SmtpUserName, from.Value);
                mailMessage.ReplyTo = new MailAddress(from.Key, from.Value);

                smtpClient.Send(mailMessage);
                msg = "Success";
            }
            catch (Exception ex)
            {
                throw ex;
                msg = "Fail";
            }
            finally
            {
                mailMessage.Dispose();
            }
            return msg;
        }
    }
}