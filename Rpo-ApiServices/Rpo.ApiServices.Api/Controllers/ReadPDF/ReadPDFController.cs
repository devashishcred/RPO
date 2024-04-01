using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rpo.ApiServices.Model;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.ReadPDF
{
    public class ReadPDFController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        [HttpGet]
        public IHttpActionResult Index(int idJobDocument)
        {
            var path = HttpRuntime.AppDomainAppPath;
            string directoryName = string.Empty;
            string newFileDirectoryName = string.Empty;
            string fileName = string.Empty;

            List <JobDocumentField> jobDocumentField = rpoContext.JobDocumentFields.Include("JobDocument").Include("DocumentField.Field").Where(x => x.IdJobDocument == idJobDocument).ToList();

            if (jobDocumentField != null && jobDocumentField.Count > 0)
            {
                fileName = jobDocumentField.Select(x=>x.JobDocument.DocumentPath).FirstOrDefault();
            }


            //var pdf_filename = "D:\\rpo document\\dropdown.pdf";


            directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentTemplatePath));
            newFileDirectoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.JobDocumentExportPath));

            
            //string directoryFileName = Convert.ToString(jobTransmittalAttachment.Id) + "_" + thisFileName;

            var pdf_filename = directoryName + fileName;

            using (var reader = new PdfReader(pdf_filename))
            {
                var fields = reader.AcroFields.Fields;
                string newfile = newFileDirectoryName + "Test.pdf";

                var pdfStamper = new PdfStamper(reader, new FileStream(newfile, FileMode.Create));

                var field = pdfStamper.AcroFields;

                foreach (var item in jobDocumentField)
                {

                    if (item.DocumentField != null && item.DocumentField.PdfFields != null)
                    {
                        string[] fieldsList = item.DocumentField.PdfFields.Split(',');

                        if (fieldsList != null && fieldsList.Count() > 0)
                        {
                            foreach (string pdfField in fieldsList)
                            {
                                field.SetField(pdfField, item.Value);
                            }
                        }

                    }
                    //if("Text1" == item.DocumentField.Field.FieldName)
                    //field.SetField(item.DocumentField.Field.FieldName, item.Value);
                }


                //field.SetField("Text1", jobDocumentField[0].Value);
                //field.SetField("Text2", jobDocumentField[1].Value);
                //field.SetField("Text3", jobDocumentField[2].Value);
                //field.SetField("Text4", jobDocumentField[3].Value);

                //pdfStamper.FormFlattening = true;
                pdfStamper.Close();

                foreach (var key in fields.Keys)
                {
                    var value = reader.AcroFields.GetField(key);

                    Console.WriteLine(key + " : " + value);
                }
            }


            //using (var reader = new PdfReader(@"d:\a.pdf"))
            //{
            //    using (var fileStream = new FileStream(@"C:\Output.pdf", FileMode.Create, FileAccess.Write))
            //    {
            //        var document = new Document(reader.GetPageSizeWithRotation(1));
            //        var writer = PdfWriter.GetInstance(document, fileStream);

            //        document.Open();

            //        for (var i = 1; i <= reader.NumberOfPages; i++)
            //        {
            //            document.NewPage();

            //            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //            var importedPage = writer.GetImportedPage(reader, i);

            //            var contentByte = writer.DirectContent;
            //            contentByte.BeginText();
            //            contentByte.SetFontAndSize(baseFont, 12);

            //            var multiLineString = "Hello,\r\nWorld!".Split('\n');

            //            foreach (var line in multiLineString)
            //            {
            //                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, line, 200, 200, 0);
            //            }

            //            contentByte.EndText();
            //            contentByte.AddTemplate(importedPage, 0, 0);
            //        }

            //        document.Close();
            //        writer.Close();
            //    }
            //}






            return Ok();
        }
    }
}
