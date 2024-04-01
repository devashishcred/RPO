using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Api.Controllers.JobDocument;
using Rpo.ApiServices.Model;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Tools
{
    public partial class GenerateJobDocuments
    {

        public static void GenerateSafetyRegistrationSigned(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            string documentDescription = string.Empty;
            var documentFieldList = rpoContext.DocumentFields.
                Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            var jobDocumentFieldList = rpoContext.JobDocumentFields.
                 Where(x => x.IdJobDocument == idJobDocument).ToList();

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                foreach (DocumentField item in documentFieldList)
                {
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                    }
                    else
                    {
                        if (item.Field.FieldName == "LIC7")
                        {

                            JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            int value = Convert.ToInt32(frontendFields.Value);
                            documentDescription = rpoContext.JobDocuments.Where(x => x.Id == value).Select(x => x.DocumentDescription).FirstOrDefault();
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
                        else
                        {
                            JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
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
            rpoContext.SaveChanges();
        }

        public static void GenerateGeneralContractorRegistrationSigned(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            string documentDescription = string.Empty;
            var documentFieldList = rpoContext.DocumentFields.
                Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            var jobDocumentFieldList = rpoContext.JobDocumentFields.
                 Where(x => x.IdJobDocument == idJobDocument).ToList();

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                foreach (DocumentField item in documentFieldList)
                {
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                    }
                    else
                    {
                        if (item.Field.FieldName == "LIC6")
                        {

                            JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            int value = Convert.ToInt32(frontendFields.Value);
                            documentDescription = rpoContext.JobDocuments.Where(x => x.Id == value).Select(x => x.DocumentDescription).FirstOrDefault();
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
                        else
                        {
                            JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
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
            rpoContext.SaveChanges();
        }

        public static void GenerateCertificateofCorrectionandStatementinSupport(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {

            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
            string certifierAddress = string.Empty;
            string certifierCompanyAddress = string.Empty;
            string certifierCompany = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                string applicantEmail = string.Empty;
                string fillingRepresentativeEmail = string.Empty;
                string applicantLicenseNumber = string.Empty;
                string applicantLastName = string.Empty;
                string certifierName = string.Empty;
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {

                        if (item.Field.FieldName == "topmostSubform[0].Page1[0].PLACE_OF_OCCURRENCE[0]")
                        {
                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + ' ' + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ' ' + job.RfpAddress.Borough.Description + ' ' + job.RfpAddress.ZipCode : string.Empty) : string.Empty,
                                    ActualValue = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + ' ' + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ' ' + job.RfpAddress.Borough.Description + ' ' + job.RfpAddress.ZipCode : string.Empty) : string.Empty,
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].RadioButtonList[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].COURSE_PROVIDER_NAME[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].DATE_OF_AGREEMENT_if_applicable[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "",
                                    ActualValue = ""
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].COURSE_PROVIDER_ADDRESS_Provide_full_address[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].WORKER_NAME_Last_First[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].JOB_TITLE[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].DATE_OF_TRAINING[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].TYPE_OF_SST_CARD_SST_Supervisor_Limited_or_Temporary[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].OSHA_10_HOUR[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].OSHA_30_HOUR[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0]._100_HOUR_COURSE[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].I_2[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = certifierName,
                                    ActualValue = certifierName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].hereby_state_that[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = certifierCompany,
                                    ActualValue = certifierCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].Name_print[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = certifierName,
                                    ActualValue = certifierName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].TextField5[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = certifierAddress,
                                    ActualValue = certifierAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].TextField5[1]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "146 WEST 29TH STREET, SUITE 2E NEW YORK, NY 10001",
                                    ActualValue = "146 WEST 29TH STREET, SUITE 2E NEW YORK, NY 10001"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].RadioButtonList[1]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].TextField5[2]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }



                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "topmostSubform[0].Page1[0].SUMMONS_NUMBER[0]")
                        {

                            int violationId = Convert.ToInt32(frontendFields.Value);
                            JobViolation jobViolation = rpoContext.JobViolations.Where(x => x.Id == violationId).FirstOrDefault();

                            jobDocument.IdJobViolation = violationId;

                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = jobViolation.SummonsNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = jobViolation.SummonsNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].I[0]")
                        {
                            int idJobApplicant = frontendFields.Value != null ? Convert.ToInt32(frontendFields.Value) : 0;
                            Contact contact = rpoContext.Contacts.Include("Company.Addresses").Include("Addresses").Where(x => x.Id == idJobApplicant).FirstOrDefault();

                            Company company = contact.Company;
                            Address companyAddress = Common.GetContactAddressForJobDocument(contact);


                            var contactaddress = contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true);
                            certifierAddress = companyAddress != null ? companyAddress.Address1 + " " + companyAddress.Address2 : string.Empty;
                            //   applicantCityZip = jobContactAddress != null ? jobContactAddress.City + (jobContactAddress.State != null ? " " + jobContactAddress.State.Acronym : string.Empty) + " " + jobContactAddress.ZipCode : string.Empty;

                            // = applicantAddress
                            //company != null && company.Addresses != null ? company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;

                            //ContactLicense contactLicense = contact.ContactLicenses.FirstOrDefault();

                            //if (companyAddress == null)
                            //{
                            //    companyAddress = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            //}

                            certifierCompany = company != null ? contact.Company.Name + ", " : string.Empty;
                            certifierCompanyAddress = companyAddress != null ? companyAddress.Address1 + " " + companyAddress.Address2 + " " + companyAddress.City + (companyAddress.State != null ? " " + companyAddress.State.Acronym : string.Empty) + " " + companyAddress.ZipCode : string.Empty;
                            // certifierAddress = certifierAddress + certifierCompanyAddress;

                            certifierName = contact != null ? contact.FirstName + " " + contact.LastName : string.Empty;
                            //documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(certifierName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(certifierName) ? "Certifier: " + certifierName : string.Empty);
                            documentDescription = "Certifier Name: " + certifierName;
                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = Convert.ToString(idJobApplicant),
                                    ActualValue = contact != null ? contact.FirstName + " " + contact.LastName : string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = Convert.ToString(idJobApplicant);
                                updateJobDocumentField.ActualValue = contact != null ? contact.FirstName + " " + contact.LastName : string.Empty;
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
                        rpoContext.SaveChanges();
                    }
                }
            }

            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateJobDocument(idJobDocument);

        }

        public static void EditCertificateofCorrectionandStatementinSupport(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
            string certifierAddress = string.Empty;
            string certifierCompanyAddress = string.Empty;
            string certifierCompany = string.Empty;
            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();
            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                string applicantEmail = string.Empty;
                string fillingRepresentativeEmail = string.Empty;
                string applicantLicenseNumber = string.Empty;
                string applicantLastName = string.Empty;
                string certifierName = string.Empty;
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {

                        if (item.Field.FieldName == "topmostSubform[0].Page1[0].PLACE_OF_OCCURRENCE[0]")
                        {
                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + ' ' + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ' ' + job.RfpAddress.Borough.Description + ' ' + job.RfpAddress.ZipCode : string.Empty) : string.Empty,
                                    ActualValue = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + ' ' + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ' ' + job.RfpAddress.Borough.Description + ' ' + job.RfpAddress.ZipCode : string.Empty) : string.Empty,
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + ' ' + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ' ' + job.RfpAddress.Borough.Description + ' ' + job.RfpAddress.ZipCode : string.Empty) : string.Empty;
                                updateJobDocumentField.ActualValue = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + ' ' + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ' ' + job.RfpAddress.Borough.Description + ' ' + job.RfpAddress.ZipCode : string.Empty) : string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();


                            }
                        }

                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].Name_print[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = certifierName,
                                    ActualValue = certifierName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = certifierName;
                                updateJobDocumentField.ActualValue = certifierName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;

                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();

                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].hereby_state_that[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = certifierCompany,
                                    ActualValue = certifierCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = certifierCompany;
                                updateJobDocumentField.ActualValue = certifierCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();

                            }
                        }

                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].I_2[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = certifierName,
                                    ActualValue = certifierName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = certifierName;
                                updateJobDocumentField.ActualValue = certifierName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();

                            }
                        }


                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].TextField5[0]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = certifierAddress,
                                    ActualValue = certifierAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = certifierAddress;
                                updateJobDocumentField.ActualValue = certifierAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();

                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].TextField5[1]")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "146 WEST 29TH STREET, SUITE 2E NEW YORK, NY 10001",
                                    ActualValue = "146 WEST 29TH STREET, SUITE 2E NEW YORK, NY 10001"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "146 WEST 29TH STREET, SUITE 2E NEW YORK, NY 10001";
                                updateJobDocumentField.ActualValue = "146 WEST 29TH STREET, SUITE 2E NEW YORK, NY 10001";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });

                                rpoContext.SaveChanges();
                            }
                        }

                        //else if (item.Field.FieldName == "topmostSubform[0].Page1[0].TextField5[2]")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = certifierName,
                        //            ActualValue = certifierName
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = certifierName;
                        //        updateJobDocumentField.ActualValue = certifierName;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;

                        //        string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                        //        pdfFields.Add(pdffield);

                        //        FieldsValue.Add(new IdItemName
                        //        {
                        //            Id = pdffield,
                        //            ItemName = updateJobDocumentField.ActualValue
                        //        });
                        //        rpoContext.SaveChanges();
                        //    }
                        //}



                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "topmostSubform[0].Page1[0].SUMMONS_NUMBER[0]")
                        {

                            int violationId = Convert.ToInt32(frontendFields.Value);
                            JobViolation jobViolation = rpoContext.JobViolations.Where(x => x.Id == violationId).FirstOrDefault();

                            jobDocument.IdJobViolation = violationId;

                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = jobViolation.SummonsNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = jobViolation.SummonsNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;

                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "topmostSubform[0].Page1[0].I[0]")
                        {
                            int idJobApplicant = frontendFields.Value != null ? Convert.ToInt32(frontendFields.Value) : 0;
                            Contact contact = rpoContext.Contacts.Include("Company.Addresses").Include("Addresses").Where(x => x.Id == idJobApplicant).FirstOrDefault();

                            Company company = contact.Company;
                            Address companyAddress = Common.GetContactAddressForJobDocument(contact);


                            var contactaddress = contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true);
                            //certifierAddress = contactaddress != null ? contactaddress.Address1 + " " + contactaddress.Address2 : string.Empty;
                            certifierAddress = companyAddress != null ? companyAddress.Address1 + " " + companyAddress.Address2 : string.Empty;
                            //   applicantCityZip = jobContactAddress != null ? jobContactAddress.City + (jobContactAddress.State != null ? " " + jobContactAddress.State.Acronym : string.Empty) + " " + jobContactAddress.ZipCode : string.Empty;

                            // = applicantAddress
                            //company != null && company.Addresses != null ? company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;

                            //ContactLicense contactLicense = contact.ContactLicenses.FirstOrDefault();

                            //if (companyAddress == null)
                            //{
                            //    companyAddress = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            //}

                            certifierCompany = company != null ? contact.Company.Name : string.Empty;
                            certifierCompanyAddress = companyAddress != null ? companyAddress.Address1 + " " + companyAddress.Address2 + " " + companyAddress.City + (companyAddress.State != null ? " " + companyAddress.State.Acronym : string.Empty) + " " + companyAddress.ZipCode : string.Empty;
                            // certifierAddress = certifierAddress + certifierCompanyAddress;

                            certifierName = contact != null ? contact.FirstName + " " + contact.LastName : string.Empty;
                            //documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(certifierName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(certifierName) ? "Certifier: " + certifierName : string.Empty);
                            documentDescription = "Certifier Name: " + certifierName;
                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = Convert.ToString(idJobApplicant),
                                    ActualValue = contact != null ? contact.FirstName + " " + contact.LastName : string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = Convert.ToString(idJobApplicant);
                                updateJobDocumentField.ActualValue = contact != null ? contact.FirstName + " " + contact.LastName : string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;

                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

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
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;

                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);

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
                }
            }
            List<string> newpdfString = pdfFields;
            List<int> newDocumentFieldId = DocumentFieldsIds;
            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            // Edit Job Document
            GenerateEditedJobDocument(idJobDocument, pdfFields, newDocumentFieldId, FieldsValue);

        }
        public static void GenerateTREnergyProgressInspections82020(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {

            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();
            JobDocumentType jobDocumentType = new JobDocumentType();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
            JobContact jobContact = new JobContact();

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();

                string applicantLastName = string.Empty;
                string applicantFirstName = string.Empty;
                string applicantMiddelName = string.Empty;
                string applicantCompanyName = string.Empty;
                string applicantCompanyPhone = string.Empty;
                string applicantCompanyAddress = string.Empty;
                string applicantCompanyCity = string.Empty;
                string applicantCompanyState = string.Empty;
                string applicantCompanyZip = string.Empty;
                string applicantMobile = string.Empty;
                string floorWorkingOn = string.Empty;
                string applicantLicenseNumber = string.Empty;
                string applicantEmail = string.Empty;

                string opgFNAR = string.Empty;
                string opgPOFI = string.Empty;
                string opgIPAR = string.Empty;
                string opgFUPR = string.Empty;
                string opgFALK = string.Empty;
                string opgBSIV = string.Empty;
                string opgABIV = string.Empty;
                string opgABT = string.Empty;
                string opgABCPTI = string.Empty;
                string opgVEST = string.Empty;
                string opgFRPL = string.Empty;
                string opgVADS = string.Empty;
                string opgSHTD = string.Empty;
                string opgHVEQ = string.Empty;
                string opgHVSC = string.Empty;
                string opgHVSWP = string.Empty;
                string opgDLKT = string.Empty;
                string opgMET = string.Empty;
                string opgLIDU = string.Empty;
                string opgILPW = string.Empty;
                string opgELPW = string.Empty;
                string opgLTCT = string.Empty;
                string opgELMO = string.Empty;
                string opgMNTI = string.Empty;
                string opgPMCT = string.Empty;
                string opgEVSER = string.Empty;
                string BIN = string.Empty;
                string engineerLicenseType = string.Empty;
                string architectLicenseType = string.Empty;
                string applicantName = string.Empty;
                string applicantDAName = string.Empty;
                string chkDA = string.Empty;
                string chkPIA = string.Empty;
                string chkCompletion = string.Empty;
                string chkCompletion_All = string.Empty;
                string applicantName2 = string.Empty;
                string chkWithdrawal = string.Empty;
                string chkProgressInspections = string.Empty;
                string reidentification = string.Empty;
                string chkChangeApplication = string.Empty;

                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtbin")
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
                        if (item.Field.FieldName == "JobFileInfo")
                        {
                            string jobFileInfo = job != null ? job.JobNumber + " - TR8-2020.pdf" : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = jobFileInfo,
                                    ActualValue = jobFileInfo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = jobFileInfo;
                                updateJobDocumentField.ActualValue = jobFileInfo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }

                        }
                        else if (item.Field.FieldName == "txtHouseNo")
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
                        else if (item.Field.FieldName == "txtFloors")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = floorWorkingOn,
                                    ActualValue = floorWorkingOn
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = floorWorkingOn;
                                updateJobDocumentField.ActualValue = floorWorkingOn;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkDA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkDA,
                                    ActualValue = chkDA
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = chkDA;
                                updateJobDocumentField.ActualValue = chkDA;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkPIA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkPIA,
                                    ActualValue = chkPIA
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = chkPIA;
                                updateJobDocumentField.ActualValue = chkPIA;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtIName2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantName2,
                                    ActualValue = applicantName2
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantName2;
                                updateJobDocumentField.ActualValue = applicantName2;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtIName")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantName,
                                    ActualValue = applicantName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantName;
                                updateJobDocumentField.ActualValue = applicantName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFirst")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantFirstName,
                                    ActualValue = applicantFirstName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantFirstName;
                                updateJobDocumentField.ActualValue = applicantFirstName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtMI")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantMiddelName,
                                    ActualValue = applicantMiddelName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantMiddelName;
                                updateJobDocumentField.ActualValue = applicantMiddelName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtCompany")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyName,
                                    ActualValue = applicantCompanyName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantCompanyName;
                                updateJobDocumentField.ActualValue = applicantCompanyName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtPhone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyPhone,
                                    ActualValue = applicantCompanyPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantCompanyPhone;
                                updateJobDocumentField.ActualValue = applicantCompanyPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAddress")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyAddress,
                                    ActualValue = applicantCompanyAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantCompanyAddress;
                                updateJobDocumentField.ActualValue = applicantCompanyAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtEmail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantEmail,
                                    ActualValue = applicantEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantEmail;
                                updateJobDocumentField.ActualValue = applicantEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtCity")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyCity,
                                    ActualValue = applicantCompanyCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantCompanyCity;
                                updateJobDocumentField.ActualValue = applicantCompanyCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtState")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyState,
                                    ActualValue = applicantCompanyState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantCompanyState;
                                updateJobDocumentField.ActualValue = applicantCompanyState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtZip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyZip,
                                    ActualValue = applicantCompanyZip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantCompanyZip;
                                updateJobDocumentField.ActualValue = applicantCompanyZip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtMobilePhone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantMobile,
                                    ActualValue = applicantMobile
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantMobile;
                                updateJobDocumentField.ActualValue = applicantMobile;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkPE")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = engineerLicenseType,
                                    ActualValue = engineerLicenseType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = engineerLicenseType;
                                updateJobDocumentField.ActualValue = engineerLicenseType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkRA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = architectLicenseType,
                                    ActualValue = architectLicenseType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = architectLicenseType;
                                updateJobDocumentField.ActualValue = architectLicenseType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLicNo")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantLicenseNumber,
                                    ActualValue = applicantLicenseNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantLicenseNumber;
                                updateJobDocumentField.ActualValue = applicantLicenseNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgFNAR")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgFNAR,
                                    ActualValue = opgFNAR
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgFNAR;
                                updateJobDocumentField.ActualValue = opgFNAR;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgPOFI")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgPOFI,
                                    ActualValue = opgPOFI
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgPOFI;
                                updateJobDocumentField.ActualValue = opgPOFI;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgIPAR")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgIPAR,
                                    ActualValue = opgIPAR
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgIPAR;
                                updateJobDocumentField.ActualValue = opgIPAR;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgFUPR")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgFUPR,
                                    ActualValue = opgFUPR
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgFUPR;
                                updateJobDocumentField.ActualValue = opgFUPR;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgFALK")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgFALK,
                                    ActualValue = opgFALK
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgFALK;
                                updateJobDocumentField.ActualValue = opgFALK;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgBSIV")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgBSIV,
                                    ActualValue = opgBSIV
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgBSIV;
                                updateJobDocumentField.ActualValue = opgBSIV;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgABIV")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgABIV,
                                    ActualValue = opgABIV
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgABIV;
                                updateJobDocumentField.ActualValue = opgABIV;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgABT")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgABT,
                                    ActualValue = opgABT
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgABT;
                                updateJobDocumentField.ActualValue = opgABT;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgABCPTI")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgABCPTI,
                                    ActualValue = opgABCPTI
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgABCPTI;
                                updateJobDocumentField.ActualValue = opgABCPTI;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgVEST")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgVEST,
                                    ActualValue = opgVEST
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgVEST;
                                updateJobDocumentField.ActualValue = opgVEST;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgFRPL")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgFRPL,
                                    ActualValue = opgFRPL
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgFRPL;
                                updateJobDocumentField.ActualValue = opgFRPL;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgVADS")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgVADS,
                                    ActualValue = opgVADS
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgVADS;
                                updateJobDocumentField.ActualValue = opgVADS;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgSHTD")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgSHTD,
                                    ActualValue = opgSHTD
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgSHTD;
                                updateJobDocumentField.ActualValue = opgSHTD;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgHVEQ")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgHVEQ,
                                    ActualValue = opgHVEQ
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgHVEQ;
                                updateJobDocumentField.ActualValue = opgHVEQ;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgHVSC")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgHVSC,
                                    ActualValue = opgHVSC
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgHVSC;
                                updateJobDocumentField.ActualValue = opgHVSC;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgHVSWP")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgHVSWP,
                                    ActualValue = opgHVSWP
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgHVSWP;
                                updateJobDocumentField.ActualValue = opgHVSWP;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgDLKT")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgDLKT,
                                    ActualValue = opgDLKT
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgDLKT;
                                updateJobDocumentField.ActualValue = opgDLKT;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgMET")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgMET,
                                    ActualValue = opgMET
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgMET;
                                updateJobDocumentField.ActualValue = opgMET;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgLIDU")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgLIDU,
                                    ActualValue = opgLIDU
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgLIDU;
                                updateJobDocumentField.ActualValue = opgLIDU;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgILPW")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgILPW,
                                    ActualValue = opgILPW
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgILPW;
                                updateJobDocumentField.ActualValue = opgILPW;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgELPW")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgELPW,
                                    ActualValue = opgELPW
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgELPW;
                                updateJobDocumentField.ActualValue = opgELPW;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgLTCT")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgLTCT,
                                    ActualValue = opgLTCT
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgLTCT;
                                updateJobDocumentField.ActualValue = opgLTCT;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgELMO")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgELMO,
                                    ActualValue = opgELMO
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgELMO;
                                updateJobDocumentField.ActualValue = opgELMO;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgMNTI")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgMNTI,
                                    ActualValue = opgMNTI
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgMNTI;
                                updateJobDocumentField.ActualValue = opgMNTI;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgPMCT")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgPMCT,
                                    ActualValue = opgPMCT
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgPMCT;
                                updateJobDocumentField.ActualValue = opgPMCT;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgEVSER")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgEVSER,
                                    ActualValue = opgEVSER
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgEVSER;
                                updateJobDocumentField.ActualValue = opgEVSER;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDADate")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCommisioning_Yes")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCommisioning_No")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkProgressInspections")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkProgressInspections,
                                    ActualValue = chkProgressInspections
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = chkProgressInspections;
                                updateJobDocumentField.ActualValue = chkProgressInspections;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkChange")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkChangeApplication,
                                    ActualValue = chkChangeApplication
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = chkChangeApplication;
                                updateJobDocumentField.ActualValue = chkChangeApplication;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkNone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkSome")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtIDate")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCompletion")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkCompletion,
                                    ActualValue = chkCompletion
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = chkCompletion;
                                updateJobDocumentField.ActualValue = chkCompletion;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCompletion_All")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkCompletion_All,
                                    ActualValue = chkCompletion_All
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = chkCompletion_All;
                                updateJobDocumentField.ActualValue = chkCompletion_All;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCompletion_Except")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
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
                        else if (item.Field.FieldName == "txtIDate2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "Application Type")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);

                            string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = jobApplication != null ? jobApplication.FloorWorking : string.Empty;

                            if (string.IsNullOrEmpty(floorWorkingOn))
                            {
                                floorWorkingOn = job != null ? job.FloorNumber : string.Empty;
                            }

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicationNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "InspectionType")
                        {

                            string ItemName = string.Empty;

                            opgFNAR = "2";
                            opgPOFI = "2";
                            opgIPAR = "2";
                            opgFUPR = "2";
                            opgFALK = "2";
                            opgBSIV = "2";
                            opgABIV = "2";
                            opgABT = "2";
                            opgABCPTI = "2";
                            opgVEST = "2";
                            opgFRPL = "2";
                            opgVADS = "2";
                            opgSHTD = "2";
                            opgHVEQ = "2";
                            opgHVSC = "2";
                            opgHVSWP = "2";
                            opgDLKT = "2";
                            opgMET = "2";
                            opgLIDU = "2";
                            opgILPW = "2";
                            opgELPW = "2";
                            opgLTCT = "2";
                            opgELMO = "2";
                            opgMNTI = "2";
                            opgPMCT = "2";
                            opgEVSER = "2";
                            foreach (string inspectionType in frontendFields.Value.Split(','))
                            {

                                switch (inspectionType)
                                {
                                    case "opgFNAR": opgFNAR = "1"; break;
                                    case "opgPOFI": opgPOFI = "1"; break;
                                    case "opgIPAR": opgIPAR = "1"; break;
                                    case "opgFUPR": opgFUPR = "1"; break;
                                    case "opgFALK": opgFALK = "1"; break;
                                    case "opgBSIV": opgBSIV = "1"; break;
                                    case "opgABIV": opgABIV = "1"; break;
                                    case "opgABT": opgABT = "1"; break;
                                    case "opgABCPTI": opgABCPTI = "1"; break;
                                    case "opgVEST": opgVEST = "1"; break;
                                    case "opgFRPL": opgFRPL = "1"; break;
                                    case "opgVADS": opgVADS = "1"; break;
                                    case "opgSHTD": opgSHTD = "1"; break;
                                    case "opgHVEQ": opgHVEQ = "1"; break;
                                    case "opgHVSC": opgHVSC = "1"; break;
                                    case "opgHVSWP": opgHVSWP = "1"; break;
                                    case "opgDLKT": opgDLKT = "1"; break;
                                    case "opgMET": opgMET = "1"; break;
                                    case "opgLIDU": opgLIDU = "1"; break;
                                    case "opgILPW": opgILPW = "1"; break;
                                    case "opgELPW": opgELPW = "1"; break;
                                    case "opgLTCT": opgLTCT = "1"; break;
                                    case "opgELMO": opgELMO = "1"; break;
                                    case "opgMNTI": opgMNTI = "1"; break;
                                    case "opgPMCT": opgPMCT = "1"; break;
                                    case "opgEVSER": opgEVSER = "1"; break;


                                    default:
                                        break;
                                }
                            }
                            foreach (string inspectionType in frontendFields.Value.Split(','))
                            {

                                switch (inspectionType)
                                {
                                    case "opgPOFI": ItemName += "Protection of exposed foundation insulation" + ", "; break;
                                    case "opgIPAR": ItemName += "Insulation placement and R-values" + ", "; break;
                                    case "opgFUPR": ItemName += "Fenestration and door U-factor and product ratings" + ", "; break;
                                    case "opgFALK": ItemName += "Fenestration air leakage" + ", "; break;
                                    case "opgFNAR": ItemName += "Fenestration areas" + ", "; break;
                                    case "opgABIV": ItemName += "Air barrier − visual inspection" + ", "; break;
                                    case "opgABT": ItemName += "Air barrier − testing" + ", "; break;
                                    case "opgABCPTI": ItemName += "Air barrier continuity plan testing/inspection" + ", "; break;
                                    case "opgVEST": ItemName += "Vestibules" + ", "; break;
                                    case "opgFRPL": ItemName += "Fireplaces" + ", "; break;
                                    case "opgVADS": ItemName += "Ventilation and air distribution system" + ", "; break;
                                    case "opgSHTD": ItemName += "Shutoff dampers" + ", "; break;
                                    case "opgHVEQ": ItemName += "HVAC-R and service water heating equipment" + ", "; break;
                                    case "opgHVSC": ItemName += "HVAC-R and service water heating system controls" + ", "; break;
                                    case "opgHVSWP": ItemName += "HVAC-R and service water piping design and insulation" + ", "; break;
                                    case "opgDLKT": ItemName += "Duct leakage testing, insulation and design" + ", "; break;
                                    case "opgMET": ItemName += "Metering" + ", "; break;
                                    case "opgLIDU": ItemName += "Lighting in dwelling units" + ", "; break;
                                    case "opgILPW": ItemName += "Interior lighting power" + ", "; break;
                                    case "opgELPW": ItemName += "Exterior lighting power" + ", "; break;
                                    case "opgLTCT": ItemName += "Lighting controls" + ", "; break;
                                    case "opgELMO": ItemName += "Electrical motors and elevators" + ", "; break;
                                    case "opgMNTI": ItemName += "Maintenance information" + ", "; break;
                                    case "opgPMCT": ItemName += "Permanent certificate" + ", "; break;
                                    case "opgEVSER": ItemName += "Electric vehicle service equipment requirements" + ", "; break;

                                    default:
                                        break;
                                }
                            }

                            if (ItemName.Length > 2)
                            {
                                ItemName = ItemName.Remove(ItemName.Length - 2);
                            }

                            documentDescription = documentDescription + " | " + "InspectionType: " + ItemName;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Applicant")
                        {
                            int applicantId = Convert.ToInt32(frontendFields.Value);
                            jobContact = rpoContext.JobContacts.FirstOrDefault(x => x.Id == applicantId);

                            applicantFirstName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                            applicantLastName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;
                            applicantMiddelName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MiddleName : string.Empty;

                            applicantCompanyName = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null ? jobContact.Contact.Company.Name : string.Empty;
                            applicantMobile = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                            applicantEmail = jobContact != null && jobContact.Contact != null ? jobContact.Contact.Email : string.Empty;
                            Address address = Common.GetContactAddressForJobDocument(jobContact);

                            applicantCompanyAddress = address != null ? address.Address1 + " " + address.Address2 : string.Empty;
                            applicantCompanyCity = address != null ? address.City : string.Empty;
                            applicantCompanyState = address != null && address.State != null ? " " + address.State.Acronym : string.Empty;
                            applicantCompanyPhone = Common.GetContactPhoneNumberForJobDocument(jobContact);  //address != null ? address.Phone : string.Empty;
                            applicantCompanyZip = address != null ? address.ZipCode : string.Empty;

                            string applicantfullname = applicantFirstName + " " + applicantLastName;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(applicantfullname) ? " | Applicant: " + applicantfullname : string.Empty);

                            string licenceTypeNumber = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactLicenses != null && jobContact.Contact.ContactLicenses.Count > 0 ?
                                   jobContact.Contact.ContactLicenses.Select(x => x.Number).FirstOrDefault() : string.Empty;
                            if (!string.IsNullOrEmpty(licenceTypeNumber))
                            {
                                ContactLicense contactLicenseType = rpoContext.ContactLicenses.Where(a => a.Number == licenceTypeNumber).FirstOrDefault();
                                ContactLicenseType LicenseType = rpoContext.ContactLicenseTypes.Where(b => b.Id == contactLicenseType.IdContactLicenseType).FirstOrDefault();
                                if (LicenseType.Name.ToLower() == ("Engineer").ToLower())
                                {
                                    engineerLicenseType = "Yes";
                                    applicantLicenseNumber = licenceTypeNumber;
                                }
                                else if (LicenseType.Name.ToLower() == ("Architect").ToLower())
                                {
                                    architectLicenseType = "Yes";
                                    applicantLicenseNumber = licenceTypeNumber;
                                }
                            }
                            else
                            {
                                licenceTypeNumber = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactLicenses != null && jobContact.Contact.ContactLicenses.Count > 0 ?
                                jobContact.Contact.ContactLicenses.Select(x => x.ContactLicenseType.Name).FirstOrDefault() : string.Empty;
                                if (licenceTypeNumber.ToLower() == ("Engineer").ToLower())
                                {
                                    engineerLicenseType = "Yes";
                                }
                                else if (licenceTypeNumber.ToLower() == ("Architect").ToLower())
                                {
                                    architectLicenseType = "Yes";
                                }
                            }
                            if (jobDocumentType.Type.ToLower() == ("Design Applicant Initial Identification").ToLower())
                            {
                                chkDA = "Yes";
                                chkPIA = string.Empty;
                                applicantDAName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Design Applicant Reidentification").ToLower())
                            {
                                chkDA = "Yes";
                                chkPIA = string.Empty;
                                reidentification = "Reidentification";
                                applicantDAName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Design/Inspector Initial Identification").ToLower())
                            {
                                chkDA = "Yes"; chkPIA = "Yes";
                                chkProgressInspections = "Yes";
                                applicantDAName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                                applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Inspector Initial Identification").ToLower())
                            {
                                chkDA = string.Empty; chkPIA = "Yes";
                                chkProgressInspections = "Yes";
                                applicantDAName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                                applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Certification").ToLower())
                            {
                                chkPIA = "Yes";
                                chkDA = string.Empty;
                                chkCompletion = "Yes";
                                chkCompletion_All = "Yes";
                                applicantName2 = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Withdrawal").ToLower())
                            {
                                chkPIA = "Yes";
                                chkDA = string.Empty;
                                chkWithdrawal = "Yes";
                                applicantName2 = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = applicantLastName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicantLastName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Type")
                        {
                            int applicantId = Convert.ToInt32(frontendFields.Value);
                            jobDocumentType = rpoContext.JobDocumentTypes.FirstOrDefault(x => x.Id == applicantId);
                            documentDescription = "Type: " + jobDocumentType.Type;
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
                        else if (item.Field.FieldName == "Responsible for Plans")
                        {
                            string docuementType = jobDocumentType != null ? jobDocumentType.Type : string.Empty;
                            JobContact jobContact2 = new JobContact();
                            if (docuementType.ToLower() == ("Inspector Initial Identification").ToLower())
                            {
                                int applicantId = Convert.ToInt32(frontendFields.Value);
                                jobContact2 = rpoContext.JobContacts.FirstOrDefault(x => x.Id == applicantId);
                                applicantDAName = jobContact2 != null && jobContact2.Contact != null ? jobContact2.Contact.FirstName + " " + jobContact2.Contact.LastName : string.Empty;
                                applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                                chkDA = string.Empty; chkPIA = "Yes";
                                chkProgressInspections = "Yes";
                            }
                            else if (docuementType.ToLower() == ("Inspector Reidentification").ToLower())
                            {
                                int applicantId = Convert.ToInt32(frontendFields.Value);
                                jobContact2 = rpoContext.JobContacts.FirstOrDefault(x => x.Id == applicantId);
                                applicantDAName = jobContact2 != null && jobContact2.Contact != null ? jobContact2.Contact.FirstName + " " + jobContact2.Contact.LastName : string.Empty;
                                applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                                chkDA = string.Empty; chkPIA = "Yes";
                                chkProgressInspections = "Yes";
                                chkChangeApplication = "Yes";
                            }

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(applicantDAName) ? " | Responsible for Plans: " + applicantDAName : string.Empty);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = applicantDAName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicantDAName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Optional Description")
                        {
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(frontendFields.Value) ? " | " : string.Empty) + (!string.IsNullOrEmpty(frontendFields.Value) ? "Optional Description: " + frontendFields.Value : string.Empty);
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
                        else
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                }
            }
            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateJobDocument(idJobDocument);
        }

        public static void EditTREnergyProgressInspections820(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {

            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();
            JobDocumentType jobDocumentType = new JobDocumentType();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
            JobContact jobContact = new JobContact();
            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();

                string applicantLastName = string.Empty;
                string applicantFirstName = string.Empty;
                string applicantMiddelName = string.Empty;
                string applicantCompanyName = string.Empty;
                string applicantCompanyPhone = string.Empty;
                string applicantCompanyAddress = string.Empty;
                string applicantCompanyCity = string.Empty;
                string applicantCompanyState = string.Empty;
                string applicantCompanyZip = string.Empty;
                string applicantMobile = string.Empty;
                string floorWorkingOn = string.Empty;
                string applicantLicenseNumber = string.Empty;
                string applicantEmail = string.Empty;

                string opgPOFI = string.Empty;
                string opgIPAR = string.Empty;
                string opgFUPR = string.Empty;
                string opgFALK = string.Empty;
                string opgFNAR = string.Empty;
                string opgABIV = string.Empty;
                string opgABT = string.Empty;
                string opgABCPTI = string.Empty;
                string opgVEST = string.Empty;
                string opgFRPL = string.Empty;
                string opgVADS = string.Empty;
                string opgSHTD = string.Empty;
                string opgHVEQ = string.Empty;
                string opgHVSC = string.Empty;
                string opgHVSWP = string.Empty;
                string opgDLKT = string.Empty;
                string opgMET = string.Empty;
                string opgLIDU = string.Empty;
                string opgILPW = string.Empty;
                string opgELPW = string.Empty;
                string opgLTCT = string.Empty;
                string opgELMO = string.Empty;
                string opgMNTI = string.Empty;
                string opgPMCT = string.Empty;
                string opgEVSER = string.Empty;


                string engineerLicenseType = string.Empty;
                string architectLicenseType = string.Empty;
                string applicantName = string.Empty;
                string applicantDAName = string.Empty;
                string chkDA = string.Empty;
                string chkPIA = string.Empty;
                string chkCompletion = string.Empty;
                string chkCompletion_All = string.Empty;
                string applicantName2 = string.Empty;
                string chkWithdrawal = string.Empty;
                string chkProgressInspections = string.Empty;
                string reidentification = string.Empty;
                string chkChangeApplication = string.Empty;

                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtbin")
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
                        if (item.Field.FieldName == "JobFileInfo")
                        {
                            string jobFileInfo = job != null ? job.JobNumber + " - TR8-2020.pdf" : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = jobFileInfo,
                                    ActualValue = jobFileInfo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = jobFileInfo;
                                updateJobDocumentField.ActualValue = jobFileInfo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtHouseNo")
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
                        else if (item.Field.FieldName == "txtFloors")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = floorWorkingOn,
                                    ActualValue = floorWorkingOn
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = floorWorkingOn;
                                updateJobDocumentField.ActualValue = floorWorkingOn;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkDA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkDA,
                                    ActualValue = chkDA
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = chkDA;
                                updateJobDocumentField.ActualValue = chkDA;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkPIA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkPIA,
                                    ActualValue = chkPIA
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = chkPIA;
                                updateJobDocumentField.ActualValue = chkPIA;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtIName2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantName2,
                                    ActualValue = applicantName2
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantName2;
                                updateJobDocumentField.ActualValue = applicantName2;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtIName")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantName,
                                    ActualValue = applicantName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantName;
                                updateJobDocumentField.ActualValue = applicantName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFirst")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantFirstName,
                                    ActualValue = applicantFirstName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantFirstName;
                                updateJobDocumentField.ActualValue = applicantFirstName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtMI")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantMiddelName,
                                    ActualValue = applicantMiddelName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantMiddelName;
                                updateJobDocumentField.ActualValue = applicantMiddelName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtCompany")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyName,
                                    ActualValue = applicantCompanyName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantCompanyName;
                                updateJobDocumentField.ActualValue = applicantCompanyName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtPhone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyPhone,
                                    ActualValue = applicantCompanyPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantCompanyPhone;
                                updateJobDocumentField.ActualValue = applicantCompanyPhone;
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
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyAddress,
                                    ActualValue = applicantCompanyAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantCompanyAddress;
                                updateJobDocumentField.ActualValue = applicantCompanyAddress;
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
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantEmail,
                                    ActualValue = applicantEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantEmail;
                                updateJobDocumentField.ActualValue = applicantEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtCity")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyCity,
                                    ActualValue = applicantCompanyCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantCompanyCity;
                                updateJobDocumentField.ActualValue = applicantCompanyCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtState")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyState,
                                    ActualValue = applicantCompanyState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantCompanyState;
                                updateJobDocumentField.ActualValue = applicantCompanyState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtZip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantCompanyZip,
                                    ActualValue = applicantCompanyZip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantCompanyZip;
                                updateJobDocumentField.ActualValue = applicantCompanyZip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtMobilePhone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantMobile,
                                    ActualValue = applicantMobile
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantMobile;
                                updateJobDocumentField.ActualValue = applicantMobile;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkPE")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = engineerLicenseType,
                                    ActualValue = engineerLicenseType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = engineerLicenseType;
                                updateJobDocumentField.ActualValue = engineerLicenseType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkRA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = architectLicenseType,
                                    ActualValue = architectLicenseType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = architectLicenseType;
                                updateJobDocumentField.ActualValue = architectLicenseType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLicNo")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantLicenseNumber,
                                    ActualValue = applicantLicenseNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantLicenseNumber;
                                updateJobDocumentField.ActualValue = applicantLicenseNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgPOFI")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgPOFI,
                                    ActualValue = opgPOFI
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgPOFI;
                                updateJobDocumentField.ActualValue = opgPOFI;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgIPAR")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgIPAR,
                                    ActualValue = opgIPAR
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgIPAR;
                                updateJobDocumentField.ActualValue = opgIPAR;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgFUPR")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgFUPR,
                                    ActualValue = opgFUPR
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgFUPR;
                                updateJobDocumentField.ActualValue = opgFUPR;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgFALK")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgFALK,
                                    ActualValue = opgFALK
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgFALK;
                                updateJobDocumentField.ActualValue = opgFALK;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgFNAR")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgFNAR,
                                    ActualValue = opgFNAR
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgFNAR;
                                updateJobDocumentField.ActualValue = opgFNAR;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgABIV")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgABIV,
                                    ActualValue = opgABIV
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgABIV;
                                updateJobDocumentField.ActualValue = opgABIV;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgABT")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgABT,
                                    ActualValue = opgABT
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgABT;
                                updateJobDocumentField.ActualValue = opgABT;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgABCPTI")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgABCPTI,
                                    ActualValue = opgABCPTI
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgABCPTI;
                                updateJobDocumentField.ActualValue = opgABCPTI;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgVEST")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgVEST,
                                    ActualValue = opgVEST
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgVEST;
                                updateJobDocumentField.ActualValue = opgVEST;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgFRPL")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgFRPL,
                                    ActualValue = opgFRPL
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgFRPL;
                                updateJobDocumentField.ActualValue = opgFRPL;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgVADS")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgVADS,
                                    ActualValue = opgVADS
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgVADS;
                                updateJobDocumentField.ActualValue = opgVADS;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgSHTD")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgSHTD,
                                    ActualValue = opgSHTD
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgSHTD;
                                updateJobDocumentField.ActualValue = opgSHTD;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgHVEQ")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgHVEQ,
                                    ActualValue = opgHVEQ
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgHVEQ;
                                updateJobDocumentField.ActualValue = opgHVEQ;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgHVSC")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgHVSC,
                                    ActualValue = opgHVSC
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgHVSC;
                                updateJobDocumentField.ActualValue = opgHVSC;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgHVSWP")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgHVSWP,
                                    ActualValue = opgHVSWP
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgHVSWP;
                                updateJobDocumentField.ActualValue = opgHVSWP;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgDLKT")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgDLKT,
                                    ActualValue = opgDLKT
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgDLKT;
                                updateJobDocumentField.ActualValue = opgDLKT;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgMET")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgMET,
                                    ActualValue = opgMET
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgMET;
                                updateJobDocumentField.ActualValue = opgMET;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgLIDU")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgLIDU,
                                    ActualValue = opgLIDU
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgLIDU;
                                updateJobDocumentField.ActualValue = opgLIDU;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgILPW")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgILPW,
                                    ActualValue = opgILPW
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgILPW;
                                updateJobDocumentField.ActualValue = opgILPW;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgELPW")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgELPW,
                                    ActualValue = opgELPW
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgELPW;
                                updateJobDocumentField.ActualValue = opgELPW;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgLTCT")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgLTCT,
                                    ActualValue = opgLTCT
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgLTCT;
                                updateJobDocumentField.ActualValue = opgLTCT;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgELMO")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgELMO,
                                    ActualValue = opgELMO
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgELMO;
                                updateJobDocumentField.ActualValue = opgELMO;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgMNTI")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgMNTI,
                                    ActualValue = opgMNTI
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgMNTI;
                                updateJobDocumentField.ActualValue = opgMNTI;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgPMCT")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgPMCT,
                                    ActualValue = opgPMCT
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgPMCT;
                                updateJobDocumentField.ActualValue = opgPMCT;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgEVSER")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgEVSER,
                                    ActualValue = opgEVSER
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgEVSER;
                                updateJobDocumentField.ActualValue = opgEVSER;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDADate")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCommisioning_Yes")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCommisioning_No")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkProgressInspections")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkProgressInspections,
                                    ActualValue = chkProgressInspections
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = chkProgressInspections;
                                updateJobDocumentField.ActualValue = chkProgressInspections;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkChange")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkChangeApplication,
                                    ActualValue = chkChangeApplication
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = chkChangeApplication;
                                updateJobDocumentField.ActualValue = chkChangeApplication;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkNone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkSome")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtIDate")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCompletion")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkCompletion,
                                    ActualValue = chkCompletion
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = chkCompletion;
                                updateJobDocumentField.ActualValue = chkCompletion;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCompletion_All")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = chkCompletion_All,
                                    ActualValue = chkCompletion_All
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = chkCompletion_All;
                                updateJobDocumentField.ActualValue = chkCompletion_All;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCompletion_Except")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = chkWithdrawal;
                                updateJobDocumentField.ActualValue = chkWithdrawal;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtIDate2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = string.Empty,
                                    ActualValue = string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "Application Type")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);

                            string applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = jobApplication != null ? jobApplication.FloorWorking : string.Empty;

                            if (string.IsNullOrEmpty(floorWorkingOn))
                            {
                                floorWorkingOn = job != null ? job.FloorNumber : string.Empty;
                            }

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
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
                        else if (item.Field.FieldName == "InspectionType")
                        {
                            string ItemName = string.Empty;
                            opgPOFI = "2";
                            opgIPAR = "2";
                            opgFUPR = "2";
                            opgFALK = "2";
                            opgFNAR = "2";
                            opgABIV = "2";
                            opgABT = "2";
                            opgABCPTI = "2";
                            opgVEST = "2";
                            opgFRPL = "2";
                            opgVADS = "2";
                            opgSHTD = "2";
                            opgHVEQ = "2";
                            opgHVSC = "2";
                            opgHVSWP = "2";
                            opgDLKT = "2";
                            opgMET = "2";
                            opgLIDU = "2";
                            opgILPW = "2";
                            opgELPW = "2";
                            opgLTCT = "2";
                            opgELMO = "2";
                            opgMNTI = "2";
                            opgPMCT = "2";
                            opgEVSER = "2";

                            foreach (string inspectionType in frontendFields.Value.Split(','))
                            {

                                switch (inspectionType)
                                {
                                    case "opgPOFI": opgPOFI = "1"; break;
                                    case "opgIPAR": opgIPAR = "1"; break;
                                    case "opgFUPR": opgFUPR = "1"; break;
                                    case "opgFALK": opgFALK = "1"; break;
                                    case "opgFNAR": opgFNAR = "1"; break;
                                    case "opgABIV": opgABIV = "1"; break;
                                    case "opgABT": opgABT = "1"; break;
                                    case "opgABCPTI": opgABCPTI = "1"; break;
                                    case "opgVEST": opgVEST = "1"; break;
                                    case "opgFRPL": opgFRPL = "1"; break;
                                    case "opgVADS": opgVADS = "1"; break;
                                    case "opgSHTD": opgSHTD = "1"; break;
                                    case "opgHVEQ": opgHVEQ = "1"; break;
                                    case "opgHVSC": opgHVSC = "1"; break;
                                    case "opgHVSWP": opgHVSWP = "1"; break;
                                    case "opgDLKT": opgDLKT = "1"; break;
                                    case "opgMET": opgMET = "1"; break;
                                    case "opgLIDU": opgLIDU = "1"; break;
                                    case "opgILPW": opgILPW = "1"; break;
                                    case "opgELPW": opgELPW = "1"; break;
                                    case "opgLTCT": opgLTCT = "1"; break;
                                    case "opgELMO": opgELMO = "1"; break;
                                    case "opgMNTI": opgMNTI = "1"; break;
                                    case "opgPMCT": opgPMCT = "1"; break;
                                    case "opgEVSER": opgEVSER = "1"; break;

                                    default:
                                        break;
                                }
                            }

                            foreach (string inspectionType in frontendFields.Value.Split(','))
                            {

                                switch (inspectionType)
                                {
                                    case "opgPOFI": ItemName = "Protection of exposed foundation insulation" + ", "; break;
                                    case "opgIPAR": ItemName += "Insulation placement and R-values" + ", "; break;
                                    case "opgFUPR": ItemName += "Fenestration and door U-factor and product ratings" + ", "; break;
                                    case "opgFALK": ItemName += "Fenestration air leakage" + ", "; break;
                                    case "opgFNAR": ItemName += "Fenestration areas" + ", "; break;
                                    case "opgABIV": ItemName += "Air barrier − visual inspection" + ", "; break;
                                    case "opgABT": ItemName += "Air barrier − testing" + ", "; break;
                                    case "opgABCPTI": ItemName += "Air barrier continuity plan testing/inspection" + ", "; break;
                                    case "opgVEST": ItemName += "Vestibules" + ", "; break;
                                    case "opgFRPL": ItemName += "Fireplaces" + ", "; break;
                                    case "opgVADS": ItemName += "Ventilation and air distribution system" + ", "; break;
                                    case "opgSHTD": ItemName += "Shutoff dampers" + ", "; break;
                                    case "opgHVEQ": ItemName += "HVAC-R and service water heating equipment" + ", "; break;
                                    case "opgHVSC": ItemName += "HVAC-R and service water heating system controls" + ", "; break;
                                    case "opgHVSWP": ItemName += "HVAC-R and service water piping design and insulation" + ", "; break;
                                    case "opgDLKT": ItemName += "Duct leakage testing, insulation and design" + ", "; break;
                                    case "opgMET": ItemName += "Metering" + ", "; break;
                                    case "opgLIDU": ItemName += "Lighting in dwelling units" + ", "; break;
                                    case "opgILPW": ItemName += "Interior lighting power" + ", "; break;
                                    case "opgELPW": ItemName += "Exterior lighting powe" + ", "; break;
                                    case "opgLTCT": ItemName += "Lighting controls" + ", "; break;
                                    case "opgELMO": ItemName += "Electrical motors and elevators" + ", "; break;
                                    case "opgMNTI": ItemName += "Maintenance information" + ", "; break;
                                    case "opgPMCT": ItemName += "Permanent certificate" + ", "; break;
                                    case "opgEVSER": ItemName += "Electric vehicle service equipment requirements" + ", "; break;
                                    default:
                                        break;
                                }
                            }

                            if (ItemName.Length > 2)
                            {
                                ItemName = ItemName.Remove(ItemName.Length - 2);
                            }

                            documentDescription = documentDescription + " | " + "InspectionType: " + ItemName;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Applicant")
                        {

                            int applicantId = Convert.ToInt32(frontendFields.Value);
                            jobContact = rpoContext.JobContacts.FirstOrDefault(x => x.Id == applicantId);

                            applicantFirstName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                            applicantLastName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;
                            applicantMiddelName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MiddleName : string.Empty;

                            applicantCompanyName = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null ? jobContact.Contact.Company.Name : string.Empty;
                            applicantMobile = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                            applicantEmail = jobContact != null && jobContact.Contact != null ? jobContact.Contact.Email : string.Empty;
                            Address address = Common.GetContactAddressForJobDocument(jobContact);

                            applicantCompanyAddress = address != null ? address.Address1 + " " + address.Address2 : string.Empty;
                            applicantCompanyCity = address != null ? address.City : string.Empty;
                            applicantCompanyState = address != null && address.State != null ? " " + address.State.Acronym : string.Empty;
                            applicantCompanyPhone = Common.GetContactPhoneNumberForJobDocument(jobContact);  //address != null ? address.Phone : string.Empty;
                            applicantCompanyZip = address != null ? address.ZipCode : string.Empty;

                            string applicantfullname = applicantFirstName + " " + applicantLastName;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(applicantfullname) ? " | Applicant: " + applicantfullname : string.Empty);

                            string licenceTypeNumber = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactLicenses != null && jobContact.Contact.ContactLicenses.Count > 0 ?
                                   jobContact.Contact.ContactLicenses.Select(x => x.Number).FirstOrDefault() : string.Empty;
                            if (!string.IsNullOrEmpty(licenceTypeNumber))
                            {
                                ContactLicense contactLicenseType = rpoContext.ContactLicenses.Where(a => a.Number == licenceTypeNumber).FirstOrDefault();
                                ContactLicenseType LicenseType = rpoContext.ContactLicenseTypes.Where(b => b.Id == contactLicenseType.IdContactLicenseType).FirstOrDefault();
                                if (LicenseType.Name.ToLower() == ("Engineer").ToLower())
                                {
                                    engineerLicenseType = "Yes";
                                    applicantLicenseNumber = licenceTypeNumber;
                                }
                                else if (LicenseType.Name.ToLower() == ("Architect").ToLower())
                                {
                                    architectLicenseType = "Yes";
                                    applicantLicenseNumber = licenceTypeNumber;
                                }
                            }
                            else
                            {
                                licenceTypeNumber = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactLicenses != null && jobContact.Contact.ContactLicenses.Count > 0 ?
                                jobContact.Contact.ContactLicenses.Select(x => x.ContactLicenseType.Name).FirstOrDefault() : string.Empty;
                                if (licenceTypeNumber.ToLower() == ("Engineer").ToLower())
                                {
                                    engineerLicenseType = "Yes";
                                }
                                else if (licenceTypeNumber.ToLower() == ("Architect").ToLower())
                                {
                                    architectLicenseType = "Yes";
                                }
                            }
                            if (jobDocumentType.Type.ToLower() == ("Design Applicant Initial Identification").ToLower())
                            {
                                chkDA = "Yes";
                                chkPIA = string.Empty;
                                applicantDAName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Design Applicant Reidentification").ToLower())
                            {
                                chkDA = "Yes";
                                chkPIA = string.Empty;
                                reidentification = "Reidentification";
                                applicantDAName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Design/Inspector Initial Identification").ToLower())
                            {
                                chkDA = "Yes"; chkPIA = "Yes";
                                chkProgressInspections = "Yes";
                                applicantDAName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                                applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Inspector Initial Identification").ToLower())
                            {
                                chkDA = string.Empty; chkPIA = "Yes";
                                chkProgressInspections = "Yes";
                                applicantDAName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                                applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Certification").ToLower())
                            {
                                chkPIA = "Yes";
                                chkDA = string.Empty;
                                chkCompletion = "Yes";
                                chkCompletion_All = "Yes";
                                applicantName2 = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            else if (jobDocumentType.Type.ToLower() == ("Withdrawal").ToLower())
                            {
                                chkPIA = "Yes";
                                chkDA = string.Empty;
                                chkWithdrawal = "Yes";
                                applicantName2 = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = applicantLastName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicantLastName;
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
                            documentDescription = "Type: " + jobDocumentType.Type;
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
                        else if (item.Field.FieldName == "Responsible for Plans")
                        {
                            string docuementType = jobDocumentType != null ? jobDocumentType.Type : string.Empty;
                            JobContact jobContact2 = new JobContact();
                            if (docuementType.ToLower() == ("Inspector Initial Identification").ToLower())
                            {
                                int applicantId = Convert.ToInt32(frontendFields.Value);
                                jobContact2 = rpoContext.JobContacts.FirstOrDefault(x => x.Id == applicantId);
                                applicantDAName = jobContact2 != null && jobContact2.Contact != null ? jobContact2.Contact.FirstName + " " + jobContact2.Contact.LastName : string.Empty;
                                applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                                chkDA = string.Empty; chkPIA = "Yes";
                                chkProgressInspections = "Yes";
                            }
                            else if (docuementType.ToLower() == ("Inspector Reidentification").ToLower())
                            {
                                int applicantId = Convert.ToInt32(frontendFields.Value);
                                jobContact2 = rpoContext.JobContacts.FirstOrDefault(x => x.Id == applicantId);
                                applicantDAName = jobContact2 != null && jobContact2.Contact != null ? jobContact2.Contact.FirstName + " " + jobContact2.Contact.LastName : string.Empty;
                                applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                                chkDA = string.Empty; chkPIA = "Yes";
                                chkProgressInspections = "Yes";
                                chkChangeApplication = "Yes";
                            }
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(applicantDAName) ? " | Responsible for Plans: " + applicantDAName : string.Empty);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = applicantDAName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicantDAName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Optional Description")
                        {
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(frontendFields.Value) ? " | " : string.Empty) + (!string.IsNullOrEmpty(frontendFields.Value) ? "Optional Description: " + frontendFields.Value : string.Empty);
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
                        else
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
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