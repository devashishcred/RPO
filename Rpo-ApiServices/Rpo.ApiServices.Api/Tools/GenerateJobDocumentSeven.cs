
namespace Rpo.ApiServices.Api.Tools
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Rpo.ApiServices.Api.Controllers.JobDocument;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Collections.Generic;
    using Enums;
    public partial class GenerateJobDocuments
    {
        public static void GenerateAfterHoursPermitApplication(int idJobDocument, JobDocumentCreateOrUpdateDTO jobDocumentCreateOrUpdateDTO)
        {
            RpoContext rpoContext = new RpoContext();
            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocumentCreateOrUpdateDTO.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();


            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);
            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            string weekdayWorkDescription = string.Empty;
            string weekendWorkDescription = string.Empty;
            bool isSameAsWeekday = false;


            string AHVFirstName = string.Empty;
            string AHVMobileNumber = string.Empty;
            string AHVEmail = string.Empty;

            string contractorFirstName = string.Empty;
            string contractorLastName = string.Empty;
            string contractorMiddleName = string.Empty;
            string contractorCompany = string.Empty;
            string contractorCompanyPhone = string.Empty;
            string contractorCompanyAddress = string.Empty;
            string contractorCompanyEmail = string.Empty;
            string contractorCompanyCity = string.Empty;
            string contractorCompanyState = string.Empty;
            string contractorCompanyZip = string.Empty;
            string contractorMobile = string.Empty;
            string contractorLicNumber = string.Empty;
            string contractorLicGC = string.Empty;
            string contractorLicElectrician = string.Empty;
            string contractorLicMP = string.Empty;
            string contractorLicFSC = string.Empty;
            string contractorLicOther = string.Empty;
            string contractorLicOtherType = string.Empty;

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
                        if (item.Field.FieldName == "JobFileInfo")
                        {
                            string jobFileInfo = job != null ? job.JobNumber + " - PW513-81459.pdf" : string.Empty;
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

                        else if (item.Field.FieldName == "chkBISJob")
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
                        else if (item.Field.FieldName == "chkElecApp")
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
                        else if (item.Field.FieldName == "chkInitial")
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
                        else if (item.Field.FieldName == "chkRenewal")
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
                        else if (item.Field.FieldName == "txtBoro")
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
                        else if (item.Field.FieldName == "txtBIN")
                        {
                            string bin = job != null && job.RfpAddress != null ? job.RfpAddress.BinNumber : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = bin,
                                    ActualValue = bin
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = bin;
                                updateJobDocumentField.ActualValue = bin;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtCB")
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
                        else if (item.Field.FieldName == "txtFloors")
                        {
                            // string floorNumber = job != null ? job.FloorNumber : string.Empty;
                            string floorNumber = job.Applications.Select(d => d.FloorWorking).FirstOrDefault() != null ? job.Applications.Select(d => d.FloorWorking).FirstOrDefault() : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = floorNumber,
                                    ActualValue = floorNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = floorNumber;
                                updateJobDocumentField.ActualValue = floorNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtApt")
                        {
                            string apartment = job != null ? job.Apartment : string.Empty;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = apartment,
                                    ActualValue = apartment
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = apartment;
                                updateJobDocumentField.ActualValue = apartment;
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
                                    Value = contractorFirstName,
                                    ActualValue = contractorFirstName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorFirstName;
                                updateJobDocumentField.ActualValue = contractorFirstName;
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
                                    Value = contractorMiddleName,
                                    ActualValue = contractorMiddleName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorMiddleName;
                                updateJobDocumentField.ActualValue = contractorMiddleName;
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
                                    Value = contractorCompany,
                                    ActualValue = contractorCompany
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorCompany;
                                updateJobDocumentField.ActualValue = contractorCompany;
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
                                    Value = contractorCompanyPhone,
                                    ActualValue = contractorCompanyPhone
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorCompanyPhone;
                                updateJobDocumentField.ActualValue = contractorCompanyPhone;
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
                                    Value = contractorCompanyAddress,
                                    ActualValue = contractorCompanyAddress
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorCompanyAddress;
                                updateJobDocumentField.ActualValue = contractorCompanyAddress;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtFax")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contractorCompanyEmail,
                                    ActualValue = contractorCompanyEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorCompanyEmail;
                                updateJobDocumentField.ActualValue = contractorCompanyEmail;
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
                                    Value = contractorCompanyCity,
                                    ActualValue = contractorCompanyCity
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorCompanyCity;
                                updateJobDocumentField.ActualValue = contractorCompanyCity;
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
                                    Value = contractorCompanyState,
                                    ActualValue = contractorCompanyState
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorCompanyState;
                                updateJobDocumentField.ActualValue = contractorCompanyState;
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
                                    Value = contractorCompanyZip,
                                    ActualValue = contractorCompanyZip
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorCompanyZip;
                                updateJobDocumentField.ActualValue = contractorCompanyZip;
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
                                    Value = contractorMobile,
                                    ActualValue = contractorMobile
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorMobile;
                                updateJobDocumentField.ActualValue = contractorMobile;
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
                                    Value = contractorLicNumber,
                                    ActualValue = contractorLicNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorLicNumber;
                                updateJobDocumentField.ActualValue = contractorLicNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkN")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contractorLicGC,
                                    ActualValue = contractorLicGC
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorLicGC;
                                updateJobDocumentField.ActualValue = contractorLicGC;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkElec")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contractorLicElectrician,
                                    ActualValue = contractorLicElectrician
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorLicElectrician;
                                updateJobDocumentField.ActualValue = contractorLicElectrician;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkP")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contractorLicMP,
                                    ActualValue = contractorLicMP
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorLicMP;
                                updateJobDocumentField.ActualValue = contractorLicMP;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkF")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contractorLicFSC,
                                    ActualValue = contractorLicFSC
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorLicFSC;
                                updateJobDocumentField.ActualValue = contractorLicFSC;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "chkOther")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contractorLicOther,
                                    ActualValue = contractorLicOther
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorLicOther;
                                updateJobDocumentField.ActualValue = contractorLicOther;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtOtherLic_Type")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contractorLicOtherType,
                                    ActualValue = contractorLicOtherType
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorLicOtherType;
                                updateJobDocumentField.ActualValue = contractorLicOtherType;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        else if (item.Field.FieldName == "txtMain_First")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = AHVFirstName,
                                    ActualValue = AHVFirstName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = AHVFirstName;
                                updateJobDocumentField.ActualValue = AHVFirstName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtMain_MobilePhone")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = AHVMobileNumber,
                                    ActualValue = AHVMobileNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = AHVMobileNumber;
                                updateJobDocumentField.ActualValue = AHVMobileNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtMain_EMail")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = AHVEmail,
                                    ActualValue = AHVEmail
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = AHVEmail;
                                updateJobDocumentField.ActualValue = AHVEmail;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }

                        //else if (item.Field.FieldName == "txtNo_Days")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtDate2")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}

                        //else if (item.Field.FieldName == "txtDate3")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtStart3")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtEnd3")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtDate4")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtStart4")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtEnd4")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtDate5")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtStart5")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtEnd5")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtDate6")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtStart6")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtEnd6")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtDate7")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtStart7")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtEnd7")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtDate1")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtStart1")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "txtEnd1")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}

                        //else if (item.Field.FieldName == "txtReasonForVariance")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = string.Empty,
                        //            ActualValue = string.Empty
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = string.Empty;
                        //        updateJobDocumentField.ActualValue = string.Empty;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}

                        else if (item.Field.FieldName == "txtName")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = contractorFirstName + " " + contractorLastName,
                                    ActualValue = contractorFirstName + " " + contractorLastName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = contractorFirstName + " " + contractorLastName;
                                updateJobDocumentField.ActualValue = contractorFirstName + " " + contractorLastName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtDate")
                        {
                            DateTime date = DateTime.Today;
                            string format = "MM/dd/yy";
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = date.ToString(format),
                                    ActualValue = date.ToString(format)
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = date.ToString(format);
                                updateJobDocumentField.ActualValue = date.ToString(format);
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }


                        //else if (item.Field.FieldName == "Job Site Contact")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = createUpdatePW517.IdJobSiteContact.ToString(),
                        //            ActualValue = createUpdatePW517.IdJobSiteContact.ToString()
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = createUpdatePW517.IdJobSiteContact.ToString();
                        //        updateJobDocumentField.ActualValue = createUpdatePW517.IdJobSiteContact.ToString();
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}

                        //else if (item.Field.FieldName == "Issued Date")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = createUpdatePW517.IssuedDate,
                        //            ActualValue = createUpdatePW517.IssuedDate
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = createUpdatePW517.IssuedDate;
                        //        updateJobDocumentField.ActualValue = createUpdatePW517.IssuedDate;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}
                        //else if (item.Field.FieldName == "Submitted Date")
                        //{
                        //    if (updateJobDocumentField == null)
                        //    {
                        //        JobDocumentField JobDocumentField = new JobDocumentField
                        //        {
                        //            IdJobDocument = idJobDocument,
                        //            IdDocumentField = Convert.ToInt32(item.Id),
                        //            Value = createUpdatePW517.SubmittedDate,
                        //            ActualValue = createUpdatePW517.SubmittedDate
                        //        };
                        //        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        //    }
                        //    else
                        //    {
                        //        updateJobDocumentField.Value = createUpdatePW517.SubmittedDate;
                        //        updateJobDocumentField.ActualValue = createUpdatePW517.SubmittedDate;
                        //        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        //        rpoContext.SaveChanges();
                        //    }
                        //}

                        rpoContext.SaveChanges();
                    }
                    else
                    {
                        if (item.Field.FieldName == "txtReasonForVariance")
                        {
                            Controllers.JobDocumentDrodown.JobDocumentDrodownController jobDocumentDrodownController = new Controllers.JobDocumentDrodown.JobDocumentDrodownController();
                            List<Controllers.JobDocumentDrodown.JobDocumentDrodownController.IdItemName> varianceReasonList = jobDocumentDrodownController.GetVarianceReasons();
                            Controllers.JobDocumentDrodown.JobDocumentDrodownController.IdItemName varianceReason = varianceReasonList.FirstOrDefault(x => x.Id == frontendFields.Value);

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = varianceReason != null ? varianceReason.ItemName : string.Empty
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = varianceReason != null ? varianceReason.ItemName : string.Empty;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Select Dates")
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
                        else if (item.Field.FieldName == "Application")
                        {
                            int idJobApplication = frontendFields.Value != null ? Convert.ToInt32(frontendFields.Value) : 0;
                            JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == idJobApplication).FirstOrDefault();
                            string jobApplicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = jobApplicationNumber
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                                rpoContext.SaveChanges();
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = jobApplicationNumber;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtLast")
                        {
                            int idJobApplicant = frontendFields.Value != null ? Convert.ToInt32(frontendFields.Value) : 0;
                            JobContact jobContact = rpoContext.JobContacts.Include("Contact.ContactLicenses").Include("Contact.Company.Addresses").Include("Contact.Addresses").Where(x => x.Id == idJobApplicant).FirstOrDefault();
                            contractorLastName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;
                            contractorFirstName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                            contractorMiddleName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MiddleName : string.Empty;
                            contractorCompany = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null ? jobContact.Contact.Company.Name : string.Empty;

                            Address address = Common.GetContactAddressForJobDocument(jobContact);

                            //if (jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null)
                            //{
                            //    address = jobContact.Contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault();
                            //}

                            //if (address == null)
                            //{
                            //    address = jobContact.Contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true);
                            //}

                            Address companyaddress = null;
                            if (jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null)
                            {
                                companyaddress = jobContact.Contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault();
                            }

                            contractorCompanyPhone = address != null && address.Phone != null ? address.Phone : string.Empty;
                            contractorCompanyAddress = address != null ? address.Address1 + " " + address.Address2 : string.Empty;
                            contractorCompanyEmail = companyaddress != null ? companyaddress.Fax : string.Empty;
                            contractorCompanyCity = address != null ? address.City : string.Empty;
                            contractorCompanyState = address != null && address.State != null ? address.State.Acronym : string.Empty;

                            contractorCompanyZip = address != null ? address.ZipCode : string.Empty;
                            contractorMobile = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;

                            contractorLicGC = "No";
                            contractorLicElectrician = "No";
                            contractorLicMP = "No";
                            contractorLicFSC = "No";
                            contractorLicOther = "No";
                            contractorLicOtherType = string.Empty;

                            Company company = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null ? jobContact.Contact.Company : null;
                            if (company != null && company.CompanyTypes != null && company.CompanyTypes.Where(x => x.ItemName == "General Contractor").Any())
                            {
                                contractorLicNumber = company.TrackingNumber;
                                contractorLicGC = "Yes";
                            }
                            else
                            {
                                ContactLicense contactLicense = jobContact.Contact.ContactLicenses.FirstOrDefault(x => x.ContactLicenseType.Name == "Electrician"
                                || x.ContactLicenseType.Name == "Master Plumber"
                                || x.ContactLicenseType.Name == "Fire Suppression Contractor");

                                if (contactLicense != null)
                                {
                                    contractorLicNumber = contactLicense.Number;

                                    if (contactLicense.ContactLicenseType.Name == "Electrician")
                                    {
                                        contractorLicElectrician = "Yes";
                                    }
                                    else if (contactLicense.ContactLicenseType.Name == "Master Plumber")
                                    {
                                        contractorLicMP = "Yes";
                                    }
                                    else if (contactLicense.ContactLicenseType.Name == "Fire Suppression Contractor")
                                    {
                                        contractorLicFSC = "Yes";
                                    }
                                }
                                else
                                {
                                    contactLicense = jobContact.Contact.ContactLicenses.FirstOrDefault();
                                    if (contactLicense != null)
                                    {
                                        contractorLicOther = "Yes";
                                        contractorLicNumber = contactLicense != null ? contactLicense.Number : string.Empty;
                                        contractorLicOtherType = contactLicense.ContactLicenseType.Name;
                                    }
                                }

                                //architectEngineer = "Yes";
                            }

                            //contractorLicNumber = string.Empty;


                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = contractorLastName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = contractorLastName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtMain_Last")
                        {
                            int idJobApplicant = frontendFields.Value != null ? Convert.ToInt32(frontendFields.Value) : 0;
                            JobContact jobContact = rpoContext.JobContacts.Include("Contact").Where(x => x.Id == idJobApplicant).FirstOrDefault();
                            string AHVLastName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;

                            AHVFirstName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                            AHVMobileNumber = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                            AHVEmail = jobContact != null && jobContact.Contact != null ? jobContact.Contact.Email : string.Empty;

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = AHVLastName
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = AHVLastName;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Same as Weekday")
                        {
                            isSameAsWeekday = Convert.ToBoolean(frontendFields.Value);
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
                        else if (item.Field.FieldName == "txtWork")
                        {
                            weekdayWorkDescription = frontendFields.Value;
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = weekdayWorkDescription,
                                    ActualValue = weekdayWorkDescription
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = weekdayWorkDescription;
                                updateJobDocumentField.ActualValue = weekdayWorkDescription;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Create Support Documents")
                        {
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
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = jobDocumentFor,
                                    ActualValue = jobDocumentFor
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = jobDocumentFor;
                                updateJobDocumentField.ActualValue = jobDocumentFor;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "Weekend Work Description")
                        {
                            var documentFieldsWeekDay = jobDocumentCreateOrUpdateDTO.JobDocumentFields.Where(x => x.IdDocumentField == 533125 || x.IdDocumentField == 533128 || x.IdDocumentField == 533131 || x.IdDocumentField == 533134 || x.IdDocumentField == 533137);
                            var documentFieldsWeekEnd = jobDocumentCreateOrUpdateDTO.JobDocumentFields.Where(x => x.IdDocumentField == 533140 || x.IdDocumentField == 533143);

                            bool isWeekDay = false;
                            bool isWeekEnd = false;

                            if (documentFieldsWeekDay.Where(x => x.Value != null && x.Value != string.Empty).Any())
                            {
                                isWeekDay = true;
                            }
                            if (documentFieldsWeekEnd.Where(x => x.Value != null && x.Value != string.Empty).Any())
                            {
                                isWeekEnd = true;
                            }

                            if (isWeekDay && isWeekEnd)
                            {
                                if (isSameAsWeekday)
                                {
                                    weekendWorkDescription = weekdayWorkDescription;
                                }
                                else
                                {
                                    weekendWorkDescription = "Weekday: " + weekdayWorkDescription + Environment.NewLine + Environment.NewLine
                                    + "Weekend: " + frontendFields.Value;
                                }
                            }
                            else if (isWeekDay)
                            {
                                weekendWorkDescription = weekdayWorkDescription;
                            }
                            else if (isWeekEnd)
                            {
                                weekendWorkDescription = frontendFields.Value;
                            }

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = weekendWorkDescription
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = weekendWorkDescription;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opg200")
                        {
                            bool is200 = Convert.ToBoolean(frontendFields.Value);
                            string opg200 = is200 ? "Yes" : "No";

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = opg200
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = opg200;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgEnclosed")
                        {
                            bool enclosed = Convert.ToBoolean(frontendFields.Value);
                            string opgEnclosed = enclosed ? "Yes" : "No";

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = opgEnclosed
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = opgEnclosed;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgDemo")
                        {
                            bool demo = Convert.ToBoolean(frontendFields.Value);
                            string opgDemo = demo ? "Yes" : "No";

                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = opgDemo
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = opgDemo;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtStart2" || item.Field.FieldName == "txtStart3" || item.Field.FieldName == "txtStart4"
                            || item.Field.FieldName == "txtStart5" || item.Field.FieldName == "txtStart6" || item.Field.FieldName == "txtStart7"
                            || item.Field.FieldName == "txtStart1")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = !string.IsNullOrEmpty(frontendFields.Value) ? (frontendFields.Value).Replace(":00", "") : frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = !string.IsNullOrEmpty(frontendFields.Value) ? (frontendFields.Value).Replace(":00", "") : frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "txtEnd2" || item.Field.FieldName == "txtEnd3" || item.Field.FieldName == "txtEnd4"
                            || item.Field.FieldName == "txtEnd5" || item.Field.FieldName == "txtEnd6" || item.Field.FieldName == "txtEnd7"
                            || item.Field.FieldName == "txtEnd1")
                        {
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = !string.IsNullOrEmpty(frontendFields.Value) ? (frontendFields.Value).Replace(":00", "") : frontendFields.Value
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = !string.IsNullOrEmpty(frontendFields.Value) ? (frontendFields.Value).Replace(":00", "") : frontendFields.Value;
                                rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                                rpoContext.SaveChanges();
                            }
                        }
                        else if (item.Field.FieldName == "opgCrane")
                        {
                            bool crane = Convert.ToBoolean(frontendFields.Value);
                            string opgCrane = crane ? "Yes" : "No";
                            if (updateJobDocumentField == null)
                            {
                                JobDocumentField JobDocumentField = new JobDocumentField
                                {
                                    IdJobDocument = idJobDocument,
                                    IdDocumentField = Convert.ToInt32(item.Id),
                                    Value = frontendFields.Value,
                                    ActualValue = opgCrane
                                };
                                rpoContext.JobDocumentFields.Add(JobDocumentField);
                            }
                            else
                            {
                                updateJobDocumentField.Value = frontendFields.Value;
                                updateJobDocumentField.ActualValue = opgCrane;
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

            var documentDates = jobDocumentCreateOrUpdateDTO.JobDocumentFields.Where(x => x.IdDocumentField == 533140 || x.IdDocumentField == 533143 || x.IdDocumentField == 533125 || x.IdDocumentField == 533128 || x.IdDocumentField == 533131 || x.IdDocumentField == 533134 || x.IdDocumentField == 533137);

            string permitDates = string.Empty;
            foreach (JobDocumentFieldDTO item in documentDates)
            {
                if (item.Value != null && !string.IsNullOrEmpty(item.Value))
                {
                    permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? "," + item.Value : item.Value);
                }
            }

            List<string> documentDescriptionDates = permitDates != null && !string.IsNullOrEmpty(permitDates) ? (permitDates.Split(',') != null && permitDates.Split(',').Any() ? permitDates.Split(',').Select(x => x).ToList() : new List<string>()) : new List<string>();

            documentDescriptionDates = documentDescriptionDates.OrderBy(x => x).ToList();
            permitDates = string.Empty;
            foreach (string item in documentDescriptionDates)
            {
                if (item != null && !string.IsNullOrEmpty(item))
                {
                    permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? ", " + item : item);
                }
            }
            jobDocument.DocumentDescription = permitDates;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();

            // Generate document
            GenerateJobDocument(idJobDocument);
        }


        public static void GenerateAfterHoursPermitApplication_PW517(int idJobDocument, CreateUpdatePW517 createUpdatePW517, int idEmployee)
        {
            RpoContext rpoContext = new RpoContext();
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.Id == idJobDocument);

            var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == jobDocument.IdDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();

            string documentDescription = string.Empty;
            string jobDocumentFor = string.Empty;

            string weekdayWorkDescription = string.Empty;
            string weekendWorkDescription = string.Empty;
            bool isSameAsWeekday = false;


            string AHVFirstName = string.Empty;
            string AHVMobileNumber = string.Empty;
            string AHVEmail = string.Empty;

            string contractorFirstName = string.Empty;
            string contractorLastName = string.Empty;
            string contractorMiddleName = string.Empty;
            string contractorCompany = string.Empty;
            string contractorCompanyPhone = string.Empty;
            string contractorCompanyAddress = string.Empty;
            string contractorCompanyEmail = string.Empty;
            string contractorCompanyCity = string.Empty;
            string contractorCompanyState = string.Empty;
            string contractorCompanyZip = string.Empty;
            string contractorMobile = string.Empty;
            string contractorLicNumber = string.Empty;
            string contractorLicGC = string.Empty;
            string contractorLicElectrician = string.Empty;
            string contractorLicMP = string.Empty;
            string contractorLicFSC = string.Empty;
            string contractorLicOther = string.Empty;
            string contractorLicOtherType = string.Empty;

            string workpermit = string.Empty;
            string initial = string.Empty;
            string renewal = string.Empty;
            string AHVName = string.Empty;


            int idJobDocumentType = Convert.ToInt32(createUpdatePW517.IdJobDocumentType);
            JobDocumentType jobDocumentType = rpoContext.JobDocumentTypes.FirstOrDefault(x => x.Id == idJobDocumentType);
            if (jobDocumentType != null && jobDocumentType.Type.ToLower() == "initial")
            {
                initial = "1";
                renewal = "1";
            }
            else
            {
                initial = "2";
                renewal = "2";
            }

            Controllers.JobDocumentDrodown.JobDocumentDrodownController jobDocumentDrodownController = new Controllers.JobDocumentDrodown.JobDocumentDrodownController();
            List<Controllers.JobDocumentDrodown.JobDocumentDrodownController.IdItemName> varianceReasonList = jobDocumentDrodownController.GetVarianceReasons();
            Controllers.JobDocumentDrodown.JobDocumentDrodownController.IdItemName varianceReason = varianceReasonList.FirstOrDefault(x => x.Id == createUpdatePW517.ReasonForVariance);
            string reasonForVariance = string.Empty;

            reasonForVariance = varianceReason != null ? varianceReason.ItemName : string.Empty;

            if (documentFieldList != null && documentFieldList.Count > 0)
            {
                Job job = rpoContext.Jobs.Include("ProjectManager").
                    Include("Contact.ContactTitle").
                    Include("RfpAddress.Borough").
                    Include("Company.Addresses.AddressType").
                    Include("Company.Addresses.State")
                    .Where(x => x.Id == jobDocument.IdJob).FirstOrDefault();
                var jobDocumentFieldList = rpoContext.JobDocumentFields.Where(x => x.IdJobDocument == idJobDocument).ToList();
                foreach (DocumentField item in documentFieldList)
                {
                    JobDocumentField updateJobDocumentField = jobDocumentFieldList.FirstOrDefault(x => x.IdDocumentField == item.Id);

                    if (item.Field.FieldName == "JobFileInfo")
                    {
                        string jobFileInfo = job != null ? job.JobNumber + " - PW517.pdf" : string.Empty;
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

                    else if (item.Field.FieldName == "chkBISJob")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = "1",
                                ActualValue = "1"
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = "1";
                            updateJobDocumentField.ActualValue = "1";
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "chkElecApp")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = "0",
                                ActualValue = "0"
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = "0";
                            updateJobDocumentField.ActualValue = "0";
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "chkInitial")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = Convert.ToString(createUpdatePW517.IdJobDocumentType),
                                ActualValue = initial
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = Convert.ToString(createUpdatePW517.IdJobDocumentType);
                            updateJobDocumentField.ActualValue = initial;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "chkRenewal")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = Convert.ToString(createUpdatePW517.IdJobDocumentType),
                                ActualValue = renewal
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = Convert.ToString(createUpdatePW517.IdJobDocumentType);
                            updateJobDocumentField.ActualValue = renewal;
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
                    else if (item.Field.FieldName == "txtNo_Days")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.NumberOfDays,
                                ActualValue = createUpdatePW517.NumberOfDays
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.NumberOfDays;
                            updateJobDocumentField.ActualValue = createUpdatePW517.NumberOfDays;
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
                    else if (item.Field.FieldName == "txtBoro")
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
                    else if (item.Field.FieldName == "txtBIN")
                    {
                        string bin = job != null && job.RfpAddress != null ? job.RfpAddress.BinNumber : string.Empty;
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = bin,
                                ActualValue = bin
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = bin;
                            updateJobDocumentField.ActualValue = bin;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtCB")
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
                    else if (item.Field.FieldName == "txtFloors")
                    {
                        // string floorNumber = job != null ? job.FloorNumber : string.Empty;
                        string floorNumber = job.Applications.Select(d => d.FloorWorking).FirstOrDefault() != null ? job.Applications.Select(d => d.FloorWorking).FirstOrDefault() : string.Empty;
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = floorNumber,
                                ActualValue = floorNumber
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = floorNumber;
                            updateJobDocumentField.ActualValue = floorNumber;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtApt")
                    {
                        string apartment = job != null ? job.Apartment : string.Empty;
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = apartment,
                                ActualValue = apartment
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = apartment;
                            updateJobDocumentField.ActualValue = apartment;
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
                                Value = contractorFirstName,
                                ActualValue = contractorFirstName
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorFirstName;
                            updateJobDocumentField.ActualValue = contractorFirstName;
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
                                Value = contractorMiddleName,
                                ActualValue = contractorMiddleName
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorMiddleName;
                            updateJobDocumentField.ActualValue = contractorMiddleName;
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
                                Value = contractorCompany,
                                ActualValue = contractorCompany
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorCompany;
                            updateJobDocumentField.ActualValue = contractorCompany;
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
                                Value = contractorCompanyPhone,
                                ActualValue = contractorCompanyPhone
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorCompanyPhone;
                            updateJobDocumentField.ActualValue = contractorCompanyPhone;
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
                                Value = contractorCompanyAddress,
                                ActualValue = contractorCompanyAddress
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorCompanyAddress;
                            updateJobDocumentField.ActualValue = contractorCompanyAddress;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtFax")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = contractorCompanyEmail,
                                ActualValue = contractorCompanyEmail
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorCompanyEmail;
                            updateJobDocumentField.ActualValue = contractorCompanyEmail;
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
                                Value = contractorCompanyCity,
                                ActualValue = contractorCompanyCity
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorCompanyCity;
                            updateJobDocumentField.ActualValue = contractorCompanyCity;
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
                                Value = contractorCompanyState,
                                ActualValue = contractorCompanyState
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorCompanyState;
                            updateJobDocumentField.ActualValue = contractorCompanyState;
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
                                Value = contractorCompanyZip,
                                ActualValue = contractorCompanyZip
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorCompanyZip;
                            updateJobDocumentField.ActualValue = contractorCompanyZip;
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
                                Value = contractorMobile,
                                ActualValue = contractorMobile
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorMobile;
                            updateJobDocumentField.ActualValue = contractorMobile;
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
                                Value = contractorLicNumber,
                                ActualValue = contractorLicNumber
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorLicNumber;
                            updateJobDocumentField.ActualValue = contractorLicNumber;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "chkN")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = contractorLicGC,
                                ActualValue = contractorLicGC
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorLicGC;
                            updateJobDocumentField.ActualValue = contractorLicGC;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "chkElec")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = contractorLicElectrician,
                                ActualValue = contractorLicElectrician
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorLicElectrician;
                            updateJobDocumentField.ActualValue = contractorLicElectrician;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "chkP")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = contractorLicMP,
                                ActualValue = contractorLicMP
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorLicMP;
                            updateJobDocumentField.ActualValue = contractorLicMP;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "chkF")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = contractorLicFSC,
                                ActualValue = contractorLicFSC
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorLicFSC;
                            updateJobDocumentField.ActualValue = contractorLicFSC;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "chkOther")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = contractorLicOther,
                                ActualValue = contractorLicOther
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorLicOther;
                            updateJobDocumentField.ActualValue = contractorLicOther;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtOtherLic_Type")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = contractorLicOtherType,
                                ActualValue = contractorLicOtherType
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorLicOtherType;
                            updateJobDocumentField.ActualValue = contractorLicOtherType;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }

                    else if (item.Field.FieldName == "txtMain_First")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = AHVFirstName,
                                ActualValue = AHVFirstName
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = AHVFirstName;
                            updateJobDocumentField.ActualValue = AHVFirstName;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtMain_MobilePhone")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = AHVMobileNumber,
                                ActualValue = AHVMobileNumber
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = AHVMobileNumber;
                            updateJobDocumentField.ActualValue = AHVMobileNumber;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtMain_EMail")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = AHVEmail,
                                ActualValue = AHVEmail
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = AHVEmail;
                            updateJobDocumentField.ActualValue = AHVEmail;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtName")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = contractorFirstName + " " + contractorLastName,
                                ActualValue = contractorFirstName + " " + contractorLastName
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = contractorFirstName + " " + contractorLastName;
                            updateJobDocumentField.ActualValue = contractorFirstName + " " + contractorLastName;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "Type")
                    {
                        string IdJobDocumentType = Convert.ToString(createUpdatePW517.IdJobDocumentType);
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = IdJobDocumentType,
                                ActualValue = jobDocumentType.Type
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = IdJobDocumentType;
                            updateJobDocumentField.ActualValue = jobDocumentType.Type;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtDate")
                    {
                        DateTime date = DateTime.Today;
                        string format = "MM/dd/yy";
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = date.ToString(format),
                                ActualValue = date.ToString(format)
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = date.ToString(format);
                            updateJobDocumentField.ActualValue = date.ToString(format);
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtReasonForVariance")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.ReasonForVariance,
                                ActualValue = reasonForVariance
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.ReasonForVariance;
                            updateJobDocumentField.ActualValue = reasonForVariance;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "Select Dates")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.EfilingDates,
                                ActualValue = createUpdatePW517.EfilingDates
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.EfilingDates;
                            updateJobDocumentField.ActualValue = createUpdatePW517.EfilingDates;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "Start Date")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.StartDate,
                                ActualValue = createUpdatePW517.StartDate
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.StartDate;
                            updateJobDocumentField.ActualValue = createUpdatePW517.StartDate;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "Next Date")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.NextDate,
                                ActualValue = createUpdatePW517.NextDate
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.NextDate;
                            updateJobDocumentField.ActualValue = createUpdatePW517.NextDate;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "Application")
                    {
                        int idJobApplication = createUpdatePW517.Application != null ? Convert.ToInt32(createUpdatePW517.Application) : 0;
                        JobApplication jobApplication = rpoContext.JobApplications.Where(x => x.Id == idJobApplication).FirstOrDefault();
                        string jobApplicationNumber = jobApplication != null ? jobApplication.ApplicationNumber : string.Empty;

                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.Application,
                                ActualValue = jobApplicationNumber
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                            rpoContext.SaveChanges();
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.Application;
                            updateJobDocumentField.ActualValue = jobApplicationNumber;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "Work Permits")
                    {
                        int idWorkPermit = createUpdatePW517.idWorkPermit != null ? Convert.ToInt32(createUpdatePW517.idWorkPermit) : 0;
                        JobApplicationWorkPermitType jobApplication = rpoContext.JobApplicationWorkPermitTypes.Where(x => x.Id == idWorkPermit).FirstOrDefault();
                        string value = jobApplication.PermitType != null && jobApplication.PermitType != "" ? jobApplication.PermitType : (jobApplication.JobWorkType != null ? jobApplication.JobWorkType.Description + "-" + jobApplication.JobWorkType.Code : string.Empty);
                        workpermit = value;
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.idWorkPermit,
                                ActualValue = value
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                            rpoContext.SaveChanges();
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.idWorkPermit;
                            updateJobDocumentField.ActualValue = value;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtLast")
                    {
                        int idJobApplicant = createUpdatePW517.Applicant != null ? Convert.ToInt32(createUpdatePW517.Applicant) : 0;
                        JobContact jobContact = rpoContext.JobContacts.Include("Contact.ContactLicenses").Include("Contact.Company.Addresses").Include("Contact.Addresses").Where(x => x.Id == idJobApplicant).FirstOrDefault();
                        contractorLastName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;
                        contractorFirstName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                        contractorMiddleName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MiddleName : string.Empty;
                        contractorCompany = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null ? jobContact.Contact.Company.Name : string.Empty;

                        Address address = Common.GetContactAddressForJobDocument(jobContact);

                        Address companyaddress = null;
                        if (jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null)
                        {
                            companyaddress = jobContact.Contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault();
                        }

                        contractorCompanyPhone = address != null && address.Phone != null ? address.Phone : string.Empty;
                        contractorCompanyAddress = address != null ? address.Address1 + " " + address.Address2 : string.Empty;
                        contractorCompanyEmail = companyaddress != null ? companyaddress.Fax : string.Empty;
                        contractorCompanyCity = address != null ? address.City : string.Empty;
                        contractorCompanyState = address != null && address.State != null ? address.State.Acronym : string.Empty;

                        contractorCompanyZip = address != null ? address.ZipCode : string.Empty;
                        contractorMobile = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;

                        contractorLicGC = "";
                        contractorLicElectrician = "";
                        contractorLicMP = "";
                        contractorLicFSC = "";
                        contractorLicOther = "";
                        contractorLicOtherType = string.Empty;

                        Company company = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null ? jobContact.Contact.Company : null;
                        if (company != null && company.CompanyTypes != null && company.CompanyTypes.Where(x => x.ItemName == "General Contractor").Any())
                        {
                            contractorLicNumber = company.TrackingNumber;
                            contractorLicGC = "1";
                        }
                        else
                        {
                            ContactLicense contactLicense = jobContact.Contact.ContactLicenses.FirstOrDefault(x => x.ContactLicenseType.Name == "Electrician"
                            || x.ContactLicenseType.Name == "Master Plumber"
                            || x.ContactLicenseType.Name == "Fire Suppression Contractor");

                            if (contactLicense != null)
                            {
                                contractorLicNumber = contactLicense.Number;

                                if (contactLicense.ContactLicenseType.Name == "Electrician")
                                {
                                    contractorLicElectrician = "Yes";
                                    contractorLicGC = "2";
                                }
                                else if (contactLicense.ContactLicenseType.Name == "Master Plumber")
                                {
                                    contractorLicMP = "Yes";
                                    contractorLicGC = "3";
                                }
                                else if (contactLicense.ContactLicenseType.Name == "Fire Suppression Contractor")
                                {
                                    contractorLicFSC = "Yes";
                                    contractorLicGC = "4";
                                }
                            }
                            else
                            {
                                contactLicense = jobContact.Contact.ContactLicenses.FirstOrDefault();
                                if (contactLicense != null)
                                {
                                    contractorLicOther = "Yes";
                                    contractorLicNumber = contactLicense != null ? contactLicense.Number : string.Empty;
                                    contractorLicOtherType = contactLicense.ContactLicenseType.Name;
                                    contractorLicGC = "5";
                                }
                            }
                        }

                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.Applicant,
                                ActualValue = contractorLastName
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.Applicant;
                            updateJobDocumentField.ActualValue = contractorLastName;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtMain_Last")
                    {
                        int idJobApplicant = createUpdatePW517.MainAHVWorkContact != null ? Convert.ToInt32(createUpdatePW517.MainAHVWorkContact) : 0;
                        JobContact jobContact = rpoContext.JobContacts.Include("Contact").Where(x => x.Id == idJobApplicant).FirstOrDefault();
                        string AHVLastName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.LastName : string.Empty;

                        AHVFirstName = jobContact != null && jobContact.Contact != null ? jobContact.Contact.FirstName : string.Empty;
                        AHVMobileNumber = jobContact != null && jobContact.Contact != null ? jobContact.Contact.MobilePhone : string.Empty;
                        AHVEmail = jobContact != null && jobContact.Contact != null ? jobContact.Contact.Email : string.Empty;

                        AHVName = AHVFirstName + " " + AHVLastName;

                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.MainAHVWorkContact,
                                ActualValue = AHVLastName
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.MainAHVWorkContact;
                            updateJobDocumentField.ActualValue = AHVLastName;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }

                    else if (item.Field.FieldName == "Create Support Documents")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.CreateSupportDocument,
                                ActualValue = createUpdatePW517.CreateSupportDocument
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                            rpoContext.SaveChanges();
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.CreateSupportDocument;
                            updateJobDocumentField.ActualValue = createUpdatePW517.CreateSupportDocument;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }


                    }
                    else if (item.Field.FieldName == "For")
                    {
                        jobDocumentFor = createUpdatePW517.ForDescription;

                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = jobDocumentFor,
                                ActualValue = jobDocumentFor
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = jobDocumentFor;
                            updateJobDocumentField.ActualValue = jobDocumentFor;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }

                    else if (item.Field.FieldName == "AHVReferenceNumber")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.AHVReferenceNumber,
                                ActualValue = createUpdatePW517.AHVReferenceNumber
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.AHVReferenceNumber;
                            updateJobDocumentField.ActualValue = createUpdatePW517.AHVReferenceNumber;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }

                    else if (item.Field.FieldName == "Job Site Contact")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = string.IsNullOrEmpty(createUpdatePW517.IdJobSiteContact) ? string.Empty : createUpdatePW517.IdJobSiteContact.ToString(),
                                ActualValue = string.IsNullOrEmpty(createUpdatePW517.IdJobSiteContact) ? string.Empty : createUpdatePW517.IdJobSiteContact.ToString()
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = string.IsNullOrEmpty(createUpdatePW517.IdJobSiteContact) ? string.Empty : createUpdatePW517.IdJobSiteContact.ToString();
                            updateJobDocumentField.ActualValue = string.IsNullOrEmpty(createUpdatePW517.IdJobSiteContact) ? string.Empty : createUpdatePW517.IdJobSiteContact.ToString();
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }

                    else if (item.Field.FieldName == "Issued Date")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.IssuedDate,
                                ActualValue = createUpdatePW517.IssuedDate
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.IssuedDate;
                            updateJobDocumentField.ActualValue = createUpdatePW517.IssuedDate;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "Submitted Date")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.SubmittedDate,
                                ActualValue = createUpdatePW517.SubmittedDate
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.SubmittedDate;
                            updateJobDocumentField.ActualValue = createUpdatePW517.SubmittedDate;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }


                    #region Work Description
                    else if (item.Field.FieldName == "Same as Weekday")
                    {
                        isSameAsWeekday = Convert.ToBoolean(createUpdatePW517.IsSameAsWeekday);
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.IsSameAsWeekday,
                                ActualValue = createUpdatePW517.IsSameAsWeekday
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.IsSameAsWeekday;
                            updateJobDocumentField.ActualValue = createUpdatePW517.IsSameAsWeekday;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtWork")
                    {
                        weekdayWorkDescription = createUpdatePW517.WeekdayDescription;
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = weekdayWorkDescription,
                                ActualValue = weekdayWorkDescription
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = weekdayWorkDescription;
                            updateJobDocumentField.ActualValue = weekdayWorkDescription;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "Weekend Work Description")
                    {
                        bool isWeekDay = false;
                        bool isWeekEnd = false;

                        if (!string.IsNullOrEmpty(createUpdatePW517.MondayDates) ||
                            !string.IsNullOrEmpty(createUpdatePW517.TuesdayDates) ||
                            !string.IsNullOrEmpty(createUpdatePW517.WednesdayDates) ||
                            !string.IsNullOrEmpty(createUpdatePW517.ThursdayDates) ||
                            !string.IsNullOrEmpty(createUpdatePW517.FridayDates)
                            )
                        {
                            isWeekDay = true;
                        }
                        if (!string.IsNullOrEmpty(createUpdatePW517.SaturdayDates) ||
                            !string.IsNullOrEmpty(createUpdatePW517.SundayDates))
                        {
                            isWeekEnd = true;
                        }

                        if (isWeekDay && isWeekEnd)
                        {
                            if (isSameAsWeekday)
                            {
                                weekendWorkDescription = weekdayWorkDescription;
                            }
                            else
                            {
                                weekendWorkDescription = "Weekday: " + weekdayWorkDescription + Environment.NewLine + Environment.NewLine
                                + "Weekend: " + createUpdatePW517.WeekendDescription;
                            }
                        }
                        else if (isWeekDay)
                        {
                            weekendWorkDescription = weekdayWorkDescription;
                        }

                        else if (isWeekEnd)
                        {
                            weekendWorkDescription = createUpdatePW517.WeekendDescription;
                        }


                        if (!string.IsNullOrEmpty(createUpdatePW517.IsSameAsWeekday) && createUpdatePW517.IsSameAsWeekday != "" && createUpdatePW517.IsSameAsWeekday.Trim().ToLower() == "true")
                        {
                            weekendWorkDescription = "Weekday & Weekend : " + createUpdatePW517.WeekdayDescription;
                        }
                        else
                        {
                            weekendWorkDescription = createUpdatePW517.WeekdayDescription;
                        }

                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.WeekendDescription,
                                ActualValue = weekendWorkDescription
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.WeekendDescription;
                            updateJobDocumentField.ActualValue = weekendWorkDescription;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }

                    #endregion

                    #region Variance Information
                    else if (item.Field.FieldName == "opg200")
                    {
                        bool is200 = Convert.ToBoolean(createUpdatePW517.Opg200);
                        string opg200 = is200 ? "Yes" : "No";

                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.Opg200 == null ? "false" : Convert.ToString(createUpdatePW517.Opg200),
                                ActualValue = opg200
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.Opg200 == null ? "false" : Convert.ToString(createUpdatePW517.Opg200);
                            updateJobDocumentField.ActualValue = opg200;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "opgEnclosed")
                    {
                        bool enclosed = Convert.ToBoolean(createUpdatePW517.OpgEnclosed);
                        string opgEnclosed = enclosed ? "Yes" : "No";

                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.OpgEnclosed == null ? "false" : Convert.ToString(createUpdatePW517.OpgEnclosed),
                                ActualValue = opgEnclosed
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.OpgEnclosed == null ? "false" : Convert.ToString(createUpdatePW517.OpgEnclosed);
                            updateJobDocumentField.ActualValue = opgEnclosed;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "opgDemo")
                    {
                        bool demo = Convert.ToBoolean(createUpdatePW517.OpgDemo);
                        string opgDemo = demo ? "Yes" : "No";

                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.OpgDemo == null ? "false" : Convert.ToString(createUpdatePW517.OpgDemo),
                                ActualValue = opgDemo
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.OpgDemo == null ? "false" : Convert.ToString(createUpdatePW517.OpgDemo);
                            updateJobDocumentField.ActualValue = opgDemo;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "opgCrane")
                    {
                        bool crane = Convert.ToBoolean(createUpdatePW517.OpgCrane);
                        string opgCrane = crane ? "Yes" : "No";
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.OpgCrane == null ? "false" : Convert.ToString(createUpdatePW517.OpgCrane),
                                ActualValue = opgCrane
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.OpgCrane == null ? "false" : Convert.ToString(createUpdatePW517.OpgCrane);
                            updateJobDocumentField.ActualValue = opgCrane;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }

                    #endregion

                    #region Time
                    else if (item.Field.FieldName == "txtStart2")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.MondayStartTime,
                                ActualValue = createUpdatePW517.MondayStartTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.MondayStartTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.MondayStartTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtStart3")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.TuesdayStartTime,
                                ActualValue = createUpdatePW517.TuesdayStartTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.TuesdayStartTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.TuesdayStartTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtStart4")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.WednesdayStartTime,
                                ActualValue = createUpdatePW517.WednesdayStartTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.WednesdayStartTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.WednesdayStartTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtStart5")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.ThursdayStartTime,
                                ActualValue = createUpdatePW517.ThursdayStartTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.ThursdayStartTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.ThursdayStartTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtStart6")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.FridayStartTime,
                                ActualValue = createUpdatePW517.FridayStartTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.FridayStartTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.FridayStartTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtStart7")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.SaturdayStartTime,
                                ActualValue = createUpdatePW517.SaturdayStartTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.SaturdayStartTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.SaturdayStartTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtStart1")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.SundayStartTime,
                                ActualValue = createUpdatePW517.SundayStartTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.SundayStartTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.SundayStartTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtEnd2")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.MondayEndTime,
                                ActualValue = createUpdatePW517.MondayEndTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.MondayEndTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.MondayEndTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtEnd3")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.TuesdayEndTime,
                                ActualValue = createUpdatePW517.TuesdayEndTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.TuesdayEndTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.TuesdayEndTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtEnd4")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.WednesdayEndTime,
                                ActualValue = createUpdatePW517.WednesdayEndTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.WednesdayEndTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.WednesdayEndTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtEnd5")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.ThursdayEndTime,
                                ActualValue = createUpdatePW517.ThursdayEndTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.ThursdayEndTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.ThursdayEndTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtEnd6")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.FridayEndTime,
                                ActualValue = createUpdatePW517.FridayEndTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.FridayEndTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.FridayEndTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtEnd7")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.SaturdayEndTime,
                                ActualValue = createUpdatePW517.SaturdayEndTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.SaturdayEndTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.SaturdayEndTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtEnd1")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.SundayEndTime,
                                ActualValue = createUpdatePW517.SundayEndTime
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.SundayEndTime;
                            updateJobDocumentField.ActualValue = createUpdatePW517.SundayEndTime;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    #endregion

                    #region Date
                    else if (item.Field.FieldName == "txtDate2")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.MondayDates,
                                ActualValue = createUpdatePW517.MondayDates
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.MondayDates;
                            updateJobDocumentField.ActualValue = createUpdatePW517.MondayDates;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtDate3")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.TuesdayDates,
                                ActualValue = createUpdatePW517.TuesdayDates
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.TuesdayDates;
                            updateJobDocumentField.ActualValue = createUpdatePW517.TuesdayDates;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtDate4")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.WednesdayDates,
                                ActualValue = createUpdatePW517.WednesdayDates
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.WednesdayDates;
                            updateJobDocumentField.ActualValue = createUpdatePW517.WednesdayDates;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtDate5")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.ThursdayDates,
                                ActualValue = createUpdatePW517.ThursdayDates
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.ThursdayDates;
                            updateJobDocumentField.ActualValue = createUpdatePW517.ThursdayDates;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtDate6")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.FridayDates,
                                ActualValue = createUpdatePW517.FridayDates
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.FridayDates;
                            updateJobDocumentField.ActualValue = createUpdatePW517.FridayDates;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtDate7")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.SaturdayDates,
                                ActualValue = createUpdatePW517.SaturdayDates
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.SaturdayDates;
                            updateJobDocumentField.ActualValue = createUpdatePW517.SaturdayDates;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (item.Field.FieldName == "txtDate1")
                    {
                        if (updateJobDocumentField == null)
                        {
                            JobDocumentField JobDocumentField = new JobDocumentField
                            {
                                IdJobDocument = idJobDocument,
                                IdDocumentField = Convert.ToInt32(item.Id),
                                Value = createUpdatePW517.SundayDates,
                                ActualValue = createUpdatePW517.SundayDates
                            };
                            rpoContext.JobDocumentFields.Add(JobDocumentField);
                        }
                        else
                        {
                            updateJobDocumentField.Value = createUpdatePW517.SundayDates;
                            updateJobDocumentField.ActualValue = createUpdatePW517.SundayDates;
                            rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                            rpoContext.SaveChanges();
                        }
                    }

                    #endregion

                    rpoContext.SaveChanges();
                }

            }

            string permitDates = string.Empty;

            if (createUpdatePW517.MondayDates != null && !string.IsNullOrEmpty(createUpdatePW517.MondayDates))
            {
                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? "," + createUpdatePW517.MondayDates : createUpdatePW517.MondayDates);
            }

            if (createUpdatePW517.TuesdayDates != null && !string.IsNullOrEmpty(createUpdatePW517.TuesdayDates))
            {
                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? "," + createUpdatePW517.TuesdayDates : createUpdatePW517.TuesdayDates);
            }

            if (createUpdatePW517.WednesdayDates != null && !string.IsNullOrEmpty(createUpdatePW517.WednesdayDates))
            {
                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? "," + createUpdatePW517.WednesdayDates : createUpdatePW517.WednesdayDates);
            }

            if (createUpdatePW517.ThursdayDates != null && !string.IsNullOrEmpty(createUpdatePW517.ThursdayDates))
            {
                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? "," + createUpdatePW517.ThursdayDates : createUpdatePW517.ThursdayDates);
            }

            if (createUpdatePW517.FridayDates != null && !string.IsNullOrEmpty(createUpdatePW517.FridayDates))
            {
                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? "," + createUpdatePW517.FridayDates : createUpdatePW517.FridayDates);
            }

            if (createUpdatePW517.SaturdayDates != null && !string.IsNullOrEmpty(createUpdatePW517.SaturdayDates))
            {
                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? "," + createUpdatePW517.SaturdayDates : createUpdatePW517.SaturdayDates);
            }

            if (createUpdatePW517.SundayDates != null && !string.IsNullOrEmpty(createUpdatePW517.SundayDates))
            {
                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? "," + createUpdatePW517.SundayDates : createUpdatePW517.SundayDates);
            }


            //List<string> documentDescriptionDates = permitDates != null && !string.IsNullOrEmpty(permitDates) ? (permitDates.Split(',') != null && permitDates.Split(',').Any() ? permitDates.Split(',').Select(x => x).ToList() : new List<string>()) : new List<string>();

            //documentDescriptionDates = documentDescriptionDates.OrderBy(x => x).ToList();
            //permitDates = string.Empty;
            //DateTime startDate = new DateTime();
            //DateTime previousDate = new DateTime();
            //DateTime currentDate = new DateTime();
            //bool isStartArray = true;
            //int index = 1;
            //bool continueDate = false;
            //string strDateRange = string.Empty;
            //foreach (string item in documentDescriptionDates)
            //{
            //    if (item != null && !string.IsNullOrEmpty(item))
            //    {
            //        DateTime dt = Convert.ToDateTime(item);
            //        if (isStartArray)
            //        {
            //            startDate = dt;
            //            previousDate = dt;
            //            currentDate = dt;
            //            isStartArray = false;
            //        }
            //        else
            //        {
            //            if (dt.AddDays(-1).Date == previousDate.Date)
            //            {
            //                continueDate = true;
            //            }
            //            else
            //            {
            //                continueDate = false;
            //            }
            //        }

            //        index = index + 1;
            //        if (continueDate == false)
            //        {
            //            strDateRange = startDate.ToString(Common.PW517ReportDateFormat);

            //            permitDates = permitDates + (!string.IsNullOrEmpty(strDateRange) ? ", " + item : item);
            //            isStartArray = true;
            //        }


            //    }
            //}

            List<string> documentDescriptionDates = permitDates != null && !string.IsNullOrEmpty(permitDates) ? (permitDates.Split(',') != null && permitDates.Split(',').Any() ? permitDates.Split(',').Select(x => x).ToList() : new List<string>()) : new List<string>();
            documentDescriptionDates = documentDescriptionDates.OrderBy(x => x).ToList();
            permitDates = string.Empty;
            DateTime startDate = new DateTime();
            DateTime previousDate = new DateTime();
            DateTime currentDate = new DateTime();
            bool isStartArray = true;
            int index = 1;
            bool continueDate = false;
            string strDateRange, strEndRange = string.Empty;
            int startIndex = 0;
            foreach (string item in documentDescriptionDates)
            {
                if (item != null && !string.IsNullOrEmpty(item))
                {
                    string[] dataValue = item.Split('/');
                    DateTime dt = new DateTime(Convert.ToInt32(dataValue[2]), Convert.ToInt32(dataValue[0]), Convert.ToInt32(dataValue[1]));
                    if (isStartArray)
                    {
                        startDate = dt;
                        previousDate = dt;
                        currentDate = dt;
                        isStartArray = false;
                        continueDate = true;
                        startIndex = startIndex + 1;
                    }
                    else
                    {
                        startIndex = startIndex + 1;
                        if (dt.AddDays(-1).Date == previousDate.Date)
                        {
                            previousDate = dt;
                            continueDate = true;
                        }
                        else
                        {
                            continueDate = false;
                        }
                    }

                    if (continueDate == false)
                    {
                        strDateRange = startDate.ToString(Common.PW517ReportDateFormat);
                        strEndRange = previousDate.ToString(Common.PW517ReportDateFormat);
                        isStartArray = true;
                        if (startIndex >= 1)
                        {
                            if (strDateRange == strEndRange)
                            {
                                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? ", " + strEndRange : strEndRange);
                            }
                            else
                            {
                                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? ", " + strDateRange + "-" + strEndRange : strDateRange + "-" + strEndRange);
                            }


                            startIndex = 0;
                        }
                        else
                        {
                            if (strDateRange == strEndRange)
                            {
                                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? ", " + strEndRange : strEndRange);
                            }
                            else
                            {
                                permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? ", " + strEndRange : strEndRange);
                            }
                            startIndex = 0;
                        }
                        startDate = dt;
                        previousDate = dt;
                        currentDate = dt;
                        isStartArray = false;
                        continueDate = true;
                    }

                    if (documentDescriptionDates.Count == index)
                    {
                        strDateRange = startDate.ToString(Common.PW517ReportDateFormat);
                        strEndRange = dt.ToString(Common.PW517ReportDateFormat);

                        if (startIndex >= 1)
                        {
                            permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? ", " + strDateRange + "-" + strEndRange : strDateRange + "-" + strEndRange);
                            startIndex = 0;
                        }
                        else
                        {
                            permitDates = permitDates + (!string.IsNullOrEmpty(permitDates) ? ", " + strEndRange : strEndRange);
                            startIndex = 0;
                        }
                        startDate = dt;
                        previousDate = dt;
                        currentDate = dt;
                        isStartArray = false;
                        continueDate = true;
                    }
                    index = index + 1;

                }
            }

            documentDescription = "Type: " + jobDocumentType.Type;

            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(createUpdatePW517.AHVReferenceNumber) ? " | " : string.Empty) + (!string.IsNullOrEmpty(createUpdatePW517.AHVReferenceNumber) ? "AHV Ref#: " + createUpdatePW517.AHVReferenceNumber : string.Empty);

            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(permitDates) ? " | " : string.Empty) + (!string.IsNullOrEmpty(permitDates) ? "Dates: " + permitDates : string.Empty);

            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(reasonForVariance) ? " | " : string.Empty) + (!string.IsNullOrEmpty(reasonForVariance) ? "Reason for variance: " + reasonForVariance : string.Empty);

            string applicantName = contractorFirstName + " " + contractorMiddleName + " " + contractorLastName;
            documentDescription = documentDescription + (!string.IsNullOrEmpty(documentDescription) && !string.IsNullOrEmpty(applicantName) ? " | " : string.Empty) + (!string.IsNullOrEmpty(applicantName) ? "Applicant: " + applicantName : string.Empty);

            
            documentDescription = documentDescription + (!string.IsNullOrEmpty(AHVName) ? " | Main AHV Work Contact: " + AHVName : string.Empty);

            documentDescription = documentDescription + (!string.IsNullOrEmpty(weekendWorkDescription) ? " | Work Description: " + weekendWorkDescription : string.Empty);

            documentDescription = documentDescription + (!string.IsNullOrEmpty(workpermit) ? " | Work Permit: " + workpermit : string.Empty);

            documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.IssuedDate) ? " | Issued Date: " + createUpdatePW517.IssuedDate : string.Empty);

            documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.SubmittedDate) ? " | Submitted Date: " + createUpdatePW517.SubmittedDate : string.Empty);

            documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.NumberOfDays) ? " | Number of Days: " + createUpdatePW517.NumberOfDays : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.MondayDates) ? " | Monday Date: " + createUpdatePW517.MondayDates : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.MondayStartTime) ? " | Monday Start Time: " + createUpdatePW517.MondayStartTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.MondayEndTime) ? " | Monday End Time: " + createUpdatePW517.MondayEndTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.TuesdayDates) ? " | Tuesday Date: " + createUpdatePW517.TuesdayDates : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.TuesdayStartTime) ? " | Tuesday Start Time: " + createUpdatePW517.TuesdayStartTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.TuesdayEndTime) ? " | Tuesday End Time: " + createUpdatePW517.TuesdayEndTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.WednesdayDates) ? " | Wednesday Date: " + createUpdatePW517.WednesdayDates : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.WednesdayStartTime) ? " | Wednesday Start Time: " + createUpdatePW517.WednesdayStartTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.WednesdayEndTime) ? " | Wednesday End Time: " + createUpdatePW517.WednesdayEndTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.ThursdayDates) ? " | Thursday Date: " + createUpdatePW517.ThursdayDates : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.ThursdayStartTime) ? " | Thursday Start Time: " + createUpdatePW517.ThursdayStartTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.ThursdayEndTime) ? " | Thursday End Time: " + createUpdatePW517.ThursdayEndTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.FridayDates) ? " | Friday Date: " + createUpdatePW517.FridayDates : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.FridayStartTime) ? " | Friday Start Time: " + createUpdatePW517.FridayStartTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.FridayEndTime) ? " | Friday End Time: " + createUpdatePW517.FridayEndTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.SaturdayDates) ? " | Saturday Date: " + createUpdatePW517.SaturdayDates : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.SaturdayStartTime) ? " | Saturday Start Time: " + createUpdatePW517.SaturdayStartTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.SaturdayEndTime) ? " | Saturday End Time: " + createUpdatePW517.SaturdayEndTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.SundayDates) ? " | Sunday Date: " + createUpdatePW517.SundayDates : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.SundayStartTime) ? " | Sunday Start Time: " + createUpdatePW517.SundayStartTime : string.Empty);

            //documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.SundayEndTime) ? " | Sunday End Time: " + createUpdatePW517.SundayEndTime : string.Empty);

            documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.AHVReferenceNumber) ? " | AHV Reference Number: " + createUpdatePW517.AHVReferenceNumber : string.Empty);
            documentDescription = documentDescription + (!string.IsNullOrEmpty(createUpdatePW517.NextDate) ? " | Next Date: " + createUpdatePW517.NextDate : string.Empty);

            jobDocument.DocumentDescription = documentDescription;
            jobDocument.JobDocumentFor = jobDocumentFor;
            rpoContext.SaveChanges();

            GenerateJobDocument(idJobDocument);

            if (createUpdatePW517 != null && createUpdatePW517.CreateSupportDocument != null && Convert.ToBoolean(createUpdatePW517.CreateSupportDocument))
            {
                GenerateSupportDocumentForAfterHoursPermitApplication_PW517(idJobDocument, jobDocument.IdJob, idEmployee, documentDescription, createUpdatePW517.Application);
            }
        }

        public static void GenerateSupportDocumentForAfterHoursPermitApplication_PW517(int idJobDocument, int idJob, int idemployee, string documentName, string Application)
        {
            RpoContext rpoContext = new RpoContext();
            int idDocument = Document.VarianceAfterHoursPermit_VARPMT.GetHashCode();
            DocumentMaster documentMaster = rpoContext.DocumentMasters.FirstOrDefault(x => x.Id == idDocument);
            JobDocument jobDocument = rpoContext.JobDocuments.FirstOrDefault(x => x.IdParent == idJobDocument);

            if (jobDocument == null)
            {
                jobDocument = new JobDocument
                {
                    IdJob = idJob,
                    IdDocument = idDocument,
                    DocumentName = documentMaster.DocumentName,
                    DocumentDescription = documentName,
                    IdJobApplication = Application != null ? Convert.ToInt32(Application) : 0,
                    IsArchived = false,
                    IdParent = idJobDocument,
                };
                jobDocument.CreatedDate = DateTime.UtcNow;
                jobDocument.LastModifiedDate = DateTime.UtcNow;
                jobDocument.CreatedBy = idemployee;
                jobDocument.LastModifiedBy = idemployee;

                rpoContext.JobDocuments.Add(jobDocument);
                rpoContext.SaveChanges();

                var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == idDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();
                foreach (var item in documentFieldList)
                {
                    if (item.Field.FieldName.ToString().Trim() == "Variance Permit")
                    {
                        JobDocumentField JobDocumentField = new JobDocumentField
                        {
                            IdJobDocument = jobDocument.Id,
                            IdDocumentField = Convert.ToInt32(item.Id),
                            Value = Convert.ToString(idJobDocument),
                            ActualValue =Convert.ToString(idJobDocument)
                        };
                        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        rpoContext.SaveChanges();
                    }
                    if (item.Field.FieldName.ToString().Trim() == "Attachment")
                    {
                        JobDocumentField JobDocumentField = new JobDocumentField
                        {
                            IdJobDocument = jobDocument.Id,
                            IdDocumentField = Convert.ToInt32(item.Id),
                            Value = string.Empty,
                            ActualValue = string.Empty
                        };
                        rpoContext.JobDocumentFields.Add(JobDocumentField);
                        rpoContext.SaveChanges();
                    }
                }
            }
            else
            {
                var documentFieldList = rpoContext.DocumentFields.Include("Field").Where(x => x.IdDocument == idDocument).OrderByDescending(x => x.Field.IsDisplayInFrontend).ThenBy(x => x.DisplayOrder).ToList();


                foreach (var item in documentFieldList)
                {
                    var updateJobDocumentField = documentFieldList.FirstOrDefault(x => x.Field.Id == item.Field.Id);
                    if (item.Field.FieldName.ToString().Trim() == "Variance Permit")
                    {
                        JobDocumentField JobDocumentField = new JobDocumentField
                        {
                            IdJobDocument = jobDocument.Id,
                            IdDocumentField = Convert.ToInt32(item.Id),
                            Value = Convert.ToString(idJobDocument),
                            ActualValue = Convert.ToString(idJobDocument)
                        };
                        //rpoContext.JobDocumentFields.Add(JobDocumentField);
                        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        rpoContext.SaveChanges();
                    }
                    if (item.Field.FieldName.ToString().Trim() == "Attachment")
                    {
                        JobDocumentField JobDocumentField = new JobDocumentField
                        {
                            IdJobDocument = jobDocument.Id,
                            IdDocumentField = Convert.ToInt32(item.Id),
                            Value = string.Empty,
                            ActualValue = string.Empty
                        };
                        //rpoContext.JobDocumentFields.Add(JobDocumentField);
                        rpoContext.Entry(updateJobDocumentField).State = EntityState.Modified;
                        rpoContext.SaveChanges();
                    }
                }
                jobDocument.IdJobApplication = Application != null ? Convert.ToInt32(Application) : 0;
                jobDocument.IdParent = idJobDocument;
                jobDocument.DocumentDescription = documentName;
                jobDocument.LastModifiedDate = DateTime.UtcNow;
                jobDocument.LastModifiedBy = idemployee;
                rpoContext.SaveChanges();
            }
        }
    }
}