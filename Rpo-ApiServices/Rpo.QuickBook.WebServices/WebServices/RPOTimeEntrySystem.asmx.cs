
namespace Rpo.QuickBook.WebServices.WebServices
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Services;
    using System.Xml;
    using ApiServices.Model.Models.Enums;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.IO;
    /// <summary>
    /// Web Service Namespace="http://developer.intuit.com/".
    /// Web Service Name="WCWebService".
    /// Web Service Description="Sample WebService in ASP.NET to demonstrate QuickBooks WebConnector".
    /// </summary>
    [WebService(
        Namespace = "http://developer.intuit.com/",
        Name = "RPOTimeEntrySystem",
        Description = "Web Services to Connect QuickBooks Desktop via QuickBooks WebConnector")]
    public class RPOTimeEntrySystem : System.Web.Services.WebService
    {




        /// <summary>
        /// The event log variable.
        /// </summary>
        private readonly System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

        ///// <summary>
        ///// The customer commands.
        ///// </summary>
        //private readonly ICustomerCommands customerCommands;

        /// <summary>
        /// The CRM invoices command.
        /// </summary>
        //private readonly ICrmInvoicesCommand crmInvoicesCommand;

        /// <summary>
        /// The components.
        /// </summary>
        private readonly IContainer components = null;

        /// <summary>
        /// The number of request variable.
        /// </summary>
        private readonly ArrayList req = new ArrayList();

        /// <summary>
        /// The count variable.
        /// </summary>
        private int count = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RPOTimeEntrySystem"/> class.
        /// </summary>
        public RPOTimeEntrySystem()
        {
            //this.customerCommands = new CustomerCommands(CLI.WebServices.Utilities.AppSettings.ConnectionString);
            //this.crmInvoicesCommand = new CrmInvoicesCommand(CLI.WebServices.Utilities.AppSettings.ConnectionString);
            this.InitializeComponent();
            this.initEvLog();
        }

        /// <summary>
        /// Servers the version.
        /// </summary>
        /// <returns>System.String Version string representing server version.</returns>
        [WebMethod]
        public string serverVersion()
        {
            string serverVersion = "2.0.0.1";
            string eventLogTxt = "WebMethod: serverVersion() has been called " +
                                 "by QBWebconnector" + "\r\n\r\n";
            eventLogTxt = eventLogTxt + "No Parameters required.";
            eventLogTxt = eventLogTxt + "Returned: " + serverVersion;
            return serverVersion;
        }

        /// <summary>
        /// Clients the version.
        /// </summary>
        /// <param name="strVersion">The string version parameter.</param>
        /// <returns>System.String client version.</returns>
        [WebMethod]
        public string clientVersion(string strVersion)
        {
            string eventLogTxt = "WebMethod: clientVersion() has been called " + "by QBWebconnector" + "\r\n\r\n";
            eventLogTxt = eventLogTxt + "Parameters received:\r\n";
            eventLogTxt = eventLogTxt + "string strVersion = " + strVersion + "\r\n";
            eventLogTxt = eventLogTxt + "\r\n";
            string retVal = null;
            double recommendedVersion = 1.5;
            double supportedMinVersion = 1.0;
            double suppliedVersion = Convert.ToDouble(this.parseForVersion(strVersion));
            eventLogTxt = eventLogTxt + "QBWebConnector version = " + strVersion + "\r\n";
            eventLogTxt = eventLogTxt + "Recommended Version = " + recommendedVersion.ToString() + "\r\n";
            eventLogTxt = eventLogTxt + "Supported Minimum Version = " + supportedMinVersion.ToString() + "\r\n";
            eventLogTxt = eventLogTxt + "SuppliedVersion = " + suppliedVersion.ToString() + "\r\n";
            if (suppliedVersion < recommendedVersion)
            {
                retVal = "W:We recommend that you upgrade your QBWebConnector";
            }
            else if (suppliedVersion < supportedMinVersion)
            {
                retVal = "E:You need to upgrade your QBWebConnector";
            }

            eventLogTxt = eventLogTxt + "\r\n";
            eventLogTxt = eventLogTxt + "Return values: " + "\r\n";
            eventLogTxt = eventLogTxt + "string retVal = " + retVal;
            this.logEvent(eventLogTxt);
            return retVal;
        }

        /// <summary>
        /// Authenticates the specified string user name.
        /// </summary>
        /// <param name="strUserName">Name of the string user.</param>
        /// <param name="strPassword">The string password parameter.</param>
        /// <returns>System.String[] authenticate.
        /// String[0] = ticket.
        /// String[1] = empty string = use current company file.
        /// String[1] = "NVU" = not valid user.
        /// String[1] = "none" = no further request/no further action required.
        /// String[1] = any other string value = use this company file.</returns>
        [WebMethod]
        public string[] authenticate(string strUserName, string strPassword)
        {
            WriteLogQB("Start authenticate Username and Password");
            string eventLogTxt = "WebMethod: authenticate() has been called by QBWebconnector" + "\r\n\r\n";
            eventLogTxt = eventLogTxt + "Parameters received:\r\n";
            eventLogTxt = eventLogTxt + "string strUserName = " + strUserName + "\r\n";
            eventLogTxt = eventLogTxt + "string strPassword = " + strPassword + "\r\n";
            eventLogTxt = eventLogTxt + "\r\n";
            WriteLogQB(eventLogTxt);
            string[] authReturn = new string[2];
            //// Code below uses a random GUID to use as session ticket
            //// An example of a GUID is {85B41BEE-5CD9-427a-A61B-83964F1EB426}
            authReturn[0] = System.Guid.NewGuid().ToString();
            //// For simplicity of sample, a hardcoded username/password is used.
            //// In real world, you should handle authentication in using a standard way. 
            //// For example, you could validate the username/password against an LDAP 
            //// or a directory server
            string pwd = "admin";
            eventLogTxt = eventLogTxt + "Password locally stored = " + pwd + "\r\n";
            if (strUserName.Trim().Equals("Admin") && strPassword.Trim().Equals(pwd))
            {
                //// An empty string for authReturn[1] means asking QBWebConnector 
                //// to connect to the company file that is currently openned in QB
                authReturn[1] = Rpo.QuickBook.WebServices.Utilities.AppSettings.RPOTimeEntrySystem;
            }
            else
            {
                authReturn[1] = "nvu";
            }
            //// You could also return "none" to indicate there is no work to do
            //// or a company filename in the format C:\full\path\to\company.qbw
            //// based on your program logic and requirements.
            eventLogTxt = eventLogTxt + "\r\n";
            eventLogTxt = eventLogTxt + "Return values: " + "\r\n";
            eventLogTxt = eventLogTxt + "string[] authReturn[0] = " + authReturn[0].ToString() + "\r\n";
            eventLogTxt = eventLogTxt + "string[] authReturn[1] = " + authReturn[1].ToString();
            this.logEvent(eventLogTxt);

            WriteLogQB(eventLogTxt);

            return authReturn;
        }

        /// <summary>
        /// WebMethod - connectionError() called To facilitate capturing of QuickBooks error and notifying it to web services.
        /// </summary>
        /// <param name="ticket">The ticket parameter.</param>
        /// <param name="hresult">The HResult parameter.</param>
        /// <param name="message">The message parameter.</param>
        /// <returns>System.String connection error.
        /// Return Value = “done” = no further action required from QBWebConnector.
        /// Return Value = any other string value = use this name for company file.</returns>
        [WebMethod(Description = "This web method facilitates web service to handle connection error between QuickBooks and QBWebConnector", EnableSession = true)]
        public string connectionError(string ticket, string hresult, string message)
        {
            WriteLogQB("Start connectionError method");
            if (this.Session["ce_counter"] == null)
            {
                this.Session["ce_counter"] = 0;
            }

            string eventLogTxt = "WebMethod: connectionError() has been called by QBWebconnector" + "\r\n\r\n";
            eventLogTxt = eventLogTxt + "Parameters received:\r\n";
            eventLogTxt = eventLogTxt + "string ticket = " + ticket + "\r\n";
            eventLogTxt = eventLogTxt + "string hresult = " + hresult + "\r\n";
            eventLogTxt = eventLogTxt + "string message = " + message + "\r\n";
            eventLogTxt = eventLogTxt + "\r\n";

            string retVal = null;
            //// 0x80040400 - QuickBooks found an error when parsing the provided XML text stream. 
            const string QB_ERROR_WHEN_PARSING = "0x80040400";
            //// 0x80040401 - Could not access QuickBooks.  
            const string QB_COULDNT_ACCESS_QB = "0x80040401";
            //// 0x80040402 - Unexpected error. Check the qbsdklog.txt file for possible, additional information. 
            const string QB_UNEXPECTED_ERROR = "0x80040402";
            if (hresult.Trim().Equals(QB_ERROR_WHEN_PARSING))
            {
                eventLogTxt = eventLogTxt + "HRESULT = " + hresult + "\r\n";
                eventLogTxt = eventLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else if (hresult.Trim().Equals(QB_COULDNT_ACCESS_QB))
            {
                eventLogTxt = eventLogTxt + "HRESULT = " + hresult + "\r\n";
                eventLogTxt = eventLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else if (hresult.Trim().Equals(QB_UNEXPECTED_ERROR))
            {
                eventLogTxt = eventLogTxt + "HRESULT = " + hresult + "\r\n";
                eventLogTxt = eventLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else
            {
                //// Depending on various hresults return different value 
                if ((int)this.Session["ce_counter"] == 0)
                {
                    //// Try again with this company file
                    eventLogTxt = eventLogTxt + "HRESULT = " + hresult + "\r\n";
                    eventLogTxt = eventLogTxt + "Message = " + message + "\r\n";
                    eventLogTxt = eventLogTxt + "Sending empty company file to try again.";
                    retVal = string.Empty;
                }
                else
                {
                    eventLogTxt = eventLogTxt + "HRESULT = " + hresult + "\r\n";
                    eventLogTxt = eventLogTxt + "Message = " + message + "\r\n";
                    eventLogTxt = eventLogTxt + "Sending DONE to stop.";
                    retVal = "DONE";
                }
            }

            eventLogTxt = eventLogTxt + "\r\n";
            eventLogTxt = eventLogTxt + "Return values: " + "\r\n";
            eventLogTxt = eventLogTxt + "string retVal = " + retVal + "\r\n";
            this.logEvent(eventLogTxt);
            WriteLogQB(eventLogTxt);
            this.Session["ce_counter"] = ((int)this.Session["ce_counter"]) + 1;
            WriteLogQB("End the Function connection Error");

            return retVal;
        }

        /// <summary>
        /// Sends the request XML.
        /// </summary>
        /// <param name="ticket">The ticket parameter.</param>
        /// <param name="strHCPResponse">The string HCP response parameter.</param>
        /// <param name="strCompanyFileName">Name of the string company file.</param>
        /// <param name="qbXMLCountry">The QB XML country parameter.</param>
        /// <param name="qbXMLMajorVers">The QB XML major version parameter.</param>
        /// <param name="qbXMLMinorVers">The QB XML minor version parameter.</param>
        /// <returns>System.String send request XML.
        /// Request XML for QBWebConnector to process.</returns>
        [WebMethod(Description = "This web method facilitates web service to send request XML to QuickBooks via QBWebConnector", EnableSession = true)]
        public string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            WriteLogQB("Start the  Send Request method");
            if (this.Session["counter"] == null)
            {
                this.Session["counter"] = 0;
                WriteLogQB("Session counter value = " + this.Session["counter"]);
            }

            string eventLogTxt = "WebMethod: sendRequestXML() has been called by QBWebconnector" + "\r\n\r\n";
            eventLogTxt = eventLogTxt + "Parameters received:\r\n";
            eventLogTxt = eventLogTxt + "string ticket = " + ticket + "\r\n";
            eventLogTxt = eventLogTxt + "string strHCPResponse = " + strHCPResponse + "\r\n";
            eventLogTxt = eventLogTxt + "string strCompanyFileName = " + strCompanyFileName + "\r\n";
            eventLogTxt = eventLogTxt + "string qbXMLCountry = " + qbXMLCountry + "\r\n";
            eventLogTxt = eventLogTxt + "int qbXMLMajorVers = " + qbXMLMajorVers.ToString() + "\r\n";
            eventLogTxt = eventLogTxt + "int qbXMLMinorVers = " + qbXMLMinorVers.ToString() + "\r\n";
            eventLogTxt = eventLogTxt + "\r\n";
            ArrayList req;

            if (this.Session["req"] == null)
            {
                req = this.buildRequest();
                this.Session["req"] = req;
                WriteLogQB("Total objects in request are = " + req.Count.ToString());
            }
            req = (ArrayList)this.Session["req"];


            string request = string.Empty;
            int total = req.Count;
            WriteLogQB("Total is = " + total.ToString());
            this.count = Convert.ToInt32(this.Session["counter"]);
            WriteLogQB("Count is = " + this.count.ToString());

            if (this.count < total)
            {

                //custom request

                //   string str  = "<? xml version =\"1.0\"?><?qbxml version=\"10.0\"?><QBXML><QBXMLMsgsRq onError=\"stopOnError\"><TimeTrackingAddRq requestID=\"4104\"><TimeTrackingAdd><TxnDate>2019-02-11</TxnDate><EntityRef><FullName>Sunay Doshi</FullName></EntityRef><CustomerRef><FullName>Contractor Inc.:012151</FullName></CustomerRef><ItemServiceRef><FullName>Consulting</FullName></ItemServiceRef><Duration>PT100H58M0S</Duration><Notes>Testing QB</Notes><BillableStatus>Billable</BillableStatus></TimeTrackingAdd></TimeTrackingAddRq></QBXMLMsgsRq></QBXML>";

                request = req[this.count].ToString();

                //   request = str;

                eventLogTxt = eventLogTxt + "sending request no = " + (this.count + 1) + "\r\n";
                this.Session["counter"] = ((int)this.Session["counter"]) + 1;
                WriteLogQB("Session counter is = " + this.Session["counter"].ToString());
            }
            else
            {
                this.count = 0;
                this.Session["counter"] = 0;
                this.Session["req"] = null;
                request = string.Empty;
            }

            eventLogTxt = eventLogTxt + "\r\n";
            eventLogTxt = eventLogTxt + "Return values: " + "\r\n";
            eventLogTxt = eventLogTxt + "string request = " + request + "\r\n";
            this.logEvent(eventLogTxt);
            WriteLogQB(eventLogTxt);
            WriteLogQB("End the Send Request method");
            return request;
        }

        /// <summary>
        /// Receives the response XML from QuickBooks.
        /// </summary>
        /// <param name="ticket">The ticket parameter.</param>
        /// <param name="response">The response parameter.</param>
        /// <param name="hresult">The HResult parameter.</param>
        /// <param name="message">The message parameter.</param>
        /// <returns>Integer Value. Where Greater than zero  = There are more request to send.
        /// 100 = Done. no more request to send.
        /// Less than zero  = Custom Error codes.</returns>
        [WebMethod(Description = "This web method facilitates web service to receive response XML from QuickBooks via QBWebConnector", EnableSession = true)]
        public int receiveResponseXML(string ticket, string response, string hresult, string message)
        {
            WriteLogQB("Start the Recived Response method");
            string eventLogTxt = "WebMethod: receiveResponseXML() has been called by QBWebconnector" + "\r\n\r\n";
            eventLogTxt = eventLogTxt + "Parameters received:\r\n";
            eventLogTxt = eventLogTxt + "string ticket = " + ticket + "\r\n";
            eventLogTxt = eventLogTxt + "string response = " + response + "\r\n";
            eventLogTxt = eventLogTxt + "string hresult = " + hresult + "\r\n";
            eventLogTxt = eventLogTxt + "string message = " + message + "\r\n";
            eventLogTxt = eventLogTxt + "\r\n";
            WriteLogQB("Res: " + response);
            WriteLogQB("Message: " + message);

            if (response != string.Empty)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNodeList customerAddRs = doc.GetElementsByTagName("TimeTrackingAddRs");
                for (int attributes = 0; attributes < customerAddRs.Count; attributes++)
                {
                    string statusMessage = customerAddRs[attributes].Attributes["statusMessage"].Value;
                    string statusCode = customerAddRs[attributes].Attributes["statusCode"].Value;
                    string requestId = customerAddRs[attributes].Attributes["requestID"].Value;

                    WriteLogQB("requestId: " + requestId);
                    WriteLogQB("statusMessage: " + statusMessage);
                    WriteLogQB("statusCode: " + statusCode);

                    if (statusCode == "0" || statusCode == "3100")
                    {
                        RpoContext rpoContext = new RpoContext();
                        int iIdJobTimeNote = Convert.ToInt32(requestId);
                        JobTimeNote jobTimeNote = rpoContext.JobTimeNotes.FirstOrDefault(x => x.Id == iIdJobTimeNote);
                        if (jobTimeNote != null)
                        {
                            jobTimeNote.IsQuickbookSynced = true;
                            jobTimeNote.QuickbookSyncError = string.Empty;
                            jobTimeNote.QuickbookSyncedDate = DateTime.UtcNow;
                            rpoContext.SaveChanges();
                        }
                    }
                    else
                    {
                        RpoContext rpoContext = new RpoContext();
                        int iIdJobTimeNote = Convert.ToInt32(requestId);
                        JobTimeNote jobTimeNote = rpoContext.JobTimeNotes.FirstOrDefault(x => x.Id == iIdJobTimeNote);
                        if (jobTimeNote != null)
                        {
                            jobTimeNote.IsQuickbookSynced = false;
                            jobTimeNote.QuickbookSyncError = statusMessage;
                            jobTimeNote.QuickbookSyncedDate = DateTime.UtcNow;
                            rpoContext.SaveChanges();
                        }
                    }
                }
            }
            //else
            //{
            //    WriteLogQB("Calling response is blank & call getlasterror");

            //    string errorresponse=  getLastError(ticket);

            //    WriteLogQB("Error Response: " + errorresponse);

            //    if (!string.IsNullOrEmpty(errorresponse))
            //    {

            //    }

            //}
            else if (message.Trim().ToLower().Contains("QuickBooks found an error when parsing the provided XML text stream".Trim().ToLower()))
            {
                RpoContext rpoContext = new RpoContext();

                int reqcount = 0;
                ArrayList reqestarray = new ArrayList();

                if (this.Session["req"] != null)
                {
                    reqestarray = (ArrayList)this.Session["req"];
                }

                if (this.Session["counter"] != null)
                {
                    reqcount = Convert.ToInt32(this.Session["counter"]);
                }
                string respstr = reqestarray[reqcount - 1].ToString();

                if (!string.IsNullOrEmpty(respstr))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(respstr);
                    if (doc != null)
                    {
                        XmlNodeList customerAddRq = doc.GetElementsByTagName("TimeTrackingAddRq");
                        for (int attributes = 0; attributes < customerAddRq.Count; attributes++)
                        {
                            string requestId = customerAddRq[attributes].Attributes["requestID"].Value;

                            int iIdJobTimeNote = Convert.ToInt32(requestId);
                            JobTimeNote jobTimeNote = rpoContext.JobTimeNotes.FirstOrDefault(x => x.Id == iIdJobTimeNote);
                            if (jobTimeNote != null)
                            {
                                jobTimeNote.IsQuickbookSynced = false;
                                jobTimeNote.QuickbookSyncError = message;
                                jobTimeNote.QuickbookSyncedDate = DateTime.UtcNow;
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }               
              
            }
            //else if (message != string.Empty)
            //{
            //    //var quickBooksActivityLog = new QuickBooksActivityLog()
            //    //{
            //    //    StatusMessage = message,
            //    //    TicketId = ticket
            //    //};

            //    //this.customerCommands.InsertQuickBooksActivityLog(quickBooksActivityLog);
            //}

            int retVal = 0;
            if (!hresult.ToString().Equals(string.Empty))
            {
                //// if there is an error with response received, web service could also return a -ve int
                eventLogTxt = eventLogTxt + "HRESULT = " + hresult + "\r\n";
                eventLogTxt = eventLogTxt + "Message = " + message + "\r\n";
                retVal = -101;
            }
            else
            {
                eventLogTxt = eventLogTxt + "Length of response received = " + response.Length + "\r\n";
                //  ArrayList req = this.buildRequest();
                // ArrayList req = reqglobal;

                ArrayList req = (ArrayList)this.Session["req"];
                int total = req.Count == 0 ? 1 : req.Count;

                int count = Convert.ToInt32(this.Session["counter"]);
                int percentage = (count * 100) / total;
                if (percentage >= 100)
                {
                    count = 0;
                    this.Session["counter"] = 0;
                }
                WriteLogQB("Percentage value return : " + percentage.ToString());
                retVal = percentage;

            }

            eventLogTxt = eventLogTxt + "\r\n";
            eventLogTxt = eventLogTxt + "Return values: " + "\r\n";
            eventLogTxt = eventLogTxt + "int retVal= " + retVal.ToString() + "\r\n";
            this.logEvent(eventLogTxt);

            WriteLogQB(eventLogTxt);
            WriteLogQB("End the Send Response method");
            return retVal;
        }

        /// <summary>
        /// Gets the last error.
        /// </summary>
        /// <param name="ticket">The ticket parameter.</param>
        /// <returns>Error message describing last web service error.</returns>
        [WebMethod]
        public string getLastError(string ticket)
        {
            WriteLogQB("Get Last Error Method Call:");
            WriteLogQB("Ticket: " + ticket);

            string eventLogTxt = "WebMethod: getLastError() has been called by QBWebconnector" + "\r\n\r\n";
            eventLogTxt = eventLogTxt + "Parameters received:\r\n";
            eventLogTxt = eventLogTxt + "string ticket = " + ticket + "\r\n";
            eventLogTxt = eventLogTxt + "\r\n";
            WriteLogQB(eventLogTxt);

            int errorCode = 0;
            string retVal = null;
            if (errorCode == -101)
            {
                retVal = "QuickBooks was not running!"; // This is just an example of custom user errors
            }
            else
            {
                retVal = "Error!";
            }

            eventLogTxt = eventLogTxt + "\r\n";
            eventLogTxt = eventLogTxt + "Return values: " + "\r\n";
            eventLogTxt = eventLogTxt + "string retVal= " + retVal + "\r\n";
            this.logEvent(eventLogTxt);

            WriteLogQB("return Response: " + eventLogTxt);
            WriteLogQB("Ticket: " + ticket);
            return retVal;
        }

        /// <summary>
        /// Closes the connection.
        /// WebMethod - closeConnection() will be called at the end of a successful update session, QBWebConnector will call this web method.
        /// </summary>
        /// <param name="ticket">The ticket parameter.</param>
        /// <returns>String closeConnection result.</returns>
        [WebMethod]
        public string closeConnection(string ticket)
        {
            string eventLogTxt = "WebMethod: closeConnection() has been called by QBWebconnector" + "\r\n\r\n";
            eventLogTxt = eventLogTxt + "Parameters received:\r\n";
            eventLogTxt = eventLogTxt + "string ticket = " + ticket + "\r\n";
            eventLogTxt = eventLogTxt + "\r\n";
            string retVal = null;

            retVal = "OK";

            eventLogTxt = eventLogTxt + "\r\n";
            eventLogTxt = eventLogTxt + "Return values: " + "\r\n";
            eventLogTxt = eventLogTxt + "string retVal= " + retVal + "\r\n";
            this.logEvent(eventLogTxt);
            return retVal;
        }

        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <returns>ArrayList build request.</returns>
        public ArrayList buildRequest()
        {
            RpoContext rpoContext = new RpoContext();
            string strRequestXML = string.Empty;
            XmlDocument inputXMLDoc = null;
            try
            {
                bool? isQuickbookSynced = false;
                List<JobTimeNote> jobTimeNoteList = rpoContext.JobTimeNotes.Include("Job")
                    .Include("JobFeeSchedule.RfpWorkType")
                    .Include("RfpJobType")
                    .Include("CreatedByEmployee")
                    .Include("LastModifiedByEmployee")


                    .Where(x => x.IsQuickbookSynced == null || x.IsQuickbookSynced == isQuickbookSynced).ToList();

                //sunay testing
                //.Where(x => x.IsQuickbookSynced == true || x.IsQuickbookSynced == isQuickbookSynced).ToList();
                foreach (var item in jobTimeNoteList.Where(d => d.JobBillingType != JobBillingType.NonBillableItems).OrderBy(d => d.QuickbookSyncError))
                {
                    if (item.CreatedByEmployee != null)
                    {
                        inputXMLDoc = new XmlDocument();
                        inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
                        inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"10.0\""));
                        XmlElement quickBoosXML = inputXMLDoc.CreateElement("QBXML");
                        inputXMLDoc.AppendChild(quickBoosXML);
                        XmlElement quickBooksXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
                        quickBoosXML.AppendChild(quickBooksXMLMsgsRq);
                        quickBooksXMLMsgsRq.SetAttribute("onError", "continueOnError");
                        XmlElement timeTrackingAddRq = inputXMLDoc.CreateElement("TimeTrackingAddRq");
                        timeTrackingAddRq.SetAttribute("requestID", item.Id.ToString());
                        quickBooksXMLMsgsRq.AppendChild(timeTrackingAddRq);
                        XmlElement timeTrackingAdd = inputXMLDoc.CreateElement("TimeTrackingAdd");
                        timeTrackingAddRq.AppendChild(timeTrackingAdd);
                        timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "TxnDate", item.TimeNoteDate.ToString("yyyy-MM-dd")));

                        XmlElement entityRef = inputXMLDoc.CreateElement("EntityRef");
                        timeTrackingAdd.AppendChild(entityRef);
                        entityRef.AppendChild(this.MakeSimpleElem(inputXMLDoc, "FullName", item.CreatedByEmployee.QBEmployeeName));

                        XmlElement customerRef = inputXMLDoc.CreateElement("CustomerRef");
                        timeTrackingAdd.AppendChild(customerRef);

                        string jobaddress = string.Empty;
                        jobaddress = (string.IsNullOrEmpty(item.Job.HouseNumber) ? string.Empty : item.Job.HouseNumber) + " " + (string.IsNullOrEmpty(item.Job.StreetNumber) ? string.Empty : item.Job.StreetNumber) + ", " + ((item.Job.Borough == null) && (string.IsNullOrEmpty(item.Job.Borough.Description)) ? string.Empty : item.Job.Borough.Description);
                        customerRef.AppendChild(this.MakeSimpleElem(inputXMLDoc, "FullName", !string.IsNullOrEmpty(item.Job.QBJobName) ? item.Job.QBJobName.Trim() : string.Empty));

                        if (item.JobBillingType == JobBillingType.AdditionalBilling || item.JobBillingType == JobBillingType.ScopeBilling)
                        {
                            XmlElement itemServiceRef = inputXMLDoc.CreateElement("ItemServiceRef");
                            timeTrackingAdd.AppendChild(itemServiceRef);
                            if (item.JobBillingType == JobBillingType.ScopeBilling)
                            {
                                itemServiceRef.AppendChild(this.MakeSimpleElem(inputXMLDoc, "FullName", item.JobFeeSchedule.RfpWorkType.Name));
                            }
                            else if (item.JobBillingType == JobBillingType.AdditionalBilling)
                            {
                                itemServiceRef.AppendChild(this.MakeSimpleElem(inputXMLDoc, "FullName", item.RfpJobType.Name));
                            }
                            else
                            {

                            }

                        }
                        //string timeQuantity = item.TimeQuantity != null ? Convert.ToString(item.TimeQuantity) : string.Empty;
                        //string hour = string.Empty;
                        //string minute = string.Empty;
                        //if (!string.IsNullOrEmpty(timeQuantity))
                        //{
                        //    string[] time = timeQuantity.Split('.');
                        //    if (time != null && time.Count() > 0)
                        //    {
                        //        if (time.Count() > 1)
                        //        {
                        //            hour = time[0];
                        //            minute = time[1];
                        //        }
                        //        else
                        //        {
                        //            hour = time[0];
                        //            minute = "0";
                        //        }
                        //    }
                        //}

                        timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "Duration", "PT" + item.TimeHours + "H" + item.TimeMinutes + "M0S"));
                        timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "Notes", item.ProgressNotes));

                        //< !--BillableStatus may have one of the following values: Billable, NotBillable, HasBeenBilled-- >
                        if (item.JobBillingType == JobBillingType.NonBillableItems)
                        {
                            timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "BillableStatus", "NotBillable"));
                        }
                        else if (item.JobBillingType == JobBillingType.ScopeBilling)
                        {
                            timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "BillableStatus", "Billable"));
                        }
                        else
                        {
                            timeTrackingAdd.AppendChild(this.MakeSimpleElem(inputXMLDoc, "BillableStatus", "Billable"));
                        }

                        strRequestXML = inputXMLDoc.OuterXml;
                        WriteLogQB(strRequestXML);
                        this.req.Add(strRequestXML);
                        strRequestXML = string.Empty;
                        inputXMLDoc = null;
                        quickBoosXML = null;
                        quickBooksXMLMsgsRq = null;
                    }
                }
            }
            catch (Exception ex)
            {
                //this.crmInvoicesCommand.InsertActivityLog("ERROR IN CUSTOMER ADD: " + ex.Message, "QUICKBOOKS WEB CONNECTOR");
            }
            // reqglobal = this.req;
            return this.req;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.MarshalByValueComponent" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Initializes the event log.
        /// </summary>
        private void initEvLog()
        {
            try
            {
                string source = "WCWebService";
                if (!System.Diagnostics.EventLog.SourceExists(source))
                {
                    System.Diagnostics.EventLog.CreateEventSource(source, "Application");
                }

                this.eventLog.Source = source;
            }
            catch
            {
            }

            return;
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="logText">The log text parameter.</param>
        private void logEvent(string logText)
        {
            try
            {
                this.eventLog.WriteEntry(logText, System.Diagnostics.EventLogEntryType.Information);
            }
            catch
            {
            }

            return;
        }

        /// <summary>
        /// Parses for version.
        /// </summary>
        /// <param name="input">The input parameter.</param>
        /// <returns>System.String parse for version.</returns>
        private string parseForVersion(string input)
        {
            // This method is created just to parse the first two version components
            // out of the standard four component version number:
            // <Major>.<Minor>.<Release>.<Build>
            // 
            // As long as you get the version in right format, you could use
            // any algorithm here. 
            string retVal = string.Empty;
            string major = string.Empty;
            string minor = string.Empty;
            Regex version = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)(\.\w+){0,2}$", RegexOptions.Compiled);
            Match versionMatch = version.Match(input);
            if (versionMatch.Success)
            {
                major = versionMatch.Result("${major}");
                minor = versionMatch.Result("${minor}");
                retVal = major + "." + minor;
            }
            else
            {
                retVal = input;
            }

            return retVal;
        }

        /// <summary>
        /// Makes the simple elem.
        /// </summary>
        /// <param name="doc">The document parameter.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="tagVal">The tag value parameter.</param>
        /// <returns>XmlElement make simple elem.</returns>
        private XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
        {
            XmlElement elem = doc.CreateElement(tagName);
            elem.InnerText = tagVal;
            return elem;
        }

        public void WriteLogQB(string message)
        {
            string errorLogFilename = "QBLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

            string path = @"C:/RPO/Quickbooks/Log/" + errorLogFilename; // server static path for using log

            if (File.Exists(path))
            {
                using (StreamWriter stwriter = new StreamWriter(path, true))
                {
                    stwriter.WriteLine("-------------------Web Client Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                    stwriter.WriteLine("Message: " + message);
                    stwriter.WriteLine("-------------------End----------------------------");
                    stwriter.Close();
                }
            }
            else
            {
                StreamWriter stwriter = File.CreateText(path);
                stwriter.WriteLine("-------------------Web Client Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                stwriter.WriteLine("Message: " + message);
                stwriter.WriteLine("-------------------End----------------------------");
                stwriter.Close();
            }



        }
    }
}