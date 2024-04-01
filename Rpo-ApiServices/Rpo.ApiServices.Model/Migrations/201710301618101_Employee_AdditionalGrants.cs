namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Employee_AdditionalGrants : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "EmployeeEmployeeInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeContactInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeePersonalInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeAgentCertificates", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeSystemAccessInformation", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeUserGroup", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeDocuments", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "RFP", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Tasks", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Reports", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "ReferenceLinks", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "ReferenceDocuments", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "UserGroup", c => c.Int(nullable: false));
            DropColumn("dbo.Employees", "EmployeeInfo");
            AlterStoredProcedure(
                "dbo.Employee_Insert",
                p => new
                    {
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        IdCity = p.Int(),
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
                    },
                body:
                    @"INSERT [dbo].[Employees]([FirstName], [LastName], [Address1], [Address2], [IdCity], [ZipCode], [WorkPhone], [WorkPhoneExt], [MobilePhone], [HomePhone], [Email], [Ssn], [Dob], [StartDate], [FinalDate], [Notes], [TelephonePassword], [ComputerPassword], [EfillingPassword], [EfillingUserName], [IdGroup], [EmployeeEmployeeInfo], [EmployeeContactInfo], [EmployeePersonalInfo], [EmployeeAgentCertificates], [EmployeeSystemAccessInformation], [EmployeeUserGroup], [EmployeeDocuments], [EmployeeStatus], [Jobs], [Contacts], [Company], [RFP], [Tasks], [Reports], [ReferenceLinks], [ReferenceDocuments], [UserGroup])
                      VALUES (@FirstName, @LastName, @Address1, @Address2, @IdCity, @ZipCode, @WorkPhone, @WorkPhoneExt, @MobilePhone, @HomePhone, @Email, @Ssn, @Dob, @StartDate, @FinalDate, @Notes, @TelephonePassword, @ComputerPassword, @EfillingPassword, @EfillingUserName, @IdGroup, @EmployeeEmployeeInfo, @EmployeeContactInfo, @EmployeePersonalInfo, @EmployeeAgentCertificates, @EmployeeSystemAccessInformation, @EmployeeUserGroup, @EmployeeDocuments, @EmployeeStatus, @Jobs, @Contacts, @Company, @RFP, @Tasks, @Reports, @ReferenceLinks, @ReferenceDocuments, @UserGroup)
                      
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
                        IdCity = p.Int(),
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
                    },
                body:
                    @"UPDATE [dbo].[Employees]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [Address1] = @Address1, [Address2] = @Address2, [IdCity] = @IdCity, [ZipCode] = @ZipCode, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [HomePhone] = @HomePhone, [Email] = @Email, [Ssn] = @Ssn, [Dob] = @Dob, [StartDate] = @StartDate, [FinalDate] = @FinalDate, [Notes] = @Notes, [TelephonePassword] = @TelephonePassword, [ComputerPassword] = @ComputerPassword, [EfillingPassword] = @EfillingPassword, [EfillingUserName] = @EfillingUserName, [IdGroup] = @IdGroup, [EmployeeEmployeeInfo] = @EmployeeEmployeeInfo, [EmployeeContactInfo] = @EmployeeContactInfo, [EmployeePersonalInfo] = @EmployeePersonalInfo, [EmployeeAgentCertificates] = @EmployeeAgentCertificates, [EmployeeSystemAccessInformation] = @EmployeeSystemAccessInformation, [EmployeeUserGroup] = @EmployeeUserGroup, [EmployeeDocuments] = @EmployeeDocuments, [EmployeeStatus] = @EmployeeStatus, [Jobs] = @Jobs, [Contacts] = @Contacts, [Company] = @Company, [RFP] = @RFP, [Tasks] = @Tasks, [Reports] = @Reports, [ReferenceLinks] = @ReferenceLinks, [ReferenceDocuments] = @ReferenceDocuments, [UserGroup] = @UserGroup
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "EmployeeInfo", c => c.Int(nullable: false));
            DropColumn("dbo.Employees", "UserGroup");
            DropColumn("dbo.Employees", "ReferenceDocuments");
            DropColumn("dbo.Employees", "ReferenceLinks");
            DropColumn("dbo.Employees", "Reports");
            DropColumn("dbo.Employees", "Tasks");
            DropColumn("dbo.Employees", "RFP");
            DropColumn("dbo.Employees", "EmployeeStatus");
            DropColumn("dbo.Employees", "EmployeeDocuments");
            DropColumn("dbo.Employees", "EmployeeUserGroup");
            DropColumn("dbo.Employees", "EmployeeSystemAccessInformation");
            DropColumn("dbo.Employees", "EmployeeAgentCertificates");
            DropColumn("dbo.Employees", "EmployeePersonalInfo");
            DropColumn("dbo.Employees", "EmployeeContactInfo");
            DropColumn("dbo.Employees", "EmployeeEmployeeInfo");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
