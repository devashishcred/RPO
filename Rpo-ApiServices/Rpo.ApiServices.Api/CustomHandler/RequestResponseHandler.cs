using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebDemoAPI.Models;

namespace WebDemoAPI.CustomHandler
{
    public class RequestResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {

             var requestedMethod = request.Method;
            var userHostAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "0.0.0.0";
            var useragent = request.Headers.UserAgent.ToString();
            var requestMessage = await request.Content.ReadAsByteArrayAsync();
            var uriAccessed = request.RequestUri.AbsoluteUri;

            var responseHeadersString = new StringBuilder();
            foreach (var header in request.Headers)
            {
                responseHeadersString.Append($"{header.Key}: {String.Join(", ", header.Value)}{Environment.NewLine}");
            }

            var messageLoggingHandler = new MessageLogging();
            
            var requestLog = new ApiLog()
            {
                Headers = responseHeadersString.ToString(),
                AbsoluteUri = uriAccessed,
                Host = userHostAddress,
                RequestBody = Encoding.UTF8.GetString(requestMessage),
                UserHostAddress = userHostAddress,
                Useragent = useragent,
                RequestedMethod = requestedMethod.ToString(),
                StatusCode = string.Empty
            };

            messageLoggingHandler.IncomingMessageAsync(requestLog);

            var response = await base.SendAsync(request, cancellationToken);

            //byte[] responseMessage;
            //if (response.IsSuccessStatusCode)
            //    responseMessage = await response.Content.ReadAsByteArrayAsync();
            //else
            //    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);

            //var responseLog = new ApiLog()
            //{
            //    Headers = responseHeadersString.ToString(),
            //    AbsoluteUri = uriAccessed,
            //    Host = userHostAddress,
            //    RequestBody = Encoding.UTF8.GetString(responseMessage),
            //    UserHostAddress = userHostAddress,
            //    Useragent = useragent,
            //    RequestedMethod = requestedMethod.ToString(),
            //    StatusCode = string.Empty
            //};

            //messageLoggingHandler.OutgoingMessageAsync(responseLog);
            return response;
            
        }

    }

    public class MessageLogging
    {
        public void IncomingMessageAsync(ApiLog apiLog)
        {
            apiLog.RequestType = "Request";
            // var sqlErrorLogging = new ApiLogging();
            // sqlErrorLogging.InsertLog(apiLog);

            //string errorLogFilename = "ErrorLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            //string path = AppDomain.CurrentDomain.BaseDirectory + "/Log/" + errorLogFilename;

            //if (File.Exists(path))
            //{
            //    using (StreamWriter stwriter = new StreamWriter(path, true))
            //    {
            //        stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
            //        stwriter.WriteLine("Requested Header :" + apiLog.Headers);
            //        stwriter.WriteLine("Requested URL :" + apiLog.AbsoluteUri);
            //        stwriter.WriteLine("Requested Method :" + apiLog.RequestedMethod);
            //        stwriter.WriteLine("Data: " + apiLog.RequestBody);
            //        stwriter.WriteLine("-------------------End----------------------------");
            //        stwriter.Close();
            //    }
            //}
            //else
            //{
            //    StreamWriter stwriter = File.CreateText(path);
            //    stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
            //    stwriter.WriteLine("Requested Header :" + apiLog.Headers);
            //    stwriter.WriteLine("Requested URL :" + apiLog.AbsoluteUri);
            //    stwriter.WriteLine("Requested Method :" + apiLog.RequestedMethod);
            //    stwriter.WriteLine("Data: " + apiLog.RequestBody);
            //    stwriter.WriteLine("-------------------End----------------------------");
            //    stwriter.Close();
            //}
        }

        public void OutgoingMessageAsync(ApiLog apiLog)
        {
            apiLog.RequestType = "Response";
            //var sqlErrorLogging = new ApiLogging();
            // sqlErrorLogging.InsertLog(apiLog);            
        }
    }

}