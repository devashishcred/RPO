using System;
using System.Windows.Forms;
using Rpo.ApiServices.Model;
using Rpo.ApiServices.Model.Models;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Rpo.Efiling.Enums;
using System.Net;
using System.Text;

namespace Rpo.Efiling
{
    public partial class RpoEfiling : Form
    {
        public RpoEfiling()
        {
            InitializeComponent();


            // wbEfiling.Navigate("about:blank");
            //dynamic ax = this.wbEfiling.ActiveXInstance;
           // ax.NewWindow += new NewWindowDelegate(this.OnNewWindow);

            this.Dock = DockStyle.Fill;
            this.WindowState = FormWindowState.Maximized;
        }

        #region Properies

        public static Job job { get; set; }

        public static JobDocument jobDocument { get; set; }

        public static int CompletedStep { get; set; }

        public static bool IseFilingCompleted = true;

        public static bool IsSimpleEfiling = true;

        public static ICollection<JobDocumentField> jobDocumentFieldList { get; set; }

        public static List<JobApplicationWorkPermitType> jobApplicationWorkPermitTypeList { get; set; }

        public static JobApplication jobApplication { get; set; }

        public static JobDocumentField jobDocumentField_Applicantion { get; set; }

        public static string workDescription { get; set; }

        public static JobDocumentField jobDocumentField_WorkPermit { get; set; }

        #endregion

        #region Events

        private delegate void NewWindowDelegate(string URL, int Flags, string TargetFrameName, ref object PostData, string Headers, ref bool Processed);
        private void OnNewWindow(string URL, int Flags, string TargetFrameName, ref object PostData, string Headers, ref bool Processed)
        {
            Processed = true;

            if (URL != "about:blank")
            {
                //TabPage tabpage = new TabPage();
                //tabpage.Text = "New File";
                //tableLayoutPanel2.Controls.Add(tabpage);
                //WebBrowser webbrowser = new WebBrowser();
                //webbrowser.Parent = tabpage;
                //webbrowser.Dock = DockStyle.Fill;
                ////webbrowser.Navigate("www.google.com");
                wbEfiling.Navigate(new Uri(URL));
            }
            //RpoEfiling Popup = new RpoEfiling();
            //Popup.wbEfiling.Navigate(new Uri(URL));
            //Popup.Show();
        }
        private void Wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            tableLayoutPanel1.Text = (sender as WebBrowser).DocumentTitle;
        }
        private void RpoEfiling_Load(object sender, EventArgs e)
        {
            try
            {
                lblDocument.MaximumSize = new Size(650, 0);
                lblDocument.AutoSize = true;
                lblDocumentDetails.Text = string.Empty;
                lblDocument.Text = string.Empty;
                lblApplicationType.Text = string.Empty;
                ClearControls();
                this.WindowState = FormWindowState.Normal;
                txtJobNumber.Focus();
            }
            catch (Exception)
            {
            }
        }

        private void wbEfiling_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (IsSimpleEfiling)
                {
                    if (!IseFilingCompleted)
                    {
                        if (wbEfiling.Url == new Uri("https://a810-efiling.nyc.gov/eRenewal/JobFiling_UpdateInfo"))
                        {
                            if (CompletedStep == 1)
                            {
                                ShowProgress();
                                FillSecondStepData();

                            }

                        }

                        else if (wbEfiling.Url == new Uri("https://a810-efiling.nyc.gov/eRenewal/JobFiling_UpdateWorkType"))
                        {
                            if (CompletedStep == 2)
                            {
                                ShowProgress();
                                FillThirdStepData();
                                //MessageBox.Show("Document Completed Step 3");
                            }

                        }
                        else if (wbEfiling.Url == new Uri("https://a810-efiling.nyc.gov/eRenewal/JobFiling_UpdateAddCons"))
                        {
                            if (CompletedStep == 3)
                            {
                                ShowProgress();
                                FillFourthStepData();
                                //MessageBox.Show("Document Completed Step 4");
                            }

                        }
                        else if (wbEfiling.Url.AbsolutePath == "/eRenewal/JobFiling_Comments" && (CompletedStep == 3 || CompletedStep == 4))
                        {
                            ShowProgress();
                            FillFifthStepData();
                            //MessageBox.Show("E-filing of the data is complete!");
                        }
                        else if (wbEfiling.Url == new Uri("https://a810-efiling.nyc.gov/eRenewal/JobFiling_UpdateComments") && CompletedStep == 5)
                        {

                            ShowProgress();
                            FillFourthStepAfterFifthData();
                        }
                        else if (wbEfiling.Url.AbsolutePath == "/eRenewal/JobFiling_Comments" && CompletedStep == 6)
                        {
                            ShowProgress();
                            FillFifthStepAfterRefillData();
                            IseFilingCompleted = true;
                            MessageBox.Show("E-filing of the data is complete!");
                        }
                    }
                }
                else
                {
                    if (wbEfiling.Url == new Uri("https://a810-efiling.nyc.gov/eRenewal/JobFiling_UpdateInfo"))
                    {
                        if (CompletedStep == 1)
                        {
                            ShowProgress();
                            FillThirdStepData();
                        }
                    }
                    else if (wbEfiling.Url == new Uri("https://a810-efiling.nyc.gov/eRenewal/JobFiling_UpdateAddCons"))
                    {
                        if (CompletedStep == 3)
                        {
                            ShowProgress();
                            FillFourthStepData();
                            //MessageBox.Show("Document Completed Step 4");
                        }

                    }
                    else if (wbEfiling.Url.AbsolutePath == "/eRenewal/JobFiling_Comments" && CompletedStep == 4)
                    {
                        ShowProgress();
                        FillFifthStepData();
                        //MessageBox.Show("E-filing of the data is complete!");
                    }
                    else if (wbEfiling.Url == new Uri("https://a810-efiling.nyc.gov/eRenewal/JobFiling_UpdateComments") && CompletedStep == 5)
                    {

                        ShowProgress();
                        FillFourthStepAfterFifthData();
                    }
                    else if (wbEfiling.Url.AbsolutePath == "/eRenewal/JobFiling_Comments" && CompletedStep == 6)
                    {
                        ShowProgress();
                        FillFifthStepAfterRefillData();
                        IseFilingCompleted = true;
                        MessageBox.Show("E-filing of the data is complete!");
                    }
                }
            }
            catch (Exception)
            {
                CloseProgress();
            }
            //finally
            //{
            //    CloseProgress();
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtJobNumber.Text != null && txtJobNumber.Text != string.Empty)
                {
                    StartProgress();
                    tlpJobDetails.Visible = false;
                    tblJobDocumentDetail.Visible = false;
                    RpoContext rpoContext = new RpoContext();
                    string jobnumber = Convert.ToString(txtJobNumber.Text);
                    job = rpoContext.Jobs.Include("RfpAddress.Borough")
                        .Include("RfpAddress.Company")
                        .Include("RfpAddress.OwnerType")
                        .Include("RfpAddress.OwnerContact")
                        .Include("RfpAddress.SecondOfficerCompany")
                        .Include("RfpAddress.SecondOfficer")
                        .Include("RfpAddress.OccupancyClassification")
                        .Include("RfpAddress.ConstructionClassification")
                        .Include("RfpAddress.MultipleDwellingClassification")
                        .Include("RfpAddress.PrimaryStructuralSystem")
                        .Include("RfpAddress.StructureOccupancyCategory")
                        .Include("RfpAddress.SeismicDesignCategory")
                        .FirstOrDefault(x => x.JobNumber == jobnumber);

                    if (job != null)
                    {
                        lblBorough.Text = job != null && job.RfpAddress != null && job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty;
                        lblHouseNumber.Text = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber : string.Empty;
                        lblStreetName.Text = job != null && job.RfpAddress != null ? job.RfpAddress.Street : string.Empty;
                        lblBlock.Text = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                        lblLot.Text = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;

                        BindDropdown(job.Id);
                        tlpJobDetails.Visible = true;
                        tblJobDocumentDetail.Visible = true;
                        CloseProgress();
                    }
                    else
                    {
                        CloseProgress();
                        MessageBox.Show("Jobnumber not found in the system");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the jobnumber to search!");
                }
            }
            catch (Exception)
            {
                CloseProgress();
            }
        }

        private void btnFillInfo_Click(object sender, EventArgs e)
        {
            if (wbEfiling.Url.AbsolutePath == "/eRenewal/AHV_InitialPermit")
            {
                #region Initial AHV

                FillDataInitialAHV();

                #endregion
            }
            else if (wbEfiling.Url.AbsolutePath == "/eRenewal/JobFiling_WorkInfo")
            {
                #region EF 1

                FillFirstStepData();

                #endregion
            }
            else if (wbEfiling.Url.AbsolutePath == "/eRenewal/AHV_PermitByJobNumber")
            {
                #region AHV Renewal

                FillDataRenewalAHV();

                #endregion
            }
            else
            {
                MessageBox.Show("Please create new application or edit existing to efile");
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ClearControls();
                job = null;
                jobDocument = null;
                tlpJobDetails.Visible = false;
                tblJobDocumentDetail.Visible = false;
                CloseProgress();
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseProgress();
            }
        }

        private void ddlJobDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnAttachEF1_Click(object sender, EventArgs e)
        {
            try
            {
                if (job.Id > 0)
                {
                    RpoContext rpoContext = new RpoContext();
                    int idJobDocument = Convert.ToInt32(ddlJobDocument.SelectedValue);
                    jobDocument = rpoContext.JobDocuments.Include("JobDocumentFields.DocumentField.Field").FirstOrDefault(x => x.Id == idJobDocument);

                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {

                            string fileName = openFileDialog.FileName;
                            string thisFileName = Path.GetFileName(fileName);
                            string thisFileExtention = thisFileName.Replace(Path.GetFileNameWithoutExtension(fileName), string.Empty);

                            if (thisFileExtention.ToLower() == ".pdf")
                            {
                                ShowProgress();
                                JobDocumentAttachment jobDocumentAttachment = rpoContext.JobDocumentAttachments.FirstOrDefault(x => x.IdJobDocument == idJobDocument);
                                if (jobDocumentAttachment == null)
                                {
                                    jobDocumentAttachment = new JobDocumentAttachment();
                                    jobDocumentAttachment.DocumentName = thisFileName;
                                    jobDocumentAttachment.Path = thisFileName;
                                    jobDocumentAttachment.IdJobDocument = idJobDocument;
                                    rpoContext.JobDocumentAttachments.Add(jobDocumentAttachment);

                                    jobDocument.DocumentPath = thisFileName;
                                    rpoContext.SaveChanges();

                                    int document_Attachment = DocumentPlaceHolderField.Document_Attachment.GetHashCode();
                                    JobDocumentField jobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.IdField == document_Attachment);
                                    if (jobDocumentField != null)
                                    {
                                        jobDocumentField.Value = thisFileName;
                                        jobDocumentField.ActualValue = thisFileName;
                                    }

                                    rpoContext.SaveChanges();
                                }
                                else
                                {
                                    jobDocumentAttachment.DocumentName = thisFileName;
                                    jobDocumentAttachment.Path = thisFileName;
                                    jobDocument.DocumentPath = thisFileName;

                                    int document_Attachment = DocumentPlaceHolderField.Document_Attachment.GetHashCode();
                                    JobDocumentField jobDocumentField = rpoContext.JobDocumentFields.FirstOrDefault(x => x.IdJobDocument == jobDocument.Id && x.DocumentField.IdField == document_Attachment);
                                    if (jobDocumentField != null)
                                    {
                                        jobDocumentField.Value = thisFileName;
                                        jobDocumentField.ActualValue = thisFileName;
                                    }

                                    rpoContext.SaveChanges();
                                }

                                var instance = new DropboxIntegration();
                                string uploadFilePath = Properties.Settings.Default.DropboxFolderPath + Convert.ToString(job.Id);
                                string dropBoxFileName = Convert.ToString(jobDocument.Id) + "_" + jobDocument.DocumentPath;
                                string filepath = fileName;
                                var task = instance.RunUpload(uploadFilePath, dropBoxFileName, filepath);
                                CloseProgress();
                                MessageBox.Show("Document attached successfully");
                            }
                            else
                            {
                                MessageBox.Show("Please select the pdf file.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseProgress();
            }
        }

        private void RpoEfiling_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region General Methods

        public void Login(string username, string password)
        {
            if (wbEfiling.Url == new Uri("https://a810-efiling.nyc.gov/eRenewal/loginER.jsp"))
            {
                username = "janvi.dodia@gmail.com";
                password = "qwerty";
                wbEfiling.Document.GetElementById("inuserid").InnerText = username;
                wbEfiling.Document.GetElementById("inpassword").InnerText = password;
                wbEfiling.Document.GetElementById("submit").InvokeMember("Click");
            }
        }

        public void ClearControls()
        {
            txtJobNumber.Text = string.Empty;
            lblBorough.Text = string.Empty;
            lblHouseNumber.Text = string.Empty;
            lblDocumentDetails.Text = string.Empty;
            lblDocument.Text = string.Empty;
            lblApplicationType.Text = string.Empty;
            lblStreetName.Text = string.Empty;
            lblBlock.Text = string.Empty;
            lblLot.Text = string.Empty;
            tlpJobDetails.Visible = false;
            tblJobDocumentDetail.Visible = false;
            ddlJobDocument.DataSource = null;
            ddlJobDocument.Enabled = false;
            txtJobNumber.Focus();
        }

        private void WaitForPageLoad()
        {
            while (wbEfiling.IsBusy || wbEfiling.ReadyState != WebBrowserReadyState.Complete)
            { System.Windows.Forms.Application.DoEvents(); }
        }

        ModalLoadingUI objfrmShowProgress;

        private void StartProgress()
        {
            lblLoader.Text = "Please wait....";
            lblLoader.ForeColor = Color.Red;
            //Cursor = Cursors.WaitCursor;
            //objfrmShowProgress = new ModalLoadingUI();
            //ShowProgress();
        }

        private void CloseProgress()
        {
            lblLoader.Text = string.Empty;
            lblLoader.ForeColor = Color.Red;
            //Thread.Sleep(200);
            //try
            //{
            //    if (objfrmShowProgress != null)
            //    {
            //        objfrmShowProgress.Invoke(new Action(objfrmShowProgress.Close));
            //    }
            //    Cursor = Cursors.Default;
            //    //txtJobNumber.Focus();
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private void ShowProgress()
        {
            lblLoader.Text = "Please wait....";
            lblLoader.ForeColor = Color.Red;
            //try
            //{
            //    if (this.InvokeRequired)
            //    {
            //        try
            //        {
            //            objfrmShowProgress.ShowDialog();
            //        }
            //        catch (Exception ex) { }
            //    }
            //    else
            //    {
            //        Thread th = new Thread(ShowProgress);
            //        th.IsBackground = false;
            //        th.Start();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void BindDropdown(int idJob)
        {
            RpoContext rpoContext = new RpoContext();
            List<JobDocumentItem> jobDocumentList = rpoContext.JobDocuments.Include("DocumentMaster").Include("JobApplication.JobApplicationType").Where(x => x.IdJob == idJob && (x.IdDocument == 4 || x.IdDocument == 144 || x.IdDocument == 172))
                .Select(p => new JobDocumentItem()
                {
                    Id = p.Id,
                    DocumentName = p.DocumentMaster.Code.ToString()
                + (p.JobApplication != null && p.JobApplication.JobApplicationType != null ? " - " + p.JobApplication.JobApplicationType.Description : string.Empty)
                + (p.JobApplication != null && p.JobApplication.ApplicationNumber != null ? " - " + p.JobApplication.ApplicationNumber : string.Empty)
                + (" - " + p.DocumentDescription)
                }).ToList();
            //&& (x.DocumentMaster.Code == "EF-1" || x.DocumentMaster.Code == "PW-513" || x.DocumentMaster.Code == "VARPMT")

            if (jobDocumentList != null && jobDocumentList.Count > 0)
            {
                List<JobDocumentItem> jobDocumentListBind = new List<JobDocumentItem>();
                jobDocumentListBind.Add(new JobDocumentItem() { Id = 0, DocumentName = "-- Select Document --" });
                jobDocumentListBind.AddRange(jobDocumentList);
                ddlJobDocument.DataSource = jobDocumentListBind;
                ddlJobDocument.ValueMember = "Id";
                ddlJobDocument.DisplayMember = "DocumentName";
                ddlJobDocument.Enabled = true;
                lblDocumentDetails.Text = string.Empty;
                lblDocument.Text = string.Empty;
                lblApplicationType.Text = string.Empty;
                btnAttachEF1.Enabled = false;
                btnFillInfo.Enabled = false;
            }
            else
            {
                ddlJobDocument.DataSource = null;
                ddlJobDocument.Enabled = false;
                lblDocumentDetails.Text = string.Empty;
                lblDocument.Text = string.Empty;
                lblApplicationType.Text = string.Empty;
                btnAttachEF1.Enabled = false;
                btnFillInfo.Enabled = false;
            }

            //ddlJobDocument.
        }

        #endregion

        #region AHV

        private void FillDataRenewalAHV()
        {
            try
            {
                ShowProgress();
                RpoContext rpoContext = new RpoContext();
                int idJobDocument = Convert.ToInt32(ddlJobDocument.SelectedValue);
                if (idJobDocument > 0)
                {
                    jobDocument = rpoContext.JobDocuments.Include("JobApplication.JobApplicationType").Include("JobDocumentFields.DocumentField.Field").FirstOrDefault(x => x.Id == idJobDocument);
                    jobDocumentFieldList = jobDocument.JobDocumentFields;
                    if (jobDocument != null)
                    {
                        job = rpoContext.Jobs.Include("RfpAddress.Borough")
                        .Include("RfpAddress.Company")
                        .Include("RfpAddress.OwnerType")
                        .Include("RfpAddress.OwnerContact")
                        .Include("RfpAddress.SecondOfficerCompany")
                        .Include("RfpAddress.SecondOfficer")
                        .Include("RfpAddress.OccupancyClassification")
                        .Include("RfpAddress.ConstructionClassification")
                        .Include("RfpAddress.MultipleDwellingClassification")
                        .Include("RfpAddress.PrimaryStructuralSystem")
                        .Include("RfpAddress.StructureOccupancyCategory")
                        .Include("RfpAddress.SeismicDesignCategory")
                        .FirstOrDefault(x => x.Id == jobDocument.IdJob);
                    }

                    if (job != null && jobDocument != null && jobDocumentFieldList != null)
                    {
                        IseFilingCompleted = false;

                        jobDocumentField_Applicantion = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Application");

                        if (jobDocumentField_Applicantion != null)
                        {
                            int idJobApplication = jobDocumentField_Applicantion != null ? Convert.ToInt32(jobDocumentField_Applicantion.Value) : 0;
                            jobApplication = rpoContext.JobApplications.Include("JobApplicationType").FirstOrDefault(x => x.Id == idJobApplication);
                        }

                        //HtmlElement VaFloorText = wbEfiling.Document.GetElementById("VaFloorText");
                        //if (VaFloorText != null)
                        //{
                        //    VaFloorText.InnerText = jobApplication != null ? jobApplication.FloorWorking : string.Empty;
                        //}

                        JobDocumentField jobDocumentField_txtStartDate1 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Start Date");
                        string[] txtStartDate1 = !string.IsNullOrEmpty(jobDocumentField_txtStartDate1.Value) ? jobDocumentField_txtStartDate1.Value.Split(',') : null;
                        if (txtStartDate1 != null && txtStartDate1.Length > 0)
                        {
                            for (int j = 0; j < txtStartDate1.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(txtStartDate1[j]))
                                {
                                    string actualDate = DateTime.ParseExact(txtStartDate1[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                                    string[] dateValue = txtStartDate1[j] != null ? txtStartDate1[j].Split('/') : null;

                                    HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                    if (elements != null && elements.Count > 0)
                                    {
                                        for (int i = 0; i < elements.Count; i++)
                                        {
                                            HtmlElement el = elements[i];
                                            string elvalue = el.GetAttribute("value");
                                            string elname = el.GetAttribute("name");
                                            string elType = el.GetAttribute("type");

                                            if (elType == "radio")
                                            {
                                                actualDate = !string.IsNullOrEmpty(actualDate) ? actualDate.Replace("-", "/").Replace("-", "/").Replace("-", "/") : actualDate;

                                                if (elvalue == actualDate)
                                                {
                                                    if (el != null)
                                                    {
                                                        el.SetAttribute("checked", "checked");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //JobDocumentField jobDocumentField_Applicant = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtMain_Last");
                        //if (jobDocumentField_Applicant != null)
                        //{
                        JobDocumentField jobDocumentField_txtDate1 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate1");
                        JobDocumentField jobDocumentField_txtDate2 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate2");
                        JobDocumentField jobDocumentField_txtDate3 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate3");
                        JobDocumentField jobDocumentField_txtDate4 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate4");
                        JobDocumentField jobDocumentField_txtDate5 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate5");
                        JobDocumentField jobDocumentField_txtDate6 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate6");
                        JobDocumentField jobDocumentField_txtDate7 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate7");

                        string[] txtDate1 = !string.IsNullOrEmpty(jobDocumentField_txtDate1.Value) ? jobDocumentField_txtDate1.Value.Split(',') : null;
                        string[] txtDate2 = !string.IsNullOrEmpty(jobDocumentField_txtDate2.Value) ? jobDocumentField_txtDate2.Value.Split(',') : null;
                        string[] txtDate3 = !string.IsNullOrEmpty(jobDocumentField_txtDate3.Value) ? jobDocumentField_txtDate3.Value.Split(',') : null;
                        string[] txtDate4 = !string.IsNullOrEmpty(jobDocumentField_txtDate4.Value) ? jobDocumentField_txtDate4.Value.Split(',') : null;
                        string[] txtDate5 = !string.IsNullOrEmpty(jobDocumentField_txtDate5.Value) ? jobDocumentField_txtDate5.Value.Split(',') : null;
                        string[] txtDate6 = !string.IsNullOrEmpty(jobDocumentField_txtDate6.Value) ? jobDocumentField_txtDate6.Value.Split(',') : null;
                        string[] txtDate7 = !string.IsNullOrEmpty(jobDocumentField_txtDate7.Value) ? jobDocumentField_txtDate7.Value.Split(',') : null;

                        #region Sunday

                        if (txtDate1 != null && txtDate1.Length > 0)
                        {
                            for (int j = 0; j < txtDate1.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(txtDate1[j]))
                                {
                                    string actualDate = DateTime.ParseExact(txtDate1[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    string[] dateValue = txtDate1[j] != null ? txtDate1[j].Split('/') : null;

                                    HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                    if (elements != null && elements.Count > 0)
                                    {
                                        for (int i = 0; i < elements.Count; i++)
                                        {
                                            HtmlElement el = elements[i];
                                            string elvalue = el.GetAttribute("value");
                                            string elname = el.GetAttribute("name");
                                            string elType = el.GetAttribute("type");

                                            //if (elType == "hidden")
                                            //{
                                            if (elvalue == actualDate)
                                            {
                                                string elnameIndex = elname.Replace("AhvDate", "");
                                                HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                if (AhvDateTimeCb != null)
                                                {
                                                    AhvDateTimeCb.SetAttribute("checked", "checked");
                                                    AhvDateTimeCb.Enabled = true;
                                                }
                                            }
                                            // }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Monday

                        if (txtDate2 != null && txtDate2.Length > 0)
                        {
                            for (int j = 0; j < txtDate2.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(txtDate2[j]))
                                {
                                    string actualDate = DateTime.ParseExact(txtDate2[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    string[] dateValue = txtDate2[j] != null ? txtDate2[j].Split('/') : null;

                                    HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                    if (elements != null && elements.Count > 0)
                                    {
                                        for (int i = 0; i < elements.Count; i++)
                                        {
                                            HtmlElement el = elements[i];
                                            string elvalue = el.GetAttribute("value");
                                            string elname = el.GetAttribute("name");
                                            string elType = el.GetAttribute("type");

                                            //if (elType == "hidden")
                                            //{
                                            if (elvalue == actualDate)
                                            {
                                                string elnameIndex = elname.Replace("AhvDate", "");
                                                HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                if (AhvDateTimeCb != null)
                                                {
                                                    AhvDateTimeCb.SetAttribute("checked", "checked");
                                                    AhvDateTimeCb.Enabled = true;
                                                }
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Tuesday

                        if (txtDate3 != null && txtDate3.Length > 0)
                        {
                            for (int j = 0; j < txtDate3.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(txtDate3[j]))
                                {
                                    string actualDate = DateTime.ParseExact(txtDate3[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    string[] dateValue = txtDate3[j] != null ? txtDate3[j].Split('/') : null;

                                    HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                    if (elements != null && elements.Count > 0)
                                    {
                                        for (int i = 0; i < elements.Count; i++)
                                        {
                                            HtmlElement el = elements[i];
                                            string elvalue = el.GetAttribute("value");
                                            string elname = el.GetAttribute("name");
                                            string elType = el.GetAttribute("type");

                                            //if (elType == "hidden")
                                            //{
                                            if (elvalue == actualDate)
                                            {
                                                string elnameIndex = elname.Replace("AhvDate", "");
                                                HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                if (AhvDateTimeCb != null)
                                                {
                                                    AhvDateTimeCb.SetAttribute("checked", "checked");
                                                    AhvDateTimeCb.Enabled = true;
                                                }
                                            }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Wednesday

                        if (txtDate4 != null && txtDate4.Length > 0)
                        {
                            for (int j = 0; j < txtDate4.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(txtDate4[j]))
                                {
                                    string actualDate = DateTime.ParseExact(txtDate4[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    string[] dateValue = txtDate4[j] != null ? txtDate4[j].Split('/') : null;

                                    HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                    if (elements != null && elements.Count > 0)
                                    {
                                        for (int i = 0; i < elements.Count; i++)
                                        {
                                            HtmlElement el = elements[i];
                                            string elvalue = el.GetAttribute("value");
                                            string elname = el.GetAttribute("name");
                                            string elType = el.GetAttribute("type");

                                            //if (elType == "hidden")
                                            //{
                                            if (elvalue == actualDate)
                                            {
                                                string elnameIndex = elname.Replace("AhvDate", "");
                                                HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                if (AhvDateTimeCb != null)
                                                {
                                                    AhvDateTimeCb.SetAttribute("checked", "checked");
                                                    AhvDateTimeCb.Enabled = true;
                                                }
                                            }
                                            // }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Thursday

                        if (txtDate5 != null && txtDate5.Length > 0)
                        {
                            for (int j = 0; j < txtDate5.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(txtDate5[j]))
                                {
                                    string actualDate = DateTime.ParseExact(txtDate5[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    string[] dateValue = txtDate5[j] != null ? txtDate5[j].Split('/') : null;

                                    HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                    if (elements != null && elements.Count > 0)
                                    {
                                        for (int i = 0; i < elements.Count; i++)
                                        {
                                            HtmlElement el = elements[i];
                                            string elvalue = el.GetAttribute("value");
                                            string elname = el.GetAttribute("name");
                                            string elType = el.GetAttribute("type");

                                            //if (elType == "hidden")
                                            //{
                                            if (elvalue == actualDate)
                                            {
                                                string elnameIndex = elname.Replace("AhvDate", "");
                                                HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                if (AhvDateTimeCb != null)
                                                {
                                                    AhvDateTimeCb.SetAttribute("checked", "checked");
                                                    AhvDateTimeCb.Enabled = true;
                                                }
                                            }
                                            // }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Friday

                        if (txtDate6 != null && txtDate6.Length > 0)
                        {
                            for (int j = 0; j < txtDate6.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(txtDate6[j]))
                                {
                                    string actualDate = DateTime.ParseExact(txtDate6[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    string[] dateValue = txtDate6[j] != null ? txtDate6[j].Split('/') : null;

                                    HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                    if (elements != null && elements.Count > 0)
                                    {
                                        for (int i = 0; i < elements.Count; i++)
                                        {
                                            HtmlElement el = elements[i];
                                            string elvalue = el.GetAttribute("value");
                                            string elname = el.GetAttribute("name");
                                            string elType = el.GetAttribute("type");

                                            //if (elType == "hidden")
                                            //{
                                            if (elvalue == actualDate)
                                            {
                                                string elnameIndex = elname.Replace("AhvDate", "");
                                                HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                if (AhvDateTimeCb != null)
                                                {
                                                    AhvDateTimeCb.SetAttribute("checked", "checked");
                                                    AhvDateTimeCb.Enabled = true;
                                                }
                                            }
                                            // }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Saturday

                        if (txtDate7 != null && txtDate7.Length > 0)
                        {
                            for (int j = 0; j < txtDate7.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(txtDate7[j]))
                                {
                                    string actualDate = DateTime.ParseExact(txtDate7[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    string[] dateValue = txtDate7[j] != null ? txtDate7[j].Split('/') : null;

                                    HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                    if (elements != null && elements.Count > 0)
                                    {
                                        for (int i = 0; i < elements.Count; i++)
                                        {
                                            HtmlElement el = elements[i];
                                            string elvalue = el.GetAttribute("value");
                                            string elname = el.GetAttribute("name");
                                            string elType = el.GetAttribute("type");

                                            //if (elType == "hidden")
                                            //{
                                            if (elvalue == actualDate)
                                            {
                                                string elnameIndex = elname.Replace("AhvDate", "");
                                                HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                if (AhvDateTimeCb != null)
                                                {
                                                    AhvDateTimeCb.SetAttribute("checked", "checked");
                                                    AhvDateTimeCb.Enabled = true;
                                                }
                                            }
                                            // }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        MessageBox.Show("E-filing of the data is complete!");
                        CloseProgress();

                    }
                    else
                    {
                        CloseProgress();
                        MessageBox.Show("Please enter the jobnumber and search to Fill the info");
                    }
                }
                else
                {
                    CloseProgress();
                    MessageBox.Show("Please select the job document");
                }
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
        }

        private void FillDataInitialAHV()
        {
            try
            {
                ShowProgress();
                RpoContext rpoContext = new RpoContext();
                int idJobDocument = Convert.ToInt32(ddlJobDocument.SelectedValue);
                if (idJobDocument > 0)
                {
                    jobDocument = rpoContext.JobDocuments.Include("JobApplication.JobApplicationType").Include("JobDocumentFields.DocumentField.Field").FirstOrDefault(x => x.Id == idJobDocument);
                    jobDocumentFieldList = jobDocument.JobDocumentFields;
                    if (jobDocument != null)
                    {
                        job = rpoContext.Jobs.Include("RfpAddress.Borough")
                        .Include("RfpAddress.Company")
                        .Include("RfpAddress.OwnerType")
                        .Include("RfpAddress.OwnerContact")
                        .Include("RfpAddress.SecondOfficerCompany")
                        .Include("RfpAddress.SecondOfficer")
                        .Include("RfpAddress.OccupancyClassification")
                        .Include("RfpAddress.ConstructionClassification")
                        .Include("RfpAddress.MultipleDwellingClassification")
                        .Include("RfpAddress.PrimaryStructuralSystem")
                        .Include("RfpAddress.StructureOccupancyCategory")
                        .Include("RfpAddress.SeismicDesignCategory")
                        .FirstOrDefault(x => x.Id == jobDocument.IdJob);
                    }

                    if (job != null && jobDocument != null && jobDocumentFieldList != null)
                    {
                        IseFilingCompleted = false;

                        jobDocumentField_Applicantion = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Application");

                        if (jobDocumentField_Applicantion != null)
                        {
                            int idJobApplication = jobDocumentField_Applicantion != null ? Convert.ToInt32(jobDocumentField_Applicantion.Value) : 0;
                            jobApplication = rpoContext.JobApplications.Include("JobApplicationType").FirstOrDefault(x => x.Id == idJobApplication);
                        }

                        HtmlElement VaFloorText = wbEfiling.Document.GetElementById("VaFloorText");
                        if (VaFloorText != null)
                        {
                            VaFloorText.InnerText = jobApplication != null ? jobApplication.FloorWorking : string.Empty;
                        }

                        JobDocumentField jobDocumentField_Applicant = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtMain_Last");
                        if (jobDocumentField_Applicant != null)
                        {
                            int idJobContact = Convert.ToInt32(jobDocumentField_Applicant.Value);
                            JobContact jobContact = rpoContext.JobContacts.FirstOrDefault(x => x.Id == idJobContact);
                            if (jobContact != null)
                            {
                                HtmlElement PocFirstName = wbEfiling.Document.GetElementById("PocFirstName");
                                if (PocFirstName != null)
                                {
                                    PocFirstName.InnerText = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                                }

                                HtmlElement PocLastName = wbEfiling.Document.GetElementById("PocLastName");
                                if (PocLastName != null)
                                {
                                    PocLastName.InnerText = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;
                                }

                                string mobilePhone = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                                string mobilePhone1 = string.Empty;
                                string mobilePhone2 = string.Empty;
                                string mobilePhone3 = string.Empty;

                                if (!string.IsNullOrEmpty(mobilePhone))
                                {
                                    mobilePhone = mobilePhone.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                                }

                                if (!string.IsNullOrEmpty(mobilePhone) && mobilePhone.Length == 10)
                                {
                                    mobilePhone1 = mobilePhone.Substring(0, 3);
                                    mobilePhone2 = mobilePhone.Substring(3, 3);
                                    mobilePhone3 = mobilePhone.Substring(6, 4);
                                }

                                HtmlElement PocMobilePhone1 = wbEfiling.Document.GetElementById("PocMobilePhone1");
                                if (PocMobilePhone1 != null)
                                {
                                    PocMobilePhone1.InnerText = mobilePhone1;
                                }

                                HtmlElement PocMobilePhone2 = wbEfiling.Document.GetElementById("PocMobilePhone2");
                                if (PocMobilePhone2 != null)
                                {
                                    PocMobilePhone2.InnerText = mobilePhone2;
                                }

                                HtmlElement PocMobilePhone3 = wbEfiling.Document.GetElementById("PocMobilePhone3");
                                if (PocMobilePhone3 != null)
                                {
                                    PocMobilePhone3.InnerText = mobilePhone3;
                                }

                                HtmlElement PocEmail = wbEfiling.Document.GetElementById("PocEmail");
                                if (PocEmail != null)
                                {
                                    PocEmail.InnerText = jobContact != null && jobContact.Contact != null ? jobContact.Contact.Email : string.Empty;
                                }
                            }

                            JobDocumentField jobDocumentField_VarianceReason = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtReasonForVariance");
                            string vaApplyReason_value = jobDocumentField_VarianceReason.Value;
                            HtmlElement VaApplyReason = wbEfiling.Document.GetElementById("VaApplyReason");
                            if (VaApplyReason != null)
                            {
                                VaApplyReason.SetAttribute("value", vaApplyReason_value);
                            }

                            JobDocumentField jobDocumentField_opg200 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "opg200");
                            bool jobDocumentField_opg200_value = Convert.ToBoolean(jobDocumentField_opg200.Value);

                            HtmlElement VaNearResidence_0 = wbEfiling.Document.GetElementById("VaNearResidence[0]");
                            if (VaNearResidence_0 != null)
                            {
                                if (jobDocumentField_opg200_value)
                                {
                                    VaNearResidence_0.SetAttribute("checked", "checked");
                                }
                                else
                                {
                                    VaNearResidence_0.SetAttribute("checked", "");
                                }

                            }

                            HtmlElement VaNearResidence_1 = wbEfiling.Document.GetElementById("VaNearResidence[1]");
                            if (VaNearResidence_1 != null)
                            {
                                if (jobDocumentField_opg200_value)
                                {
                                    VaNearResidence_1.SetAttribute("checked", "");
                                }
                                else
                                {
                                    VaNearResidence_1.SetAttribute("checked", "checked");
                                }
                            }

                            JobDocumentField jobDocumentField_opgEnclosed = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "opgEnclosed");
                            bool jobDocumentField_opgEnclosed_value = Convert.ToBoolean(jobDocumentField_opgEnclosed.Value);

                            HtmlElement VaEnclosedBldg_0 = wbEfiling.Document.GetElementById("VaEnclosedBldg[0]");
                            if (VaEnclosedBldg_0 != null)
                            {
                                if (jobDocumentField_opgEnclosed_value)
                                {
                                    VaEnclosedBldg_0.SetAttribute("checked", "checked");
                                }
                                else
                                {
                                    VaEnclosedBldg_0.SetAttribute("checked", "");
                                }

                            }

                            HtmlElement VaEnclosedBldg_1 = wbEfiling.Document.GetElementById("VaEnclosedBldg[1]");
                            if (VaEnclosedBldg_1 != null)
                            {
                                if (jobDocumentField_opgEnclosed_value)
                                {
                                    VaEnclosedBldg_1.SetAttribute("checked", "");
                                }
                                else
                                {
                                    VaEnclosedBldg_1.SetAttribute("checked", "checked");
                                }
                            }

                            JobDocumentField jobDocumentField_opgDemo = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "opgDemo");
                            bool jobDocumentField_opgDemo_value = Convert.ToBoolean(jobDocumentField_opgDemo.Value);

                            HtmlElement VaDemoWork_0 = wbEfiling.Document.GetElementById("VaDemoWork[0]");
                            if (VaDemoWork_0 != null)
                            {
                                if (jobDocumentField_opgDemo_value)
                                {
                                    VaDemoWork_0.SetAttribute("checked", "checked");
                                }
                                else
                                {
                                    VaDemoWork_0.SetAttribute("checked", "");
                                }

                            }

                            HtmlElement VaDemoWork_1 = wbEfiling.Document.GetElementById("VaDemoWork[1]");
                            if (VaDemoWork_1 != null)
                            {
                                if (jobDocumentField_opgDemo_value)
                                {
                                    VaDemoWork_1.SetAttribute("checked", "");
                                }
                                else
                                {
                                    VaDemoWork_1.SetAttribute("checked", "checked");
                                }
                            }

                            JobDocumentField jobDocumentField_opgCrane = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "opgCrane");
                            bool jobDocumentField_opgCrane_value = Convert.ToBoolean(jobDocumentField_opgCrane.Value);

                            HtmlElement VaCraneUse_0 = wbEfiling.Document.GetElementById("VaCraneUse[0]");
                            if (VaCraneUse_0 != null)
                            {
                                if (jobDocumentField_opgCrane_value)
                                {
                                    VaCraneUse_0.SetAttribute("checked", "checked");
                                }
                                else
                                {
                                    VaCraneUse_0.SetAttribute("checked", "");
                                }
                            }

                            HtmlElement VaCraneUse_1 = wbEfiling.Document.GetElementById("VaCraneUse[1]");
                            if (VaCraneUse_1 != null)
                            {
                                if (jobDocumentField_opgCrane_value)
                                {
                                    VaCraneUse_1.SetAttribute("checked", "");
                                }
                                else
                                {
                                    VaCraneUse_1.SetAttribute("checked", "checked");
                                }
                            }

                            bool isSameAsWeekday = false;

                             JobDocumentField jobDocumentField_SameAdWeekDay = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Same as Weekday");

                            if (jobDocumentField_SameAdWeekDay != null && jobDocumentField_SameAdWeekDay.ToString() != "" && jobDocumentField_SameAdWeekDay.ToString() != "null" && jobDocumentField_SameAdWeekDay.Value != null && jobDocumentField_SameAdWeekDay.Value.ToString().Trim().ToLower() == "true")
                            {
                                isSameAsWeekday = true;
                            }

                            JobDocumentField jobDocumentField_WeekDayWorkDescription = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtWork");

                            JobDocumentField jobDocumentField_WeekendWorkDescription = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Weekend Work Description");

                            string jobDocumentField_WeekendWorkDescription_value = string.Empty;

                            if (isSameAsWeekday == true)
                            {
                                jobDocumentField_WeekendWorkDescription_value = "Weekday & Weekend :" + jobDocumentField_WeekDayWorkDescription.Value;
                            }
                            else if (jobDocumentField_WeekDayWorkDescription != null && !string.IsNullOrEmpty(jobDocumentField_WeekDayWorkDescription.Value.ToString().Trim()) && jobDocumentField_WeekendWorkDescription != null && jobDocumentField_WeekendWorkDescription.Value != null && !string.IsNullOrEmpty(jobDocumentField_WeekendWorkDescription.Value.ToString().Trim()))
                            {
                                jobDocumentField_WeekendWorkDescription_value = "Weekday :" + jobDocumentField_WeekDayWorkDescription.Value + " Weekend :" + jobDocumentField_WeekDayWorkDescription.Value;
                            }
                            else
                            {
                                jobDocumentField_WeekendWorkDescription_value = jobDocumentField_WeekendWorkDescription.ActualValue;
                            }

                            HtmlElement AHVDescOfText = wbEfiling.Document.GetElementById("AHVDescOfText");
                            if (AHVDescOfText != null)
                            {
                                AHVDescOfText.InnerText = jobDocumentField_WeekendWorkDescription_value;
                            }

                            JobDocumentField jobDocumentField_AhvStartTimeHr7 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart1");
                            string jobDocumentField_AhvStartTimeHr7_value = jobDocumentField_AhvStartTimeHr7.Value;

                            HtmlElement AhvStartTimeHr7 = wbEfiling.Document.GetElementById("AhvStartTimeHr7");
                            if (AhvStartTimeHr7 != null)
                            {
                                AhvStartTimeHr7.InnerText = jobDocumentField_AhvStartTimeHr7_value;
                            }

                            JobDocumentField jobDocumentField_AhvEndTimeHr7 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd1");
                            string jobDocumentField_AhvEndTimeHr7_value = jobDocumentField_AhvEndTimeHr7.Value;

                            HtmlElement AhvEndTimeHr7 = wbEfiling.Document.GetElementById("AhvEndTimeHr7");
                            if (AhvEndTimeHr7 != null)
                            {
                                AhvEndTimeHr7.InnerText = jobDocumentField_AhvEndTimeHr7_value;
                            }

                            JobDocumentField jobDocumentField_AhvStartTimeHr1 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart2");
                            string jobDocumentField_AhvStartTimeHr1_value = jobDocumentField_AhvStartTimeHr1.Value;

                            HtmlElement AhvStartTimeHr1 = wbEfiling.Document.GetElementById("AhvStartTimeHr1");
                            if (AhvStartTimeHr1 != null)
                            {
                                AhvStartTimeHr1.InnerText = jobDocumentField_AhvStartTimeHr1_value;
                            }

                            JobDocumentField jobDocumentField_AhvEndTimeHr1 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd2");
                            string jobDocumentField_AhvEndTimeHr1_value = jobDocumentField_AhvEndTimeHr1.Value;

                            HtmlElement AhvEndTimeHr1 = wbEfiling.Document.GetElementById("AhvEndTimeHr1");
                            if (AhvEndTimeHr1 != null)
                            {
                                AhvEndTimeHr1.InnerText = jobDocumentField_AhvEndTimeHr1_value;
                            }


                            JobDocumentField jobDocumentField_AhvStartTimeHr2 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart3");
                            string jobDocumentField_AhvStartTimeHr2_value = jobDocumentField_AhvStartTimeHr2.Value;

                            HtmlElement AhvStartTimeHr2 = wbEfiling.Document.GetElementById("AhvStartTimeHr2");
                            if (AhvStartTimeHr2 != null)
                            {
                                AhvStartTimeHr2.InnerText = jobDocumentField_AhvStartTimeHr2_value;
                            }

                            JobDocumentField jobDocumentField_AhvEndTimeHr2 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd3");
                            string jobDocumentField_AhvEndTimeHr2_value = jobDocumentField_AhvEndTimeHr2.Value;

                            HtmlElement AhvEndTimeHr2 = wbEfiling.Document.GetElementById("AhvEndTimeHr2");
                            if (AhvEndTimeHr2 != null)
                            {
                                AhvEndTimeHr2.InnerText = jobDocumentField_AhvEndTimeHr2_value;
                            }

                            //AhvStartTimeHr1
                            //AhvEndTimeHr1

                            JobDocumentField jobDocumentField_AhvStartTimeHr3 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart4");
                            string jobDocumentField_AhvStartTimeHr3_value = jobDocumentField_AhvStartTimeHr3.Value;

                            HtmlElement AhvStartTimeHr3 = wbEfiling.Document.GetElementById("AhvStartTimeHr3");
                            if (AhvStartTimeHr3 != null)
                            {
                                AhvStartTimeHr3.InnerText = jobDocumentField_AhvStartTimeHr3_value;
                            }

                            JobDocumentField jobDocumentField_AhvEndTimeHr3 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd4");
                            string jobDocumentField_AhvEndTimeHr3_value = jobDocumentField_AhvEndTimeHr3.Value;

                            HtmlElement AhvEndTimeHr3 = wbEfiling.Document.GetElementById("AhvEndTimeHr3");
                            if (AhvEndTimeHr3 != null)
                            {
                                AhvEndTimeHr3.InnerText = jobDocumentField_AhvEndTimeHr3_value;
                            }

                            JobDocumentField jobDocumentField_AhvStartTimeHr4 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart5");
                            string jobDocumentField_AhvStartTimeHr4_value = jobDocumentField_AhvStartTimeHr4.Value;

                            HtmlElement AhvStartTimeHr4 = wbEfiling.Document.GetElementById("AhvStartTimeHr4");
                            if (AhvStartTimeHr4 != null)
                            {
                                AhvStartTimeHr4.InnerText = jobDocumentField_AhvStartTimeHr4_value;
                            }

                            JobDocumentField jobDocumentField_AhvEndTimeHr4 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd5");
                            string jobDocumentField_AhvEndTimeHr4_value = jobDocumentField_AhvEndTimeHr4.Value;

                            HtmlElement AhvEndTimeHr4 = wbEfiling.Document.GetElementById("AhvEndTimeHr4");
                            if (AhvEndTimeHr4 != null)
                            {
                                AhvEndTimeHr4.InnerText = jobDocumentField_AhvEndTimeHr4_value;
                            }

                            JobDocumentField jobDocumentField_AhvStartTimeHr5 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart6");
                            string jobDocumentField_AhvStartTimeHr5_value = jobDocumentField_AhvStartTimeHr5.Value;

                            HtmlElement AhvStartTimeHr5 = wbEfiling.Document.GetElementById("AhvStartTimeHr5");
                            if (AhvStartTimeHr5 != null)
                            {
                                AhvStartTimeHr5.InnerText = jobDocumentField_AhvStartTimeHr5_value;
                            }

                            JobDocumentField jobDocumentField_AhvEndTimeHr5 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd6");
                            string jobDocumentField_AhvEndTimeHr5_value = jobDocumentField_AhvEndTimeHr5.Value;

                            HtmlElement AhvEndTimeHr5 = wbEfiling.Document.GetElementById("AhvEndTimeHr5");
                            if (AhvEndTimeHr5 != null)
                            {
                                AhvEndTimeHr5.InnerText = jobDocumentField_AhvEndTimeHr5_value;
                            }

                            JobDocumentField jobDocumentField_AhvStartTimeHr6 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtStart7");
                            string jobDocumentField_AhvStartTimeHr6_value = jobDocumentField_AhvStartTimeHr6.Value;

                            HtmlElement AhvStartTimeHr6 = wbEfiling.Document.GetElementById("AhvStartTimeHr6");
                            if (AhvStartTimeHr6 != null)
                            {
                                AhvStartTimeHr6.InnerText = jobDocumentField_AhvStartTimeHr6_value;
                            }

                            JobDocumentField jobDocumentField_AhvEndTimeHr6 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtEnd7");
                            string jobDocumentField_AhvEndTimeHr6_value = jobDocumentField_AhvEndTimeHr6.Value;

                            HtmlElement AhvEndTimeHr6 = wbEfiling.Document.GetElementById("AhvEndTimeHr6");
                            if (AhvEndTimeHr6 != null)
                            {
                                AhvEndTimeHr6.InnerText = jobDocumentField_AhvEndTimeHr6_value;
                            }

                            JobDocumentField jobDocumentField_txtDate1 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate1");
                            JobDocumentField jobDocumentField_txtDate2 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate2");
                            JobDocumentField jobDocumentField_txtDate3 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate3");
                            JobDocumentField jobDocumentField_txtDate4 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate4");
                            JobDocumentField jobDocumentField_txtDate5 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate5");
                            JobDocumentField jobDocumentField_txtDate6 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate6");
                            JobDocumentField jobDocumentField_txtDate7 = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "txtDate7");

                            string[] txtDate1 = !string.IsNullOrEmpty(jobDocumentField_txtDate1.Value) ? jobDocumentField_txtDate1.Value.Split(',') : null;
                            string[] txtDate2 = !string.IsNullOrEmpty(jobDocumentField_txtDate2.Value) ? jobDocumentField_txtDate2.Value.Split(',') : null;
                            string[] txtDate3 = !string.IsNullOrEmpty(jobDocumentField_txtDate3.Value) ? jobDocumentField_txtDate3.Value.Split(',') : null;
                            string[] txtDate4 = !string.IsNullOrEmpty(jobDocumentField_txtDate4.Value) ? jobDocumentField_txtDate4.Value.Split(',') : null;
                            string[] txtDate5 = !string.IsNullOrEmpty(jobDocumentField_txtDate5.Value) ? jobDocumentField_txtDate5.Value.Split(',') : null;
                            string[] txtDate6 = !string.IsNullOrEmpty(jobDocumentField_txtDate6.Value) ? jobDocumentField_txtDate6.Value.Split(',') : null;
                            string[] txtDate7 = !string.IsNullOrEmpty(jobDocumentField_txtDate7.Value) ? jobDocumentField_txtDate7.Value.Split(',') : null;

                            #region Sunday

                            if (txtDate1 != null && txtDate1.Length > 0)
                            {
                                for (int j = 0; j < txtDate1.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(txtDate1[j]))
                                    {
                                        string actualDate = DateTime.ParseExact(txtDate1[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        string[] dateValue = txtDate1[j] != null ? txtDate1[j].Split('/') : null;

                                        HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                        if (elements != null && elements.Count > 0)
                                        {
                                            for (int i = 0; i < elements.Count; i++)
                                            {
                                                HtmlElement el = elements[i];
                                                string elvalue = el.GetAttribute("value");
                                                string elname = el.GetAttribute("name");
                                                string elType = el.GetAttribute("type");

                                                //if (elType == "hidden")
                                                //{
                                                if (elvalue == actualDate)
                                                {
                                                    string elnameIndex = elname.Replace("AhvDate", "");
                                                    HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                    if (AhvDateTimeCb != null)
                                                    {
                                                        AhvDateTimeCb.SetAttribute("checked", "checked");
                                                        AhvDateTimeCb.Enabled = true;
                                                    }
                                                }
                                                // }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Monday

                            if (txtDate2 != null && txtDate2.Length > 0)
                            {
                                for (int j = 0; j < txtDate2.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(txtDate2[j]))
                                    {
                                        string actualDate = DateTime.ParseExact(txtDate2[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        string[] dateValue = txtDate2[j] != null ? txtDate2[j].Split('/') : null;

                                        HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                        if (elements != null && elements.Count > 0)
                                        {
                                            for (int i = 0; i < elements.Count; i++)
                                            {
                                                HtmlElement el = elements[i];
                                                string elvalue = el.GetAttribute("value");
                                                string elname = el.GetAttribute("name");
                                                string elType = el.GetAttribute("type");

                                                //if (elType == "hidden")
                                                //{
                                                if (elvalue == actualDate)
                                                {
                                                    string elnameIndex = elname.Replace("AhvDate", "");
                                                    HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                    if (AhvDateTimeCb != null)
                                                    {
                                                        AhvDateTimeCb.SetAttribute("checked", "checked");
                                                        AhvDateTimeCb.Enabled = true;
                                                    }
                                                }
                                                //}
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Tuesday

                            if (txtDate3 != null && txtDate3.Length > 0)
                            {
                                for (int j = 0; j < txtDate3.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(txtDate3[j]))
                                    {
                                        string actualDate = DateTime.ParseExact(txtDate3[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        string[] dateValue = txtDate3[j] != null ? txtDate3[j].Split('/') : null;

                                        HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                        if (elements != null && elements.Count > 0)
                                        {
                                            for (int i = 0; i < elements.Count; i++)
                                            {
                                                HtmlElement el = elements[i];
                                                string elvalue = el.GetAttribute("value");
                                                string elname = el.GetAttribute("name");
                                                string elType = el.GetAttribute("type");

                                                //if (elType == "hidden")
                                                //{
                                                if (elvalue == actualDate)
                                                {
                                                    string elnameIndex = elname.Replace("AhvDate", "");
                                                    HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                    if (AhvDateTimeCb != null)
                                                    {
                                                        AhvDateTimeCb.SetAttribute("checked", "checked");
                                                        AhvDateTimeCb.Enabled = true;
                                                    }
                                                }
                                                // }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Wednesday

                            if (txtDate4 != null && txtDate4.Length > 0)
                            {
                                for (int j = 0; j < txtDate4.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(txtDate4[j]))
                                    {
                                        string actualDate = DateTime.ParseExact(txtDate4[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        string[] dateValue = txtDate4[j] != null ? txtDate4[j].Split('/') : null;

                                        HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                        if (elements != null && elements.Count > 0)
                                        {
                                            for (int i = 0; i < elements.Count; i++)
                                            {
                                                HtmlElement el = elements[i];
                                                string elvalue = el.GetAttribute("value");
                                                string elname = el.GetAttribute("name");
                                                string elType = el.GetAttribute("type");

                                                //if (elType == "hidden")
                                                //{
                                                if (elvalue == actualDate)
                                                {
                                                    string elnameIndex = elname.Replace("AhvDate", "");
                                                    HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                    if (AhvDateTimeCb != null)
                                                    {
                                                        AhvDateTimeCb.SetAttribute("checked", "checked");
                                                        AhvDateTimeCb.Enabled = true;
                                                    }
                                                }
                                                // }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Thursday

                            if (txtDate5 != null && txtDate5.Length > 0)
                            {
                                for (int j = 0; j < txtDate5.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(txtDate5[j]))
                                    {
                                        string actualDate = DateTime.ParseExact(txtDate5[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        string[] dateValue = txtDate5[j] != null ? txtDate5[j].Split('/') : null;

                                        HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                        if (elements != null && elements.Count > 0)
                                        {
                                            for (int i = 0; i < elements.Count; i++)
                                            {
                                                HtmlElement el = elements[i];
                                                string elvalue = el.GetAttribute("value");
                                                string elname = el.GetAttribute("name");
                                                string elType = el.GetAttribute("type");

                                                //if (elType == "hidden")
                                                //{
                                                if (elvalue == actualDate)
                                                {
                                                    string elnameIndex = elname.Replace("AhvDate", "");
                                                    HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                    if (AhvDateTimeCb != null)
                                                    {
                                                        AhvDateTimeCb.SetAttribute("checked", "checked");
                                                        AhvDateTimeCb.Enabled = true;
                                                    }
                                                }
                                                // }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Friday

                            if (txtDate6 != null && txtDate6.Length > 0)
                            {
                                for (int j = 0; j < txtDate6.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(txtDate6[j]))
                                    {
                                        string actualDate = DateTime.ParseExact(txtDate6[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        string[] dateValue = txtDate6[j] != null ? txtDate6[j].Split('/') : null;

                                        HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                        if (elements != null && elements.Count > 0)
                                        {
                                            for (int i = 0; i < elements.Count; i++)
                                            {
                                                HtmlElement el = elements[i];
                                                string elvalue = el.GetAttribute("value");
                                                string elname = el.GetAttribute("name");
                                                string elType = el.GetAttribute("type");

                                                //if (elType == "hidden")
                                                //{
                                                if (elvalue == actualDate)
                                                {
                                                    string elnameIndex = elname.Replace("AhvDate", "");
                                                    HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                    if (AhvDateTimeCb != null)
                                                    {
                                                        AhvDateTimeCb.SetAttribute("checked", "checked");
                                                        AhvDateTimeCb.Enabled = true;
                                                    }
                                                }
                                                // }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Saturday

                            if (txtDate7 != null && txtDate7.Length > 0)
                            {
                                for (int j = 0; j < txtDate7.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(txtDate7[j]))
                                    {
                                        string actualDate = DateTime.ParseExact(txtDate7[j], "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        string[] dateValue = txtDate7[j] != null ? txtDate7[j].Split('/') : null;

                                        HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                                        if (elements != null && elements.Count > 0)
                                        {
                                            for (int i = 0; i < elements.Count; i++)
                                            {
                                                HtmlElement el = elements[i];
                                                string elvalue = el.GetAttribute("value");
                                                string elname = el.GetAttribute("name");
                                                string elType = el.GetAttribute("type");

                                                //if (elType == "hidden")
                                                //{
                                                if (elvalue == actualDate)
                                                {
                                                    string elnameIndex = elname.Replace("AhvDate", "");
                                                    HtmlElement AhvDateTimeCb = wbEfiling.Document.GetElementById("AhvDateTimeCb" + elnameIndex);
                                                    if (AhvDateTimeCb != null)
                                                    {
                                                        AhvDateTimeCb.SetAttribute("checked", "checked");
                                                        AhvDateTimeCb.Enabled = true;
                                                    }
                                                }
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            MessageBox.Show("E-filing of the data is complete!");
                            CloseProgress();
                        }
                    }
                    else
                    {
                        CloseProgress();
                        MessageBox.Show("Please enter the jobnumber and search to Fill the info");
                    }
                }
                else
                {
                    CloseProgress();
                    MessageBox.Show("Please select the job document");
                }
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region EF 1 Methods

        private void FillFirstStepData()
        {
            try
            {
                ShowProgress();
                RpoContext rpoContext = new RpoContext();
                int idJobDocument = Convert.ToInt32(ddlJobDocument.SelectedValue);
                if (idJobDocument > 0)
                {
                    jobDocument = rpoContext.JobDocuments.Include("JobApplication.JobApplicationType").Include("JobDocumentFields.DocumentField.Field").FirstOrDefault(x => x.Id == idJobDocument);
                    jobDocumentFieldList = jobDocument.JobDocumentFields;

                    if (jobDocument != null && jobDocument.JobApplication != null && jobDocument.JobApplication.JobApplicationType != null &&
                        (jobDocument.JobApplication.JobApplicationType.Description.ToLower() == "subdivision condominiums"
                        || jobDocument.JobApplication.JobApplicationType.Description.ToLower() == "subdivision improved property"))
                    {
                        IsSimpleEfiling = false;
                    }
                    else
                    {
                        IsSimpleEfiling = true;
                    }

                    if (jobDocument != null)
                    {
                        job = rpoContext.Jobs.Include("RfpAddress.Borough")
                        .Include("RfpAddress.Company")
                        .Include("RfpAddress.OwnerType")
                        .Include("RfpAddress.OwnerContact")
                        .Include("RfpAddress.SecondOfficerCompany")
                        .Include("RfpAddress.SecondOfficer")
                        .Include("RfpAddress.OccupancyClassification")
                        .Include("RfpAddress.ConstructionClassification")
                        .Include("RfpAddress.MultipleDwellingClassification")
                        .Include("RfpAddress.PrimaryStructuralSystem")
                        .Include("RfpAddress.StructureOccupancyCategory")
                        .Include("RfpAddress.SeismicDesignCategory")
                        .FirstOrDefault(x => x.Id == jobDocument.IdJob);
                    }

                    if (job != null && jobDocument != null && jobDocumentFieldList != null)
                    {
                        IseFilingCompleted = false;

                        #region Job Address

                        HtmlElement JBoro = wbEfiling.Document.GetElementById("JBoro");
                        if (JBoro != null)
                        {
                            JBoro.SetAttribute("value", job != null && job.RfpAddress != null && job.RfpAddress.Borough != null ? Convert.ToString(job.RfpAddress.Borough.BisCode) : string.Empty);
                        }

                        HtmlElement JHouseNumber = wbEfiling.Document.GetElementById("JHouseNumber");
                        if (JHouseNumber != null)
                        {
                            JHouseNumber.InnerText = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber : string.Empty;
                        }

                        HtmlElement JStreetName = wbEfiling.Document.GetElementById("JStreetName");
                        if (JStreetName != null)
                        {
                            JStreetName.InnerText = job != null && job.RfpAddress != null ? job.RfpAddress.Street : string.Empty;
                        }

                        HtmlElement JBlock = wbEfiling.Document.GetElementById("JBlock");
                        if (JBlock != null)
                        {
                            JBlock.InnerText = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                        }

                        HtmlElement JLot = wbEfiling.Document.GetElementById("JLot");
                        if (JLot != null)
                        {
                            JLot.InnerText = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                        }
                        #endregion

                        #region Applicant

                        JobDocumentField jobDocumentField_Applicant = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Applicant");
                        if (jobDocumentField_Applicant != null)
                        {
                            int idJobContact = Convert.ToInt32(jobDocumentField_Applicant.Value);
                            JobContact jobContact = rpoContext.JobContacts.FirstOrDefault(x => x.Id == idJobContact);
                            if (jobContact != null)
                            {
                                HtmlElement NAppLastName = wbEfiling.Document.GetElementById("NAppLastName");
                                if (NAppLastName != null)
                                {
                                    NAppLastName.InnerText = jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;
                                }

                                HtmlElement NAppFirstName = wbEfiling.Document.GetElementById("NAppFirstName");
                                if (NAppFirstName != null)
                                {
                                    NAppFirstName.InnerText = jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                                }

                                HtmlElement NAppMI = wbEfiling.Document.GetElementById("NAppMI");
                                if (NAppMI != null)
                                {
                                    NAppMI.InnerText = jobContact.Contact != null ? jobContact.Contact.MiddleName : string.Empty;
                                }

                                HtmlElement nappLicenseType = wbEfiling.Document.GetElementById("NAppLicenseType");
                                ContactLicense contactLicense = jobContact.Contact != null && jobContact.Contact.ContactLicenses != null ? jobContact.Contact.ContactLicenses
                                              .FirstOrDefault(x => x.ContactLicenseType.Name == "Architect"
                                                                || x.ContactLicenseType.Name == "Engineer"
                                                                || x.ContactLicenseType.Name == "Registered Landscape Architect") : null;
                                if (contactLicense != null)
                                {
                                    string applicantType = string.Empty;
                                    if (contactLicense.ContactLicenseType.Name == "Architect")
                                    {
                                        applicantType = "RA";
                                    }
                                    else if (contactLicense.ContactLicenseType.Name == "Engineer")
                                    {
                                        applicantType = "PE";
                                    }
                                    else if (contactLicense.ContactLicenseType.Name == "Registered Landscape Architect")
                                    {
                                        applicantType = "LA";
                                    }

                                    if (nappLicenseType != null)
                                    {
                                        nappLicenseType.SetAttribute("value", applicantType);
                                        nappLicenseType.InvokeMember("onchange");
                                    }

                                    HtmlElement NAppLicenseNumber = wbEfiling.Document.GetElementById("NAppLicenseNumber");
                                    if (NAppLicenseNumber != null)
                                    {
                                        NAppLicenseNumber.InnerText = contactLicense.Number;
                                    }
                                }


                                Company company = jobContact.Contact != null ? jobContact.Contact.Company : null;

                                //Address address = company != null && company.Addresses != null ? company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;

                                Address address = Common.GetContactAddressForJobDocument(jobContact);

                                if (address == null)
                                {
                                    address = jobContact.Contact != null && jobContact.Contact.Addresses != null ? jobContact.Contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                                }

                                HtmlElement NAppBusName = wbEfiling.Document.GetElementById("NAppBusName");
                                if (NAppBusName != null)
                                {
                                    NAppBusName.InnerText = company != null ? company.Name : string.Empty;
                                }

                                HtmlElement NAppEmail = wbEfiling.Document.GetElementById("NAppEmail");
                                if (NAppEmail != null)
                                {
                                    NAppEmail.InnerText = jobContact.Contact != null ? jobContact.Contact.Email : string.Empty;
                                }

                                string applicantAddress1 = address != null ? address.Address1 : string.Empty;
                                string housnumber = string.Empty;
                                string street = string.Empty;

                                string[] splitedAddress = applicantAddress1.Split(' ');
                                if (splitedAddress != null && splitedAddress.Count() > 0)
                                {
                                    string tempHouseNumber = splitedAddress[0];
                                    if (Char.IsDigit(tempHouseNumber.First()))
                                    {
                                        housnumber = splitedAddress[0];
                                        street = applicantAddress1.Replace(housnumber, string.Empty);
                                    }
                                    else
                                    {
                                        housnumber = string.Empty;
                                        street = applicantAddress1;
                                    }
                                }

                                HtmlElement NAppBusHouseNumber = wbEfiling.Document.GetElementById("NAppBusHouseNumber");
                                if (NAppBusHouseNumber != null)
                                {
                                    NAppBusHouseNumber.InnerText = housnumber;
                                }

                                HtmlElement NAppBusStreetName = wbEfiling.Document.GetElementById("NAppBusStreetName");
                                if (NAppBusStreetName != null)
                                {
                                    NAppBusStreetName.InnerText = street;
                                }

                                HtmlElement NAppBusAddress2 = wbEfiling.Document.GetElementById("NAppBusAddress2");
                                if (NAppBusAddress2 != null)
                                {
                                    NAppBusAddress2.InnerText = address != null ? address.Address2 : string.Empty;
                                }

                                HtmlElement NAppBusCity = wbEfiling.Document.GetElementById("NAppBusCity");
                                if (NAppBusCity != null)
                                {
                                    NAppBusCity.InnerText = address != null ? address.City : string.Empty;
                                }

                                HtmlElement NAppBusState = wbEfiling.Document.GetElementById("NAppBusState");
                                if (NAppBusState != null)
                                {
                                    NAppBusState.SetAttribute("value", address != null && address.State != null ? Convert.ToString(address.State.Acronym) : string.Empty);
                                }

                                HtmlElement NAppBusZip5 = wbEfiling.Document.GetElementById("NAppBusZip5");
                                if (NAppBusZip5 != null)
                                {
                                    NAppBusZip5.InnerText = address != null ? address.ZipCode : string.Empty;
                                }

                                HtmlElement NAppBusZip4 = wbEfiling.Document.GetElementById("NAppBusZip4");
                                if (NAppBusZip4 != null)
                                {
                                    NAppBusZip4.InnerText = string.Empty;
                                }

                                string workPhone = Common.GetContactPhoneNumberForJobDocument(jobContact);
                                string workPhone1 = string.Empty;
                                string workPhone2 = string.Empty;
                                string workPhone3 = string.Empty;

                                if (!string.IsNullOrEmpty(workPhone))
                                {
                                    workPhone = workPhone.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                                }

                                if (!string.IsNullOrEmpty(workPhone) && workPhone.Length == 10)
                                {
                                    workPhone1 = workPhone.Substring(0, 3);
                                    workPhone2 = workPhone.Substring(3, 3);
                                    workPhone3 = workPhone.Substring(6, 4);
                                }

                                HtmlElement NAppBusPhoneArea = wbEfiling.Document.GetElementById("NAppBusPhoneArea");
                                if (NAppBusPhoneArea != null)
                                {
                                    NAppBusPhoneArea.InnerText = workPhone1;
                                }

                                HtmlElement NAppBusPhonePrefix = wbEfiling.Document.GetElementById("NAppBusPhonePrefix");
                                if (NAppBusPhonePrefix != null)
                                {
                                    NAppBusPhonePrefix.InnerText = workPhone2;
                                }

                                HtmlElement NAppBusPhoneLine = wbEfiling.Document.GetElementById("NAppBusPhoneLine");
                                if (NAppBusPhoneLine != null)
                                {
                                    NAppBusPhoneLine.InnerText = workPhone3;
                                }

                                string mobilePhone = jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                                string mobilePhone1 = string.Empty;
                                string mobilePhone2 = string.Empty;
                                string mobilePhone3 = string.Empty;

                                if (!string.IsNullOrEmpty(mobilePhone))
                                {
                                    mobilePhone = mobilePhone.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                                }

                                if (!string.IsNullOrEmpty(mobilePhone) && mobilePhone.Length == 10)
                                {
                                    mobilePhone1 = mobilePhone.Substring(0, 3);
                                    mobilePhone2 = mobilePhone.Substring(3, 3);
                                    mobilePhone3 = mobilePhone.Substring(6, 4);
                                }

                                HtmlElement NAppMobilePhoneArea = wbEfiling.Document.GetElementById("NAppMobilePhoneArea");
                                if (NAppMobilePhoneArea != null)
                                {
                                    NAppMobilePhoneArea.InnerText = mobilePhone1;
                                }

                                HtmlElement NAppMobilePhonePrefix = wbEfiling.Document.GetElementById("NAppMobilePhonePrefix");
                                if (NAppMobilePhonePrefix != null)
                                {
                                    NAppMobilePhonePrefix.InnerText = mobilePhone2;
                                }

                                HtmlElement NAppMobilePhoneLine = wbEfiling.Document.GetElementById("NAppMobilePhoneLine");
                                if (NAppMobilePhoneLine != null)
                                {
                                    NAppMobilePhoneLine.InnerText = mobilePhone3;
                                }

                                string workFax = Common.GetContactFaxNumberForJobDocument(jobContact);
                                string workFax1 = string.Empty;
                                string workFax2 = string.Empty;
                                string workFax3 = string.Empty;

                                if (!string.IsNullOrEmpty(workFax))
                                {
                                    workFax = workFax.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                                }

                                if (!string.IsNullOrEmpty(workFax) && workFax.Length == 10)
                                {
                                    workFax1 = workFax.Substring(0, 3);
                                    workFax2 = workFax.Substring(3, 3);
                                    workFax3 = workFax.Substring(6, 4);
                                }

                                HtmlElement NAppFaxArea = wbEfiling.Document.GetElementById("NAppFaxArea");
                                if (NAppFaxArea != null)
                                {
                                    NAppFaxArea.InnerText = workFax1;
                                }

                                HtmlElement NAppFaxPrefix = wbEfiling.Document.GetElementById("NAppFaxPrefix");
                                if (NAppFaxPrefix != null)
                                {
                                    NAppFaxPrefix.InnerText = workFax2;
                                }

                                HtmlElement NAppFaxLine = wbEfiling.Document.GetElementById("NAppFaxLine");
                                if (NAppFaxLine != null)
                                {
                                    NAppFaxLine.InnerText = workFax3;
                                }
                            }
                            if (jobDocument.JobApplication.IdJobApplicationType == 26)
                            {
                                HtmlElement Dir14_0 = wbEfiling.Document.GetElementById("JDirective14[0]");
                                if (Dir14_0 != null)
                                {
                                    Dir14_0.SetAttribute("checked", "checked");
                                }
                            }
                            else
                            {
                                HtmlElement Dir14_1 = wbEfiling.Document.GetElementById("JDirective14[1]");
                                if (Dir14_1 != null)
                                {
                                    Dir14_1.SetAttribute("checked", "checked");
                                }
                            }
                        }

                        #endregion

                        #region Filling Representative

                        JobDocumentField jobDocumentField_FillingRepresentative = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Filing Representative");
                        JobDocumentField jobDocumentField_FillingRepresentative_Two = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Filing Representative #2");
                        JobDocumentField jobDocumentField_FillingRepresentative_Three = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Filing Representative #3");
                        if (jobDocumentField_FillingRepresentative != null)
                        {
                            HtmlElement RepSameAsApplFlag_1 = wbEfiling.Document.GetElementById("RepSameAsApplFlag[1]");
                            if (RepSameAsApplFlag_1 != null)
                            {
                                RepSameAsApplFlag_1.SetAttribute("checked", "checked");
                            }

                            HtmlElement repForm = wbEfiling.Document.GetElementById("repForm");
                            if (repForm != null)
                            {
                                repForm.Style = "display: inline;";
                            }

                            int idEmployee = Convert.ToInt32(jobDocumentField_FillingRepresentative.Value);
                            Employee employee = rpoContext.Employees.FirstOrDefault(x => x.Id == idEmployee);
                            int idEmployee_Two = 0;
                            if (jobDocumentField_FillingRepresentative_Two !=null)
                            {
                                idEmployee_Two = Convert.ToInt32(jobDocumentField_FillingRepresentative_Two.Value);
                            }
                            int idEmployee_Three = 0;
                            if (jobDocumentField_FillingRepresentative_Three != null)
                            {
                                idEmployee_Three = Convert.ToInt32(jobDocumentField_FillingRepresentative_Three.Value);
                            }

                            Employee employee_Two = rpoContext.Employees.FirstOrDefault(x => x.Id == idEmployee_Two);

                            Employee employee_Three = rpoContext.Employees.FirstOrDefault(x => x.Id == idEmployee_Three);

                            if (employee != null)
                            {
                                string fillingRepresentative_LastName = employee.LastName;
                                string fillingRepresentative_FirstName = employee.FirstName;

                                if (employee_Two != null)
                                {
                                    fillingRepresentative_LastName = fillingRepresentative_LastName + "/" + employee_Two.LastName;
                                    fillingRepresentative_FirstName = fillingRepresentative_FirstName + "/" + employee_Two.FirstName;
                                }

                                if (employee_Three != null)
                                {
                                    fillingRepresentative_LastName = fillingRepresentative_LastName + "/" + employee_Three.LastName;
                                    fillingRepresentative_FirstName = fillingRepresentative_FirstName + "/" + employee_Three.FirstName;
                                }

                                HtmlElement NRepLastName = wbEfiling.Document.GetElementById("NRepLastName");
                                if (NRepLastName != null)
                                {
                                    NRepLastName.InnerText = fillingRepresentative_LastName;
                                }
                                HtmlElement NRepFirstName = wbEfiling.Document.GetElementById("NRepFirstName");
                                if (NRepFirstName != null)
                                {
                                    NRepFirstName.InnerText = fillingRepresentative_FirstName;
                                }
                                HtmlElement NRepMI = wbEfiling.Document.GetElementById("NRepMI");
                                if (NRepMI != null)
                                {
                                    NRepMI.InnerText = string.Empty;
                                }
                                HtmlElement NRepBusName = wbEfiling.Document.GetElementById("NRepBusName");
                                if (NRepBusName != null)
                                {
                                    NRepBusName.InnerText = "RPO INC";
                                }
                                HtmlElement NRepEmail = wbEfiling.Document.GetElementById("NRepEmail");
                                if (NRepEmail != null)
                                {
                                    NRepEmail.InnerText = employee.Email;
                                }
                                HtmlElement NRepBusHouseNumber = wbEfiling.Document.GetElementById("NRepBusHouseNumber");
                                if (NRepBusHouseNumber != null)
                                {
                                    NRepBusHouseNumber.InnerText = "146";
                                }
                                HtmlElement NRepBusStreetName = wbEfiling.Document.GetElementById("NRepBusStreetName");
                                if (NRepBusStreetName != null)
                                {
                                    NRepBusStreetName.InnerText = "W 29TH STREET";
                                }
                                HtmlElement NRepBusAddress2 = wbEfiling.Document.GetElementById("NRepBusAddress2");
                                if (NRepBusAddress2 != null)
                                {
                                    NRepBusAddress2.InnerText = "SUITE 2E";
                                }
                                HtmlElement NRepBusCity = wbEfiling.Document.GetElementById("NRepBusCity");
                                if (NRepBusCity != null)
                                {
                                    NRepBusCity.InnerText = "NEW YORK";
                                }
                                HtmlElement NRepBusState = wbEfiling.Document.GetElementById("NRepBusState");
                                if (NRepBusState != null)
                                {
                                    NRepBusState.SetAttribute("value", "NY");
                                }
                                HtmlElement NRepBusZip5 = wbEfiling.Document.GetElementById("NRepBusZip5");
                                if (NRepBusZip5 != null)
                                {
                                    NRepBusZip5.InnerText = "10001";
                                }

                                AgentCertificate agentCertificate = employee != null && employee.AgentCertificates != null && employee.AgentCertificates.Count() > 0 ?
                                    employee.AgentCertificates.FirstOrDefault(x => x.DocumentType.Name.ToLower() == "dob filing representative") : null;

                                if (agentCertificate == null)
                                {
                                    agentCertificate = employee_Two != null && employee_Two.AgentCertificates != null && employee_Two.AgentCertificates.Count() > 0 ?
                                    employee_Two.AgentCertificates.FirstOrDefault(x => x.DocumentType.Name.ToLower() == "dob filing representative") : null;
                                }

                                if (agentCertificate == null)
                                {
                                    agentCertificate = employee_Three != null && employee_Three.AgentCertificates != null && employee_Three.AgentCertificates.Count() > 0 ?
                                    employee_Three.AgentCertificates.FirstOrDefault(x => x.DocumentType.Name.ToLower() == "dob filing representative") : null;
                                }

                                string licenseNumber = agentCertificate != null ? agentCertificate.NumberId : string.Empty;

                                HtmlElement NRepLicenseNumber = wbEfiling.Document.GetElementById("NRepLicenseNumber");
                                if (NRepLicenseNumber != null)
                                {
                                    NRepLicenseNumber.InnerText = licenseNumber;
                                }


                                string mobilePhone = employee.MobilePhone;
                                string mobilePhone1 = string.Empty;
                                string mobilePhone2 = string.Empty;
                                string mobilePhone3 = string.Empty;

                                if (!string.IsNullOrEmpty(mobilePhone))
                                {
                                    mobilePhone = mobilePhone.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                                }

                                if (!string.IsNullOrEmpty(mobilePhone) && mobilePhone.Length == 10)
                                {
                                    mobilePhone1 = mobilePhone.Substring(0, 3);
                                    mobilePhone2 = mobilePhone.Substring(3, 3);
                                    mobilePhone3 = mobilePhone.Substring(6, 4);
                                }

                                HtmlElement NRepMobilePhoneArea = wbEfiling.Document.GetElementById("NRepMobilePhoneArea");
                                if (NRepMobilePhoneArea != null)
                                {
                                    NRepMobilePhoneArea.InnerText = mobilePhone1;
                                }
                                HtmlElement NRepMobilePhonePrefix = wbEfiling.Document.GetElementById("NRepMobilePhonePrefix");
                                if (NRepMobilePhonePrefix != null)
                                {
                                    NRepMobilePhonePrefix.InnerText = mobilePhone2;
                                }
                                HtmlElement NRepMobilePhoneLine = wbEfiling.Document.GetElementById("NRepMobilePhoneLine");
                                if (NRepMobilePhoneLine != null)
                                {
                                    NRepMobilePhoneLine.InnerText = mobilePhone3;
                                }
                                HtmlElement NRepFaxArea = wbEfiling.Document.GetElementById("NRepFaxArea");
                                if (NRepFaxArea != null)
                                {
                                    NRepFaxArea.InnerText = "212";
                                }

                                HtmlElement NRepFaxPrefix = wbEfiling.Document.GetElementById("NRepFaxPrefix");
                                if (NRepFaxPrefix != null)
                                {
                                    NRepFaxPrefix.InnerText = "566";
                                }

                                HtmlElement NRepFaxLine = wbEfiling.Document.GetElementById("NRepFaxLine");
                                if (NRepFaxLine != null)
                                {
                                    NRepFaxLine.InnerText = "5112";
                                }

                                HtmlElement NRepBusPhoneArea = wbEfiling.Document.GetElementById("NRepBusPhoneArea");
                                if (NRepBusPhoneArea != null)
                                {
                                    NRepBusPhoneArea.InnerText = "212";
                                }
                                HtmlElement NRepBusPhonePrefix = wbEfiling.Document.GetElementById("NRepBusPhonePrefix");
                                if (NRepBusPhonePrefix != null)
                                {
                                    NRepBusPhonePrefix.InnerText = "566";
                                }

                                HtmlElement NRepBusPhoneLine = wbEfiling.Document.GetElementById("NRepBusPhoneLine");
                                if (NRepBusPhoneLine != null)
                                {
                                    NRepBusPhoneLine.InnerText = "5110";
                                }
                            }
                        }

                        #endregion

                        #region Owner
                        Contact property_Owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                        if (property_Owner != null)
                        {
                            HtmlElement NOwnerLastName = wbEfiling.Document.GetElementById("NOwnerLastName");
                            if (NOwnerLastName != null)
                            {
                                NOwnerLastName.InnerText = property_Owner.LastName;
                            }

                            HtmlElement NOwnerFirstName = wbEfiling.Document.GetElementById("NOwnerFirstName");
                            if (NOwnerFirstName != null)
                            {
                                NOwnerFirstName.InnerText = property_Owner.FirstName;
                            }

                            HtmlElement NOwnerMI = wbEfiling.Document.GetElementById("NOwnerMI");
                            if (NOwnerMI != null)
                            {
                                NOwnerMI.InnerText = property_Owner.MiddleName;
                            }

                            HtmlElement NOwnerRelToOwner = wbEfiling.Document.GetElementById("NOwnerRelToOwner");
                            if (NOwnerRelToOwner != null)
                            {
                                NOwnerRelToOwner.InnerText = property_Owner.ContactTitle != null ? property_Owner.ContactTitle.Name : string.Empty;
                            }

                            string ownerNameClass = string.Empty;
                            switch (job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null ? job.RfpAddress.OwnerType.Name.ToUpper().Trim() : string.Empty)
                            {
                                case "INDIVIDUAL":
                                    ownerNameClass = "01";
                                    HtmlElement ownerNonProfitForm_01 = wbEfiling.Document.GetElementById("ownerNonProfitForm");
                                    if (ownerNonProfitForm_01 != null)
                                    {
                                        ownerNonProfitForm_01.Style = "display: none;";
                                    }

                                    HtmlElement corpForm_01 = wbEfiling.Document.GetElementById("corpForm");
                                    if (corpForm_01 != null)
                                    {
                                        corpForm_01.Style = "display: none;";
                                    }

                                    break;
                                case "CORPORATION":
                                    ownerNameClass = "02";
                                    HtmlElement ownerNonProfitForm_02 = wbEfiling.Document.GetElementById("ownerNonProfitForm");
                                    if (ownerNonProfitForm_02 != null)
                                    {
                                        ownerNonProfitForm_02.Style = "display: inline;";
                                    }

                                    HtmlElement corpForm_02 = wbEfiling.Document.GetElementById("corpForm");
                                    if (corpForm_02 != null)
                                    {
                                        corpForm_02.Style = "display: none;";
                                    }
                                    break;
                                case "PARTNERSHIP":
                                    ownerNameClass = "03";

                                    HtmlElement ownerNonProfitForm_03 = wbEfiling.Document.GetElementById("ownerNonProfitForm");
                                    if (ownerNonProfitForm_03 != null)
                                    {
                                        ownerNonProfitForm_03.Style = "display: inline;";
                                    }

                                    HtmlElement corpForm_03 = wbEfiling.Document.GetElementById("corpForm");
                                    if (corpForm_03 != null)
                                    {
                                        corpForm_03.Style = "display: none;";
                                    }
                                    break;
                                case "CONDO/CO-OP":
                                    ownerNameClass = "04";
                                    HtmlElement ownerNonProfitForm_04 = wbEfiling.Document.GetElementById("ownerNonProfitForm");
                                    if (ownerNonProfitForm_04 != null)
                                    {
                                        ownerNonProfitForm_04.Style = "display: inline;";
                                    }
                                    HtmlElement corpForm_04 = wbEfiling.Document.GetElementById("corpForm");
                                    if (corpForm_04 != null)
                                    {
                                        corpForm_04.Style = "display: inline;";
                                    }
                                    break;
                                case "OTHER GOV'T AGEN":
                                    ownerNameClass = "11";
                                    HtmlElement ownerNonProfitForm_05 = wbEfiling.Document.GetElementById("ownerNonProfitForm");
                                    if (ownerNonProfitForm_05 != null)
                                    {
                                        ownerNonProfitForm_05.Style = "display: none;";
                                    }

                                    HtmlElement corpForm_05 = wbEfiling.Document.GetElementById("corpForm");
                                    if (corpForm_05 != null)
                                    {
                                        corpForm_05.Style = "display: none;";
                                    }
                                    break;
                                case "NYC AGENCY":
                                    ownerNameClass = "13";
                                    HtmlElement ownerNonProfitForm_06 = wbEfiling.Document.GetElementById("ownerNonProfitForm");
                                    if (ownerNonProfitForm_06 != null)
                                    {
                                        ownerNonProfitForm_06.Style = "display: none;";
                                    }

                                    HtmlElement corpForm_06 = wbEfiling.Document.GetElementById("corpForm");
                                    if (corpForm_06 != null)
                                    {
                                        corpForm_06.Style = "display: none;";
                                    }
                                    break;
                                case "NYCHA/HHC":
                                    ownerNameClass = "14";
                                    HtmlElement ownerNonProfitForm_07 = wbEfiling.Document.GetElementById("ownerNonProfitForm");
                                    if (ownerNonProfitForm_07 != null)
                                    {
                                        ownerNonProfitForm_07.Style = "display: none;";
                                    }

                                    HtmlElement corpForm_07 = wbEfiling.Document.GetElementById("corpForm");
                                    if (corpForm_07 != null)
                                    {
                                        corpForm_07.Style = "display: none;";
                                    }
                                    break;
                            }

                            HtmlElement NOwnerNameClass = wbEfiling.Document.GetElementById("NOwnerNameClass");
                            if (NOwnerNameClass != null)
                            {
                                NOwnerNameClass.SetAttribute("value", ownerNameClass);
                            }

                            Company company = property_Owner.Company;
                            //Address address = company != null && company.Addresses != null ? company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;

                            Address address = Common.GetContactAddressForJobDocument(property_Owner);

                            if (address == null)
                            {
                                address = property_Owner.Addresses != null ? property_Owner.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            HtmlElement NOwnerBusName = wbEfiling.Document.GetElementById("NOwnerBusName");
                            if (NOwnerBusName != null)
                            {
                                NOwnerBusName.InnerText = company != null ? company.Name : string.Empty;
                            }

                            HtmlElement NOwnerEmail = wbEfiling.Document.GetElementById("NOwnerEmail");
                            if (NOwnerEmail != null)
                            {
                                NOwnerEmail.InnerText = property_Owner.Email;
                            }

                            string applicantAddress1 = address != null ? address.Address1 : string.Empty;
                            string housnumber = string.Empty;
                            string street = string.Empty;

                            string[] splitedAddress = applicantAddress1.Split(' ');
                            if (splitedAddress != null && splitedAddress.Count() > 0)
                            {
                                string tempHouseNumber = splitedAddress[0];
                                if (Char.IsDigit(tempHouseNumber.First()))
                                {
                                    housnumber = splitedAddress[0];
                                    street = applicantAddress1.Replace(housnumber, string.Empty);
                                }
                                else
                                {
                                    housnumber = string.Empty;
                                    street = applicantAddress1;
                                }
                            }

                            HtmlElement NOwnerBusHouseNumber = wbEfiling.Document.GetElementById("NOwnerBusHouseNumber");
                            if (NOwnerBusHouseNumber != null)
                            {
                                NOwnerBusHouseNumber.InnerText = housnumber;
                            }

                            HtmlElement NOwnerBusStreetName = wbEfiling.Document.GetElementById("NOwnerBusStreetName");
                            if (NOwnerBusStreetName != null)
                            {
                                NOwnerBusStreetName.InnerText = street;
                            }
                            HtmlElement NOwnerBusAddress2 = wbEfiling.Document.GetElementById("NOwnerBusAddress2");
                            if (NOwnerBusAddress2 != null)
                            {
                                NOwnerBusAddress2.InnerText = address != null ? address.Address2 : string.Empty;
                            }

                            HtmlElement NOwnerBusCity = wbEfiling.Document.GetElementById("NOwnerBusCity");
                            if (NOwnerBusCity != null)
                            {
                                NOwnerBusCity.InnerText = address != null ? address.City : string.Empty;
                            }

                            HtmlElement NOwnerBusState = wbEfiling.Document.GetElementById("NOwnerBusState");
                            if (NOwnerBusState != null)
                            {
                                NOwnerBusState.SetAttribute("value", address != null && address.State != null ? Convert.ToString(address.State.Acronym) : string.Empty);
                            }

                            HtmlElement NOwnerBusZip5 = wbEfiling.Document.GetElementById("NOwnerBusZip5");
                            if (NOwnerBusZip5 != null)
                            {
                                NOwnerBusZip5.InnerText = address != null ? address.ZipCode : string.Empty;
                            }

                            HtmlElement NOwnerBusZip4 = wbEfiling.Document.GetElementById("NOwnerBusZip4");
                            if (NOwnerBusZip4 != null)
                            {
                                NOwnerBusZip4.InnerText = string.Empty;
                            }
                            bool isNonProfit = job != null && job.RfpAddress != null ? job.RfpAddress.NonProfit : false;

                            HtmlElement JOwnerNonProfFlag_0 = wbEfiling.Document.GetElementById("JOwnerNonProfFlag[0]");
                            if (JOwnerNonProfFlag_0 != null)
                            {
                                JOwnerNonProfFlag_0.SetAttribute("checked", "");
                            }

                            HtmlElement JOwnerNonProfFlag_1 = wbEfiling.Document.GetElementById("JOwnerNonProfFlag1[0]");
                            if (JOwnerNonProfFlag_1 != null)
                            {
                                JOwnerNonProfFlag_1.SetAttribute("checked", "");
                            }

                            if (isNonProfit)
                            {
                                if (JOwnerNonProfFlag_0 != null)
                                {
                                    JOwnerNonProfFlag_0.SetAttribute("checked", "checked");
                                }
                            }
                            else
                            {
                                if (JOwnerNonProfFlag_1 != null)
                                {
                                    JOwnerNonProfFlag_1.SetAttribute("checked", "checked");
                                }
                            }

                            string workPhone = Common.GetContactPhoneNumberForJobDocument(property_Owner);
                            string workPhone1 = string.Empty;
                            string workPhone2 = string.Empty;
                            string workPhone3 = string.Empty;

                            if (!string.IsNullOrEmpty(workPhone))
                            {
                                workPhone = workPhone.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                            }

                            if (!string.IsNullOrEmpty(workPhone) && workPhone.Length == 10)
                            {
                                workPhone1 = workPhone.Substring(0, 3);
                                workPhone2 = workPhone.Substring(3, 3);
                                workPhone3 = workPhone.Substring(6, 4);
                            }

                            HtmlElement NOwnerBusPhoneArea = wbEfiling.Document.GetElementById("NOwnerBusPhoneArea");
                            if (NOwnerBusPhoneArea != null)
                            {
                                NOwnerBusPhoneArea.InnerText = workPhone1;
                            }

                            HtmlElement NOwnerBusPhonePrefix = wbEfiling.Document.GetElementById("NOwnerBusPhonePrefix");
                            if (NOwnerBusPhonePrefix != null)
                            {
                                NOwnerBusPhonePrefix.InnerText = workPhone2;
                            }

                            HtmlElement NOwnerBusPhoneLine = wbEfiling.Document.GetElementById("NOwnerBusPhoneLine");
                            if (NOwnerBusPhoneLine != null)
                            {
                                NOwnerBusPhoneLine.InnerText = workPhone3;
                            }

                            string mobilePhone = property_Owner.MobilePhone;
                            string mobilePhone1 = string.Empty;
                            string mobilePhone2 = string.Empty;
                            string mobilePhone3 = string.Empty;

                            if (!string.IsNullOrEmpty(mobilePhone))
                            {
                                mobilePhone = mobilePhone.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                            }

                            if (!string.IsNullOrEmpty(mobilePhone) && mobilePhone.Length == 10)
                            {
                                mobilePhone1 = mobilePhone.Substring(0, 3);
                                mobilePhone2 = mobilePhone.Substring(3, 3);
                                mobilePhone3 = mobilePhone.Substring(6, 4);
                            }

                            HtmlElement NOwnerMobilePhoneArea = wbEfiling.Document.GetElementById("NOwnerMobilePhoneArea");
                            if (NOwnerMobilePhoneArea != null)
                            {
                                NOwnerMobilePhoneArea.InnerText = mobilePhone1;
                            }

                            HtmlElement NOwnerMobilePhonePrefix = wbEfiling.Document.GetElementById("NOwnerMobilePhonePrefix");
                            if (NOwnerMobilePhonePrefix != null)
                            {
                                NOwnerMobilePhonePrefix.InnerText = mobilePhone2;
                            }

                            HtmlElement NOwnerMobilePhoneLine = wbEfiling.Document.GetElementById("NOwnerMobilePhoneLine");
                            if (NOwnerMobilePhoneLine != null)
                            {
                                NOwnerMobilePhoneLine.InnerText = mobilePhone3;
                            }

                            string workFax = Common.GetContactFaxNumberForJobDocument(property_Owner);
                            string workFax1 = string.Empty;
                            string workFax2 = string.Empty;
                            string workFax3 = string.Empty;

                            if (!string.IsNullOrEmpty(workFax))
                            {
                                workFax = workFax.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                            }

                            if (!string.IsNullOrEmpty(workFax) && workFax.Length == 10)
                            {
                                workFax1 = workFax.Substring(0, 3);
                                workFax2 = workFax.Substring(3, 3);
                                workFax3 = workFax.Substring(6, 4);
                            }

                            HtmlElement NOwnerFaxArea = wbEfiling.Document.GetElementById("NOwnerFaxArea");
                            if (NOwnerFaxArea != null)
                            {
                                NOwnerFaxArea.InnerText = workFax1;
                            }

                            HtmlElement NOwnerFaxPrefix = wbEfiling.Document.GetElementById("NOwnerFaxPrefix");
                            if (NOwnerFaxPrefix != null)
                            {
                                NOwnerFaxPrefix.InnerText = workFax2;
                            }

                            HtmlElement NOwnerFaxLine = wbEfiling.Document.GetElementById("NOwnerFaxLine");
                            if (NOwnerFaxLine != null)
                            {
                                NOwnerFaxLine.InnerText = workFax3;
                            }
                        }

                        #endregion

                        #region Second Officer

                        Contact property_SecondOfficer = job != null && job.RfpAddress != null && job.RfpAddress.SecondOfficer != null ? job.RfpAddress.SecondOfficer : null;
                        if (property_SecondOfficer != null)
                        {
                            HtmlElement NCorpLastName = wbEfiling.Document.GetElementById("NCorpLastName");
                            if (NCorpLastName != null)
                            {
                                NCorpLastName.InnerText = property_SecondOfficer.LastName;
                            }

                            HtmlElement NCorpFirstName = wbEfiling.Document.GetElementById("NCorpFirstName");
                            if (NCorpFirstName != null)
                            {
                                NCorpFirstName.InnerText = property_SecondOfficer.FirstName;
                            }

                            HtmlElement NCorpMI = wbEfiling.Document.GetElementById("NCorpMI");
                            if (NCorpMI != null)
                            {
                                NCorpMI.InnerText = property_SecondOfficer.MiddleName;
                            }

                            HtmlElement NCorpTitle = wbEfiling.Document.GetElementById("NCorpTitle");
                            if (NCorpTitle != null)
                            {
                                NCorpTitle.InnerText = property_SecondOfficer.ContactTitle != null ? property_SecondOfficer.ContactTitle.Name : string.Empty;
                            }

                            Company company = property_SecondOfficer.Company;
                            //Address address = company != null && company.Addresses != null ? company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;

                            Address address = Common.GetContactAddressForJobDocument(property_SecondOfficer);

                            if (address == null)
                            {
                                address = property_SecondOfficer.Addresses != null ? property_SecondOfficer.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            HtmlElement NCorpBusName = wbEfiling.Document.GetElementById("NCorpBusName");
                            if (NCorpBusName != null)
                            {
                                NCorpBusName.InnerText = company != null ? company.Name : string.Empty;
                            }

                            HtmlElement NCorpEmail = wbEfiling.Document.GetElementById("NCorpEmail");
                            if (NCorpEmail != null)
                            {
                                NCorpEmail.InnerText = property_SecondOfficer.Email;
                            }

                            string applicantAddress1 = address != null ? address.Address1 : string.Empty;
                            string housnumber = string.Empty;
                            string street = string.Empty;

                            string[] splitedAddress = applicantAddress1.Split(' ');
                            if (splitedAddress != null && splitedAddress.Count() > 0)
                            {
                                string tempHouseNumber = splitedAddress[0];
                                if (Char.IsDigit(tempHouseNumber.First()))
                                {
                                    housnumber = splitedAddress[0];
                                    street = applicantAddress1.Replace(housnumber, string.Empty);
                                }
                                else
                                {
                                    housnumber = string.Empty;
                                    street = applicantAddress1;
                                }
                            }

                            HtmlElement NCorpBusHouseNumber = wbEfiling.Document.GetElementById("NCorpBusHouseNumber");
                            if (NCorpBusHouseNumber != null)
                            {
                                NCorpBusHouseNumber.InnerText = housnumber;
                            }

                            HtmlElement NCorpBusStreetName = wbEfiling.Document.GetElementById("NCorpBusStreetName");
                            if (NCorpBusStreetName != null)
                            {
                                NCorpBusStreetName.InnerText = street;
                            }

                            HtmlElement NCorpBusAddress2 = wbEfiling.Document.GetElementById("NCorpBusAddress2");
                            if (NCorpBusAddress2 != null)
                            {
                                NCorpBusAddress2.InnerText = address != null ? address.Address2 : string.Empty;
                            }

                            HtmlElement NCorpBusCity = wbEfiling.Document.GetElementById("NCorpBusCity");
                            if (NCorpBusCity != null)
                            {
                                NCorpBusCity.InnerText = address != null ? address.City : string.Empty;
                            }

                            HtmlElement NCorpBusState = wbEfiling.Document.GetElementById("NCorpBusState");
                            if (NCorpBusState != null)
                            {
                                NCorpBusState.SetAttribute("value", address != null && address.State != null ? Convert.ToString(address.State.Acronym) : string.Empty);
                            }

                            HtmlElement NCorpBusZip5 = wbEfiling.Document.GetElementById("NCorpBusZip5");
                            if (NCorpBusZip5 != null)
                            {
                                NCorpBusZip5.InnerText = address != null ? address.ZipCode : string.Empty;
                            }

                            HtmlElement NCorpBusZip4 = wbEfiling.Document.GetElementById("NCorpBusZip4");
                            if (NCorpBusZip4 != null)
                            {
                                NCorpBusZip4.InnerText = string.Empty;
                            }

                            string workPhone = Common.GetContactPhoneNumberForJobDocument(property_SecondOfficer);
                            string workPhone1 = string.Empty;
                            string workPhone2 = string.Empty;
                            string workPhone3 = string.Empty;

                            if (!string.IsNullOrEmpty(workPhone))
                            {
                                workPhone = workPhone.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                            }

                            if (!string.IsNullOrEmpty(workPhone) && workPhone.Length == 10)
                            {
                                workPhone1 = workPhone.Substring(0, 3);
                                workPhone2 = workPhone.Substring(3, 3);
                                workPhone3 = workPhone.Substring(6, 4);
                            }

                            HtmlElement NCorpBusPhoneArea = wbEfiling.Document.GetElementById("NCorpBusPhoneArea");
                            if (NCorpBusPhoneArea != null)
                            {
                                NCorpBusPhoneArea.InnerText = workPhone1;
                            }

                            HtmlElement NCorpBusPhonePrefix = wbEfiling.Document.GetElementById("NCorpBusPhonePrefix");
                            if (NCorpBusPhonePrefix != null)
                            {
                                NCorpBusPhonePrefix.InnerText = workPhone2;
                            }

                            HtmlElement NCorpBusPhoneLine = wbEfiling.Document.GetElementById("NCorpBusPhoneLine");
                            if (NCorpBusPhoneLine != null)
                            {
                                NCorpBusPhoneLine.InnerText = workPhone3;
                            }
                            string mobilePhone = property_SecondOfficer.MobilePhone;
                            string mobilePhone1 = string.Empty;
                            string mobilePhone2 = string.Empty;
                            string mobilePhone3 = string.Empty;

                            if (!string.IsNullOrEmpty(mobilePhone))
                            {
                                mobilePhone = mobilePhone.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                            }

                            if (!string.IsNullOrEmpty(mobilePhone) && mobilePhone.Length == 10)
                            {
                                mobilePhone1 = mobilePhone.Substring(0, 3);
                                mobilePhone2 = mobilePhone.Substring(3, 3);
                                mobilePhone3 = mobilePhone.Substring(6, 4);
                            }

                            HtmlElement NCorpMobilePhoneArea = wbEfiling.Document.GetElementById("NCorpMobilePhoneArea");
                            if (NCorpMobilePhoneArea != null)
                            {
                                NCorpMobilePhoneArea.InnerText = mobilePhone1;
                            }

                            HtmlElement NCorpMobilePhonePrefix = wbEfiling.Document.GetElementById("NCorpMobilePhonePrefix");
                            if (NCorpMobilePhonePrefix != null)
                            {
                                NCorpMobilePhonePrefix.InnerText = mobilePhone2;
                            }

                            HtmlElement NCorpMobilePhoneLine = wbEfiling.Document.GetElementById("NCorpMobilePhoneLine");
                            if (NCorpMobilePhoneLine != null)
                            {
                                NCorpMobilePhoneLine.InnerText = mobilePhone3;
                            }


                            string workFax = Common.GetContactFaxNumberForJobDocument(property_SecondOfficer);
                            string workFax1 = string.Empty;
                            string workFax2 = string.Empty;
                            string workFax3 = string.Empty;

                            if (!string.IsNullOrEmpty(workFax))
                            {
                                workFax = workFax.Replace("(", "").Replace("(", "").Replace(")", "").Replace(")", "").Replace(" ", "").Replace(" ", "").Replace("-", "");
                            }

                            if (!string.IsNullOrEmpty(workFax) && workFax.Length == 10)
                            {
                                workFax1 = workFax.Substring(0, 3);
                                workFax2 = workFax.Substring(3, 3);
                                workFax3 = workFax.Substring(6, 4);
                            }

                            HtmlElement NCorpFaxArea = wbEfiling.Document.GetElementById("NCorpFaxArea");
                            if (NCorpFaxArea != null)
                            {
                                NCorpFaxArea.InnerText = workFax1;
                            }

                            HtmlElement NCorpFaxPrefix = wbEfiling.Document.GetElementById("NCorpFaxPrefix");
                            if (NCorpFaxPrefix != null)
                            {
                                NCorpFaxPrefix.InnerText = workFax2;
                            }

                            HtmlElement NCorpFaxLine = wbEfiling.Document.GetElementById("NCorpFaxLine");
                            if (NCorpFaxLine != null)
                            {
                                NCorpFaxLine.InnerText = workFax3;
                            }
                        }

                        #endregion

                        jobDocumentField_Applicantion = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Application");
                        jobDocumentField_WorkPermit = jobDocumentFieldList.FirstOrDefault(x => x.DocumentField.Field.FieldName == "Job Document Work Type");

                        if (jobDocumentField_Applicantion != null)
                        {
                            List<int> jobWorkType = jobDocumentField_WorkPermit != null && !string.IsNullOrEmpty(jobDocumentField_WorkPermit.Value) ? (jobDocumentField_WorkPermit.Value.Split(',') != null && jobDocumentField_WorkPermit.Value.Split(',').Any() ? jobDocumentField_WorkPermit.Value.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                            int idJobApplication = jobDocumentField_Applicantion != null ? Convert.ToInt32(jobDocumentField_Applicantion.Value) : 0;
                            jobApplicationWorkPermitTypeList = rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => jobWorkType.Contains(x.Id)).ToList();
                            workDescription = jobApplicationWorkPermitTypeList != null ? string.Join(" ", jobApplicationWorkPermitTypeList.Select(x => x.WorkDescription)) : string.Empty;

                            jobApplication = rpoContext.JobApplications.Include("JobApplicationType").FirstOrDefault(x => x.Id == idJobApplication);
                        }

                        #region Job Description

                        HtmlElement memojdsc = wbEfiling.Document.GetElementById("memojdsc");
                        if (memojdsc != null)
                        {
                            memojdsc.InnerText = workDescription;
                        }

                        HtmlElement ExJobNumberFlag_1 = wbEfiling.Document.GetElementById("ExJobNumberFlag[1]");
                        if (ExJobNumberFlag_1 != null)
                        {
                            ExJobNumberFlag_1.SetAttribute("checked", "checked");
                        }


                        #endregion

                        #region Special Programs

                        HtmlElement JBuildItBackFlag_1 = wbEfiling.Document.GetElementById("JBuildItBackFlag[1]");
                        if (JBuildItBackFlag_1 != null)
                        {
                            JBuildItBackFlag_1.SetAttribute("checked", "checked");
                        }

                        HtmlElement JNbatRequested_1 = wbEfiling.Document.GetElementById("JNbatRequested[1]");
                        if (JNbatRequested_1 != null)
                        {
                            JNbatRequested_1.SetAttribute("checked", "checked");
                        }


                        #endregion

                        HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                        if (elements != null && elements.Count > 0)
                        {
                            for (int i = 0; i < elements.Count; i++)
                            {
                                HtmlElement el = elements[i];
                                string elvalue = el.GetAttribute("value");
                                string elType = el.GetAttribute("type");

                                switch (elType)
                                {
                                    case "radio":
                                    case "checkbox":
                                    case "button":
                                        {
                                            if (elvalue == "Next >")
                                            {
                                                var abc = el.InvokeMember("click");
                                            }
                                            break;
                                        }
                                }
                            }
                        }

                        CompletedStep = 1;
                    }
                    else
                    {
                        CloseProgress();
                        MessageBox.Show("Please enter the jobnumber and search to Fill the info");
                    }
                }
                else
                {
                    CloseProgress();
                    MessageBox.Show("Please select the job document");
                }
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }


        }

        private void FillSecondStepData()
        {
            try
            {
                RpoContext rpoContext = new RpoContext();

                if (jobDocumentField_Applicantion != null)
                {
                    if (jobApplicationWorkPermitTypeList != null)
                    {
                        if (jobApplicationWorkPermitTypeList.Where(x => x.JobWorkType.Code == "BL").Any())
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JBL[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }
                        }

                        if (jobApplicationWorkPermitTypeList.Where(x => x.JobWorkType.Code == "FB").Any())
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JFB[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }
                        }

                        if (jobApplicationWorkPermitTypeList.Where(x => x.JobWorkType.Code == "FS").Any())
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JFS[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }
                        }

                        JobApplicationWorkPermitType FP_WorkPermit = jobApplicationWorkPermitTypeList.FirstOrDefault(x => x.JobWorkType.Code == "FP");
                        if (FP_WorkPermit != null)
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JFP[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }
                            HtmlElement sFPCost = wbEfiling.Document.GetElementById("sFPCost");
                            if (sFPCost != null)
                            {
                                sFPCost.InnerText = Convert.ToString(FP_WorkPermit.EstimatedCost);
                            }

                        }

                        JobApplicationWorkPermitType PL_WorkPermit = jobApplicationWorkPermitTypeList.FirstOrDefault(x => x.JobWorkType.Code == "PL");
                        if (PL_WorkPermit != null)
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JPL[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }

                            HtmlElement sPLCost = wbEfiling.Document.GetElementById("sPLCost");
                            if (sPLCost != null)
                            {
                                sPLCost.InnerText = Convert.ToString(PL_WorkPermit.EstimatedCost);
                            }


                        }

                        JobApplicationWorkPermitType SD_WorkPermit = jobApplicationWorkPermitTypeList.FirstOrDefault(x => x.JobWorkType.Code == "SD");
                        if (SD_WorkPermit != null)
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JSD[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }

                            HtmlElement sSDCost = wbEfiling.Document.GetElementById("sSDCost");
                            if (sSDCost != null)
                            {
                                sSDCost.InnerText = Convert.ToString(SD_WorkPermit.EstimatedCost);
                            }
                        }

                        JobApplicationWorkPermitType SP_WorkPermit = jobApplicationWorkPermitTypeList.FirstOrDefault(x => x.JobWorkType.Code == "SP");
                        if (SP_WorkPermit != null)
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JSP[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }
                            HtmlElement sSPCost = wbEfiling.Document.GetElementById("sSPCost");
                            if (sSPCost != null)
                            {
                                sSPCost.InnerText = Convert.ToString(SP_WorkPermit.EstimatedCost);
                            }

                        }

                        JobApplicationWorkPermitType MH_WorkPermit = jobApplicationWorkPermitTypeList.FirstOrDefault(x => x.JobWorkType.Code == "MH");
                        if (MH_WorkPermit != null)
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JMH[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }

                            HtmlElement sMHCost = wbEfiling.Document.GetElementById("sMHCost");
                            if (sMHCost != null)
                            {
                                sMHCost.InnerText = Convert.ToString(MH_WorkPermit.EstimatedCost);
                            }
                        }

                        if (jobApplicationWorkPermitTypeList.Where(x => x.JobWorkType.Code == "EQ").Any())
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JEQ[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }
                        }

                        JobApplicationWorkPermitType FA_WorkPermit = jobApplicationWorkPermitTypeList.FirstOrDefault(x => x.JobWorkType.Code == "FA" || x.JobWorkType.Code == "FA/New" || x.JobWorkType.Code == "FA/Mod");
                        if (FA_WorkPermit != null)
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("JFA[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }

                            var FA_WorkPermittotal = jobApplicationWorkPermitTypeList.Where(x => x.JobWorkType.Code == "FA" || x.JobWorkType.Code == "FA/New" || x.JobWorkType.Code == "FA/Mod").Sum(d=>d.EstimatedCost);
                            HtmlElement sFACost = wbEfiling.Document.GetElementById("sFACost");
                            if (sFACost != null && FA_WorkPermittotal!=null)
                            {
                                sFACost.InnerText = Convert.ToString(FA_WorkPermittotal);
                            }
                        }

                        JobApplicationWorkPermitType OT_WorkPermit = jobApplicationWorkPermitTypeList.FirstOrDefault(x => x.JobWorkType.Code == "OT" || x.JobWorkType.Code == "OT/GC" || x.JobWorkType.Code == "OT/EA");
                        if (OT_WorkPermit != null)
                        {
                            HtmlElement el = wbEfiling.Document.GetElementById("OtherWorktypeFlag[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }
                            HtmlElement sOTCost = wbEfiling.Document.GetElementById("sOTCost");
                            if (sOTCost != null)
                            {
                                sOTCost.InnerText = Convert.ToString(OT_WorkPermit.EstimatedCost);
                            }
                        }

                        //x.JobWorkType.Code == "OT").Any()
                        var otherJobType = jobApplicationWorkPermitTypeList.Where(x =>
                             x.JobWorkType.Code != "BL" && x.JobWorkType.Code != "FB" &&
                             x.JobWorkType.Code != "FS" && x.JobWorkType.Code != "FP" &&
                             x.JobWorkType.Code != "PL" && x.JobWorkType.Code != "SD" &&
                             x.JobWorkType.Code != "SP" && x.JobWorkType.Code != "MH" &&
                             x.JobWorkType.Code != "EQ" && x.JobWorkType.Code != "FA").FirstOrDefault();
                        if (otherJobType != null)
                        {

                            HtmlElement el = wbEfiling.Document.GetElementById("OtherWorktypeFlag[0]");
                            if (el != null)
                            {
                                el.SetAttribute("checked", "checked");
                                el.InvokeMember("checked");
                            }

                            HtmlElement elSfiling = wbEfiling.Document.GetElementById("sFilingworktype[1]");
                            if (elSfiling != null)
                            {
                                elSfiling.SetAttribute("checked", "checked");
                                //elSfiling.InvokeMember("checked");
                                elSfiling.InvokeMember("click");
                            }

                            //if (jobApplicationWorkPermitTypeList.Where(x => x.JobWorkType.Code == "OT").Any())
                            //{
                            //    HtmlElement el = wbEfiling.Document.GetElementById("OtherWorktypeFlag[0]");
                            //    if (el != null)
                            //    {
                            //        el.SetAttribute("checked", "checked");
                            //        el.InvokeMember("checked");
                            //    }

                            HtmlElement sOtDescriptionCode1 = wbEfiling.Document.GetElementById("sOtDescriptionCode1");
                            if (sOtDescriptionCode1 != null)
                            {
                                if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "BPP" || otherJobType.JobWorkType.Description.ToLower() == "builders pavement plan"))
                                {
                                    sOtDescriptionCode1.SetAttribute("value", "BPP");
                                    HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                    if (JOtCode != null)
                                    {
                                        JOtCode.SetAttribute("value", "BPP");
                                    }
                                }
                                if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "FAC" || otherJobType.JobWorkType.Description.ToLower() == "facade repair"))
                                {
                                    sOtDescriptionCode1.SetAttribute("value", "FAC");
                                    HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                    if (JOtCode != null)
                                    {
                                        JOtCode.SetAttribute("value", "FAC");
                                    }
                                }
                                else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "GC" || otherJobType.JobWorkType.Description.ToLower() == "general construction"))
                                {
                                    sOtDescriptionCode1.SetAttribute("value", "GC");
                                    HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                    if (JOtCode != null)
                                    {
                                        JOtCode.SetAttribute("value", "GC");
                                    }
                                }
                                else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "LAN" || otherJobType.JobWorkType.Description.ToLower() == "landscape"))
                                {
                                    sOtDescriptionCode1.SetAttribute("value", "LAN");
                                    HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                    if (JOtCode != null)
                                    {
                                        JOtCode.SetAttribute("value", "LAN");
                                    }
                                }
                                else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "PART" || otherJobType.JobWorkType.Description.ToLower() == "Partitions"))
                                {
                                    sOtDescriptionCode1.SetAttribute("value", "PART");
                                    HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                    if (JOtCode != null)
                                    {
                                        JOtCode.SetAttribute("value", "PART");
                                    }
                                }
                                else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "STRU" || otherJobType.JobWorkType.Description.ToLower() == "Structural".ToLower()))
                                {
                                    sOtDescriptionCode1.SetAttribute("value", "STRU");
                                    HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                    if (JOtCode != null)
                                    {
                                        JOtCode.SetAttribute("value", "STRU");
                                    }

                                    HtmlElement sOTCost = wbEfiling.Document.GetElementById("sOTCost");
                                    if (sOTCost != null)
                                    {
                                        sOTCost.InnerText = Convert.ToString(otherJobType.EstimatedCost);
                                    }
                                }
                                else
                                {
                                    sOtDescriptionCode1.SetAttribute("value", "OT");
                                    HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                    if (JOtCode != null)
                                    {
                                        JOtCode.SetAttribute("value", "OT");
                                    }

                                    sOtDescriptionCode1.InvokeMember("change");

                                    HtmlElement OtherWorkTypeDescForm = wbEfiling.Document.GetElementById("OtherWorkTypeDescForm");
                                    if (OtherWorkTypeDescForm != null)
                                    {
                                        OtherWorkTypeDescForm.Style = "display: inline;";
                                    }

                                    HtmlElement JOTDescription = wbEfiling.Document.GetElementById("JOTDescription");
                                    if (JOTDescription != null)
                                    {
                                        JOTDescription.InnerText = otherJobType.JobWorkType != null ? Convert.ToString(otherJobType.JobWorkType.Description) : string.Empty;
                                    }

                                    HtmlElement sOTCost = wbEfiling.Document.GetElementById("sOTCost");
                                    if (sOTCost != null)
                                    {
                                        sOTCost.InnerText = Convert.ToString(otherJobType.EstimatedCost);
                                    }
                                }
                            }
                            else
                            {
                                HtmlElement sOtDescriptionCode = wbEfiling.Document.GetElementById("sOtDescriptionCode");
                                if (sOtDescriptionCode != null)
                                {
                                    if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "BPP" || otherJobType.JobWorkType.Description.ToLower() == "builders pavement plan"))
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "BPP");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "BPP");
                                        }

                                        HtmlElement jStreetFrontageA = wbEfiling.Document.GetElementById("JStreetFrontageA");
                                        if (jStreetFrontageA != null)
                                        {
                                            jStreetFrontageA.InnerText = Convert.ToString(otherJobType.EstimatedCost);
                                        }
                                    }
                                    else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "FAC" || otherJobType.JobWorkType.Description.ToLower() == "facade repair"))
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "FAC");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "FAC");
                                        }
                                    }

                                    else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "FPP" || otherJobType.JobWorkType.Description.ToLower() == "fire protection plan"))
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "FPP");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "FPP");
                                        }
                                    }
                                    else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "GC" || otherJobType.JobWorkType.Description.ToLower() == "general construction"))
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "GC");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "GC");
                                        }
                                    }
                                    else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "MARQUEE" || otherJobType.JobWorkType.Description.ToLower() == "marquee"))
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "MAR");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "MAR");
                                        }
                                    }
                                    else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "LAN" || otherJobType.JobWorkType.Description.ToLower() == "landscape"))
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "LAN");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "LAN");
                                        }
                                    }
                                    else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "PART" || otherJobType.JobWorkType.Description.ToLower() == "Partitions"))
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "PART");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "PART");
                                        }
                                    }
                                    else if (otherJobType.Code != null && otherJobType.JobWorkType != null && (otherJobType.Code.ToUpper() == "STRU" || otherJobType.JobWorkType.Description.ToLower() == "Structural".ToLower()))
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "STRU");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "STRU");
                                        }

                                        HtmlElement sOTCost = wbEfiling.Document.GetElementById("sOTCost");
                                        if (sOTCost != null)
                                        {
                                            sOTCost.InnerText = Convert.ToString(otherJobType.EstimatedCost);
                                        }
                                    }
                                    else
                                    {
                                        sOtDescriptionCode.SetAttribute("value", "OT");
                                        HtmlElement JOtCode = wbEfiling.Document.GetElementById("JOtCode");
                                        if (JOtCode != null)
                                        {
                                            JOtCode.SetAttribute("value", "OT");
                                        }

                                        sOtDescriptionCode.InvokeMember("change");

                                        HtmlElement OtherWorkTypeDescForm = wbEfiling.Document.GetElementById("OtherWorkTypeDescForm");
                                        if (OtherWorkTypeDescForm != null)
                                        {
                                            OtherWorkTypeDescForm.Style = "display: inline;";
                                        }

                                        HtmlElement JOTDescription = wbEfiling.Document.GetElementById("JOTDescription");
                                        if (JOTDescription != null)
                                        {
                                            JOTDescription.InnerText = otherJobType.JobWorkType != null ? Convert.ToString(otherJobType.JobWorkType.Description) : string.Empty;
                                        }

                                        HtmlElement sOTCost = wbEfiling.Document.GetElementById("sOTCost");
                                        if (sOTCost != null)
                                        {
                                            sOTCost.InnerText = Convert.ToString(otherJobType.EstimatedCost);
                                        }
                                    }
                                }
                            }
                           
                        }


                    }
                }

                double totalEstimatedCost = 0;
                if (jobApplicationWorkPermitTypeList != null)
                {
                    totalEstimatedCost = jobApplicationWorkPermitTypeList.Sum(x => x.EstimatedCost ?? 0);
                }

                HtmlElement JJobEstimCostA = wbEfiling.Document.GetElementById("JJobEstimCostA");
                if (JJobEstimCostA != null)
                {
                    JJobEstimCostA.InnerText = totalEstimatedCost != null ? Convert.ToString(totalEstimatedCost) : string.Empty;
                }

                Thread.Sleep(5000);
                HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                if (elements != null && elements.Count > 0)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        HtmlElement el = elements[i];
                        string elvalue = el.GetAttribute("value");
                        string elType = el.GetAttribute("type");

                        switch (elType)
                        {
                            case "radio":
                            case "checkbox":
                            case "button":
                                {
                                    if (elvalue == "Next >")
                                    {
                                        var abc = el.InvokeMember("click");
                                    }
                                    if (elvalue == "Continue")
                                    {
                                        var abc = el.InvokeMember("click");
                                    }
                                    break;
                                }
                        }
                    }
                }

                CompletedStep = 2;
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
        }

        private void FillThirdStepData()
        {
            try
            {
                #region Considerations

                HtmlElement localLawFlag = wbEfiling.Document.GetElementById("LocalLawFlag[1]");
                if (localLawFlag != null)
                {
                    localLawFlag.SetAttribute("checked", "checked");
                }

                HtmlElement jRestrDeclFlag = wbEfiling.Document.GetElementById("JRestrDeclFlag[1]");
                if (jRestrDeclFlag != null)
                {
                    jRestrDeclFlag.SetAttribute("checked", "checked");
                }

                HtmlElement JZoneExRecFlag = wbEfiling.Document.GetElementById("JZoneExRecFlag[1]");
                if (JZoneExRecFlag != null)
                {
                    JZoneExRecFlag.SetAttribute("checked", "checked");
                }

                HtmlElement OtherConsiderFlag = wbEfiling.Document.GetElementById("OtherConsiderFlag[1]");
                if (OtherConsiderFlag != null)
                {
                    OtherConsiderFlag.SetAttribute("checked", "checked");
                }
                HtmlElement HrtNoFlag = wbEfiling.Document.GetElementById("HrtNoFlag[1]");

                if (HrtNoFlag != null)
                {
                    HrtNoFlag.SetAttribute("checked", "checked");
                }

                HtmlElement CpcCalNumberFlag = wbEfiling.Document.GetElementById("CpcCalNumberFlag[1]");
                if (CpcCalNumberFlag != null)
                {
                    CpcCalNumberFlag.SetAttribute("checked", "checked");
                }

                HtmlElement BsaCalNumberFlag = wbEfiling.Document.GetElementById("BsaCalNumberFlag[1]");
                if (BsaCalNumberFlag != null)
                {
                    BsaCalNumberFlag.SetAttribute("checked", "checked");
                }

                HtmlElement JRemoveViolations = wbEfiling.Document.GetElementById("JRemoveViolations[1]");
                if (JRemoveViolations != null)
                {
                    JRemoveViolations.SetAttribute("checked", "checked");
                }
                HtmlElement JUnmappedFlag = wbEfiling.Document.GetElementById("JUnmappedFlag[1]");
                if (JUnmappedFlag != null)
                {
                    JUnmappedFlag.SetAttribute("checked", "checked");
                }
                HtmlElement JAdultEstab = wbEfiling.Document.GetElementById("JAdultEstab[1]");
                if (JAdultEstab != null)
                {
                    JAdultEstab.SetAttribute("checked", "checked");
                }
                HtmlElement JInclusionaryHousing = wbEfiling.Document.GetElementById("JInclusionaryHousing[1]");
                if (JInclusionaryHousing != null)
                {
                    JInclusionaryHousing.SetAttribute("checked", "checked");
                }
                HtmlElement JLowIncomeHousing = wbEfiling.Document.GetElementById("JLowIncomeHousing[1]");
                if (JLowIncomeHousing != null)
                {
                    JLowIncomeHousing.SetAttribute("checked", "checked");
                }
                HtmlElement JSRO = wbEfiling.Document.GetElementById("JSRO[1]");
                if (JSRO != null)
                {
                    JSRO.SetAttribute("checked", "checked");
                }
                HtmlElement JLmcccFlag = wbEfiling.Document.GetElementById("JLmcccFlag[1]");
                if (JLmcccFlag != null)
                {
                    JLmcccFlag.SetAttribute("checked", "checked");
                }
                HtmlElement JQualityHousing = wbEfiling.Document.GetElementById("JQualityHousing[1]");
                if (JQualityHousing != null)
                {
                    JQualityHousing.SetAttribute("checked", "checked");
                }
                HtmlElement JSiteSafety = wbEfiling.Document.GetElementById("JSiteSafety[1]");
                if (JSiteSafety != null)
                {
                    JSiteSafety.SetAttribute("checked", "checked");
                }

                HtmlElement JStructStability = wbEfiling.Document.GetElementById("JStructStability[1]");
                if (JStructStability != null)
                {
                    JStructStability.SetAttribute("checked", "checked");
                }
                HtmlElement JA1PartialDemo = wbEfiling.Document.GetElementById("JA1PartialDemo[1]");
                if (JA1PartialDemo != null)
                {
                    JA1PartialDemo.SetAttribute("checked", "checked");
                }

                HtmlElement JModularNYS = wbEfiling.Document.GetElementById("JModularNYS[1]");
                if (JModularNYS != null)
                {
                    JModularNYS.SetAttribute("checked", "checked");
                }

                HtmlElement JModularNYC = wbEfiling.Document.GetElementById("JModularNYC[1]");
                if (JModularNYC != null)
                {
                    JModularNYC.SetAttribute("checked", "checked");
                }

                HtmlElement JLegalizationFlag = wbEfiling.Document.GetElementById("JLegalizationFlag[1]");
                if (JLegalizationFlag != null)
                {
                    JLegalizationFlag.SetAttribute("checked", "checked");
                }

                HtmlElement JLightingFixtures = wbEfiling.Document.GetElementById("JLightingFixtures[1]");
                if (JLightingFixtures != null)
                {
                    JLightingFixtures.SetAttribute("checked", "checked");
                }

                #endregion

                #region Plans-Construction Documents Submitted

                HtmlElement JPlansFiled = wbEfiling.Document.GetElementById("JPlansFiled[0]");
                if (JPlansFiled != null)
                {
                    JPlansFiled.SetAttribute("checked", "checked");
                }

                #endregion


                HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                if (elements != null && elements.Count > 0)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        HtmlElement el = elements[i];
                        string elvalue = el.GetAttribute("value");
                        string elType = el.GetAttribute("type");

                        switch (elType)
                        {
                            case "radio":
                            case "checkbox":
                            case "button":
                                {
                                    if (elvalue == "Next >")
                                    {
                                        var abc = el.InvokeMember("click");
                                    }
                                    break;
                                }
                        }
                    }
                }

                CompletedStep = 3;
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFourthStepData()
        {
            try
            {
                bool isOcupancyClassification20082014 = job != null && job.RfpAddress != null ? job.RfpAddress.IsOcupancyClassification20082014 : false;
                bool isConstructionClassification20082014 = job != null && job.RfpAddress != null ? job.RfpAddress.IsConstructionClassification20082014 : false;

                ConstructionClassification constructionClassification = job != null && job.RfpAddress != null && job.RfpAddress.ConstructionClassification != null ? job.RfpAddress.ConstructionClassification : null;
                OccupancyClassification occupancyClassification = job != null && job.RfpAddress != null && job.RfpAddress.OccupancyClassification != null ? job.RfpAddress.OccupancyClassification : null;
                MultipleDwellingClassification multipleDwellingClassification = job != null && job.RfpAddress != null && job.RfpAddress.MultipleDwellingClassification != null ? job.RfpAddress.MultipleDwellingClassification : null;
                SeismicDesignCategory seismicDesignCategory = job != null && job.RfpAddress != null && job.RfpAddress.SeismicDesignCategory != null ? job.RfpAddress.SeismicDesignCategory : null;
                StructureOccupancyCategory structureOccupancyCategory = job != null && job.RfpAddress != null && job.RfpAddress.StructureOccupancyCategory != null ? job.RfpAddress.StructureOccupancyCategory : null;

                string numberOfDwellingUnits = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.DwellingUnits) : string.Empty;
                string buildingHeight = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Feet) : string.Empty;
                string numberOfStories = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
                string streetLegalWidth = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.StreetLegalWidth) : string.Empty;

                bool isTidalWetland = job != null && job.RfpAddress != null ? job.RfpAddress.TidalWetlandsMapCheck : false;
                bool isFreshwaterWetlands = job != null && job.RfpAddress != null ? job.RfpAddress.FreshwaterWetlandsMapCheck : false;
                bool isCoastalErosionHazardArea = job != null && job.RfpAddress != null ? job.RfpAddress.CoastalErosionHazardAreaMapCheck : false;
                bool isSpecialFloodHazard = job != null && job.RfpAddress != null ? job.RfpAddress.SpecialFloodHazardAreaCheck : false;

                #region Building Characteristics

                if (isOcupancyClassification20082014)
                {
                    HtmlElement el = wbEfiling.Document.GetElementById("JExOccupYearTbl[0]");
                    if (el != null)
                    {
                        el.SetAttribute("checked", "checked");
                        el.InvokeMember("checked");
                    }
                }
                else
                {
                    HtmlElement el = wbEfiling.Document.GetElementById("JExOccupYearTbl[1]");
                    if (el != null)
                    {
                        el.SetAttribute("checked", "checked");
                        el.InvokeMember("checked");
                    }

                    //HtmlElement JPrOccupancyClassif = wbEfiling.Document.GetElementById("JPrOccupancyClassif");
                    //if (JPrOccupancyClassif != null)
                    //{
                    //    JPrOccupancyClassif.SetAttribute("value", occupancyClassification != null ? Convert.ToString(occupancyClassification.Code) : string.Empty);
                    //}

                    //HtmlElement JExOccupancyClassif = wbEfiling.Document.GetElementById("JExOccupancyClassif");
                    //if (JExOccupancyClassif != null)
                    //{
                    //    JExOccupancyClassif.SetAttribute("value", occupancyClassification != null ? Convert.ToString(occupancyClassification.Code) : string.Empty);
                    //}
                }

                //JExOccupancyClassif

                if (isConstructionClassification20082014)
                {
                    HtmlElement el = wbEfiling.Document.GetElementById("JExConstYearTbl[0]");
                    if (el != null)
                    {
                        el.SetAttribute("checked", "checked");
                        el.InvokeMember("click");
                        Thread.Sleep(500);
                    }
                    HtmlElement el_pr = wbEfiling.Document.GetElementById("JPrConstYearTbl[0]");
                    if (el_pr != null)
                    {
                        el_pr.SetAttribute("checked", "checked");
                        el_pr.InvokeMember("click");
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    HtmlElement el = wbEfiling.Document.GetElementById("JExConstYearTbl[1]");
                    if (el != null)
                    {
                        el.SetAttribute("checked", "checked");
                        el.InvokeMember("click");
                        Thread.Sleep(500);
                    }

                    HtmlElement el_pr = wbEfiling.Document.GetElementById("JPrConstYearTbl[1]");
                    if (el_pr != null)
                    {
                        el_pr.SetAttribute("checked", "checked");
                        el_pr.InvokeMember("click");
                        Thread.Sleep(500);
                    }
                }

                wbEfiling.Document.InvokeScript("setBehaviours");

                HtmlElement JPrMultDwellClass = wbEfiling.Document.GetElementById("JPrMultDwellClass");
                if (JPrMultDwellClass != null)
                {
                    JPrMultDwellClass.SetAttribute("value", multipleDwellingClassification != null ? Convert.ToString(multipleDwellingClassification.Code) : string.Empty);
                }

                HtmlElement JExMultDwellClass = wbEfiling.Document.GetElementById("JExMultDwellClass");
                if (JExMultDwellClass != null)
                {
                    JExMultDwellClass.SetAttribute("value", multipleDwellingClassification != null ? Convert.ToString(multipleDwellingClassification.Code) : string.Empty);
                }

                HtmlElement JPrSeismicCategory = wbEfiling.Document.GetElementById("JPrSeismicCategory");
                if (JPrSeismicCategory != null)
                {
                    JPrSeismicCategory.SetAttribute("value", seismicDesignCategory != null ? Convert.ToString(seismicDesignCategory.Code) : string.Empty);
                }

                HtmlElement JPrStructuralCategory = wbEfiling.Document.GetElementById("JPrStructuralCategory");
                if (JPrStructuralCategory != null)
                {
                    JPrStructuralCategory.SetAttribute("value", structureOccupancyCategory != null ? Convert.ToString(structureOccupancyCategory.Code) : string.Empty);
                }

                HtmlElement JPrDwellingUnits = wbEfiling.Document.GetElementById("JPrDwellingUnits");
                if (JPrDwellingUnits != null)
                {
                    JPrDwellingUnits.InnerText = numberOfDwellingUnits;
                }

                HtmlElement JExDwellingUnits = wbEfiling.Document.GetElementById("JExDwellingUnits");
                if (JExDwellingUnits != null)
                {
                    JExDwellingUnits.InnerText = numberOfDwellingUnits;
                }

                HtmlElement JPrHeightA = wbEfiling.Document.GetElementById("JPrHeightA");
                if (JPrHeightA != null)
                {
                    JPrHeightA.InnerText = buildingHeight;
                }


                HtmlElement JExHeightA = wbEfiling.Document.GetElementById("JExHeightA");
                if (JExHeightA != null)
                {
                    JExHeightA.InnerText = buildingHeight;
                }


                HtmlElement JPrStoriesA = wbEfiling.Document.GetElementById("JPrStoriesA");
                if (JPrStoriesA != null)
                {
                    JPrStoriesA.InnerText = numberOfStories;
                }
                HtmlElement JExStoriesA = wbEfiling.Document.GetElementById("JExStoriesA");
                if (JExStoriesA != null)
                {
                    JExStoriesA.InnerText = numberOfStories;
                }
                #endregion

                #region Site Characteristics

                if (isTidalWetland)
                {
                    HtmlElement JTidalWetlands_0 = wbEfiling.Document.GetElementById("JTidalWetlands[0]");
                    HtmlElement JTidalWetlands_1 = wbEfiling.Document.GetElementById("JTidalWetlands[1]");
                    if (JTidalWetlands_0 != null)
                    {
                        JTidalWetlands_0.SetAttribute("checked", "checked");
                    }
                    if (JTidalWetlands_1 != null)
                    {
                        JTidalWetlands_1.SetAttribute("checked", "");
                    }
                }
                else
                {
                    HtmlElement JTidalWetlands_0 = wbEfiling.Document.GetElementById("JTidalWetlands[0]");
                    HtmlElement JTidalWetlands_1 = wbEfiling.Document.GetElementById("JTidalWetlands[1]");
                    if (JTidalWetlands_1 != null)
                    {
                        JTidalWetlands_1.SetAttribute("checked", "checked");
                    }

                    if (JTidalWetlands_0 != null)
                    {
                        JTidalWetlands_0.SetAttribute("checked", "");
                    }
                }

                if (isFreshwaterWetlands)
                {
                    HtmlElement JFreshWetlands_0 = wbEfiling.Document.GetElementById("JFreshWetlands[0]");
                    HtmlElement JFreshWetlands_1 = wbEfiling.Document.GetElementById("JFreshWetlands[1]");
                    if (JFreshWetlands_0 != null)
                    {
                        JFreshWetlands_0.SetAttribute("checked", "checked");
                    }

                    if (JFreshWetlands_1 != null)
                    {
                        JFreshWetlands_1.SetAttribute("checked", "");
                    }
                }
                else
                {
                    HtmlElement JFreshWetlands_0 = wbEfiling.Document.GetElementById("JFreshWetlands[0]");
                    HtmlElement JFreshWetlands_1 = wbEfiling.Document.GetElementById("JFreshWetlands[1]");
                    if (JFreshWetlands_0 != null)
                    {
                        JFreshWetlands_0.SetAttribute("checked", "");
                    }
                    if (JFreshWetlands_1 != null)
                    {
                        JFreshWetlands_1.SetAttribute("checked", "checked");
                    }
                }

                if (isCoastalErosionHazardArea)
                {
                    HtmlElement JCoastalErosion_0 = wbEfiling.Document.GetElementById("JCoastalErosion[0]");
                    HtmlElement JCoastalErosion_1 = wbEfiling.Document.GetElementById("JCoastalErosion[1]");
                    if (JCoastalErosion_0 != null)
                    {
                        JCoastalErosion_0.SetAttribute("checked", "checked");
                    }
                    if (JCoastalErosion_1 != null)
                    {
                        JCoastalErosion_1.SetAttribute("checked", "");
                    }
                }
                else
                {
                    HtmlElement JCoastalErosion_0 = wbEfiling.Document.GetElementById("JCoastalErosion[0]");
                    HtmlElement JCoastalErosion_1 = wbEfiling.Document.GetElementById("JCoastalErosion[1]");
                    if (JCoastalErosion_0 != null)
                    {
                        JCoastalErosion_0.SetAttribute("checked", "");
                    }
                    if (JCoastalErosion_1 != null)
                    {
                        JCoastalErosion_1.SetAttribute("checked", "checked");
                    }
                }

                HtmlElement JUrbanRenewal_0 = wbEfiling.Document.GetElementById("JUrbanRenewal[0]");
                HtmlElement JUrbanRenewal_1 = wbEfiling.Document.GetElementById("JUrbanRenewal[1]");
                if (JUrbanRenewal_0 != null)
                {
                    JUrbanRenewal_0.SetAttribute("checked", "");
                }
                if (JUrbanRenewal_1 != null)
                {
                    JUrbanRenewal_1.SetAttribute("checked", "checked");
                }

                HtmlElement JFireDist_0 = wbEfiling.Document.GetElementById("JFireDist[0]");
                HtmlElement JFireDist_1 = wbEfiling.Document.GetElementById("JFireDist[1]");

                if (JFireDist_0 != null)
                {
                    JFireDist_0.SetAttribute("checked", "checked");
                }
                if (JFireDist_1 != null)
                {
                    JFireDist_1.SetAttribute("checked", "");
                }

                if (isSpecialFloodHazard)
                {
                    HtmlElement JFloodHazard_0 = wbEfiling.Document.GetElementById("JFloodHazard[0]");
                    HtmlElement JFloodHazard_1 = wbEfiling.Document.GetElementById("JFloodHazard[1]");
                    if (JFloodHazard_0 != null)
                    {
                        JFloodHazard_0.SetAttribute("checked", "checked");
                    }
                    if (JFloodHazard_1 != null)
                    {
                        JFloodHazard_1.SetAttribute("checked", "");
                    }
                }
                else
                {
                    HtmlElement JFloodHazard_0 = wbEfiling.Document.GetElementById("JFloodHazard[0]");
                    HtmlElement JFloodHazard_1 = wbEfiling.Document.GetElementById("JFloodHazard[1]");
                    if (JFloodHazard_0 != null)
                    {
                        JFloodHazard_0.SetAttribute("checked", "");
                    }

                    if (JFloodHazard_1 != null)
                    {
                        JFloodHazard_1.SetAttribute("checked", "checked");
                    }
                }

                #endregion

                #region Zoning & Property Information

                HtmlElement el_Zoning_Yes = wbEfiling.Document.GetElementById("JZoningChallengeFlag[0]");
                if (el_Zoning_Yes != null)
                {
                    el_Zoning_Yes.SetAttribute("checked", "checked");
                    el_Zoning_Yes.InvokeMember("checked");
                }


                HtmlElement el_Zoning_No = wbEfiling.Document.GetElementById("JZoningChallengeFlag[1]");
                if (el_Zoning_No != null)
                {
                    el_Zoning_No.SetAttribute("checked", "");
                }

                string zoningMap = job != null && job.RfpAddress != null ? job.RfpAddress.Map : string.Empty;

                HtmlElement JMapNumber = wbEfiling.Document.GetElementById("JMapNumber");
                if (JMapNumber != null)
                {
                    JMapNumber.InnerText = zoningMap;
                }

                bool isLittleE = job != null && job.RfpAddress != null ? job.RfpAddress.IsLittleE : false;

                if (isLittleE)
                {
                    HtmlElement JLittleE_0 = wbEfiling.Document.GetElementById("JLittleE[0]");
                    HtmlElement JLittleE_1 = wbEfiling.Document.GetElementById("JLittleE[1]");
                    if (JLittleE_0 != null)
                    {
                        JLittleE_0.SetAttribute("checked", "checked");
                    }

                    if (JLittleE_1 != null)
                    {
                        JLittleE_1.SetAttribute("checked", "");
                    }
                }
                else
                {
                    HtmlElement JLittleE_0 = wbEfiling.Document.GetElementById("JLittleE[0]");
                    HtmlElement JLittleE_1 = wbEfiling.Document.GetElementById("JLittleE[1]");
                    if (JLittleE_0 != null)
                    {
                        JLittleE_0.SetAttribute("checked", "");
                    }
                    if (JLittleE_1 != null)
                    {
                        JLittleE_1.SetAttribute("checked", "checked");
                    }
                }

                HtmlElement JLoftBoard_0 = wbEfiling.Document.GetElementById("JLoftBoard[0]");
                HtmlElement JLoftBoard_1 = wbEfiling.Document.GetElementById("JLoftBoard[1]");

                if (JLoftBoard_0 != null)
                {
                    JLoftBoard_0.SetAttribute("checked", "");
                }
                if (JLoftBoard_1 != null)
                {
                    JLoftBoard_1.SetAttribute("checked", "checked");
                }

                HtmlElement JPlotLegalWidthA = wbEfiling.Document.GetElementById("JPlotLegalWidthA");
                if (JPlotLegalWidthA != null)
                {
                    JPlotLegalWidthA.InnerText = streetLegalWidth;
                }

                #endregion

                #region Tax Lot Characteristics            

                HtmlElement JLotMerger_0 = wbEfiling.Document.GetElementById("JLotMerger[0]");
                HtmlElement JLotMerger_1 = wbEfiling.Document.GetElementById("JLotMerger[1]");

                if (JLotMerger_0 != null)
                {
                    JLotMerger_0.SetAttribute("checked", "");
                }

                if (JLotMerger_1 != null)
                {
                    JLotMerger_1.SetAttribute("checked", "checked");
                }

                #endregion

                HtmlElement JPrOccupancyClassif = wbEfiling.Document.GetElementById("JPrOccupancyClassif");
                if (JPrOccupancyClassif != null)
                {
                    JPrOccupancyClassif.SetAttribute("value", occupancyClassification != null ? Convert.ToString(occupancyClassification.Code) : string.Empty);
                }

                HtmlElement JExOccupancyClassif = wbEfiling.Document.GetElementById("JExOccupancyClassif");
                if (JExOccupancyClassif != null)
                {
                    JExOccupancyClassif.SetAttribute("value", occupancyClassification != null ? Convert.ToString(occupancyClassification.Code) : string.Empty);
                }

                HtmlElement JPrConstClassif = wbEfiling.Document.GetElementById("JPrConstClassif");
                if (JPrConstClassif != null)
                {
                    JPrConstClassif.SetAttribute("value", constructionClassification != null ? Convert.ToString(constructionClassification.Code) : string.Empty);
                }

                HtmlElement JExConstClassif = wbEfiling.Document.GetElementById("JExConstClassif");
                if (JExConstClassif != null)
                {
                    JExConstClassif.SetAttribute("value", constructionClassification != null ? Convert.ToString(constructionClassification.Code) : string.Empty);
                }

                HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                if (elements != null && elements.Count > 0)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        HtmlElement el = elements[i];
                        string elvalue = el.GetAttribute("value");
                        string elType = el.GetAttribute("type");
                        switch (elType)
                        {
                            case "radio":
                            case "checkbox":
                            case "button":
                                {
                                    if (elvalue == "Next >")
                                    {
                                        var abc = el.InvokeMember("click");
                                    }
                                    break;
                                }
                        }
                    }
                }

                CompletedStep = 4;
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFifthStepData()
        {
            try
            {
                //wbEfiling.Document.GetElementById("jcomtext").InnerText = workDescription;

                HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                if (elements != null && elements.Count > 0)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        HtmlElement el = elements[i];
                        string elvalue = el.GetAttribute("value");
                        string elType = el.GetAttribute("type");
                        switch (elType)
                        {
                            case "radio":
                            case "checkbox":
                            case "button":
                                {
                                    if (elvalue == " < Prev")
                                    {
                                        var abc = el.InvokeMember("click");
                                    }
                                    break;
                                }
                        }
                    }
                }

                CompletedStep = 5;
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFourthStepAfterFifthData()
        {
            try
            {
                ConstructionClassification constructionClassification = job != null && job.RfpAddress != null && job.RfpAddress.ConstructionClassification != null ? job.RfpAddress.ConstructionClassification : null;
                OccupancyClassification occupancyClassification = job != null && job.RfpAddress != null && job.RfpAddress.OccupancyClassification != null ? job.RfpAddress.OccupancyClassification : null;

                #region Building Characteristics

                HtmlElement JPrOccupancyClassif = wbEfiling.Document.GetElementById("JPrOccupancyClassif");
                if (JPrOccupancyClassif != null)
                {
                    JPrOccupancyClassif.SetAttribute("value", occupancyClassification != null ? Convert.ToString(occupancyClassification.Code) : string.Empty);
                }

                HtmlElement JExOccupancyClassif = wbEfiling.Document.GetElementById("JExOccupancyClassif");
                if (JExOccupancyClassif != null)
                {
                    JExOccupancyClassif.SetAttribute("value", occupancyClassification != null ? Convert.ToString(occupancyClassification.Code) : string.Empty);
                }

                HtmlElement JPrConstClassif = wbEfiling.Document.GetElementById("JPrConstClassif");
                if (JPrConstClassif != null)
                {
                    JPrConstClassif.SetAttribute("value", constructionClassification != null ? Convert.ToString(constructionClassification.Code) : string.Empty);
                }

                HtmlElement JExConstClassif = wbEfiling.Document.GetElementById("JExConstClassif");
                if (JExConstClassif != null)
                {
                    JExConstClassif.SetAttribute("value", constructionClassification != null ? Convert.ToString(constructionClassification.Code) : string.Empty);
                }

                #endregion

                HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                if (elements != null && elements.Count > 0)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        HtmlElement el = elements[i];
                        string elvalue = el.GetAttribute("value");
                        string elType = el.GetAttribute("type");

                        switch (elType)
                        {
                            case "radio":
                            case "checkbox":
                            case "button":
                                {
                                    if (elvalue == "Next >")
                                    {
                                        var abc = el.InvokeMember("click");
                                    }
                                    break;
                                }
                        }
                    }
                }

                CompletedStep = 6;
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFifthStepAfterRefillData()
        {
            try
            {
                //wbEfiling.Document.GetElementById("jcomtext").InnerText = workDescription;

                //HtmlElementCollection elements = wbEfiling.Document.GetElementsByTagName("input");
                //if (elements != null && elements.Count > 0)
                //{
                //    for (int i = 0; i < elements.Count; i++)
                //    {
                //        HtmlElement el = elements[i];
                //        string elvalue = el.GetAttribute("value");
                //        string elType = el.GetAttribute("type");

                //        switch (elType)
                //        {
                //            case "radio":
                //            case "checkbox":
                //            case "button":
                //                {
                //                    if (elvalue == "Next >")
                //                    {
                //                        var abc = el.InvokeMember("click");
                //                    }
                //                    break;
                //                }
                //        }
                //    }
                //}

                CompletedStep = 7;
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseProgress();
            }

        }

        #endregion

        public class JobDocumentItem
        {
            public int Id { get; set; }
            public string DocumentName { get; set; }
        }

        private void ddlJobDocument_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                lblDocumentDetails.Text = string.Empty;
                lblDocument.Text = string.Empty;
                lblApplicationType.Text = string.Empty;
                if (ddlJobDocument.SelectedValue != null && ddlJobDocument.SelectedValue != string.Empty)
                {
                    ShowProgress();
                    RpoContext rpoContext = new RpoContext();
                    int idJobDocument = Convert.ToInt32(ddlJobDocument.SelectedValue);
                    if (idJobDocument > 0)
                    {
                        jobDocument = rpoContext.JobDocuments.Include("JobApplication.JobApplicationType").FirstOrDefault(x => x.Id == idJobDocument);
                        string applicationNumber = jobDocument != null && jobDocument.JobApplication != null ? jobDocument.JobApplication.ApplicationNumber : string.Empty;
                        string applicationType = jobDocument != null && jobDocument.JobApplication != null && jobDocument.JobApplication.JobApplicationType != null ? jobDocument.JobApplication.JobApplicationType.Description : string.Empty;
                        string documentDescription = jobDocument != null && jobDocument.DocumentDescription != null ? jobDocument.DocumentDescription : string.Empty;

                        lblDocumentDetails.Text = applicationNumber;
                        lblApplicationType.Text = applicationType;
                        lblDocument.Text = documentDescription;
                        btnAttachEF1.Enabled = true;
                        btnFillInfo.Enabled = true;
                    }
                    else
                    {
                        lblDocumentDetails.Text = string.Empty;
                        lblApplicationType.Text = string.Empty;
                        lblDocument.Text = string.Empty;
                        btnAttachEF1.Enabled = false;
                        btnFillInfo.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CloseProgress();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseProgress();
            }
        }

        private void btnHomePage_Click(object sender, EventArgs e)
        {
            this.wbEfiling.Navigate("https://a810-efiling.nyc.gov/eRenewal/Landing");
        }

        private void wbEfiling_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            //StartProgressbar();
            // ShowProgressbar();
            // CloseProgressbar();
            try
            {
                progressBar1.Maximum = (int)e.MaximumProgress;
                if (e.CurrentProgress > 0)
                    progressBar1.Value = (int)e.CurrentProgress;
            }
            catch (Exception)
            {
                               
            }
           
        }


        #region Progressbar
        private void StartProgressbar()
        {
            Cursor = Cursors.WaitCursor;
            objfrmShowProgress = new ModalLoadingUI();
            ShowProgressbar();
        }
        private void CloseProgressbar()
        {
            Thread.Sleep(200);
            try
            {
                if (objfrmShowProgress != null)
                {
                    objfrmShowProgress.Invoke(new Action(objfrmShowProgress.Close));
                }
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowProgressbar()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    try
                    {
                        objfrmShowProgress.ShowDialog();
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    Thread th = new Thread(ShowProgressbar);
                    th.IsBackground = false;
                    th.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
