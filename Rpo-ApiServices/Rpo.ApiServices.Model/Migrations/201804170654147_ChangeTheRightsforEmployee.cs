namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTheRightsforEmployee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "IdGroupPermission", "dbo.UserGroupPermissions");
            DropIndex("dbo.Groups", new[] { "IdGroupPermission" });
            AddColumn("dbo.Employees", "Permissions", c => c.String());
            DropColumn("dbo.Employees", "EmployeeEmployeeInfo");
            DropColumn("dbo.Employees", "EmployeeContactInfo");
            DropColumn("dbo.Employees", "EmployeePersonalInfo");
            DropColumn("dbo.Employees", "EmployeeAgentCertificates");
            DropColumn("dbo.Employees", "EmployeeSystemAccessInformation");
            DropColumn("dbo.Employees", "EmployeeUserGroup");
            DropColumn("dbo.Employees", "EmployeeDocuments");
            DropColumn("dbo.Employees", "EmployeeStatus");
            DropColumn("dbo.Employees", "Jobs");
            DropColumn("dbo.Employees", "Contacts");
            DropColumn("dbo.Employees", "ContactsExport");
            DropColumn("dbo.Employees", "Company");
            DropColumn("dbo.Employees", "CompanyExport");
            DropColumn("dbo.Employees", "RFP");
            DropColumn("dbo.Employees", "Tasks");
            DropColumn("dbo.Employees", "Reports");
            DropColumn("dbo.Employees", "ReferenceLinks");
            DropColumn("dbo.Employees", "ReferenceDocuments");
            DropColumn("dbo.Employees", "UserGroup");
            DropColumn("dbo.Employees", "Masters");
            DropColumn("dbo.Groups", "IdGroupPermission");
            DropColumn("dbo.Groups", "EmployeeEmployeeInfo");
            DropColumn("dbo.Groups", "EmployeeContactInfo");
            DropColumn("dbo.Groups", "EmployeePersonalInfo");
            DropColumn("dbo.Groups", "EmployeeAgentCertificates");
            DropColumn("dbo.Groups", "EmployeeSystemAccessInformation");
            DropColumn("dbo.Groups", "EmployeeUserGroup");
            DropColumn("dbo.Groups", "EmployeeDocuments");
            DropColumn("dbo.Groups", "EmployeeStatus");
            DropColumn("dbo.Groups", "Jobs");
            DropColumn("dbo.Groups", "Contacts");
            DropColumn("dbo.Groups", "ContactsExport");
            DropColumn("dbo.Groups", "Company");
            DropColumn("dbo.Groups", "CompanyExport");
            DropColumn("dbo.Groups", "RFP");
            DropColumn("dbo.Groups", "Tasks");
            DropColumn("dbo.Groups", "Reports");
            DropColumn("dbo.Groups", "ReferenceLinks");
            DropColumn("dbo.Groups", "ReferenceDocuments");
            DropColumn("dbo.Groups", "UserGroup");
            DropColumn("dbo.Groups", "Masters");
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
                        Permissions = p.String(),
                        IsActive = p.Boolean(),
                        LoginPassword = p.String(maxLength: 25),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        EmergencyContactName = p.String(maxLength: 50),
                        EmergencyContactNumber = p.String(maxLength: 25),
                        LockScreenPassword = p.String(maxLength: 25),
                        AppleId = p.String(maxLength: 50),
                        ApplePassword = p.String(maxLength: 25),
                        IsArchive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Employees]([FirstName], [LastName], [Address1], [Address2], [City], [IdState], [ZipCode], [WorkPhone], [WorkPhoneExt], [MobilePhone], [HomePhone], [Email], [Ssn], [Dob], [StartDate], [FinalDate], [Notes], [TelephonePassword], [ComputerPassword], [EfillingPassword], [EfillingUserName], [IdGroup], [Permissions], [IsActive], [LoginPassword], [CreatedBy], [CreatedDate], [EmergencyContactName], [EmergencyContactNumber], [LockScreenPassword], [AppleId], [ApplePassword], [IsArchive])
                      VALUES (@FirstName, @LastName, @Address1, @Address2, @City, @IdState, @ZipCode, @WorkPhone, @WorkPhoneExt, @MobilePhone, @HomePhone, @Email, @Ssn, @Dob, @StartDate, @FinalDate, @Notes, @TelephonePassword, @ComputerPassword, @EfillingPassword, @EfillingUserName, @IdGroup, @Permissions, @IsActive, @LoginPassword, @CreatedBy, @CreatedDate, @EmergencyContactName, @EmergencyContactNumber, @LockScreenPassword, @AppleId, @ApplePassword, @IsArchive)
                      
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
                        Permissions = p.String(),
                        IsActive = p.Boolean(),
                        LoginPassword = p.String(maxLength: 25),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        EmergencyContactName = p.String(maxLength: 50),
                        EmergencyContactNumber = p.String(maxLength: 25),
                        LockScreenPassword = p.String(maxLength: 25),
                        AppleId = p.String(maxLength: 50),
                        ApplePassword = p.String(maxLength: 25),
                        IsArchive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Employees]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [Address1] = @Address1, [Address2] = @Address2, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [HomePhone] = @HomePhone, [Email] = @Email, [Ssn] = @Ssn, [Dob] = @Dob, [StartDate] = @StartDate, [FinalDate] = @FinalDate, [Notes] = @Notes, [TelephonePassword] = @TelephonePassword, [ComputerPassword] = @ComputerPassword, [EfillingPassword] = @EfillingPassword, [EfillingUserName] = @EfillingUserName, [IdGroup] = @IdGroup, [Permissions] = @Permissions, [IsActive] = @IsActive, [LoginPassword] = @LoginPassword, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [EmergencyContactName] = @EmergencyContactName, [EmergencyContactNumber] = @EmergencyContactNumber, [LockScreenPassword] = @LockScreenPassword, [AppleId] = @AppleId, [ApplePassword] = @ApplePassword, [IsArchive] = @IsArchive
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Group_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
                        Description = p.String(),
                        Permissions = p.String(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Groups]([Name], [Description], [Permissions], [IsActive])
                      VALUES (@Name, @Description, @Permissions, @IsActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Groups]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Groups] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Group_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 100),
                        Description = p.String(),
                        Permissions = p.String(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Groups]
                      SET [Name] = @Name, [Description] = @Description, [Permissions] = @Permissions, [IsActive] = @IsActive
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "Masters", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "UserGroup", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "ReferenceDocuments", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "ReferenceLinks", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "Reports", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "Tasks", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "RFP", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "CompanyExport", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "Company", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "ContactsExport", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "Contacts", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "Jobs", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "EmployeeStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "EmployeeDocuments", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "EmployeeUserGroup", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "EmployeeSystemAccessInformation", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "EmployeeAgentCertificates", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "EmployeePersonalInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "EmployeeContactInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "EmployeeEmployeeInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "IdGroupPermission", c => c.Int());
            AddColumn("dbo.Employees", "Masters", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "UserGroup", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "ReferenceDocuments", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "ReferenceLinks", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Reports", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Tasks", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "RFP", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "CompanyExport", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Company", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "ContactsExport", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Contacts", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Jobs", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeDocuments", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeUserGroup", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeSystemAccessInformation", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeAgentCertificates", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeePersonalInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeContactInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeEmployeeInfo", c => c.Int(nullable: false));
            DropColumn("dbo.Employees", "Permissions");
            CreateIndex("dbo.Groups", "IdGroupPermission");
            AddForeignKey("dbo.Groups", "IdGroupPermission", "dbo.UserGroupPermissions", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
