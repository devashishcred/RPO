namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Employee_NewFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyTypes", "IdParent", "dbo.CompanyTypes");
            DropForeignKey("dbo.Companies", "IdCompanyType", "dbo.CompanyTypes");
            DropIndex("dbo.Companies", new[] { "IdCompanyType" });
            DropIndex("dbo.CompanyTypes", "IX_CompanyTypeName");
            DropIndex("dbo.CompanyTypes", new[] { "IdParent" });
            AddColumn("dbo.Employees", "EmployeeInfo", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Jobs", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Contacts", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Company", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "CompanyType", c => c.Int(nullable: false));
            DropColumn("dbo.Companies", "IdCompanyType");
            DropTable("dbo.CompanyTypes");
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
                        EmployeeInfo = p.Int(),
                        Jobs = p.Int(),
                        Contacts = p.Int(),
                        Company = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Employees]([FirstName], [LastName], [Address1], [Address2], [IdCity], [ZipCode], [WorkPhone], [WorkPhoneExt], [MobilePhone], [HomePhone], [Email], [Ssn], [Dob], [StartDate], [FinalDate], [Notes], [TelephonePassword], [ComputerPassword], [EfillingPassword], [EfillingUserName], [IdGroup], [EmployeeInfo], [Jobs], [Contacts], [Company])
                      VALUES (@FirstName, @LastName, @Address1, @Address2, @IdCity, @ZipCode, @WorkPhone, @WorkPhoneExt, @MobilePhone, @HomePhone, @Email, @Ssn, @Dob, @StartDate, @FinalDate, @Notes, @TelephonePassword, @ComputerPassword, @EfillingPassword, @EfillingUserName, @IdGroup, @EmployeeInfo, @Jobs, @Contacts, @Company)
                      
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
                        EmployeeInfo = p.Int(),
                        Jobs = p.Int(),
                        Contacts = p.Int(),
                        Company = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Employees]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [Address1] = @Address1, [Address2] = @Address2, [IdCity] = @IdCity, [ZipCode] = @ZipCode, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [HomePhone] = @HomePhone, [Email] = @Email, [Ssn] = @Ssn, [Dob] = @Dob, [StartDate] = @StartDate, [FinalDate] = @FinalDate, [Notes] = @Notes, [TelephonePassword] = @TelephonePassword, [ComputerPassword] = @ComputerPassword, [EfillingPassword] = @EfillingPassword, [EfillingUserName] = @EfillingUserName, [IdGroup] = @IdGroup, [EmployeeInfo] = @EmployeeInfo, [Jobs] = @Jobs, [Contacts] = @Contacts, [Company] = @Company
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
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 10),
                        InsuranceWorkCompensation = p.DateTime(),
                        InsuranceDisability = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                        CompanyType = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [TrackingNumber], [TrackingExpiry], [IBMNumber], [HICNumber], [HICExpiry], [TaxIdNumber], [InsuranceWorkCompensation], [InsuranceDisability], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes], [CompanyType])
                      VALUES (@Name, @TrackingNumber, @TrackingExpiry, @IBMNumber, @HICNumber, @HICExpiry, @TaxIdNumber, @InsuranceWorkCompensation, @InsuranceDisability, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes, @CompanyType)
                      
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
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 10),
                        InsuranceWorkCompensation = p.DateTime(),
                        InsuranceDisability = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                        CompanyType = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [TrackingNumber] = @TrackingNumber, [TrackingExpiry] = @TrackingExpiry, [IBMNumber] = @IBMNumber, [HICNumber] = @HICNumber, [HICExpiry] = @HICExpiry, [TaxIdNumber] = @TaxIdNumber, [InsuranceWorkCompensation] = @InsuranceWorkCompensation, [InsuranceDisability] = @InsuranceDisability, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes, [CompanyType] = @CompanyType
                      WHERE ([Id] = @Id)"
            );
            
            DropStoredProcedure("dbo.CompanyType_Insert");
            DropStoredProcedure("dbo.CompanyType_Update");
            DropStoredProcedure("dbo.CompanyType_Delete");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CompanyTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IdParent = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Companies", "IdCompanyType", c => c.Int(nullable: false));
            DropColumn("dbo.Companies", "CompanyType");
            DropColumn("dbo.Employees", "Company");
            DropColumn("dbo.Employees", "Contacts");
            DropColumn("dbo.Employees", "Jobs");
            DropColumn("dbo.Employees", "EmployeeInfo");
            CreateIndex("dbo.CompanyTypes", "IdParent");
            CreateIndex("dbo.CompanyTypes", "Name", unique: true, name: "IX_CompanyTypeName");
            CreateIndex("dbo.Companies", "IdCompanyType");
            AddForeignKey("dbo.Companies", "IdCompanyType", "dbo.CompanyTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyTypes", "IdParent", "dbo.CompanyTypes", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
