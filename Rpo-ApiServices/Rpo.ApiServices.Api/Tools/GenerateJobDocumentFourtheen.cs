using System;
using System.Data.Entity;
using System.Linq;
using Rpo.ApiServices.Api.Controllers.JobDocument;
using Rpo.ApiServices.Model;
using Rpo.ApiServices.Model.Models;
using System.Collections.Generic;
using Rpo.ApiServices.Api.Enums;
using System.Web;
using iTextSharp.text.pdf;

namespace Rpo.ApiServices.Api.Tools
{
    public partial class GenerateJobDocuments
    {
        public static void GenerateCertificateofOccupancyWorksheet2022(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ToList();
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
            string applicantName = string.Empty;
            string applicationNumber = string.Empty;

            var keyvalue = new List<KeyValuePair<string, string>>();
            string docuementType = string.Empty;

            string opgRequestType = string.Empty;
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();


                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);

                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtBIN")
                        {
                            string binNumber = job != null && job.RfpAddress != null ? job.RfpAddress.BinNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = binNumber,
                                    ActualValue = binNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = binNumber;
                                updateJobDocumentField.ActualValue = binNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAddress")
                        {
                            string jobRFPAddress = string.Empty;
                            jobRFPAddress = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ", " + job.RfpAddress.Borough.Description : string.Empty) + (job.RfpAddress.ZipCode != null ? ", " + job.RfpAddress.ZipCode : string.Empty) : string.Empty;


                            string fulladdr = Common.GetFulladdressforjob(job);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = jobRFPAddress,
                                    ActualValue = jobRFPAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = jobRFPAddress;
                                updateJobDocumentField.ActualValue = jobRFPAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgRequestType")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgRequestType,
                                    ActualValue = opgRequestType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgRequestType;
                                updateJobDocumentField.ActualValue = opgRequestType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDate")
                        {
                            DateTime time = DateTime.Now;
                            string format = "MM/dd/yyyy";
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = time.ToString(format),
                                    ActualValue = time.ToString(format)
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = time.ToString(format);
                                updateJobDocumentField.ActualValue = time.ToString(format); ;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "For")
                        {
                            jobDocumentFor = frontendFields.Value;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }

                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "ApplicationType")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);
                            string ApplicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                            //applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                            documentDescription = "Application Type: " + ApplicationType;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = ApplicationType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = ApplicationType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Type")
                        {
                            int certifierid = Convert.ToInt32(frontendFields.Value);
                            JobDocumentType jobDocumentType = rpoContext.JobDocumentTypes.FirstOrDefault(x => x.Id == certifierid);
                            docuementType = jobDocumentType != null ? jobDocumentType.Type : string.Empty;

                            documentDescription = (documentDescription) + (!string.IsNullOrEmpty(docuementType) ? " | Type: " + docuementType : string.Empty);


                            opgRequestType = string.Empty;

                            switch (docuementType)
                            {
                                case "Core & Shell":
                                    {
                                        opgRequestType = "1";
                                        break;
                                    }
                                case "TCO-Initial":
                                    {
                                        opgRequestType = "2";
                                        break;
                                    }
                                case "TCO Renewal with Change":
                                    {
                                        opgRequestType = "3";
                                        break;
                                    }
                                case "Final":
                                    {
                                        opgRequestType = "4";
                                        break;
                                    }
                            }


                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = docuementType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = docuementType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else
                        {

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value

                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }
            }

            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateJobDocument(idJobDocument);

        }

        public static void EditCertificateofOccupancyWorksheet2022(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ToList();
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
            string applicantName = string.Empty;
            string applicationNumber = string.Empty;
            var keyvalue = new List<KeyValuePair<string, string>>();
            string docuementType = string.Empty;
            string opgRequestType = string.Empty;
            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();


                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);

                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtBIN")
                        {
                            string binNumber = job != null && job.RfpAddress != null ? job.RfpAddress.BinNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = binNumber,
                                    ActualValue = binNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = binNumber;
                                updateJobDocumentField.ActualValue = binNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });

                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAddress")
                        {
                            string jobRFPAddress = string.Empty;
                            jobRFPAddress = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ", " + job.RfpAddress.Borough.Description : string.Empty) + (job.RfpAddress.ZipCode != null ? ", " + job.RfpAddress.ZipCode : string.Empty) : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = jobRFPAddress,
                                    ActualValue = jobRFPAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = jobRFPAddress;
                                updateJobDocumentField.ActualValue = jobRFPAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgRequestType")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgRequestType,
                                    ActualValue = opgRequestType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgRequestType;
                                updateJobDocumentField.ActualValue = opgRequestType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDate")
                        {
                            DateTime time = DateTime.Now;
                            string format = "MM/dd/yyyy";
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = time.ToString(format),
                                    ActualValue = time.ToString(format)
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = time.ToString(format);
                                updateJobDocumentField.ActualValue = time.ToString(format);
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "For")
                        {
                            jobDocumentFor = frontendFields.Value;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }

                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "ApplicationType")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);
                            string ApplicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                            //applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;

                            documentDescription = "Application Type: " + ApplicationType;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = ApplicationType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = ApplicationType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Type")
                        {
                            int certifierid = Convert.ToInt32(frontendFields.Value);
                            JobDocumentType jobDocumentType = rpoContext.JobDocumentTypes.FirstOrDefault(x => x.Id == certifierid);
                            docuementType = jobDocumentType != null ? jobDocumentType.Type : string.Empty;

                            documentDescription = (documentDescription) + (!string.IsNullOrEmpty(docuementType) ? " | Type: " + docuementType : string.Empty);


                            opgRequestType = string.Empty;

                            switch (docuementType)
                            {
                                case "Core & Shell":
                                    {
                                        opgRequestType = "1";
                                        break;
                                    }
                                case "TCO-Initial":
                                    {
                                        opgRequestType = "2";
                                        break;
                                    }
                                case "TCO Renewal with Change":
                                    {
                                        opgRequestType = "3";
                                        break;
                                    }
                                case "Final":
                                    {
                                        opgRequestType = "4";
                                        break;
                                    }
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = docuementType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = docuementType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else
                        {

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value

                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            List<string> newpdfString = pdfFields;
            List<int> newDocumentFieldId = DocumentFieldsIds;
            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateEditedJobDocument(idJobDocument, pdfFields, newDocumentFieldId, FieldsValue);

        }

        public static void GenerateWithdrawalRequestForm2023(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ToList();
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
            string Email = string.Empty;
            string licenceName = string.Empty;
            string applicationNumber = string.Empty;
            string chkWithdrawal = string.Empty;
            string contactName = string.Empty;
            JobDocumentType jobDocumentType = new JobDocumentType();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();


                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);

                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string jobRFPAddress = string.Empty;
                            jobRFPAddress = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ", " + job.RfpAddress.Borough.Description : string.Empty) + (job.RfpAddress.ZipCode != null ? ", " + job.RfpAddress.ZipCode : string.Empty) : string.Empty;


                            string fulladdr = Common.GetFulladdressforjob(job);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = jobRFPAddress,
                                    ActualValue = jobRFPAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = jobRFPAddress;
                                updateJobDocumentField.ActualValue = jobRFPAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDOBNow")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicationNumber,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicationNumber;
                                updateJobDocumentField.ActualValue = applicationNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwnerName")
                        {
                            Contact owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.MiddleName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerName,
                                    ActualValue = ownerName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerName;
                                updateJobDocumentField.ActualValue = ownerName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLicense")
                        {
                            string ApplicenceName = string.Empty;
                            if (jobDocumentType.Type.ToLower() == ("Withdrawal of Stakeholder").ToLower())
                            {
                                ApplicenceName = licenceName;
                            }
                            else
                                ApplicenceName = string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ApplicenceName,
                                    ActualValue = ApplicenceName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ApplicenceName;
                                updateJobDocumentField.ActualValue = ApplicenceName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtEmail")
                        {
                            string AppEmail = string.Empty;
                            if (jobDocumentType.Type.ToLower() == ("Withdrawal of Stakeholder").ToLower())
                            {
                                AppEmail = Email;
                            }
                            else
                                AppEmail = string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = AppEmail,
                                    ActualValue = AppEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = AppEmail;
                                updateJobDocumentField.ActualValue = AppEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkWithdrawal")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkWithdrawal,
                                    ActualValue = chkWithdrawal
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = chkWithdrawal;
                                updateJobDocumentField.ActualValue = chkWithdrawal;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtApplicantName2")
                        {
                            string ApplicantName = string.Empty;
                            if (jobDocumentType.Type.ToLower() == ("Withdrawal of Stakeholder").ToLower())
                            {
                                ApplicantName = contactName;
                            }
                            else
                                ApplicantName = string.Empty; ;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ApplicantName,
                                    ActualValue = ApplicantName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ApplicantName;
                                updateJobDocumentField.ActualValue = ApplicantName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDate")
                        {
                            DateTime time = DateTime.Now;
                            string format = "MM/dd/yyyy";
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = time.ToString(format),
                                    ActualValue = time.ToString(format)
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = time.ToString(format);
                                updateJobDocumentField.ActualValue = time.ToString(format); ;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "txtApplicantName")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);

                            JobContact jobContact = rpoContext.JobContacts.Where(x => x.Id == Certifierid).FirstOrDefault();

                            contactName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;

                            Email = jobContact != null && jobContact.Contact != null ? jobContact.Contact.Email : string.Empty;

                            ContactLicense contactLicNumber = jobContact != null ? jobContact.Contact.ContactLicenses.FirstOrDefault() : null;
                            licenceName = contactLicNumber != null ? contactLicNumber.Number : string.Empty;
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(contactName) ? "Applicant: " + contactName : string.Empty);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Type")
                        {
                            int applicantId = Convert.ToInt32(frontendFields.Value);
                            jobDocumentType = rpoContext.JobDocumentTypes.FirstOrDefault(x => x.Id == applicantId);
                            documentDescription = "Withdrawal Type: " + jobDocumentType.Type;
                            switch (jobDocumentType.Type)
                            {
                                case "Withdrawal of Stakeholder":
                                    chkWithdrawal = "2";
                                    break;
                                case "Withdrawal of Filing":
                                    chkWithdrawal = "1";
                                    break;
                                default:
                                    break;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }

                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "For")
                        {
                            jobDocumentFor = frontendFields.Value;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }

                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        
                        else if (item.Field.FieldName == "Application")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);
                            string ApplicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                            applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                            documentDescription = "Application Type: " + ApplicationType;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = ApplicationType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = ApplicationType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else
                        {

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value

                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }
            }

            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateJobDocument(idJobDocument);

        }

        public static void EditWithdrawalRequestForm2023(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ToList();
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
            string licenceName = string.Empty;
            string Email = string.Empty;
            string applicationNumber = string.Empty;
            string chkWithdrawal = string.Empty;
            string contactName = string.Empty;
            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();
            JobDocumentType jobDocumentType = new JobDocumentType();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();


                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);

                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string jobRFPAddress = string.Empty;
                            jobRFPAddress = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ", " + job.RfpAddress.Borough.Description : string.Empty) + (job.RfpAddress.ZipCode != null ? ", " + job.RfpAddress.ZipCode : string.Empty) : string.Empty;


                            string fulladdr = Common.GetFulladdressforjob(job);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = jobRFPAddress,
                                    ActualValue = jobRFPAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = jobRFPAddress;
                                updateJobDocumentField.ActualValue = jobRFPAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });

                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDOBNow")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicationNumber,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicationNumber;
                                updateJobDocumentField.ActualValue = applicationNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });

                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwnerName")
                        {
                            Contact owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.MiddleName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerName,
                                    ActualValue = ownerName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerName;
                                updateJobDocumentField.ActualValue = ownerName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLicense")
                        {
                            string ApplicenceName = string.Empty;
                            if (jobDocumentType.Type.ToLower() == ("Withdrawal of Stakeholder").ToLower())
                            {
                                ApplicenceName = licenceName;
                            }
                            else
                                ApplicenceName = string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ApplicenceName,
                                    ActualValue = ApplicenceName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ApplicenceName;
                                updateJobDocumentField.ActualValue = ApplicenceName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtEmail")
                        {
                            string AppEmail = string.Empty;
                            if (jobDocumentType.Type.ToLower() == ("Withdrawal of Stakeholder").ToLower())
                            {
                                AppEmail = Email;
                            }
                            else
                                AppEmail = string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = AppEmail,
                                    ActualValue = AppEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = AppEmail;
                                updateJobDocumentField.ActualValue = AppEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkWithdrawal")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkWithdrawal,
                                    ActualValue = chkWithdrawal
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = chkWithdrawal;
                                updateJobDocumentField.ActualValue = chkWithdrawal;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtApplicantName2")
                        {
                            string ApplicantName = string.Empty;
                            if (jobDocumentType.Type.ToLower() == ("Withdrawal of Stakeholder").ToLower())
                            {
                                ApplicantName = contactName;
                            }
                            else
                                ApplicantName = string.Empty; ;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ApplicantName,
                                    ActualValue = ApplicantName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ApplicantName;
                                updateJobDocumentField.ActualValue = ApplicantName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDate")
                        {
                            DateTime time = DateTime.Now;
                            string format = "MM/dd/yyyy";
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = time.ToString(format),
                                    ActualValue = time.ToString(format)
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = time.ToString(format);
                                updateJobDocumentField.ActualValue = time.ToString(format);
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();

                    }
                    else
                    {
                        if (item.Field.FieldName == "txtApplicantName")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);

                            JobContact jobContact = rpoContext.JobContacts.Where(x => x.Id == Certifierid).FirstOrDefault();

                            contactName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;

                            Email = jobContact != null && jobContact.Contact != null ? jobContact.Contact.Email : string.Empty;

                            ContactLicense contactLicNumber = jobContact != null ? jobContact.Contact.ContactLicenses.FirstOrDefault() : null;
                            licenceName = contactLicNumber != null ? contactLicNumber.Number : string.Empty;
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(contactName) ? "Applicant: " + contactName : string.Empty);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Type")
                        {
                            int applicantId = Convert.ToInt32(frontendFields.Value);
                            jobDocumentType = rpoContext.JobDocumentTypes.FirstOrDefault(x => x.Id == applicantId);
                            documentDescription = "Withdrawal Type: " + jobDocumentType.Type;
                            switch (jobDocumentType.Type)
                            {
                                case "Withdrawal of Stakeholder":
                                    chkWithdrawal = "2";
                                    break;
                                case "Withdrawal of Filing":
                                    chkWithdrawal = "1";
                                    break;
                                default:
                                    break;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }

                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "For")
                        {
                            jobDocumentFor = frontendFields.Value;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }

                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);
                            string ApplicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                            applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;

                            documentDescription = "Application Type: " + ApplicationType;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = ApplicationType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = ApplicationType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else
                        {

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            List<string> newpdfString = pdfFields;
            List<int> newDocumentFieldId = DocumentFieldsIds;
            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateEditedJobDocument(idJobDocument, pdfFields, newDocumentFieldId, FieldsValue);

        }

        public static void GenerateLoftBoardForm2023(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ToList();
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;          
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();


                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);

                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtBin")
                        {
                            string binNumber = job != null && job.RfpAddress != null ? job.RfpAddress.BinNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = binNumber,
                                    ActualValue = binNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = binNumber;
                                updateJobDocumentField.ActualValue = binNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtBrough")
                        {
                            string borough = job != null && job.RfpAddress != null && job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = borough,
                                    ActualValue = borough
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = borough;
                                updateJobDocumentField.ActualValue = borough;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtStreet")
                        {
                            string street = job != null && job.RfpAddress != null ? job.RfpAddress.Street : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = street,
                                    ActualValue = street
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = street;
                                updateJobDocumentField.ActualValue = street;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtBuildingNo")
                        {
                            string houseNo = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = houseNo,
                                    ActualValue = houseNo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = houseNo;
                                updateJobDocumentField.ActualValue = houseNo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDate")
                        {
                            DateTime time = DateTime.Now;
                            string format = "MM/dd/yyyy";
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = time.ToString(format),
                                    ActualValue = time.ToString(format)
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = time.ToString(format);
                                updateJobDocumentField.ActualValue = time.ToString(format); ;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "For")
                        {
                            jobDocumentFor = frontendFields.Value;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }

                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);
                            string ApplicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();
                            documentDescription = "Application Type: " + ApplicationType;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = ApplicationType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = ApplicationType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else
                        {

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value

                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }
            }

            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateJobDocument(idJobDocument);

        }

        public static void EditLoftBoardForm2023(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ToList();
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;           
            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();


                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);

                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtBin")
                        {
                            string binNumber = job != null && job.RfpAddress != null ? job.RfpAddress.BinNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = binNumber,
                                    ActualValue = binNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = binNumber;
                                updateJobDocumentField.ActualValue = binNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });

                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtBrough")
                        {
                            string borough = job != null && job.RfpAddress != null && job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = borough,
                                    ActualValue = borough
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = borough;
                                updateJobDocumentField.ActualValue = borough;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });

                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtStreet")
                        {
                            string street = job != null && job.RfpAddress != null ? job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = street,
                                    ActualValue = street
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = street;
                                updateJobDocumentField.ActualValue = street;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtBuildingNo")
                        {
                            string houseNo = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = houseNo,
                                    ActualValue = houseNo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = houseNo;
                                updateJobDocumentField.ActualValue = houseNo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }                       
                        else if (item.Field.FieldName == "txtDate")
                        {
                            DateTime time = DateTime.Now;
                            string format = "MM/dd/yyyy";
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = time.ToString(format),
                                    ActualValue = time.ToString(format)
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = time.ToString(format);
                                updateJobDocumentField.ActualValue = time.ToString(format);
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();

                    }
                    else
                    {
                        if (item.Field.FieldName == "For")
                        {
                            jobDocumentFor = frontendFields.Value;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }

                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);
                            string ApplicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();                            

                            documentDescription = "Application Type: " + ApplicationType;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = ApplicationType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = ApplicationType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else
                        {

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            List<string> newpdfString = pdfFields;
            List<int> newDocumentFieldId = DocumentFieldsIds;
            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateEditedJobDocument(idJobDocument, pdfFields, newDocumentFieldId, FieldsValue);

        }

    }
}