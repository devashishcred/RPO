
namespace Rpo.ApiServices.Model
{
    using Rpo.ApiServices.Model.Models;
    using Rpo.Identity.Core;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Linq;
    public class RpoContext : RpoIdentityDbContext
    {

        public DbSet<Group> Groups { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<AgentCertificate> AgentCertificates { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }

        public DbSet<RfpDocument> RfpDocuments { get; set; }

        public DbSet<TaskDocument> TaskDocuments { get; set; }

        public DbSet<AddressType> AddressTypes { get; set; }

        public DbSet<OwnerType> OwnerTypes { get; set; }
        
        public DbSet<JobContactGroup> JobContactGroups { get; set; }

        public DbSet<HolidayCalender> HolidayCalenders { get; set; }


        public DbSet<Address> Addresses { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<TaxIdType> TaxIdTypes { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<ContactLicense> ContactLicenses { get; set; }

        public DbSet<ContactLicenseType> ContactLicenseTypes { get; set; }

        public DbSet<CompanyLicense> CompanyLicenses { get; set; }

        public DbSet<CompanyLicenseType> CompanyLicenseTypes { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }

        public DbSet<ContactTitle> ContactTitles { get; set; }

        public DbSet<JobHistory> JobHistories { get; set; }

        public DbSet<TaskJobDocument> TaskJobDocuments { get; set; }

        public DbSet<SeismicDesignCategory> SeismicDesignCategories { get; set; }

        public DbSet<StructureOccupancyCategory> StructureOccupancyCategories { get; set; }

        public DbSet<PrimaryStructuralSystem> PrimaryStructuralSystems { get; set; }

        public DbSet<MultipleDwellingClassification> MultipleDwellingClassifications { get; set; }

        public DbSet<ConstructionClassification> ConstructionClassifications { get; set; }

        public DbSet<Borough> Boroughes { get; set; }

        public DbSet<OccupancyClassification> OccupancyClassifications { get; set; }

        public DbSet<RfpAddress> RfpAddresses { get; set; }

        public DbSet<TaskEmailReminderLog> TaskEmailReminderLogs { get; set; }

        public DbSet<Rfp> Rfps { get; set; }

        public DbSet<JobWorkType> JobWorkTypes { get; set; }

        public DbSet<JobTransmittal> JobTransmittals { get; set; }

        public DbSet<JobTransmittalCC> JobTransmittalCCs { get; set; }

        public DbSet<JobTransmittalAttachment> JobTransmittalAttachments { get; set; }

        public DbSet<JobTransmittalJobDocument> JobTransmittalJobDocuments { get; set; }

        public DbSet<JobTimeNoteCategory> JobTimeNoteCategories { get; set; }

        public DbSet<ApplicationStatus> ApplicationStatus { get; set; }

        public DbSet<OpenMapAddress> OpenMapAddress { get; set; }

        public DbSet<JobApplicationType> JobApplicationTypes { get; set; }

        public DbSet<JobApplicationWorkPermitType> JobApplicationWorkPermitTypes { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<ProjectDetail> ProjectDetails { get; set; }

        public DbSet<RfpFeeSchedule> RfpFeeSchedules { get; set; }

        public DbSet<JobFeeSchedule> JobFeeSchedules { get; set; }

        public DbSet<ContactDocument> ContactDocuments { get; set; }
        public DbSet<CompanyDocument> CompanyDocuments { get; set; }

        public DbSet<Verbiage> Verbiages { get; set; }

        public DbSet<EmailType> EmailTypes { get; set; }

        public DbSet<RfpStatus> RfpStatuses { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<TransmissionType> TransmissionTypes { get; set; }

        public DbSet<TransmissionTypeDefaultCC> TransmissionTypeDefaultCCs { get; set; }

        public DbSet<ReferenceLink> ReferenceLinks { get; set; }

        public DbSet<ReferenceDocument> ReferenceDocuments { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobScope> JobScopes { get; set; }

        public DbSet<JobContactType> JobContactTypes { get; set; }

        public DbSet<JobMilestone> JobMilestones { get; set; }

        public DbSet<Milestone> Milestones { get; set; }

        public DbSet<MilestoneService> MilestoneServices { get; set; }

        public DbSet<JobMilestoneService> JobMilestoneServices { get; set; }

        public DbSet<Suffix> Sufixes { get; set; }

        public DbSet<DEPCostSetting> DEPCostSettings { get; set; }


        public DbSet<CompanyType> CompanyTypes { get; set; }

        public DbSet<JobTimeNote> JobTimeNotes { get; set; }

        public DbSet<RFPEmailCCHistory> RFPEmailCCHistories { get; set; }

        public DbSet<RFPEmailAttachmentHistory> RFPEmailAttachmentHistories { get; set; }

        public DbSet<RFPEmailHistory> RFPEmailHistories { get; set; }

        public DbSet<CompanyEmailCCHistory> CompanyEmailCCHistories { get; set; }

        public DbSet<CompanyEmailAttachmentHistory> CompanyEmailAttachmentHistories { get; set; }

        public DbSet<CompanyEmailHistory> CompanyEmailHistories { get; set; }

        public DbSet<ContactEmailCCHistory> ContactEmailCCHistories { get; set; }

        public DbSet<ContactEmailAttachmentHistory> ContactEmailAttachmentHistories { get; set; }

        public DbSet<ContactEmailHistory> ContactEmailHistories { get; set; }

        public DbSet<TaskNote> TaskNotes { get; set; }

        public DbSet<TaskReminder> TaskReminders { get; set; }
        
        public DbSet<TaskStatus> TaskStatuses { get; set; }

        public DbSet<TaskType> TaskTypes { get; set; }

        public DbSet<RfpJobType> RfpJobTypes { get; set; }

        public DbSet<RfpProposalReview> RfpProposalReviews { get; set; }

        public DbSet<RfpJobTypeCostRange> RfpJobTypeCostRanges { get; set; }

        public DbSet<RfpJobTypeCumulativeCost> RfpJobTypeCumulativeCosts { get; set; }

        public DbSet<RfpProgressNote> RfpProgressNotes { get; set; }
        public DbSet<JobProgressNote> JobProgressNotes { get; set; }
        public DbSet<JobViolationNote> JobViolationNotes { get; set; }
        
        public DbSet<RfpReviewer> RfpReviewers { get; set; }

        public DbSet<RfpScopeReview> RfpScopeReviews { get; set; }

        public DbSet<SystemSetting> SystemSettings { get; set; }

        public DbSet<EmployeePermission> EmployeePermissions { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserGroupPermission> UserGroupPermissions { get; set; }

        public DbSet<DocumentMaster> DocumentMasters { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<DocumentField> DocumentFields { get; set; }

        public DbSet<JobDocument> JobDocuments { get; set; }

        public DbSet<JobDocumentField> JobDocumentFields { get; set; }

        public DbSet<JobDocumentAttachment> JobDocumentAttachments { get; set; }

        public DbSet<TaskHistory> TaskHistories { get; set; }

        public DbSet<JobTimeNoteHistory> JobTimeNoteHistories { get; set; }

        public DbSet<JobDocumentType> JobDocumentTypes { get; set; }

        public DbSet<JobViolation> JobViolations { get; set; }

        public DbSet<ViolationPaneltyCode> ViolationPaneltyCodes { get; set; }

        public DbSet<JobViolationDocument> JobViolationDocuments { get; set; }

        public DbSet<JobViolationExplanationOfCharges> JobViolationExplanationOfCharges { get; set; }

        public DbSet<JobWorkPermitHistory> JobWorkPermitHistories { get; set; }

        public DbSet<DOBPenaltySchedule> DOBPenaltySchedules { get; set; }

        public DbSet<DEPNoiseCodePenaltySchedule> DEPNoiseCodePenaltySchedules { get; set; }
        
        public DbSet<DOTPenaltySchedule> DOTPenaltySchedules { get; set; }
        
        public DbSet<DOHMHCoolingTowerPenaltySchedule> DOHMHCoolingTowerPenaltySchedules { get; set; }

        public DbSet<FDNYPenaltySchedule> FDNYPenaltySchedules { get; set; }
        public DbSet<CheckListGroup> CheckListGroups { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<ChecklistAddressProperty> ChecklistAddressProperty { get; set; }
        public DbSet<ChecklistAddressPropertyMaping> ChecklistAddressPropertyMaping { get; set; }
        public DbSet<JobChecklistHeader> JobChecklistHeaders { get; set; }
      //  public DbSet<JobChecklistGroupDetail> JobChecklistGroupDetails { get; set; }
        public DbSet<JobChecklistGroup> JobChecklistGroups { get; set; }
        
        //  public DbSet<JobPlumbingCheckListFloors> JobPlumbingCheckListFloors { get; set; }
        public DbSet<JobChecklistItemDetail> JobChecklistItemDetails { get; set; }
        public DbSet<JobCheckListProgressNoteHistory> JobCheckListProgressNoteHistories { get; set; }
        public DbSet<JobCheckListCommentHistory> JobCheckListCommentHistories { get; set; }
        public DbSet<JobChecklistItemDueDate> JobChecklistItemDueDate { get; set; }

      //  public DbSet<JobChecklistPlumbingInspection> JobPlumbingCheckListInspection { get; set; }
        public DbSet<JobPlumbingInspection> JobPlumbingInspection { get; set; }
        

        public DbSet<JobPlumbingChecklistFloors> JobPlumbingChecklistFloors { get; set; }       

        public DbSet<JobPlumbingInspectionComment> JobPlumbingInspectionComments { get; set; }
        public DbSet<JobPlumbingInspectionDueDate> JobPlumbingInspectionDueDate { get; set; }
        public DbSet<JobPlumbingInspectionProgressNoteHistory> JobPlumbingInspectionProgressNoteHistory { get; set; }
        public DbSet<CompositeChecklist> CompositeChecklists { get; set; }
        public DbSet<CompositeChecklistDetail> CompositeChecklistDetails { get; set; }
        public DbSet<CompositeViolations> CompositeViolations { get; set; }
        public DbSet<ChecklistJobViolationComment> ChecklistJobViolationComments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerInvitationStatus> CustomerInvitationStatus { get; set; }

        public DbSet<CustomerJobAccess> CustomerJobAccess { get; set; }
        public DbSet<CustomerPasswordReset> CustomerPasswordResets { get; set; }
      
          public DbSet<ClientNoteCustomer> ClientNoteCustomers { get; set; }
        public DbSet<CustomerJobName> CustomerJobNames { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<CustomerNotificationSetting> CustomerNotificationSettings { get; set; }
        public DbSet<CustomerNotification> CustomerNotifications { get; set; }
        public DbSet<ClientNotePlumbingCustomer> ClientNotePlumbingCustomers { get; set; }
        


        public static RpoContext CreateRpoContext()
        {
            return new RpoContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Map Stored Procedures
            modelBuilder.Entity<Group>().MapToStoredProcedures();
            modelBuilder.Entity<Employee>().MapToStoredProcedures();
            modelBuilder.Entity<State>().MapToStoredProcedures();
            modelBuilder.Entity<City>().MapToStoredProcedures();
            modelBuilder.Entity<AgentCertificate>().MapToStoredProcedures();
            modelBuilder.Entity<DocumentType>().MapToStoredProcedures();
            modelBuilder.Entity<EmployeeDocument>().MapToStoredProcedures();
            //modelBuilder.Entity<CompanyType>().MapToStoredProcedures();
            modelBuilder.Entity<AddressType>().MapToStoredProcedures();
            modelBuilder.Entity<Address>().MapToStoredProcedures();
            modelBuilder.Entity<Company>().MapToStoredProcedures();
            modelBuilder.Entity<TaxIdType>().MapToStoredProcedures();
            modelBuilder.Entity<Contact>().MapToStoredProcedures();
            modelBuilder.Entity<ContactLicense>().MapToStoredProcedures();
            modelBuilder.Entity<ContactLicenseType>().MapToStoredProcedures();
            modelBuilder.Entity<ContactTitle>().MapToStoredProcedures();
            modelBuilder.Entity<SeismicDesignCategory>().MapToStoredProcedures();
            modelBuilder.Entity<StructureOccupancyCategory>().MapToStoredProcedures();
            modelBuilder.Entity<PrimaryStructuralSystem>().MapToStoredProcedures();
            modelBuilder.Entity<MultipleDwellingClassification>().MapToStoredProcedures();
            modelBuilder.Entity<ConstructionClassification>().MapToStoredProcedures();
            modelBuilder.Entity<Borough>().MapToStoredProcedures();
            modelBuilder.Entity<OccupancyClassification>().MapToStoredProcedures();
            modelBuilder.Entity<RfpAddress>().MapToStoredProcedures();
            modelBuilder.Entity<Rfp>().MapToStoredProcedures();

            //modelBuilder.Entity<WorkTypeNote>().MapToStoredProcedures();
            //modelBuilder.Entity<WorkType>().MapToStoredProcedures();
            //modelBuilder.Entity<JobType>().MapToStoredProcedures();
            modelBuilder.Entity<ProjectDetail>().MapToStoredProcedures();

            modelBuilder.Entity<RfpProposalReview>().MapToStoredProcedures();
            modelBuilder.Entity<Milestone>().MapToStoredProcedures();
            modelBuilder.Entity<RfpScopeReview>().MapToStoredProcedures();

            modelBuilder.Entity<Verbiage>().MapToStoredProcedures();

            modelBuilder.Entity<EmailType>().MapToStoredProcedures();

            modelBuilder.Entity<TransmissionType>().MapToStoredProcedures();

            modelBuilder.Entity<ReferenceLink>().MapToStoredProcedures();

            modelBuilder.Entity<ReferenceDocument>().MapToStoredProcedures();

            modelBuilder.Entity<Suffix>().MapToStoredProcedures();

            modelBuilder.Entity<Job>().MapToStoredProcedures();

            modelBuilder.Entity<CompanyType>().MapToStoredProcedures();

            modelBuilder.Entity<JobTimeNote>().MapToStoredProcedures();

            modelBuilder.Entity<Task>().MapToStoredProcedures();

            modelBuilder.Entity<TaskNote>().MapToStoredProcedures();
            modelBuilder.Entity<CheckListGroup>().MapToStoredProcedures();
            modelBuilder.Entity<ChecklistItem>().MapToStoredProcedures();
            modelBuilder.Entity<ChecklistAddressProperty>().MapToStoredProcedures();
            modelBuilder.Entity<ChecklistAddressPropertyMaping>().MapToStoredProcedures();
            modelBuilder.Entity<JobChecklistHeader>().MapToStoredProcedures();
          //  modelBuilder.Entity<JobChecklistGroupDetail>().MapToStoredProcedures();
            modelBuilder.Entity<JobChecklistGroup>().MapToStoredProcedures(); 
            modelBuilder.Entity<JobPlumbingChecklistFloors>().MapToStoredProcedures();
            modelBuilder.Entity<JobChecklistItemDetail>().MapToStoredProcedures();
            modelBuilder.Entity<JobCheckListCommentHistory>().MapToStoredProcedures();
            modelBuilder.Entity<JobCheckListProgressNoteHistory>().MapToStoredProcedures();
            modelBuilder.Entity<JobChecklistItemDueDate>().MapToStoredProcedures();
            modelBuilder.Entity<JobPlumbingInspection>().MapToStoredProcedures();          
            modelBuilder.Entity<JobPlumbingInspectionComment>().MapToStoredProcedures();
            modelBuilder.Entity<JobPlumbingInspectionDueDate>().MapToStoredProcedures();
            modelBuilder.Entity<JobPlumbingInspectionProgressNoteHistory>().MapToStoredProcedures();
            modelBuilder.Entity<CompositeChecklist>().MapToStoredProcedures();
            modelBuilder.Entity<CompositeChecklistDetail>().MapToStoredProcedures();
            modelBuilder.Entity<Customer>().MapToStoredProcedures();
            modelBuilder.Entity<CustomerInvitationStatus>().MapToStoredProcedures();
            modelBuilder.Entity<CustomerJobAccess>().MapToStoredProcedures();
            modelBuilder.Entity<CustomerPasswordReset>().MapToStoredProcedures();        
            modelBuilder.Entity<CustomerJobName>().MapToStoredProcedures();
            modelBuilder.Entity<News>().MapToStoredProcedures();
            modelBuilder.Entity<CustomerNotificationSetting>().MapToStoredProcedures();
            modelBuilder.Entity<ClientNoteCustomer>().MapToStoredProcedures();
            modelBuilder.Entity<ClientNotePlumbingCustomer>().MapToStoredProcedures();
            
            //modelBuilder.Entity<Rfp>()
            //    .HasOptional(s => s.ProposalReview)
            //    .WithRequired();
            //modelBuilder.Entity<Rfp>()
            //    .HasOptional(s => s.ScopeReview)
            //    .WithRequired();

            //modelBuilder.Entity<RfpScopeReview>()
            //    .HasMany<Contact>(sr => sr.ContactsCc)
            //    .WithMany();

            //modelBuilder.Entity<JobType>()
            //    .HasMany<WorkType>(jt => jt.WorkTypes)
            //    .WithMany();

            modelBuilder.Entity<ContactDocument>().MapToStoredProcedures();

            modelBuilder.Entity<CompanyDocument>().MapToStoredProcedures();

            modelBuilder.Entity<AgentCertificate>()
                .HasRequired(a => a.Employee)
                .WithMany(e => e.AgentCertificates)
                .HasForeignKey(a => a.IdEmployee)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<EmployeeDocument>()
                .HasRequired(d => d.Employee)
                .WithMany(e => e.Documents)
                .HasForeignKey(a => a.IdEmployee)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Employee>()
                .HasOptional(a => a.LastModifiedByEmployee)
                .WithMany()
                .HasForeignKey(a => a.LastModifiedBy);

            modelBuilder.Entity<ContactDocument>()
                .HasRequired(d => d.Contact)
                .WithMany(e => e.Documents)
                .HasForeignKey(a => a.IdContact)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CompanyDocument>()
              .HasRequired(d => d.Company)
              .WithMany(e => e.Documents)
              .HasForeignKey(a => a.IdCompany)
              .WillCascadeOnDelete(true);

            modelBuilder.Entity<ContactLicense>()
                .HasRequired(d => d.Contact)
                .WithMany(e => e.ContactLicenses)
                .HasForeignKey(a => a.IdContact)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Address>()
                .HasOptional(a => a.Contact)
                .WithMany(e => e.Addresses)
                .HasForeignKey(a => a.IdContact)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Job>()
                .HasMany<JobApplicationType>(j => j.JobApplicationTypes)
                .WithMany();

            modelBuilder.Entity<Company>()
                .HasMany<CompanyType>(c => c.CompanyTypes)
                .WithMany();

            modelBuilder.Entity<ChecklistItem>()
             .HasMany<JobApplicationType>(c => c.JobApplicationTypes)
             .WithMany();
           // modelBuilder.Entity<ChecklistItem>()
           //.HasMany<JobApplicationWorkPermitType>(c => c.JobApplicationWorkPermitTypes)
           //.WithMany();
            modelBuilder.Entity<ChecklistItem>()
          .HasMany<JobWorkType>(c => c.JobWorkTypes)
          .WithMany();
            modelBuilder.Entity<ChecklistItem>()
       .HasMany<ReferenceDocument>(c => c.ReferenceDocuments)
       .WithMany();
            modelBuilder.Entity<JobChecklistHeader>()
      .HasMany<JobApplicationWorkPermitType>(c => c.JobApplicationWorkPermitTypes)
      .WithMany();

            modelBuilder.Entity<Job>()
                .HasMany<Job>(j => j.Jobs)
                .WithMany()
                .MapToStoredProcedures();
        }

        public DbSet<Prefix> Prefixes { get; set; }

        public DbSet<JobContact> JobContacts { get; set; }

        public DbSet<JobContactJobContactGroup> JobContactJobContactGroups { get; set; }
        
        public DbSet<Task> Tasks { get; set; }
        public DbSet<DOBPermitMapping> DOBPermitMappings { get; set; }


        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);

                        string strLogText = "";

                        string innerExceptionmessage = string.Empty;

                        strLogText += Environment.NewLine + "------------------------------------------------------------";
                        strLogText += Environment.NewLine + "DatabaseEntityMessage ---\n{0}" + message;
                        strLogText += Environment.NewLine + "------------------------------------------------------------";


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
                                stwriter.WriteLine("DatabaseEntityMessage: " + strLogText.ToString());
                                stwriter.WriteLine("-------------------End----------------------------");
                                stwriter.Close();
                            }
                        }
                        else
                        {
                            StreamWriter stwriter = File.CreateText(path);
                            stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                            stwriter.WriteLine("DatabaseEntityMessage: " + strLogText.ToString());
                            stwriter.WriteLine("-------------------End----------------------------");
                            stwriter.Close();
                        }
                    }
                }
                throw raise;
            }
            catch (DbUpdateException ex)
            {

                string strLogText = "";

                string innerExceptionmessage = string.Empty;

                strLogText += Environment.NewLine + "------------------------------------------------------------";
                strLogText += Environment.NewLine + "DatabaseEntityMessage ---\n{0}" + ex.Message;
                strLogText += Environment.NewLine + "DatabaseEntityInnerException ---\n{0}" + ex.InnerException.Message;
                strLogText += Environment.NewLine + "------------------------------------------------------------";


                var timeUtc = DateTime.Now;

                string errorLogFilename = "ErrorLogDbentity_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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
                        stwriter.WriteLine("DatabaseEntityMessage: " + strLogText.ToString());
                        stwriter.WriteLine("-------------------End----------------------------");
                        stwriter.Close();
                    }
                }
                else
                {
                    StreamWriter stwriter = File.CreateText(path);
                    stwriter.WriteLine("-------------------Error Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                    stwriter.WriteLine("DatabaseEntityMessage: " + strLogText.ToString());
                    stwriter.WriteLine("-------------------End----------------------------");
                    stwriter.Close();
                }                

                throw ex;
            }
        }
    }
}
