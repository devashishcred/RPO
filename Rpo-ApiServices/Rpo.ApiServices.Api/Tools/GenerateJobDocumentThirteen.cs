using System;
using System.Collections.Generic;
using System.Linq;
using Rpo.ApiServices.Api.Controllers.JobDocument;
using Rpo.ApiServices.Model;
using Rpo.ApiServices.Model.Models;
using System.Data.Entity;

namespace Rpo.ApiServices.Api.Tools
{
    public partial class GenerateJobDocuments
    {
        public static void GenerateLandmarkFasttrackServiceApplication2021(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string floorWorkingOn = string.Empty;
                string licenseNumber = string.Empty;

                string filingCompany = string.Empty;
                string filingPhone = string.Empty;
                string filingEmail = string.Empty;
                string filingAddress = string.Empty;
                string filingCity = string.Empty;
                string filingState = string.Empty;
                string filingZipCode = string.Empty;

                string ownerrfpAddress = string.Empty;
                string ownerCompany = string.Empty;
                string ownerCity = string.Empty;
                string ownerState = string.Empty;
                string ownerZipCode = string.Empty;

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string address = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = address,
                                    ActualValue = address
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = address;
                                updateJobDocumentField.ActualValue = address;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFloorApt")
                        {
                            string Stories = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
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
                        else if (item.Field.FieldName == "txtBorough")
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
                        else if (item.Field.FieldName == "txtBlock")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLot")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Title")
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
                        else if (item.Field.FieldName == "txtFil_Rep_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCompany,
                                    ActualValue = filingCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingCompany;
                                updateJobDocumentField.ActualValue = filingCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingAddress,
                                    ActualValue = filingAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingAddress;
                                updateJobDocumentField.ActualValue = filingAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCity,
                                    ActualValue = filingCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingCity;
                                updateJobDocumentField.ActualValue = filingCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingState,
                                    ActualValue = filingState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingState;
                                updateJobDocumentField.ActualValue = filingState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingZipCode,
                                    ActualValue = filingZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingZipCode;
                                updateJobDocumentField.ActualValue = filingZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingPhone,
                                    ActualValue = filingPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingPhone;
                                updateJobDocumentField.ActualValue = filingPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingEmail,
                                    ActualValue = filingEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingEmail;
                                updateJobDocumentField.ActualValue = filingEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCompany,
                                    ActualValue = ownerCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerCompany;
                                updateJobDocumentField.ActualValue = ownerCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerrfpAddress,
                                    ActualValue = ownerrfpAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerrfpAddress;
                                updateJobDocumentField.ActualValue = ownerrfpAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCity,
                                    ActualValue = ownerCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerCity;
                                updateJobDocumentField.ActualValue = ownerCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerState,
                                    ActualValue = ownerState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerState;
                                updateJobDocumentField.ActualValue = ownerState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerZipCode,
                                    ActualValue = ownerZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerZipCode;
                                updateJobDocumentField.ActualValue = ownerZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_EMail")
                        {
                            string ownerEmail = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerEmail,
                                    ActualValue = ownerEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerEmail;
                                updateJobDocumentField.ActualValue = ownerEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Phone")
                        {
                            // string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (ownerPhone == null || ownerPhone == "")
                            {
                                ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.MobilePhone : string.Empty;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name2")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name3")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title2")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "txtFil_Rep_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            //Employee employee = rpoContext.Employees.Where(x => x.Id == Certifierid).FirstOrDefault();
                            Employee employee = Common.GetEmployeeInformation(Certifierid);

                            string filingFullName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;
                            filingCompany = "RPO INC";
                            filingPhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingEmail = employee != null ? employee.Email : string.Empty;

                            filingAddress = employee != null ? employee.Address1 + " " + employee.Address2 : string.Empty;
                            filingCity = employee != null ? employee.City : string.Empty;
                            filingState = employee != null && employee.State != null ? employee.State.Acronym : string.Empty;
                            filingZipCode = employee != null ? employee.ZipCode : string.Empty;

                            Contact owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress.Value)
                            {
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault() : null;
                            }
                            
                            ownerZipCode = ownerAddress != null ? ownerAddress.ZipCode : string.Empty;
                            ownerState = ownerAddress != null && ownerAddress.State != null ? ownerAddress.State.Acronym : string.Empty;
                            ownerCity = ownerAddress != null ? ownerAddress.City : string.Empty;
                            ownerrfpAddress = ownerAddress != null ? ownerAddress.Address1 + " " + ownerAddress.Address2 : string.Empty;
                            ownerCompany = owner != null && owner.Company != null ? owner.Company.Name : string.Empty;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(filingFullName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filingFullName) ? "Filing Representative: " + filingFullName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = filingFullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingFullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application Type")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string applicationNumber = (jobApplication != null) ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = jobApplication != null ? jobApplication.FloorWorking : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicationNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Description")
                        {
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(frontendFields.Value) ? " | " : string.Empty) + (!string.IsNullOrEmpty(frontendFields.Value) ? "Optional Additional Description: " + frontendFields.Value : string.Empty);
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

        public static void EditLandmarkFasttrackServiceApplication2021(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();
            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();


                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string floorWorkingOn = string.Empty;
                string licenseNumber = string.Empty;

                string filingCompany = string.Empty;
                string filingPhone = string.Empty;
                string filingEmail = string.Empty;
                string filingAddress = string.Empty;
                string filingCity = string.Empty;
                string filingState = string.Empty;
                string filingZipCode = string.Empty;

                string ownerrfpAddress = string.Empty;
                string ownerCompany = string.Empty;
                string ownerCity = string.Empty;
                string ownerState = string.Empty;
                string ownerZipCode = string.Empty;

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string address = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = address,
                                    ActualValue = address
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = address;
                                updateJobDocumentField.ActualValue = address;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFloorApt")
                        {
                            string Stories = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
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
                        else if (item.Field.FieldName == "txtBorough")
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
                        else if (item.Field.FieldName == "txtBlock")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLot")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Title")
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCompany,
                                    ActualValue = filingCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingCompany;
                                updateJobDocumentField.ActualValue = filingCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingAddress,
                                    ActualValue = filingAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingAddress;
                                updateJobDocumentField.ActualValue = filingAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCity,
                                    ActualValue = filingCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingCity;
                                updateJobDocumentField.ActualValue = filingCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingState,
                                    ActualValue = filingState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingState;
                                updateJobDocumentField.ActualValue = filingState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingZipCode,
                                    ActualValue = filingZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingZipCode;
                                updateJobDocumentField.ActualValue = filingZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingPhone,
                                    ActualValue = filingPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingPhone;
                                updateJobDocumentField.ActualValue = filingPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingEmail,
                                    ActualValue = filingEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingEmail;
                                updateJobDocumentField.ActualValue = filingEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCompany,
                                    ActualValue = ownerCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerCompany;
                                updateJobDocumentField.ActualValue = ownerCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerrfpAddress,
                                    ActualValue = ownerrfpAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerrfpAddress;
                                updateJobDocumentField.ActualValue = ownerrfpAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCity,
                                    ActualValue = ownerCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerCity;
                                updateJobDocumentField.ActualValue = ownerCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerState,
                                    ActualValue = ownerState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerState;
                                updateJobDocumentField.ActualValue = ownerState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerZipCode,
                                    ActualValue = ownerZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerZipCode;
                                updateJobDocumentField.ActualValue = ownerZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_EMail")
                        {
                            string ownerEmail = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerEmail,
                                    ActualValue = ownerEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerEmail;
                                updateJobDocumentField.ActualValue = ownerEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Phone")
                        {
                            // string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (ownerPhone == null || ownerPhone == "")
                            {
                                ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.MobilePhone : string.Empty;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name2")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name3")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title2")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
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
                        if (item.Field.FieldName == "txtFil_Rep_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            //Employee employee = rpoContext.Employees.Where(x => x.Id == Certifierid).FirstOrDefault();
                            Employee employee = Common.GetEmployeeInformation(Certifierid);

                            string filingFullName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;
                            filingCompany = "RPO INC";
                            filingPhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingEmail = employee != null ? employee.Email : string.Empty;

                            filingAddress = employee != null ? employee.Address1 + " " + employee.Address2 : string.Empty;
                            filingCity = employee != null ? employee.City : string.Empty;
                            filingState = employee != null && employee.State != null ? employee.State.Acronym : string.Empty;
                            filingZipCode = employee != null ? employee.ZipCode : string.Empty;                            

                            Contact owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress.Value)
                            {
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault() : null;
                            }
                            ContactLicense ownerLicense = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactLicenses != null ? job.RfpAddress.OwnerContact.ContactLicenses.FirstOrDefault() : null;
                            
                            ownerZipCode = ownerAddress != null ? ownerAddress.ZipCode : string.Empty;
                            ownerState = ownerAddress != null && ownerAddress.State != null ? ownerAddress.State.Acronym : string.Empty;
                            ownerCity = ownerAddress != null ? ownerAddress.City : string.Empty;
                            ownerrfpAddress = ownerAddress != null ? ownerAddress.Address1 + " " + ownerAddress.Address2 : string.Empty;
                            ownerCompany = owner != null && owner.Company != null ? owner.Company.Name : string.Empty;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(filingFullName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filingFullName) ? "Filing Representative: " + filingFullName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = filingFullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingFullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application Type")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string applicationNumber = (jobApplication != null) ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = jobApplication != null ? jobApplication.FloorWorking : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
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
                        else if (item.Field.FieldName == "Description")
                        {
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(frontendFields.Value) ? " | " : string.Empty) + (!string.IsNullOrEmpty(frontendFields.Value) ? "Optional Additional Description: " + frontendFields.Value : string.Empty);
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


        public static void GenerateLandmarkPreservationCommissionApplicationForm2021(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string floorWorkingOn = string.Empty;
                string licenseNumber = string.Empty;

                string filingCompany = string.Empty;
                string filingPhone = string.Empty;
                string filingEmail = string.Empty;
                string filingAddress = string.Empty;
                string filingCity = string.Empty;
                string filingState = string.Empty;
                string filingZipCode = string.Empty;


                string ownerrfpAddress = string.Empty;
                string ownerCompany = string.Empty;
                string ownerCity = string.Empty;
                string ownerState = string.Empty;
                string ownerZipCode = string.Empty;
                string LEYES = string.Empty;
                string LASYes = string.Empty;
                string opgLegalize = string.Empty;
                string LENo = string.Empty;
                string LASNo = string.Empty;

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string address = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = address,
                                    ActualValue = address
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = address;
                                updateJobDocumentField.ActualValue = address;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFloorApt")
                        {
                            string floor = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
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
                        else if (item.Field.FieldName == "txtBorough")
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
                        else if (item.Field.FieldName == "txtBlock")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtCB")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtZone_Dist")
                        {
                            string zoneDistrict = job != null && job.RfpAddress != null ? job.RfpAddress.ZoneDistrict : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = zoneDistrict,
                                    ActualValue = zoneDistrict
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = zoneDistrict;
                                updateJobDocumentField.ActualValue = zoneDistrict;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "txtJob_descr")
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
                        else if (item.Field.FieldName == "opgLegalize")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgLegalize,
                                    ActualValue = opgLegalize
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = opgLegalize;
                                updateJobDocumentField.ActualValue = opgLegalize;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtNov_no")
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
                        else if (item.Field.FieldName == "chkLAS_No")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = LASNo,
                                    ActualValue = LASNo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = LASNo;
                                updateJobDocumentField.ActualValue = LASNo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkLAS_Yes")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = LASYes,
                                    ActualValue = LASYes
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = LASYes;
                                updateJobDocumentField.ActualValue = LASYes;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txt_LDN")
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
                        else if (item.Field.FieldName == "chkAgencyNo")
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
                        else if (item.Field.FieldName == "chkDOB")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCityPlanningComm")
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
                        else if (item.Field.FieldName == "chkBoardofSA")
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
                        else if (item.Field.FieldName == "Contact Info")
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
                        else if (item.Field.FieldName == "chkLE_No")
                        {

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = LENo,
                                    ActualValue = LENo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = LENo;
                                updateJobDocumentField.ActualValue = LENo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkLE_Yes")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = LEYES,
                                    ActualValue = LEYES
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = LEYES;
                                updateJobDocumentField.ActualValue = LEYES;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "chkFil_Rep_Primary")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCompany,
                                    ActualValue = filingCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingCompany;
                                updateJobDocumentField.ActualValue = filingCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingAddress,
                                    ActualValue = filingAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingAddress;
                                updateJobDocumentField.ActualValue = filingAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCity,
                                    ActualValue = filingCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingCity;
                                updateJobDocumentField.ActualValue = filingCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingState,
                                    ActualValue = filingState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingState;
                                updateJobDocumentField.ActualValue = filingState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingZipCode,
                                    ActualValue = filingZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingZipCode;
                                updateJobDocumentField.ActualValue = filingZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingPhone,
                                    ActualValue = filingPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingPhone;
                                updateJobDocumentField.ActualValue = filingPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingEmail,
                                    ActualValue = filingEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingEmail;
                                updateJobDocumentField.ActualValue = filingEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name2")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name3")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Title2")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCompany,
                                    ActualValue = ownerCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerCompany;
                                updateJobDocumentField.ActualValue = ownerCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerrfpAddress,
                                    ActualValue = ownerrfpAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerrfpAddress;
                                updateJobDocumentField.ActualValue = ownerrfpAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCity,
                                    ActualValue = ownerCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerCity;
                                updateJobDocumentField.ActualValue = ownerCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerState,
                                    ActualValue = ownerState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerState;
                                updateJobDocumentField.ActualValue = ownerState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerZipCode,
                                    ActualValue = ownerZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerZipCode;
                                updateJobDocumentField.ActualValue = ownerZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_EMail")
                        {
                            string ownerEmail = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerEmail,
                                    ActualValue = ownerEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerEmail;
                                updateJobDocumentField.ActualValue = ownerEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Phone")
                        {
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "txtFil_Rep_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            Employee employee = Common.GetEmployeeInformation(Certifierid);

                            string filingFullName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;
                            filingCompany = "RPO INC";
                            filingPhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingEmail = employee != null ? employee.Email : string.Empty;

                            //Address contactAddress = employee != null && employee.Company != null&& contact.Company.Addresses != null ? contact.Company.Addresses.Where(x => x.IsMainAddress == true).FirstOrDefault() : null;
                            filingAddress = employee != null ? employee.Address1 + " " + employee.Address2 : string.Empty;
                            filingCity = employee != null ? employee.City : string.Empty;
                            filingState = employee != null && employee.State != null ? employee.State.Acronym : string.Empty;
                            filingZipCode = employee != null ? employee.ZipCode : string.Empty;


                            Contact owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress.Value)
                            {
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault() : null;
                            }

                            //  ContactLicense ownerLicense = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactLicenses != null ? job.RfpAddress.OwnerContact.ContactLicenses.FirstOrDefault() : null;


                            ownerZipCode = ownerAddress != null ? ownerAddress.ZipCode : string.Empty;
                            ownerState = ownerAddress != null && ownerAddress.State != null ? ownerAddress.State.Acronym : string.Empty;
                            ownerCity = ownerAddress != null ? ownerAddress.City : string.Empty;
                            ownerrfpAddress = ownerAddress != null ? ownerAddress.Address1 + " " + ownerAddress.Address2 : string.Empty;
                            ownerCompany = owner != null && owner.Company != null ? owner.Company.Name : string.Empty;


                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(filingFullName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filingFullName) ? "Filing Representative: " + filingFullName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = filingFullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingFullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application Type")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string applicationNumber = (jobApplication != null) ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = (jobApplication != null) && !string.IsNullOrEmpty(jobApplication.FloorWorking) ? jobApplication.FloorWorking : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicationNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Type")
                        {
                            int typeId = Convert.ToInt32(frontendFields.Value);
                            JobDocumentType jobDocumentType = rpoContext.JobDocumentTypes.Where(x => x.Id == typeId).FirstOrDefault();

                            string jobDocumentTypeName = (jobDocumentType != null) ? jobDocumentType.Type : string.Empty;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(jobDocumentTypeName) ? "Type: " + jobDocumentTypeName : string.Empty);

                            if (jobDocumentTypeName.ToLower() == ("Amend or Extend Existing Permit").ToLower())
                            {
                                LASYes = "Yes";
                                opgLegalize = "NO";
                                LENo = "Yes";

                            }
                            else if (jobDocumentTypeName.ToLower() == ("Easement").ToLower())
                            {
                                LEYES = "Yes";
                                LASNo = "Yes";
                            }
                            else if (jobDocumentTypeName.ToLower() == ("New").ToLower())
                            {
                                LENo = "Yes";
                                LASNo = "Yes";
                            }
                            else if (jobDocumentTypeName.ToLower() == ("Request Notice of Compliance-Sign Off").ToLower())
                            {
                                LASYes = "Yes";
                                opgLegalize = "NO";
                                LENo = "Yes";

                            }

                            if (updateJobDocumentField == null)
                            {

                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = jobDocumentTypeName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);

                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = jobDocumentTypeName;
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

        public static void EditLandmarkPreservationCommissionApplicationForm2021(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string floorWorkingOn = string.Empty;
                string licenseNumber = string.Empty;

                string filingCompany = string.Empty;
                string filingPhone = string.Empty;
                string filingEmail = string.Empty;
                string filingAddress = string.Empty;
                string filingCity = string.Empty;
                string filingState = string.Empty;
                string filingZipCode = string.Empty;


                string ownerrfpAddress = string.Empty;
                string ownerCompany = string.Empty;
                string ownerCity = string.Empty;
                string ownerState = string.Empty;
                string ownerZipCode = string.Empty;
                string LEYES = string.Empty;
                string LASYes = string.Empty;
                string opgLegalize = string.Empty;
                string LENo = string.Empty;
                string LASNo = string.Empty;

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string address = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = address,
                                    ActualValue = address
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = address;
                                updateJobDocumentField.ActualValue = address;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFloorApt")
                        {
                            string floor = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
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
                        else if (item.Field.FieldName == "txtBorough")
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
                        else if (item.Field.FieldName == "txtBlock")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtCB")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtZone_Dist")
                        {
                            string zoneDistrict = job != null && job.RfpAddress != null ? job.RfpAddress.ZoneDistrict : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = zoneDistrict,
                                    ActualValue = zoneDistrict
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = zoneDistrict;
                                updateJobDocumentField.ActualValue = zoneDistrict;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "txtJob_descr")
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgLegalize")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = opgLegalize,
                                    ActualValue = opgLegalize
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = opgLegalize;
                                updateJobDocumentField.ActualValue = opgLegalize;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtNov_no")
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkLAS_No")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = LASNo,
                                    ActualValue = LASNo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = LASNo;
                                updateJobDocumentField.ActualValue = LASNo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkLAS_Yes")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = LASYes,
                                    ActualValue = LASYes
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = LASYes;
                                updateJobDocumentField.ActualValue = LASYes;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txt_LDN")
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkAgencyNo")
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkDOB")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkCityPlanningComm")
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkBoardofSA")
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Contact Info")
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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkLE_No")
                        {

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = LENo,
                                    ActualValue = LENo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = LENo;
                                updateJobDocumentField.ActualValue = LENo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkLE_Yes")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = LEYES,
                                    ActualValue = LEYES
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = LEYES;
                                updateJobDocumentField.ActualValue = LEYES;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "chkFil_Rep_Primary")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCompany,
                                    ActualValue = filingCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingCompany;
                                updateJobDocumentField.ActualValue = filingCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingAddress,
                                    ActualValue = filingAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingAddress;
                                updateJobDocumentField.ActualValue = filingAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCity,
                                    ActualValue = filingCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingCity;
                                updateJobDocumentField.ActualValue = filingCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingState,
                                    ActualValue = filingState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingState;
                                updateJobDocumentField.ActualValue = filingState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingZipCode,
                                    ActualValue = filingZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingZipCode;
                                updateJobDocumentField.ActualValue = filingZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingPhone,
                                    ActualValue = filingPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingPhone;
                                updateJobDocumentField.ActualValue = filingPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingEmail,
                                    ActualValue = filingEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingEmail;
                                updateJobDocumentField.ActualValue = filingEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name2")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name3")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Title2")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCompany,
                                    ActualValue = ownerCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerCompany;
                                updateJobDocumentField.ActualValue = ownerCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerrfpAddress,
                                    ActualValue = ownerrfpAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerrfpAddress;
                                updateJobDocumentField.ActualValue = ownerrfpAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCity,
                                    ActualValue = ownerCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerCity;
                                updateJobDocumentField.ActualValue = ownerCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerState,
                                    ActualValue = ownerState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerState;
                                updateJobDocumentField.ActualValue = ownerState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerZipCode,
                                    ActualValue = ownerZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerZipCode;
                                updateJobDocumentField.ActualValue = ownerZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_EMail")
                        {
                            string ownerEmail = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerEmail,
                                    ActualValue = ownerEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerEmail;
                                updateJobDocumentField.ActualValue = ownerEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Phone")
                        {
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
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
                        if (item.Field.FieldName == "txtFil_Rep_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            Employee employee = Common.GetEmployeeInformation(Certifierid);

                            string filingFullName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;
                            filingCompany = "RPO INC";
                            filingPhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingEmail = employee != null ? employee.Email : string.Empty;

                            //Address contactAddress = employee != null && employee.Company != null&& contact.Company.Addresses != null ? contact.Company.Addresses.Where(x => x.IsMainAddress == true).FirstOrDefault() : null;
                            filingAddress = employee != null ? employee.Address1 + " " + employee.Address2 : string.Empty;
                            filingCity = employee != null ? employee.City : string.Empty;
                            filingState = employee != null && employee.State != null ? employee.State.Acronym : string.Empty;
                            filingZipCode = employee != null ? employee.ZipCode : string.Empty;


                            Contact owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress.Value)
                            {
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault() : null;
                            }
                            // Address ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            //  ContactLicense ownerLicense = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactLicenses != null ? job.RfpAddress.OwnerContact.ContactLicenses.FirstOrDefault() : null;


                            ownerZipCode = ownerAddress != null ? ownerAddress.ZipCode : string.Empty;
                            ownerState = ownerAddress != null && ownerAddress.State != null ? ownerAddress.State.Acronym : string.Empty;
                            ownerCity = ownerAddress != null ? ownerAddress.City : string.Empty;
                            ownerrfpAddress = ownerAddress != null ? ownerAddress.Address1 + " " + ownerAddress.Address2 : string.Empty;
                            ownerCompany = owner != null && owner.Company != null ? owner.Company.Name : string.Empty;


                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(filingFullName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filingFullName) ? "Filing Representative: " + filingFullName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = filingFullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingFullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application Type")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string applicationNumber = (jobApplication != null) ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = (jobApplication != null) && !string.IsNullOrEmpty(jobApplication.FloorWorking) ? jobApplication.FloorWorking : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
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
                        else if (item.Field.FieldName == "Type")
                        {
                            int typeId = Convert.ToInt32(frontendFields.Value);
                            JobDocumentType jobDocumentType = rpoContext.JobDocumentTypes.Where(x => x.Id == typeId).FirstOrDefault();

                            string jobDocumentTypeName = (jobDocumentType != null) ? jobDocumentType.Type : string.Empty;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(jobDocumentTypeName) ? "Type: " + jobDocumentTypeName : string.Empty);

                            if (jobDocumentTypeName.ToLower() == ("Amend or Extend Existing Permit").ToLower())
                            {
                                LASYes = "Yes";
                                opgLegalize = "NO";
                                LENo = "Yes";

                            }
                            else if (jobDocumentTypeName.ToLower() == ("Easement").ToLower())
                            {
                                LEYES = "Yes";
                                LASNo = "Yes";
                            }
                            else if (jobDocumentTypeName.ToLower() == ("New").ToLower())
                            {
                                LENo = "Yes";
                                LASNo = "Yes";
                            }
                            else if (jobDocumentTypeName.ToLower() == ("Request Notice of Compliance-Sign Off").ToLower())
                            {
                                LASYes = "Yes";
                                opgLegalize = "NO";
                                LENo = "Yes";

                            }

                            if (updateJobDocumentField == null)
                            {

                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = jobDocumentTypeName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);

                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = jobDocumentTypeName;
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
            List<string> newpdfString = pdfFields;
            List<int> newDocumentFieldId = DocumentFieldsIds;
            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateEditedJobDocument(idJobDocument, pdfFields, newDocumentFieldId, FieldsValue);
        }

        public static void GenerateExpeditedCertificateNoEffectApplication2021(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                string businessCompanyName = string.Empty;
                string businessPhone = string.Empty;
                string businessAddress = string.Empty;
                string businessFax = string.Empty;
                string businessCity = string.Empty;
                string businessState = string.Empty;
                string businessZip = string.Empty;
                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string floorWorkingOn = string.Empty;
                string licenseNumber = string.Empty;

                string filingCompany = string.Empty;
                string filingPhone = string.Empty;
                string filingEmail = string.Empty;
                string filingAddress = string.Empty;
                string filingCity = string.Empty;
                string filingState = string.Empty;
                string filingZipCode = string.Empty;

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string address = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = address,
                                    ActualValue = address
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = address;
                                updateJobDocumentField.ActualValue = address;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFloorApt")
                        {
                            string stories = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = stories,
                                    ActualValue = stories
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = stories;
                                updateJobDocumentField.ActualValue = stories;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtBorough")
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
                        else if (item.Field.FieldName == "txtBlock")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLot")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessCompanyName,
                                    ActualValue = businessCompanyName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessCompanyName;
                                updateJobDocumentField.ActualValue = businessCompanyName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_AddressFull")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessAddress,
                                    ActualValue = businessAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessAddress;
                                updateJobDocumentField.ActualValue = businessAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessCity,
                                    ActualValue = businessCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessCity;
                                updateJobDocumentField.ActualValue = businessCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessState,
                                    ActualValue = businessState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessState;
                                updateJobDocumentField.ActualValue = businessState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessZip,
                                    ActualValue = businessZip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessZip;
                                updateJobDocumentField.ActualValue = businessZip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessPhone,
                                    ActualValue = businessPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessPhone;
                                updateJobDocumentField.ActualValue = businessPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_Email")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactEmail,
                                    ActualValue = contactEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contactEmail;
                                updateJobDocumentField.ActualValue = contactEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCompany,
                                    ActualValue = filingCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingCompany;
                                updateJobDocumentField.ActualValue = filingCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingAddress,
                                    ActualValue = filingAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingAddress;
                                updateJobDocumentField.ActualValue = filingAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCity,
                                    ActualValue = filingCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingCity;
                                updateJobDocumentField.ActualValue = filingCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingState,
                                    ActualValue = filingState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingState;
                                updateJobDocumentField.ActualValue = filingState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingZipCode,
                                    ActualValue = filingZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingZipCode;
                                updateJobDocumentField.ActualValue = filingZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingPhone,
                                    ActualValue = filingPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingPhone;
                                updateJobDocumentField.ActualValue = filingPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingEmail,
                                    ActualValue = filingEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingEmail;
                                updateJobDocumentField.ActualValue = filingEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkApplicants")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3Proposed")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedB")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedC")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedD")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedE")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3Request")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3Aware")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLic_No")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = licenseNumber,
                                    ActualValue = licenseNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = licenseNumber;
                                updateJobDocumentField.ActualValue = licenseNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDate")
                        {
                            DateTime date = DateTime.Now;
                            //string format = "MM/dd/yy";

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
                        else if (item.Field.FieldName == "chk4Proposed")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4LPCViolations")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4ApplicationComplete")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4ArchitectEngineer")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4Modification")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4RemedialMeasures")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Company")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            string ownerCompany = address != null && address.Company != null ? address.Company.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCompany,
                                    ActualValue = ownerCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerCompany;
                                updateJobDocumentField.ActualValue = ownerCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Address")
                        {
                            string ownerAddress = string.Empty;
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            ownerAddress = address != null ? address.Address1 + " " + address.Address2 : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerAddress,
                                    ActualValue = ownerAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerAddress;
                                updateJobDocumentField.ActualValue = ownerAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_City")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            string ownerCity = address != null ? address.City : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCity,
                                    ActualValue = ownerCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerCity;
                                updateJobDocumentField.ActualValue = ownerCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_State")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            string ownerState = address != null & address.State != null ? address.State.Acronym : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerState,
                                    ActualValue = ownerState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerState;
                                updateJobDocumentField.ActualValue = ownerState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "txtOwner_Zip")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            string ownerZipCode = address != null ? address.ZipCode : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerZipCode,
                                    ActualValue = ownerZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerZipCode;
                                updateJobDocumentField.ActualValue = ownerZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_EMail")
                        {
                            string ownerEmail = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerEmail,
                                    ActualValue = ownerEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerEmail;
                                updateJobDocumentField.ActualValue = ownerEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Phone")
                        {
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (ownerPhone == null || ownerPhone == "")
                            {
                                ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.MobilePhone : string.Empty;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Date")
                        {
                            DateTime date = DateTime.Now;
                            //string format = "MM/dd/yy";

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
                        if (item.Field.FieldName == "txtAE_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobContact jobContact = rpoContext.JobContacts.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string fullName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;

                            //Address compnayAddress = jobContact != null && jobContact.Company != null && jobContact.Company.Addresses != null ? jobContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            Address compnayAddress = Common.GetContactAddressForJobDocument(jobContact);

                            businessCompanyName = jobContact != null && jobContact.Company != null ? jobContact.Company.Name : string.Empty;
                            //businessPhone = compnayAddress != null ? compnayAddress.Phone : string.Empty;
                            businessPhone = Common.GetContactPhoneNumberForJobDocument(jobContact);
                            businessAddress = compnayAddress != null ? compnayAddress.Address1 + " " + compnayAddress.Address2 : string.Empty;
                            businessCity = compnayAddress != null ? compnayAddress.City : string.Empty;
                            businessState = compnayAddress != null && compnayAddress.State != null ? compnayAddress.State.Acronym : string.Empty;
                            businessZip = compnayAddress != null ? compnayAddress.ZipCode : string.Empty;
                            contactMobilePhone = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                            contactEmail = jobContact != null ? jobContact.Contact.Email : string.Empty;

                            string applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(applicantName) ? "Applicant: " + applicantName : string.Empty);

                            ContactLicense contactLicNumber = jobContact != null ? jobContact.Contact.ContactLicenses.FirstOrDefault() : null;
                            licenseNumber = contactLicNumber != null ? contactLicNumber.Number : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = fullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = fullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            //Employee employee = rpoContext.Employees.Where(x => x.Id == Certifierid).FirstOrDefault();
                            Employee employee = Common.GetEmployeeInformation(Certifierid);
                            string filingFullName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;
                            filingCompany = "RPO Inc";
                            filingPhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingEmail = employee != null ? employee.Email : string.Empty;

                            filingAddress = employee != null ? employee.Address1 + " " + employee.Address2 : string.Empty;
                            filingCity = employee != null ? employee.City : string.Empty;
                            filingState = employee != null && employee.State != null ? employee.State.Acronym : string.Empty;
                            filingZipCode = employee != null ? employee.ZipCode : string.Empty;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(filingFullName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filingFullName) ? "Filing Representative: " + filingFullName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = filingFullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingFullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application Type")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string applicationNumber = (jobApplication != null) ? jobApplication.ApplicationNumber : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicationNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Description")
                        {
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(frontendFields.Value) ? " | " : string.Empty) + (!string.IsNullOrEmpty(frontendFields.Value) ? "Optional Additional Description: " + frontendFields.Value : string.Empty);
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

        public static void EditExpeditedCertificateNoEffectApplication2021(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                string businessCompanyName = string.Empty;
                string businessPhone = string.Empty;
                string businessAddress = string.Empty;
                string businessFax = string.Empty;
                string businessCity = string.Empty;
                string businessState = string.Empty;
                string businessZip = string.Empty;
                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string floorWorkingOn = string.Empty;
                string licenseNumber = string.Empty;

                string filingCompany = string.Empty;
                string filingPhone = string.Empty;
                string filingEmail = string.Empty;
                string filingAddress = string.Empty;
                string filingCity = string.Empty;
                string filingState = string.Empty;
                string filingZipCode = string.Empty;

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string address = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = address,
                                    ActualValue = address
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = address;
                                updateJobDocumentField.ActualValue = address;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFloorApt")
                        {
                            string stories = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = stories,
                                    ActualValue = stories
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = stories;
                                updateJobDocumentField.ActualValue = stories;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtBorough")
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
                        else if (item.Field.FieldName == "txtBlock")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLot")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessCompanyName,
                                    ActualValue = businessCompanyName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessCompanyName;
                                updateJobDocumentField.ActualValue = businessCompanyName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_AddressFull")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessAddress,
                                    ActualValue = businessAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessAddress;
                                updateJobDocumentField.ActualValue = businessAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessCity,
                                    ActualValue = businessCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessCity;
                                updateJobDocumentField.ActualValue = businessCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessState,
                                    ActualValue = businessState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessState;
                                updateJobDocumentField.ActualValue = businessState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessZip,
                                    ActualValue = businessZip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessZip;
                                updateJobDocumentField.ActualValue = businessZip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessPhone,
                                    ActualValue = businessPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessPhone;
                                updateJobDocumentField.ActualValue = businessPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtAE_Email")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactEmail,
                                    ActualValue = contactEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = contactEmail;
                                updateJobDocumentField.ActualValue = contactEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCompany,
                                    ActualValue = filingCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingCompany;
                                updateJobDocumentField.ActualValue = filingCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingAddress,
                                    ActualValue = filingAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingAddress;
                                updateJobDocumentField.ActualValue = filingAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCity,
                                    ActualValue = filingCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingCity;
                                updateJobDocumentField.ActualValue = filingCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingState,
                                    ActualValue = filingState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingState;
                                updateJobDocumentField.ActualValue = filingState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingZipCode,
                                    ActualValue = filingZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingZipCode;
                                updateJobDocumentField.ActualValue = filingZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingPhone,
                                    ActualValue = filingPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingPhone;
                                updateJobDocumentField.ActualValue = filingPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingEmail,
                                    ActualValue = filingEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingEmail;
                                updateJobDocumentField.ActualValue = filingEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkApplicants")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3Proposed")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedB")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedC")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedD")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3ProposedE")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3Request")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk3Aware")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLic_No")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = licenseNumber,
                                    ActualValue = licenseNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = licenseNumber;
                                updateJobDocumentField.ActualValue = licenseNumber;
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
                            DateTime date = DateTime.Now;
                            //string format = "MM/dd/yy";

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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4Proposed")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4LPCViolations")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4ApplicationComplete")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4ArchitectEngineer")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4Modification")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chk4RemedialMeasures")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Company")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            string ownerCompany = address != null && address.Company != null ? address.Company.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCompany,
                                    ActualValue = ownerCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerCompany;
                                updateJobDocumentField.ActualValue = ownerCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Address")
                        {
                            string ownerAddress = string.Empty;
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            ownerAddress = address != null ? address.Address1 + " " + address.Address2 : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerAddress,
                                    ActualValue = ownerAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerAddress;
                                updateJobDocumentField.ActualValue = ownerAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_City")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            string ownerCity = address != null ? address.City : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCity,
                                    ActualValue = ownerCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerCity;
                                updateJobDocumentField.ActualValue = ownerCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_State")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            string ownerState = address != null & address.State != null ? address.State.Acronym : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerState,
                                    ActualValue = ownerState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerState;
                                updateJobDocumentField.ActualValue = ownerState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "txtOwner_Zip")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            string ownerZipCode = address != null ? address.ZipCode : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerZipCode,
                                    ActualValue = ownerZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerZipCode;
                                updateJobDocumentField.ActualValue = ownerZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_EMail")
                        {
                            string ownerEmail = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerEmail,
                                    ActualValue = ownerEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerEmail;
                                updateJobDocumentField.ActualValue = ownerEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Phone")
                        {
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (ownerPhone == null || ownerPhone == "")
                            {
                                ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.MobilePhone : string.Empty;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Date")
                        {
                            DateTime date = DateTime.Now;
                            //string format = "MM/dd/yy";

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
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = string.Empty;
                                updateJobDocumentField.ActualValue = string.Empty;
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
                        if (item.Field.FieldName == "txtAE_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobContact jobContact = rpoContext.JobContacts.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string fullName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;

                            //Address compnayAddress = jobContact != null && jobContact.Company != null && jobContact.Company.Addresses != null ? jobContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            Address compnayAddress = Common.GetContactAddressForJobDocument(jobContact);

                            businessCompanyName = jobContact != null && jobContact.Company != null ? jobContact.Company.Name : string.Empty;
                            //businessPhone = compnayAddress != null ? compnayAddress.Phone : string.Empty;
                            businessPhone = Common.GetContactPhoneNumberForJobDocument(jobContact);
                            businessAddress = compnayAddress != null ? compnayAddress.Address1 + " " + compnayAddress.Address2 : string.Empty;
                            businessCity = compnayAddress != null ? compnayAddress.City : string.Empty;
                            businessState = compnayAddress != null && compnayAddress.State != null ? compnayAddress.State.Acronym : string.Empty;
                            businessZip = compnayAddress != null ? compnayAddress.ZipCode : string.Empty;
                            contactMobilePhone = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                            contactEmail = jobContact != null ? jobContact.Contact.Email : string.Empty;

                            string applicantName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName + " " + jobContact.Contact.LastName : string.Empty;
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(applicantName) ? "Applicant: " + applicantName : string.Empty);

                            ContactLicense contactLicNumber = jobContact != null ? jobContact.Contact.ContactLicenses.FirstOrDefault() : null;
                            licenseNumber = contactLicNumber != null ? contactLicNumber.Number : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = fullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = fullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            //Employee employee = rpoContext.Employees.Where(x => x.Id == Certifierid).FirstOrDefault();
                            Employee employee = Common.GetEmployeeInformation(Certifierid);
                            string filingFullName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;
                            filingCompany = "RPO Inc";
                            filingPhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingEmail = employee != null ? employee.Email : string.Empty;

                            filingAddress = employee != null ? employee.Address1 + " " + employee.Address2 : string.Empty;
                            filingCity = employee != null ? employee.City : string.Empty;
                            filingState = employee != null && employee.State != null ? employee.State.Acronym : string.Empty;
                            filingZipCode = employee != null ? employee.ZipCode : string.Empty;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(filingFullName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filingFullName) ? "Filing Representative: " + filingFullName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = filingFullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingFullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application Type")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string applicationNumber = (jobApplication != null) ? jobApplication.ApplicationNumber : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
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
                        else if (item.Field.FieldName == "Description")
                        {
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(frontendFields.Value) ? " | " : string.Empty) + (!string.IsNullOrEmpty(frontendFields.Value) ? "Optional Additional Description: " + frontendFields.Value : string.Empty);
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
        public static void GeneratePostApprovalApplication(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string floorWorkingOn = string.Empty;
                string licenseNumber = string.Empty;

                string filingCompany = string.Empty;
                string filingPhone = string.Empty;
                string filingEmail = string.Empty;
                string filingAddress = string.Empty;
                string filingCity = string.Empty;
                string filingState = string.Empty;
                string filingZipCode = string.Empty;

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string address = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = address,
                                    ActualValue = address
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = address;
                                updateJobDocumentField.ActualValue = address;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFloorApt")
                        {
                            string floor = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
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
                        else if (item.Field.FieldName == "txtBorough")
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
                        else if (item.Field.FieldName == "txtBlock")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLot")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkApplicant")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCompany,
                                    ActualValue = filingCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingCompany;
                                updateJobDocumentField.ActualValue = filingCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingAddress,
                                    ActualValue = filingAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingAddress;
                                updateJobDocumentField.ActualValue = filingAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCity,
                                    ActualValue = filingCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingCity;
                                updateJobDocumentField.ActualValue = filingCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingState,
                                    ActualValue = filingState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingState;
                                updateJobDocumentField.ActualValue = filingState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingZipCode,
                                    ActualValue = filingZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingZipCode;
                                updateJobDocumentField.ActualValue = filingZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingPhone,
                                    ActualValue = filingPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingPhone;
                                updateJobDocumentField.ActualValue = filingPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingEmail,
                                    ActualValue = filingEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingEmail;
                                updateJobDocumentField.ActualValue = filingEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name2")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name3")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Title2")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Company")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            string ownerCompany = address != null && address.Company != null ? address.Company.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCompany,
                                    ActualValue = ownerCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerCompany;
                                updateJobDocumentField.ActualValue = ownerCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Address")
                        {
                            string ownerAddress = string.Empty;
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            ownerAddress = address != null ? address.Address1 + " " + address.Address2 : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerAddress,
                                    ActualValue = ownerAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerAddress;
                                updateJobDocumentField.ActualValue = ownerAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_City")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            string ownerCity = address != null ? address.City : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCity,
                                    ActualValue = ownerCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerCity;
                                updateJobDocumentField.ActualValue = ownerCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_State")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            string ownerState = address != null & address.State != null ? address.State.Acronym : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerState,
                                    ActualValue = ownerState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerState;
                                updateJobDocumentField.ActualValue = ownerState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "txtOwner_Zip")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            string ownerZipCode = address != null ? address.ZipCode : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerZipCode,
                                    ActualValue = ownerZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerZipCode;
                                updateJobDocumentField.ActualValue = ownerZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_EMail")
                        {
                            string ownerEmail = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerEmail,
                                    ActualValue = ownerEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerEmail;
                                updateJobDocumentField.ActualValue = ownerEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Phone")
                        {
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (ownerPhone == null || ownerPhone == "")
                            {
                                ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.MobilePhone : string.Empty;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "txtFil_Rep_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            Employee employee = Common.GetEmployeeInformation(Certifierid);

                            string filingFullName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;
                            filingCompany = "RPO INC";
                            filingPhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingEmail = employee != null ? employee.Email : string.Empty;

                            //Address contactAddress = employee != null && employee.Company != null&& contact.Company.Addresses != null ? contact.Company.Addresses.Where(x => x.IsMainAddress == true).FirstOrDefault() : null;
                            filingAddress = employee != null ? employee.Address1 + " " + employee.Address2 : string.Empty;
                            filingCity = employee != null ? employee.City : string.Empty;
                            filingState = employee != null && employee.State != null ? employee.State.Acronym : string.Empty;
                            filingZipCode = employee != null ? employee.ZipCode : string.Empty;

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(filingFullName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filingFullName) ? "Filing Representative: " + filingFullName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = filingFullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingFullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application Type")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string applicationNumber = (jobApplication != null) ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = (jobApplication != null) && !string.IsNullOrEmpty(jobApplication.FloorWorking) ? jobApplication.FloorWorking : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicationNumber;
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
        public static void EditPostApprovalApplication(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            List<IdItemName> FieldsValue = new List<IdItemName>();
            List<string> pdfFields = new List<string>();
            List<int> DocumentFieldsIds = new List<int>();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string floorWorkingOn = string.Empty;
                string licenseNumber = string.Empty;

                string filingCompany = string.Empty;
                string filingPhone = string.Empty;
                string filingEmail = string.Empty;
                string filingAddress = string.Empty;
                string filingCity = string.Empty;
                string filingState = string.Empty;
                string filingZipCode = string.Empty;

                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "txtAddress")
                        {
                            string address = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = address,
                                    ActualValue = address
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = address;
                                updateJobDocumentField.ActualValue = address;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFloorApt")
                        {
                            string floor = job != null && job.RfpAddress != null ? Convert.ToString(job.RfpAddress.Stories) : string.Empty;
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
                        else if (item.Field.FieldName == "txtBorough")
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
                        else if (item.Field.FieldName == "txtBlock")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLot")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkApplicant")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = "Yes",
                                    ActualValue = "Yes"
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = "Yes";
                                updateJobDocumentField.ActualValue = "Yes";
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Company")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCompany,
                                    ActualValue = filingCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingCompany;
                                updateJobDocumentField.ActualValue = filingCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingAddress,
                                    ActualValue = filingAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingAddress;
                                updateJobDocumentField.ActualValue = filingAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingCity,
                                    ActualValue = filingCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingCity;
                                updateJobDocumentField.ActualValue = filingCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingState,
                                    ActualValue = filingState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingState;
                                updateJobDocumentField.ActualValue = filingState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingZipCode,
                                    ActualValue = filingZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingZipCode;
                                updateJobDocumentField.ActualValue = filingZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_Phone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingPhone,
                                    ActualValue = filingPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingPhone;
                                updateJobDocumentField.ActualValue = filingPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFil_Rep_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingEmail,
                                    ActualValue = filingEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingEmail;
                                updateJobDocumentField.ActualValue = filingEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Name")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name2")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Name3")
                        {
                            string ownerName = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;
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
                        else if (item.Field.FieldName == "txtOwner_Title")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Title2")
                        {
                            string ownerTitle = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerTitle,
                                    ActualValue = ownerTitle
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerTitle;
                                updateJobDocumentField.ActualValue = ownerTitle;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Company")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            string ownerCompany = address != null && address.Company != null ? address.Company.Name : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCompany,
                                    ActualValue = ownerCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerCompany;
                                updateJobDocumentField.ActualValue = ownerCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Address")
                        {
                            string ownerAddress = string.Empty;
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            ownerAddress = address != null ? address.Address1 + " " + address.Address2 : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerAddress,
                                    ActualValue = ownerAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerAddress;
                                updateJobDocumentField.ActualValue = ownerAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_City")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }

                            string ownerCity = address != null ? address.City : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerCity,
                                    ActualValue = ownerCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerCity;
                                updateJobDocumentField.ActualValue = ownerCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_State")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            string ownerState = address != null & address.State != null ? address.State.Acronym : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerState,
                                    ActualValue = ownerState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerState;
                                updateJobDocumentField.ActualValue = ownerState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "txtOwner_Zip")
                        {
                            Address address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null
                  ? job.RfpAddress.OwnerContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            if (address == null)
                            {
                                address = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            }
                            string ownerZipCode = address != null ? address.ZipCode : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerZipCode,
                                    ActualValue = ownerZipCode
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerZipCode;
                                updateJobDocumentField.ActualValue = ownerZipCode;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_EMail")
                        {
                            string ownerEmail = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerEmail,
                                    ActualValue = ownerEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerEmail;
                                updateJobDocumentField.ActualValue = ownerEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOwner_Phone")
                        {
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (ownerPhone == null || ownerPhone == "")
                            {
                                ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.MobilePhone : string.Empty;
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
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
                        if (item.Field.FieldName == "txtFil_Rep_Name")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            Employee employee = Common.GetEmployeeInformation(Certifierid);

                            string filingFullName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;
                            filingCompany = "RPO INC";
                            filingPhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingEmail = employee != null ? employee.Email : string.Empty;

                            //Address contactAddress = employee != null && employee.Company != null&& contact.Company.Addresses != null ? contact.Company.Addresses.Where(x => x.IsMainAddress == true).FirstOrDefault() : null;
                            filingAddress = employee != null ? employee.Address1 + " " + employee.Address2 : string.Empty;
                            filingCity = employee != null ? employee.City : string.Empty;
                            filingState = employee != null && employee.State != null ? employee.State.Acronym : string.Empty;
                            filingZipCode = employee != null ? employee.ZipCode : string.Empty;
                            
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(filingFullName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filingFullName) ? "Filing Representative: " + filingFullName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = filingFullName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingFullName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Application Type")
                        {
                            int Certifierid = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == Certifierid).FirstOrDefault();

                            string applicationNumber = (jobApplication != null) ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = (jobApplication != null) && !string.IsNullOrEmpty(jobApplication.FloorWorking) ? jobApplication.FloorWorking : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
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
            List<string> newpdfString = pdfFields;
            List<int> newDocumentFieldId = DocumentFieldsIds;
            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateEditedJobDocument(idJobDocument, pdfFields, newDocumentFieldId, FieldsValue);
        }
        //ML
        public static void GeneratePostTopoApplicationSet2021(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocumentCreateOrUpdateDTO.IdJob).FirstOrDefault();

                string contactName = string.Empty;
                string licenceName = string.Empty;
                string contactName2 = string.Empty;
                string companyName2 = string.Empty;
                string location2 = string.Empty;
                string docuementType = string.Empty;
                string floorWorkingOn = string.Empty;
                string applicantFirstName = string.Empty;
                string applicantMiddleName = string.Empty;
                string applicantLastName = string.Empty;
                string contactcompany = string.Empty;
                string businessAddress = string.Empty;
                //   string businessAddress = string.Empty;
                string businessCity = string.Empty;
                string businessState = string.Empty;
                string businessZip = string.Empty;
                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string contactTitle = string.Empty;
                string documentType = string.Empty;
                string documentName = string.Empty;
                //   string contactcompany = string.Empty;
                string applicantmobilenumber = string.Empty;
                string contactLic_state_engineer = string.Empty;
                string contactLic_state_architect = string.Empty;
                string contactLic_No = string.Empty;
                string filingRepresentativeCompany = string.Empty;
                string filingRepresentativePhone = string.Empty;
                string filingRepresentativeEmail = string.Empty;

                string filingRepresentativeCompany2 = string.Empty;
                string filingRepresentativePhone2 = string.Empty;
                string filingRepresentativeEmail2 = string.Empty;


                string filingRepresentativeCompany3 = string.Empty;
                string filingRepresentativePhone3 = string.Empty;
                string filingRepresentativeEmail3 = string.Empty;


                string filingRepresentativefirstname = string.Empty;
                string frfirstname = string.Empty;
                string frLastname = string.Empty;
                string filingRepresentative2firstname = string.Empty;
                string filingRepresentativef3irstname = string.Empty;
                string filingRepresentativelastname = string.Empty;
                string filingRepresentative2lastname = string.Empty;
                string filingRepresentative3lastname = string.Empty;
                string filingRepresentativeaddress = string.Empty;
                string filingRepresentativecity = string.Empty;
                string filingRepresentativestate = string.Empty;
                string filingRepresentativezip = string.Empty;
                string filingRepresentativemobile = string.Empty;
                string filingRepresentativeemail = string.Empty;
                string filingRepresentativRegnumber = string.Empty;
                string[] permitTypecodeArr = new string[20];
                string permitTypecode = string.Empty;
                string estfees = string.Empty;
                string LR_Name = string.Empty;
                string LR_Relationship = string.Empty;
                string LR_Company = string.Empty;
                string LR_Address = string.Empty;
                string LR_City = string.Empty;
                string LR_State = string.Empty;
                string LR_Zip = string.Empty;
                string LR_Phone = string.Empty;
                string LR_Fax = string.Empty;
                string LR_EMail = string.Empty;
                string applicantName = string.Empty;
                string comments = string.Empty;
                string applicationNumber = string.Empty;
                string ApplicationType = string.Empty;
                string desc = string.Empty;
                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "House Number")
                        {
                            string houseNumber = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = houseNumber,
                                    ActualValue = houseNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = houseNumber;
                                updateJobDocumentField.ActualValue = houseNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Street Name")
                        {
                            string houseStreet = job != null && job.RfpAddress != null ? job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = houseStreet,
                                    ActualValue = houseStreet
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = houseStreet;
                                updateJobDocumentField.ActualValue = houseStreet;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "BIN")
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
                        else if (item.Field.FieldName == "Block")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Lot")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Borough")
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
                        else if (item.Field.FieldName == "DOB Job Number")
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
                        else if (item.Field.FieldName == "AptCondo Nos")
                        {
                            string aptNo = job != null && job.RfpAddress != null ? job.Apartment : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = aptNo,
                                    ActualValue = aptNo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = aptNo;
                                updateJobDocumentField.ActualValue = aptNo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Work on Floors")
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
                        else if (item.Field.FieldName == "CBNo")
                        {
                            string comunityBoardNumber = job != null && job.RfpAddress != null ? job.RfpAddress.ComunityBoardNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = comunityBoardNumber,
                                    ActualValue = comunityBoardNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = comunityBoardNumber;
                                updateJobDocumentField.ActualValue = comunityBoardNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "First Name")
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

                        else if (item.Field.FieldName == "License Number")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactLic_No,
                                    ActualValue = contactLic_No
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contactLic_No;
                                updateJobDocumentField.ActualValue = contactLic_No;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Name_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactcompany,
                                    ActualValue = contactcompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contactcompany;
                                updateJobDocumentField.ActualValue = contactcompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Address_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessAddress,
                                    ActualValue = businessAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessAddress;
                                updateJobDocumentField.ActualValue = businessAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "City_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessCity,
                                    ActualValue = businessCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessCity;
                                updateJobDocumentField.ActualValue = businessCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Telephone_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactMobilePhone,
                                    ActualValue = contactMobilePhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contactMobilePhone;
                                updateJobDocumentField.ActualValue = contactMobilePhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Cell Number_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantmobilenumber,
                                    ActualValue = applicantmobilenumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = applicantmobilenumber;
                                updateJobDocumentField.ActualValue = applicantmobilenumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Email Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactEmail,
                                    ActualValue = contactEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contactEmail;
                                updateJobDocumentField.ActualValue = contactEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "State_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessState,
                                    ActualValue = businessState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessState;
                                updateJobDocumentField.ActualValue = businessState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Zip_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessZip,
                                    ActualValue = businessZip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = businessZip;
                                updateJobDocumentField.ActualValue = businessZip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "PE")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactLic_state_engineer,
                                    ActualValue = contactLic_state_engineer
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contactLic_state_engineer;
                                updateJobDocumentField.ActualValue = contactLic_state_engineer;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "RA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactLic_state_architect,
                                    ActualValue = contactLic_state_architect
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contactLic_state_architect;
                                updateJobDocumentField.ActualValue = contactLic_state_architect;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "First Name_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativelastname,
                                    ActualValue = filingRepresentativelastname
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativelastname;
                                updateJobDocumentField.ActualValue = filingRepresentativelastname;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Name_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeCompany,
                                    ActualValue = filingRepresentativeCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativeCompany;
                                updateJobDocumentField.ActualValue = filingRepresentativeCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Registration Number")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativRegnumber,
                                    ActualValue = filingRepresentativRegnumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativRegnumber;
                                updateJobDocumentField.ActualValue = filingRepresentativRegnumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Email Address_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeEmail,
                                    ActualValue = filingRepresentativeEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativeEmail;
                                updateJobDocumentField.ActualValue = filingRepresentativeEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Telephone_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativePhone,
                                    ActualValue = filingRepresentativePhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativePhone;
                                updateJobDocumentField.ActualValue = filingRepresentativePhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Cell Number_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativemobile,
                                    ActualValue = filingRepresentativemobile
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativemobile;
                                updateJobDocumentField.ActualValue = filingRepresentativemobile;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Address_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeaddress,
                                    ActualValue = filingRepresentativeaddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativeaddress;
                                updateJobDocumentField.ActualValue = filingRepresentativeaddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "City_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativecity,
                                    ActualValue = filingRepresentativecity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativecity;
                                updateJobDocumentField.ActualValue = filingRepresentativecity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "State_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativestate,
                                    ActualValue = filingRepresentativestate
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativestate;
                                updateJobDocumentField.ActualValue = filingRepresentativestate;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Zip_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativezip,
                                    ActualValue = filingRepresentativezip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativezip;
                                updateJobDocumentField.ActualValue = filingRepresentativezip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Name")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeCompany,
                                    ActualValue = filingRepresentativeCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativeCompany;
                                updateJobDocumentField.ActualValue = filingRepresentativeCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Telephone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativePhone,
                                    ActualValue = filingRepresentativePhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativePhone;
                                updateJobDocumentField.ActualValue = filingRepresentativePhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Cell Number")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativemobile,
                                    ActualValue = filingRepresentativemobile
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativemobile;
                                updateJobDocumentField.ActualValue = filingRepresentativemobile;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeaddress,
                                    ActualValue = filingRepresentativeaddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativeaddress;
                                updateJobDocumentField.ActualValue = filingRepresentativeaddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativecity,
                                    ActualValue = filingRepresentativecity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativecity;
                                updateJobDocumentField.ActualValue = filingRepresentativecity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativestate,
                                    ActualValue = filingRepresentativestate
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativestate;
                                updateJobDocumentField.ActualValue = filingRepresentativestate;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativezip,
                                    ActualValue = filingRepresentativezip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativezip;
                                updateJobDocumentField.ActualValue = filingRepresentativezip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "EMAIL for Pick Up notification")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeEmail,
                                    ActualValue = filingRepresentativeEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = filingRepresentativeEmail;
                                updateJobDocumentField.ActualValue = filingRepresentativeEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "Name please print")
                        {
                            string val = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Relationship to owner")
                        {
                            string val = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Telephone")
                        {
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (ownerPhone == null || ownerPhone == "")
                            {
                                ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.MobilePhone : string.Empty;
                            }

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "undefined_20")
                        {
                            Contact owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                            string val = owner != null && owner.Company != null ? owner.Company.Name : string.Empty;


                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business NameAgency")
                        {
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (ownerAddress == null)
                            {
                                if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress == true)
                                {
                                    int? company_id = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.IdCompany : 0;
                                    ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.Company != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IdCompany == company_id) : null;
                                }
                            }
                            string val = ownerAddress != null ? ownerAddress.Address1 + " " + ownerAddress.Address2 : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Street Address")
                        {
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress == true)
                            {
                                int? company_id = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.IdCompany : 0;
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.Company != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IdCompany == company_id) : null;
                            }
                            string val = ownerAddress != null ? ownerAddress.City : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "State_4")
                        {
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress == true)
                            {
                                int? company_id = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.IdCompany : 0;
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.Company != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IdCompany == company_id) : null;
                            }

                            string val = ownerAddress != null && ownerAddress.State != null ? ownerAddress.State.Acronym : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "undefined_21")
                        {
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress == true)
                            {
                                int? company_id = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.IdCompany : 0;
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.Company != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IdCompany == company_id) : null;
                            }

                            string val = ownerAddress != null ? ownerAddress.ZipCode : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Zip_4")
                        {
                            string val = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "TenantShareholder")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "INDIVIDUAL")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Individual")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "INDIVIDUAL")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Partnership Corporation")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;

                            if (ownertype.ToUpper() == "PARTNERSHIP")
                            {
                                val = "On";
                            }


                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "CondoCoOp")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;

                            if (ownertype.ToLower() == "Condo/Co-Op".ToLower())
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "NYCHAHHCSCA")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "NYCHA / HHC")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "NYC Agency")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "NYC AGENCY")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Other Government Agency")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "GOV-OTHER")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "Application")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);
                            // ApplicationType = jobApplication != null && jobApplication.JobApplicationType != null ? jobApplication.JobApplicationType.Description : string.Empty;
                            ApplicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();

                            applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = jobApplication != null ? jobApplication.FloorWorking : string.Empty;

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
                        else if (item.Field.FieldName == "Last Name")
                        {
                            int certifierid = Convert.ToInt32(frontendFields.Value);

                            JobContact jobContact = rpoContext.JobContacts.Where(x => x.Id == certifierid).FirstOrDefault();

                            applicantFirstName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                            applicantLastName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;
                            contactcompany = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null ? jobContact.Contact.Company.Name : string.Empty;
                            // Address compnayAddress = jobContact != null && jobContact.Company != null && jobContact.Company.Addresses != null ? jobContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            Address compnayAddress = Common.GetContactAddressForJobDocument(jobContact);
                            businessAddress = compnayAddress != null ? compnayAddress.Address1 + " " + compnayAddress.Address2 : string.Empty;
                            businessCity = compnayAddress != null ? compnayAddress.City : string.Empty;
                            businessState = compnayAddress != null && compnayAddress.State != null ? compnayAddress.State.Acronym : string.Empty;
                            businessZip = compnayAddress != null ? compnayAddress.ZipCode : string.Empty;

                            contactMobilePhone = Common.GetContactPhoneNumberForJobDocument(jobContact);
                            //!= null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                            contactEmail = jobContact != null ? jobContact.Contact.Email : string.Empty;
                            applicantmobilenumber = jobContact != null ? jobContact.Contact.MobilePhone : string.Empty;

                            string licenceTypeNumber = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactLicenses != null && jobContact.Contact.ContactLicenses.Count > 0 ?
                                 jobContact.Contact.ContactLicenses.Select(x => x.Number).FirstOrDefault() : string.Empty;
                            if (!string.IsNullOrEmpty(licenceTypeNumber))
                            {
                                ContactLicense contactLicenseType = rpoContext.ContactLicenses.Where(a => a.Number == licenceTypeNumber).FirstOrDefault();
                                ContactLicenseType LicenseType = rpoContext.ContactLicenseTypes.Where(b => b.Id == contactLicenseType.IdContactLicenseType).FirstOrDefault();
                                if (LicenseType.Name == "Engineer")
                                {
                                    contactLic_state_engineer = "On";
                                    contactLic_No = licenceTypeNumber;
                                }
                                else if (LicenseType.Name == "Architect")
                                {
                                    contactLic_state_architect = "On";
                                    contactLic_No = licenceTypeNumber;
                                }
                            }
                            else
                            {
                                licenceTypeNumber = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactLicenses != null && jobContact.Contact.ContactLicenses.Count > 0 ?
                                jobContact.Contact.ContactLicenses.Select(x => x.ContactLicenseType.Name).FirstOrDefault() : string.Empty;
                                if (licenceTypeNumber.ToLower() == ("Engineer").ToLower())
                                {
                                    contactLic_state_engineer = "On";
                                }
                                else if (licenceTypeNumber.ToLower() == ("Architect").ToLower())
                                {
                                    contactLic_state_architect = "On";
                                }
                            }
                            contactTitle = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactTitle != null ? jobContact.Contact.ContactTitle.Name : string.Empty;
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(applicantFirstName + " " + applicantLastName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(applicantFirstName + " " + applicantLastName) ? "Applicant: " + applicantFirstName + " " + applicantLastName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicantLastName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicantLastName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "Name of Representative dropping off applications please print")
                        {
                            int idFilingRepresentative = Convert.ToInt32(frontendFields.Value);
                            //Employee employee = rpoContext.Employees.Where(x => x.Id == idFilingRepresentative).FirstOrDefault();
                            Employee employee = Common.GetEmployeeInformation(idFilingRepresentative);

                            //filingRepresentativefirstname = employee != null ? employee.FirstName : string.Empty;
                            //filingRepresentativelastname = employee != null ? employee.LastName : string.Empty;
                            frfirstname = employee != null ? employee.FirstName : string.Empty;
                            frLastname = employee != null ? employee.LastName : string.Empty;

                            filingRepresentativeCompany = "RPO INC";
                            filingRepresentativeaddress = employee != null ? employee.Address1 : string.Empty;
                            filingRepresentativePhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingRepresentativeEmail = employee != null ? employee.Email : string.Empty;
                            filingRepresentativestate = employee != null ? employee.State.Acronym : string.Empty;
                            filingRepresentativecity = employee != null ? employee.City : string.Empty;
                            filingRepresentativezip = employee != null ? employee.ZipCode : string.Empty;
                            filingRepresentativemobile = employee != null ? employee.MobilePhone : string.Empty;
                            //  filingRepresentativeEmail = employee != null ? employee.Email : string.Empty;

                            string filingRepresentative = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;

                            AgentCertificate agentCertificate = rpoContext.AgentCertificates
                            .Include("DocumentType").FirstOrDefault(x => x.IdEmployee == idFilingRepresentative && x.DocumentType.Name.Equals("DOB Filing Representative"));
                            filingRepresentativRegnumber = agentCertificate != null ? agentCertificate.NumberId : string.Empty;
                            // documentDescription = documentDescription + (!string.IsNullOrEmpty(filingRepresentative) ? "Filing Representative: " + filingRepresentative : string.Empty);

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(frfirstname) && !string.IsNullOrEmpty(frLastname) ? " | Filing Representative: " + frfirstname + " " + frLastname : string.Empty);
                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = filingRepresentative,

                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingRepresentative;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Last Name_2")
                        {
                            int idFilingRepresentative = Convert.ToInt32(frontendFields.Value);
                            //Employee employee = rpoContext.Employees.Where(x => x.Id == idFilingRepresentative).FirstOrDefault();
                            if (idFilingRepresentative != 0)
                            {
                                Employee employee = Common.GetEmployeeInformation(idFilingRepresentative);

                                filingRepresentativefirstname = filingRepresentativefirstname + (employee != null ? (!string.IsNullOrEmpty(filingRepresentativefirstname) ? "/" : string.Empty) + employee.FirstName : string.Empty);
                                filingRepresentativelastname = filingRepresentativelastname + (employee != null ? (!string.IsNullOrEmpty(filingRepresentativelastname) ? "/" : string.Empty) + employee.LastName : string.Empty);
                                frfirstname = employee != null ? employee.FirstName : string.Empty;
                                frLastname = employee != null ? employee.LastName : string.Empty;

                                filingRepresentativeCompany = "RPO INC";
                                filingRepresentativePhone = employee != null ? employee.WorkPhone : string.Empty;
                            }
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(frfirstname) && !string.IsNullOrEmpty(frLastname) ? " | Filing Representative2: " + frfirstname + " " + frLastname : string.Empty);
                            //  filingRepresentativeEmail = employee != null ? employee.Email : string.Empty;

                            //documentDescription = documentDescription + (!string.IsNullOrEmpty(filingRepresentative) ? "Filing Representative: " + filingRepresentative : string.Empty);

                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = filingRepresentativefirstname,

                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingRepresentativefirstname;
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
                    }
                }
            }
            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();
            GenerateJobDocument(idJobDocument);


        }


        public static void EditPostTopoApplicationSet2021(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;
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

                string contactName = string.Empty;
                string licenceName = string.Empty;
                string contactName2 = string.Empty;
                string companyName2 = string.Empty;
                string location2 = string.Empty;
                string docuementType = string.Empty;
                string floorWorkingOn = string.Empty;
                string applicantFirstName = string.Empty;
                string applicantMiddleName = string.Empty;
                string applicantLastName = string.Empty;
                string contactcompany = string.Empty;
                string businessAddress = string.Empty;
                //   string businessAddress = string.Empty;
                string businessCity = string.Empty;
                string businessState = string.Empty;
                string businessZip = string.Empty;
                string contactMobilePhone = string.Empty;
                string contactEmail = string.Empty;
                string contactTitle = string.Empty;
                string documentType = string.Empty;
                string documentName = string.Empty;
                //   string contactcompany = string.Empty;
                string applicantmobilenumber = string.Empty;
                string contactLic_state_engineer = string.Empty;
                string contactLic_state_architect = string.Empty;
                string contactLic_No = string.Empty;
                string filingRepresentativeCompany = string.Empty;
                string filingRepresentativePhone = string.Empty;
                string filingRepresentativeEmail = string.Empty;

                string filingRepresentativeCompany2 = string.Empty;
                string filingRepresentativePhone2 = string.Empty;
                string filingRepresentativeEmail2 = string.Empty;


                string filingRepresentativeCompany3 = string.Empty;
                string filingRepresentativePhone3 = string.Empty;
                string filingRepresentativeEmail3 = string.Empty;


                string filingRepresentativefirstname = string.Empty;
                string frfirstname = string.Empty;
                string frLastname = string.Empty;
                string filingRepresentative2firstname = string.Empty;
                string filingRepresentativef3irstname = string.Empty;
                string filingRepresentativelastname = string.Empty;
                string filingRepresentative2lastname = string.Empty;
                string filingRepresentative3lastname = string.Empty;
                string filingRepresentativeaddress = string.Empty;
                string filingRepresentativecity = string.Empty;
                string filingRepresentativestate = string.Empty;
                string filingRepresentativezip = string.Empty;
                string filingRepresentativemobile = string.Empty;
                string filingRepresentativeemail = string.Empty;
                string filingRepresentativRegnumber = string.Empty;
                string[] permitTypecodeArr = new string[20];
                string permitTypecode = string.Empty;
                string estfees = string.Empty;
                string LR_Name = string.Empty;
                string LR_Relationship = string.Empty;
                string LR_Company = string.Empty;
                string LR_Address = string.Empty;
                string LR_City = string.Empty;
                string LR_State = string.Empty;
                string LR_Zip = string.Empty;
                string LR_Phone = string.Empty;
                string LR_Fax = string.Empty;
                string LR_EMail = string.Empty;
                string applicantName = string.Empty;
                string comments = string.Empty;
                string applicationNumber = string.Empty;
                string ApplicationType = string.Empty;
                string desc = string.Empty;
                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    var frontendFields = jobDocumentCreateOrUpdateDTO.JobDocumentFields.FirstOrDefault(x => x.IdDocumentField == item.Id);
                    if (frontendFields == null)
                    {
                        if (item.Field.FieldName == "House Number")
                        {
                            string houseNumber = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = houseNumber,
                                    ActualValue = houseNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = houseNumber;
                                updateJobDocumentField.ActualValue = houseNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Street Name")
                        {
                            string houseStreet = job != null && job.RfpAddress != null ? job.RfpAddress.Street : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = houseStreet,
                                    ActualValue = houseStreet
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = houseStreet;
                                updateJobDocumentField.ActualValue = houseStreet;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "BIN")
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
                        else if (item.Field.FieldName == "Block")
                        {
                            string block = job != null && job.RfpAddress != null ? job.RfpAddress.Block : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = block,
                                    ActualValue = block
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = block;
                                updateJobDocumentField.ActualValue = block;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Lot")
                        {
                            string lot = job != null && job.RfpAddress != null ? job.RfpAddress.Lot : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = lot,
                                    ActualValue = lot
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = lot;
                                updateJobDocumentField.ActualValue = lot;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Borough")
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
                        else if (item.Field.FieldName == "DOB Job Number")
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
                        else if (item.Field.FieldName == "AptCondo Nos")
                        {
                            string aptNo = job != null && job.RfpAddress != null ? job.Apartment : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = aptNo,
                                    ActualValue = aptNo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = aptNo;
                                updateJobDocumentField.ActualValue = aptNo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Work on Floors")
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
                        else if (item.Field.FieldName == "CBNo")
                        {
                            string comunityBoardNumber = job != null && job.RfpAddress != null ? job.RfpAddress.ComunityBoardNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = comunityBoardNumber,
                                    ActualValue = comunityBoardNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = comunityBoardNumber;
                                updateJobDocumentField.ActualValue = comunityBoardNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "First Name")
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

                        else if (item.Field.FieldName == "License Number")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactLic_No,
                                    ActualValue = contactLic_No
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = contactLic_No;
                                updateJobDocumentField.ActualValue = contactLic_No;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Name_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactcompany,
                                    ActualValue = contactcompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = contactcompany;
                                updateJobDocumentField.ActualValue = contactcompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Address_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessAddress,
                                    ActualValue = businessAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessAddress;
                                updateJobDocumentField.ActualValue = businessAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "City_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessCity,
                                    ActualValue = businessCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessCity;
                                updateJobDocumentField.ActualValue = businessCity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Telephone_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactMobilePhone,
                                    ActualValue = contactMobilePhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = contactMobilePhone;
                                updateJobDocumentField.ActualValue = contactMobilePhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Cell Number_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = applicantmobilenumber,
                                    ActualValue = applicantmobilenumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = applicantmobilenumber;
                                updateJobDocumentField.ActualValue = applicantmobilenumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Email Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactEmail,
                                    ActualValue = contactEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = contactEmail;
                                updateJobDocumentField.ActualValue = contactEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "State_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessState,
                                    ActualValue = businessState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessState;
                                updateJobDocumentField.ActualValue = businessState;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Zip_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = businessZip,
                                    ActualValue = businessZip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = businessZip;
                                updateJobDocumentField.ActualValue = businessZip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "PE")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactLic_state_engineer,
                                    ActualValue = contactLic_state_engineer
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = contactLic_state_engineer;
                                updateJobDocumentField.ActualValue = contactLic_state_engineer;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "RA")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contactLic_state_architect,
                                    ActualValue = contactLic_state_architect
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = contactLic_state_architect;
                                updateJobDocumentField.ActualValue = contactLic_state_architect;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "First Name_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativelastname,
                                    ActualValue = filingRepresentativelastname
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativelastname;
                                updateJobDocumentField.ActualValue = filingRepresentativelastname;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Name_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeCompany,
                                    ActualValue = filingRepresentativeCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativeCompany;
                                updateJobDocumentField.ActualValue = filingRepresentativeCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Registration Number")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativRegnumber,
                                    ActualValue = filingRepresentativRegnumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativRegnumber;
                                updateJobDocumentField.ActualValue = filingRepresentativRegnumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Email Address_2")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeEmail,
                                    ActualValue = filingRepresentativeEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativeEmail;
                                updateJobDocumentField.ActualValue = filingRepresentativeEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Telephone_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativePhone,
                                    ActualValue = filingRepresentativePhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativePhone;
                                updateJobDocumentField.ActualValue = filingRepresentativePhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Cell Number_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativemobile,
                                    ActualValue = filingRepresentativemobile
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativemobile;
                                updateJobDocumentField.ActualValue = filingRepresentativemobile;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Address_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeaddress,
                                    ActualValue = filingRepresentativeaddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativeaddress;
                                updateJobDocumentField.ActualValue = filingRepresentativeaddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "City_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativecity,
                                    ActualValue = filingRepresentativecity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativecity;
                                updateJobDocumentField.ActualValue = filingRepresentativecity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "State_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativestate,
                                    ActualValue = filingRepresentativestate
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativestate;
                                updateJobDocumentField.ActualValue = filingRepresentativestate;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Zip_3")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativezip,
                                    ActualValue = filingRepresentativezip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativezip;
                                updateJobDocumentField.ActualValue = filingRepresentativezip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Name")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeCompany,
                                    ActualValue = filingRepresentativeCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativeCompany;
                                updateJobDocumentField.ActualValue = filingRepresentativeCompany;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Telephone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativePhone,
                                    ActualValue = filingRepresentativePhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativePhone;
                                updateJobDocumentField.ActualValue = filingRepresentativePhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Cell Number")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativemobile,
                                    ActualValue = filingRepresentativemobile
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativemobile;
                                updateJobDocumentField.ActualValue = filingRepresentativemobile;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business Address")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeaddress,
                                    ActualValue = filingRepresentativeaddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativeaddress;
                                updateJobDocumentField.ActualValue = filingRepresentativeaddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "City")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativecity,
                                    ActualValue = filingRepresentativecity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativecity;
                                updateJobDocumentField.ActualValue = filingRepresentativecity;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "State")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativestate,
                                    ActualValue = filingRepresentativestate
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativestate;
                                updateJobDocumentField.ActualValue = filingRepresentativestate;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Zip")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativezip,
                                    ActualValue = filingRepresentativezip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativezip;
                                updateJobDocumentField.ActualValue = filingRepresentativezip;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "EMAIL for Pick Up notification")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = filingRepresentativeEmail,
                                    ActualValue = filingRepresentativeEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = filingRepresentativeEmail;
                                updateJobDocumentField.ActualValue = filingRepresentativeEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "Name please print")
                        {
                            string val = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.FirstName + " " + job.RfpAddress.OwnerContact.LastName : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Relationship to owner")
                        {
                            string val = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.ContactTitle != null ? job.RfpAddress.OwnerContact.ContactTitle.Name : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Telephone")
                        {
                            string ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.WorkPhone : string.Empty;
                            if (ownerPhone == null || ownerPhone == "")
                            {
                                ownerPhone = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.MobilePhone : string.Empty;
                            }

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = ownerPhone,
                                    ActualValue = ownerPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = ownerPhone;
                                updateJobDocumentField.ActualValue = ownerPhone;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "undefined_20")
                        {
                            Contact owner = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact : null;
                            string val = owner != null && owner.Company != null ? owner.Company.Name : string.Empty;


                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Business NameAgency")
                        {
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Company != null && job.RfpAddress.OwnerContact.Company.Addresses != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (ownerAddress == null)
                            {
                                if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress == true)
                                {
                                    int? company_id = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.IdCompany : 0;
                                    ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.Company != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IdCompany == company_id) : null;
                                }
                            }
                            string val = ownerAddress != null ? ownerAddress.Address1 + " " + ownerAddress.Address2 : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Street Address")
                        {
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress == true)
                            {
                                int? company_id = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.IdCompany : 0;
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.Company != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IdCompany == company_id) : null;
                            }
                            string val = ownerAddress != null ? ownerAddress.City : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "State_4")
                        {
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress == true)
                            {
                                int? company_id = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.IdCompany : 0;
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.Company != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IdCompany == company_id) : null;
                            }

                            string val = ownerAddress != null && ownerAddress.State != null ? ownerAddress.State.Acronym : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "undefined_21")
                        {
                            Address ownerAddress = new Address();
                            ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null ? job.RfpAddress.OwnerContact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                            if (job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.IsPrimaryCompanyAddress == true)
                            {
                                int? company_id = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.IdCompany : 0;
                                ownerAddress = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null && job.RfpAddress.OwnerContact.Addresses != null && job.RfpAddress.OwnerContact.Company != null ? job.RfpAddress.OwnerContact.Company.Addresses.FirstOrDefault(x => x.IdCompany == company_id) : null;
                            }

                            string val = ownerAddress != null ? ownerAddress.ZipCode : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Zip_4")
                        {
                            string val = job != null && job.RfpAddress != null && job.RfpAddress.OwnerContact != null ? job.RfpAddress.OwnerContact.Email : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "TenantShareholder")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "INDIVIDUAL")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Individual")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "INDIVIDUAL")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Partnership Corporation")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;

                            if (ownertype.ToUpper() == "PARTNERSHIP")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "CondoCoOp")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;

                            if (ownertype.ToLower() == "Condo/Co-Op".ToLower())
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "NYCHAHHCSCA")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "NYCHA / HHC")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "NYC Agency")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "NYC AGENCY")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                FieldsValue.Add(new IdItemName
                                {
                                    Id = pdffield,
                                    ItemName = updateJobDocumentField.ActualValue
                                });
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Other Government Agency")
                        {
                            string val = string.Empty;
                            string ownertype = job != null && job.RfpAddress != null && job.RfpAddress.OwnerType != null && !string.IsNullOrEmpty(job.RfpAddress.OwnerType.Name) ? job.RfpAddress.OwnerType.Name : string.Empty;
                            if (ownertype.ToUpper() == "GOV-OTHER")
                            {
                                val = "On";
                            }
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = val,
                                    ActualValue = val
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                string pdffield = rpoContext.DocumentFields.Where(x => x.IdField == item.IdField).Select(x => x.PdfFields).FirstOrDefault();
                                pdfFields.Add(pdffield);
                                updateJobDocumentField.Value = val;
                                updateJobDocumentField.ActualValue = val;
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
                        if (item.Field.FieldName == "Application")
                        {
                            int jobApplicantId = Convert.ToInt32(frontendFields.Value);
                            JobApplication jobApplication = rpoContext.JobApplications.FirstOrDefault(x => x.Id == jobApplicantId);
                            // ApplicationType = jobApplication != null && jobApplication.JobApplicationType != null ? jobApplication.JobApplicationType.Description : string.Empty;
                            ApplicationType = rpoContext.JobApplicationTypes.Where(x => x.Id == jobApplication.IdJobApplicationType).Select(x => x.Description).FirstOrDefault();

                            applicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;
                            floorWorkingOn = jobApplication != null ? jobApplication.FloorWorking : string.Empty;

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
                        else if (item.Field.FieldName == "Last Name")
                        {
                            int certifierid = Convert.ToInt32(frontendFields.Value);

                            JobContact jobContact = rpoContext.JobContacts.Where(x => x.Id == certifierid).FirstOrDefault();

                            applicantFirstName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                            applicantLastName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;
                            contactcompany = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null ? jobContact.Contact.Company.Name : string.Empty;
                            // Address compnayAddress = jobContact != null && jobContact.Company != null && jobContact.Company.Addresses != null ? jobContact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                            Address compnayAddress = Common.GetContactAddressForJobDocument(jobContact);
                            businessAddress = compnayAddress != null ? compnayAddress.Address1 + " " + compnayAddress.Address2 : string.Empty;
                            businessCity = compnayAddress != null ? compnayAddress.City : string.Empty;
                            businessState = compnayAddress != null && compnayAddress.State != null ? compnayAddress.State.Acronym : string.Empty;
                            businessZip = compnayAddress != null ? compnayAddress.ZipCode : string.Empty;

                            contactMobilePhone = Common.GetContactPhoneNumberForJobDocument(jobContact);
                            //!= null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                            contactEmail = jobContact != null ? jobContact.Contact.Email : string.Empty;
                            applicantmobilenumber = jobContact != null ? jobContact.Contact.MobilePhone : string.Empty;

                            string licenceTypeNumber = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactLicenses != null && jobContact.Contact.ContactLicenses.Count > 0 ?
                                 jobContact.Contact.ContactLicenses.Select(x => x.Number).FirstOrDefault() : string.Empty;
                            if (!string.IsNullOrEmpty(licenceTypeNumber))
                            {
                                ContactLicense contactLicenseType = rpoContext.ContactLicenses.Where(a => a.Number == licenceTypeNumber).FirstOrDefault();
                                ContactLicenseType LicenseType = rpoContext.ContactLicenseTypes.Where(b => b.Id == contactLicenseType.IdContactLicenseType).FirstOrDefault();
                                if (LicenseType.Name == "Engineer")
                                {
                                    contactLic_state_engineer = "On";
                                    contactLic_No = licenceTypeNumber;
                                }
                                else if (LicenseType.Name == "Architect")
                                {
                                    contactLic_state_architect = "On";
                                    contactLic_No = licenceTypeNumber;
                                }
                            }
                            else
                            {
                                licenceTypeNumber = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactLicenses != null && jobContact.Contact.ContactLicenses.Count > 0 ?
                                jobContact.Contact.ContactLicenses.Select(x => x.ContactLicenseType.Name).FirstOrDefault() : string.Empty;
                                if (licenceTypeNumber.ToLower() == ("Engineer").ToLower())
                                {
                                    contactLic_state_engineer = "On";
                                }
                                else if (licenceTypeNumber.ToLower() == ("Architect").ToLower())
                                {
                                    contactLic_state_architect = "On";
                                }
                            }
                            contactTitle = jobContact != null && jobContact.Contact != null && jobContact.Contact.ContactTitle != null ? jobContact.Contact.ContactTitle.Name : string.Empty;
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(applicantFirstName + " " + applicantLastName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(applicantFirstName + " " + applicantLastName) ? "Applicant: " + applicantFirstName + " " + applicantLastName : string.Empty);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = frontendFields.IdDocumentField,
                                    Value = frontendFields.Value,
                                    ActualValue = applicantLastName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = applicantLastName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "Name of Representative dropping off applications please print")
                        {
                            int idFilingRepresentative = Convert.ToInt32(frontendFields.Value);
                            //Employee employee = rpoContext.Employees.Where(x => x.Id == idFilingRepresentative).FirstOrDefault();
                            Employee employee = Common.GetEmployeeInformation(idFilingRepresentative);

                            //filingRepresentativefirstname = employee != null ? employee.FirstName : string.Empty;
                            //filingRepresentativelastname = employee != null ? employee.LastName : string.Empty;
                            frfirstname = employee != null ? employee.FirstName : string.Empty;
                            frLastname = employee != null ? employee.LastName : string.Empty;

                            filingRepresentativeCompany = "RPO INC";
                            filingRepresentativeaddress = employee != null ? employee.Address1 : string.Empty;
                            filingRepresentativePhone = employee != null ? employee.WorkPhone : string.Empty;
                            filingRepresentativeEmail = employee != null ? employee.Email : string.Empty;
                            filingRepresentativestate = employee != null ? employee.State.Acronym : string.Empty;
                            filingRepresentativecity = employee != null ? employee.City : string.Empty;
                            filingRepresentativezip = employee != null ? employee.ZipCode : string.Empty;
                            filingRepresentativemobile = employee != null ? employee.MobilePhone : string.Empty;
                            //  filingRepresentativeEmail = employee != null ? employee.Email : string.Empty;

                            string filingRepresentative = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;

                            AgentCertificate agentCertificate = rpoContext.AgentCertificates
                            .Include("DocumentType").FirstOrDefault(x => x.IdEmployee == idFilingRepresentative && x.DocumentType.Name.Equals("DOB Filing Representative"));
                            filingRepresentativRegnumber = agentCertificate != null ? agentCertificate.NumberId : string.Empty;
                            // documentDescription = documentDescription + (!string.IsNullOrEmpty(filingRepresentative) ? "Filing Representative: " + filingRepresentative : string.Empty);

                            documentDescription = documentDescription + (!string.IsNullOrEmpty(frfirstname) && !string.IsNullOrEmpty(frLastname) ? " | Filing Representative: " + frfirstname + " " + frLastname : string.Empty);
                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = filingRepresentative,

                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingRepresentative;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Last Name_2")
                        {
                            int idFilingRepresentative = Convert.ToInt32(frontendFields.Value);
                            //Employee employee = rpoContext.Employees.Where(x => x.Id == idFilingRepresentative).FirstOrDefault();
                            if (idFilingRepresentative != 0)
                            {
                                Employee employee = Common.GetEmployeeInformation(idFilingRepresentative);

                                filingRepresentativefirstname = filingRepresentativefirstname + (employee != null ? (!string.IsNullOrEmpty(filingRepresentativefirstname) ? "/" : string.Empty) + employee.FirstName : string.Empty);
                                filingRepresentativelastname = filingRepresentativelastname + (employee != null ? (!string.IsNullOrEmpty(filingRepresentativelastname) ? "/" : string.Empty) + employee.LastName : string.Empty);
                                frfirstname = employee != null ? employee.FirstName : string.Empty;
                                frLastname = employee != null ? employee.LastName : string.Empty;

                                filingRepresentativeCompany = "RPO INC";
                                filingRepresentativePhone = employee != null ? employee.WorkPhone : string.Empty;
                            }
                            documentDescription = documentDescription + (!string.IsNullOrEmpty(frfirstname) && !string.IsNullOrEmpty(frLastname) ? " | Filing Representative2: " + frfirstname + " " + frLastname : string.Empty);
                            //  filingRepresentativeEmail = employee != null ? employee.Email : string.Empty;

                            //documentDescription = documentDescription + (!string.IsNullOrEmpty(filingRepresentative) ? "Filing Representative: " + filingRepresentative : string.Empty);

                            updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = filingRepresentativefirstname,

                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = filingRepresentativefirstname;
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