namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesOnCompanyContactsAddress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.Employees", "IdCity", "dbo.Cities");
            DropIndex("dbo.Addresses", new[] { "IdCity" });
            DropIndex("dbo.Employees", new[] { "IdCity" });
            CreateTable(
                "dbo.Suffixes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Addresses", "City", c => c.String());
            AddColumn("dbo.Addresses", "IdState", c => c.Int(nullable: false));
            AddColumn("dbo.Contacts", "IdSuffix", c => c.Int());
            AddColumn("dbo.Contacts", "OtherPhone", c => c.String(maxLength: 15));
            AddColumn("dbo.Employees", "City", c => c.String());
            AddColumn("dbo.Employees", "IdState", c => c.Int());
            CreateIndex("dbo.Addresses", "IdState");
            CreateIndex("dbo.Contacts", "IdSuffix");
            CreateIndex("dbo.Employees", "IdState");
            AddForeignKey("dbo.Contacts", "IdSuffix", "dbo.Suffixes", "Id");
            AddForeignKey("dbo.Addresses", "IdState", "dbo.States", "Id");
            AddForeignKey("dbo.Employees", "IdState", "dbo.States", "Id");
            DropColumn("dbo.Addresses", "IdCity");
            DropColumn("dbo.Companies", "CompanyType");
            DropColumn("dbo.Employees", "IdCity");
            CreateStoredProcedure(
                "dbo.Suffix_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 20),
                    },
                body:
                    @"INSERT [dbo].[Suffixes]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Suffixes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Suffixes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Suffix_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 20),
                    },
                body:
                    @"UPDATE [dbo].[Suffixes]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Suffix_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Suffixes]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Address_Insert",
                p => new
                    {
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Addresses]([IdAddressType], [Address1], [Address2], [City], [IdState], [ZipCode], [Phone], [IdCompany], [IdContact])
                      VALUES (@IdAddressType, @Address1, @Address2, @City, @IdState, @ZipCode, @Phone, @IdCompany, @IdContact)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Addresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Addresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Address_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Addresses]
                      SET [IdAddressType] = @IdAddressType, [Address1] = @Address1, [Address2] = @Address2, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [Phone] = @Phone, [IdCompany] = @IdCompany, [IdContact] = @IdContact
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        TrackingNumber = p.String(maxLength: 10),
                        TrackingExpiry = p.DateTime(),
                        IBMNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyExpiry = p.DateTime(),
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 10),
                        InsuranceWorkCompensation = p.DateTime(),
                        InsuranceDisability = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                        Url = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [TrackingNumber], [TrackingExpiry], [IBMNumber], [SpecialInspectionAgencyNumber], [SpecialInspectionAgencyExpiry], [HICNumber], [HICExpiry], [TaxIdNumber], [InsuranceWorkCompensation], [InsuranceDisability], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes], [Url])
                      VALUES (@Name, @TrackingNumber, @TrackingExpiry, @IBMNumber, @SpecialInspectionAgencyNumber, @SpecialInspectionAgencyExpiry, @HICNumber, @HICExpiry, @TaxIdNumber, @InsuranceWorkCompensation, @InsuranceDisability, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes, @Url)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Companies]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Companies] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Company_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        TrackingNumber = p.String(maxLength: 10),
                        TrackingExpiry = p.DateTime(),
                        IBMNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyExpiry = p.DateTime(),
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 10),
                        InsuranceWorkCompensation = p.DateTime(),
                        InsuranceDisability = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                        Url = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [TrackingNumber] = @TrackingNumber, [TrackingExpiry] = @TrackingExpiry, [IBMNumber] = @IBMNumber, [SpecialInspectionAgencyNumber] = @SpecialInspectionAgencyNumber, [SpecialInspectionAgencyExpiry] = @SpecialInspectionAgencyExpiry, [HICNumber] = @HICNumber, [HICExpiry] = @HICExpiry, [TaxIdNumber] = @TaxIdNumber, [InsuranceWorkCompensation] = @InsuranceWorkCompensation, [InsuranceDisability] = @InsuranceDisability, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes, [Url] = @Url
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Contact_Insert",
                p => new
                    {
                        Image = p.Binary(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        IdSuffix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        OtherPhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Notes = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Contacts]([Image], [PersonalType], [IdPrefix], [IdSuffix], [FirstName], [MiddleName], [LastName], [IdCompany], [IdContactTitle], [BirthDate], [WorkPhone], [WorkPhoneExt], [MobilePhone], [OtherPhone], [Email], [Notes])
                      VALUES (@Image, @PersonalType, @IdPrefix, @IdSuffix, @FirstName, @MiddleName, @LastName, @IdCompany, @IdContactTitle, @BirthDate, @WorkPhone, @WorkPhoneExt, @MobilePhone, @OtherPhone, @Email, @Notes)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Contacts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Contacts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Contact_Update",
                p => new
                    {
                        Id = p.Int(),
                        Image = p.Binary(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        IdSuffix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        OtherPhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Notes = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Contacts]
                      SET [Image] = @Image, [PersonalType] = @PersonalType, [IdPrefix] = @IdPrefix, [IdSuffix] = @IdSuffix, [FirstName] = @FirstName, [MiddleName] = @MiddleName, [LastName] = @LastName, [IdCompany] = @IdCompany, [IdContactTitle] = @IdContactTitle, [BirthDate] = @BirthDate, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [OtherPhone] = @OtherPhone, [Email] = @Email, [Notes] = @Notes
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Employee_Insert",
                p => new
                    {
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        HomePhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Ssn = p.String(maxLength: 10),
                        Dob = p.DateTime(),
                        StartDate = p.DateTime(),
                        FinalDate = p.DateTime(),
                        Notes = p.String(),
                        TelephonePassword = p.String(maxLength: 25),
                        ComputerPassword = p.String(maxLength: 25),
                        EfillingPassword = p.String(maxLength: 25),
                        EfillingUserName = p.String(maxLength: 25),
                        IdGroup = p.Int(),
                        EmployeeEmployeeInfo = p.Int(),
                        EmployeeContactInfo = p.Int(),
                        EmployeePersonalInfo = p.Int(),
                        EmployeeAgentCertificates = p.Int(),
                        EmployeeSystemAccessInformation = p.Int(),
                        EmployeeUserGroup = p.Int(),
                        EmployeeDocuments = p.Int(),
                        EmployeeStatus = p.Int(),
                        Jobs = p.Int(),
                        Contacts = p.Int(),
                        Company = p.Int(),
                        RFP = p.Int(),
                        Tasks = p.Int(),
                        Reports = p.Int(),
                        ReferenceLinks = p.Int(),
                        ReferenceDocuments = p.Int(),
                        UserGroup = p.Int(),
                        Masters = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Employees]([FirstName], [LastName], [Address1], [Address2], [City], [IdState], [ZipCode], [WorkPhone], [WorkPhoneExt], [MobilePhone], [HomePhone], [Email], [Ssn], [Dob], [StartDate], [FinalDate], [Notes], [TelephonePassword], [ComputerPassword], [EfillingPassword], [EfillingUserName], [IdGroup], [EmployeeEmployeeInfo], [EmployeeContactInfo], [EmployeePersonalInfo], [EmployeeAgentCertificates], [EmployeeSystemAccessInformation], [EmployeeUserGroup], [EmployeeDocuments], [EmployeeStatus], [Jobs], [Contacts], [Company], [RFP], [Tasks], [Reports], [ReferenceLinks], [ReferenceDocuments], [UserGroup], [Masters])
                      VALUES (@FirstName, @LastName, @Address1, @Address2, @City, @IdState, @ZipCode, @WorkPhone, @WorkPhoneExt, @MobilePhone, @HomePhone, @Email, @Ssn, @Dob, @StartDate, @FinalDate, @Notes, @TelephonePassword, @ComputerPassword, @EfillingPassword, @EfillingUserName, @IdGroup, @EmployeeEmployeeInfo, @EmployeeContactInfo, @EmployeePersonalInfo, @EmployeeAgentCertificates, @EmployeeSystemAccessInformation, @EmployeeUserGroup, @EmployeeDocuments, @EmployeeStatus, @Jobs, @Contacts, @Company, @RFP, @Tasks, @Reports, @ReferenceLinks, @ReferenceDocuments, @UserGroup, @Masters)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Employees]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Employees] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Employee_Update",
                p => new
                    {
                        Id = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        HomePhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Ssn = p.String(maxLength: 10),
                        Dob = p.DateTime(),
                        StartDate = p.DateTime(),
                        FinalDate = p.DateTime(),
                        Notes = p.String(),
                        TelephonePassword = p.String(maxLength: 25),
                        ComputerPassword = p.String(maxLength: 25),
                        EfillingPassword = p.String(maxLength: 25),
                        EfillingUserName = p.String(maxLength: 25),
                        IdGroup = p.Int(),
                        EmployeeEmployeeInfo = p.Int(),
                        EmployeeContactInfo = p.Int(),
                        EmployeePersonalInfo = p.Int(),
                        EmployeeAgentCertificates = p.Int(),
                        EmployeeSystemAccessInformation = p.Int(),
                        EmployeeUserGroup = p.Int(),
                        EmployeeDocuments = p.Int(),
                        EmployeeStatus = p.Int(),
                        Jobs = p.Int(),
                        Contacts = p.Int(),
                        Company = p.Int(),
                        RFP = p.Int(),
                        Tasks = p.Int(),
                        Reports = p.Int(),
                        ReferenceLinks = p.Int(),
                        ReferenceDocuments = p.Int(),
                        UserGroup = p.Int(),
                        Masters = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Employees]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [Address1] = @Address1, [Address2] = @Address2, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [HomePhone] = @HomePhone, [Email] = @Email, [Ssn] = @Ssn, [Dob] = @Dob, [StartDate] = @StartDate, [FinalDate] = @FinalDate, [Notes] = @Notes, [TelephonePassword] = @TelephonePassword, [ComputerPassword] = @ComputerPassword, [EfillingPassword] = @EfillingPassword, [EfillingUserName] = @EfillingUserName, [IdGroup] = @IdGroup, [EmployeeEmployeeInfo] = @EmployeeEmployeeInfo, [EmployeeContactInfo] = @EmployeeContactInfo, [EmployeePersonalInfo] = @EmployeePersonalInfo, [EmployeeAgentCertificates] = @EmployeeAgentCertificates, [EmployeeSystemAccessInformation] = @EmployeeSystemAccessInformation, [EmployeeUserGroup] = @EmployeeUserGroup, [EmployeeDocuments] = @EmployeeDocuments, [EmployeeStatus] = @EmployeeStatus, [Jobs] = @Jobs, [Contacts] = @Contacts, [Company] = @Company, [RFP] = @RFP, [Tasks] = @Tasks, [Reports] = @Reports, [ReferenceLinks] = @ReferenceLinks, [ReferenceDocuments] = @ReferenceDocuments, [UserGroup] = @UserGroup, [Masters] = @Masters
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Suffix_Delete");
            DropStoredProcedure("dbo.Suffix_Update");
            DropStoredProcedure("dbo.Suffix_Insert");
            AddColumn("dbo.Employees", "IdCity", c => c.Int());
            AddColumn("dbo.Companies", "CompanyType", c => c.Int(nullable: false));
            AddColumn("dbo.Addresses", "IdCity", c => c.Int(nullable: false));
            DropForeignKey("dbo.Employees", "IdState", "dbo.States");
            DropForeignKey("dbo.Addresses", "IdState", "dbo.States");
            DropForeignKey("dbo.Contacts", "IdSuffix", "dbo.Suffixes");
            DropIndex("dbo.Employees", new[] { "IdState" });
            DropIndex("dbo.Contacts", new[] { "IdSuffix" });
            DropIndex("dbo.Addresses", new[] { "IdState" });
            DropColumn("dbo.Employees", "IdState");
            DropColumn("dbo.Employees", "City");
            DropColumn("dbo.Contacts", "OtherPhone");
            DropColumn("dbo.Contacts", "IdSuffix");
            DropColumn("dbo.Addresses", "IdState");
            DropColumn("dbo.Addresses", "City");
            DropTable("dbo.Suffixes");
            CreateIndex("dbo.Employees", "IdCity");
            CreateIndex("dbo.Addresses", "IdCity");
            AddForeignKey("dbo.Employees", "IdCity", "dbo.Cities", "Id");
            AddForeignKey("dbo.Addresses", "IdCity", "dbo.Cities", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
