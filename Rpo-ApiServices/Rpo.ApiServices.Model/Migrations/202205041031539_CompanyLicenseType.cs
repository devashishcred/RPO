namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyLicenseType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyLicenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCompanyLicenseType = c.Int(nullable: false),
                        Number = c.String(maxLength: 15),
                        ExpirationLicenseDate = c.DateTime(),
                        IdCompany = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.IdCompany)
                .ForeignKey("dbo.CompanyLicenseTypes", t => t.IdCompanyLicenseType)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.IdCompanyLicenseType)
                .Index(t => t.IdCompany)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.CompanyLicenseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.Name, unique: true, name: "IX_CompanyLicenseTypeName")
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            CreateTable(
                "dbo.Responsibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResponsibilityName = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
            AddColumn("dbo.Companies", "EmailAddress", c => c.String());
            AddColumn("dbo.Companies", "EmailPassword", c => c.String());
            AddColumn("dbo.Companies", "IDResponsibility", c => c.Int());
            AddColumn("dbo.JobApplications", "IdJobWorkType", c => c.Int());
            CreateIndex("dbo.Companies", "IDResponsibility");
            CreateIndex("dbo.JobApplications", "IdJobWorkType");
            AddForeignKey("dbo.Companies", "IDResponsibility", "dbo.Responsibilities", "Id");
            AddForeignKey("dbo.JobApplications", "IdJobWorkType", "dbo.JobWorkTypes", "Id");
            AlterStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
                        TrackingNumber = p.String(maxLength: 10),
                        TrackingExpiry = p.DateTime(),
                        IBMNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyExpiry = p.DateTime(),
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        CTLicenseNumber = p.String(),
                        CTExpirationDate = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 9),
                        InsuranceWorkCompensation = p.DateTime(),
                        InsuranceDisability = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                        Url = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        DOTInsuranceWorkCompensation = p.DateTime(),
                        DOTInsuranceGeneralLiability = p.DateTime(),
                        EmailAddress = p.String(),
                        EmailPassword = p.String(),
                        IDResponsibility = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [TrackingNumber], [TrackingExpiry], [IBMNumber], [SpecialInspectionAgencyNumber], [SpecialInspectionAgencyExpiry], [HICNumber], [HICExpiry], [CTLicenseNumber], [CTExpirationDate], [TaxIdNumber], [InsuranceWorkCompensation], [InsuranceDisability], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes], [Url], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [DOTInsuranceWorkCompensation], [DOTInsuranceGeneralLiability], [EmailAddress], [EmailPassword], [IDResponsibility])
                      VALUES (@Name, @TrackingNumber, @TrackingExpiry, @IBMNumber, @SpecialInspectionAgencyNumber, @SpecialInspectionAgencyExpiry, @HICNumber, @HICExpiry, @CTLicenseNumber, @CTExpirationDate, @TaxIdNumber, @InsuranceWorkCompensation, @InsuranceDisability, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes, @Url, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @DOTInsuranceWorkCompensation, @DOTInsuranceGeneralLiability, @EmailAddress, @EmailPassword, @IDResponsibility)
                      
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
                        Name = p.String(maxLength: 100),
                        TrackingNumber = p.String(maxLength: 10),
                        TrackingExpiry = p.DateTime(),
                        IBMNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyNumber = p.String(maxLength: 10),
                        SpecialInspectionAgencyExpiry = p.DateTime(),
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        CTLicenseNumber = p.String(),
                        CTExpirationDate = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 9),
                        InsuranceWorkCompensation = p.DateTime(),
                        InsuranceDisability = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                        Url = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        DOTInsuranceWorkCompensation = p.DateTime(),
                        DOTInsuranceGeneralLiability = p.DateTime(),
                        EmailAddress = p.String(),
                        EmailPassword = p.String(),
                        IDResponsibility = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [TrackingNumber] = @TrackingNumber, [TrackingExpiry] = @TrackingExpiry, [IBMNumber] = @IBMNumber, [SpecialInspectionAgencyNumber] = @SpecialInspectionAgencyNumber, [SpecialInspectionAgencyExpiry] = @SpecialInspectionAgencyExpiry, [HICNumber] = @HICNumber, [HICExpiry] = @HICExpiry, [CTLicenseNumber] = @CTLicenseNumber, [CTExpirationDate] = @CTExpirationDate, [TaxIdNumber] = @TaxIdNumber, [InsuranceWorkCompensation] = @InsuranceWorkCompensation, [InsuranceDisability] = @InsuranceDisability, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes, [Url] = @Url, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [DOTInsuranceWorkCompensation] = @DOTInsuranceWorkCompensation, [DOTInsuranceGeneralLiability] = @DOTInsuranceGeneralLiability, [EmailAddress] = @EmailAddress, [EmailPassword] = @EmailPassword, [IDResponsibility] = @IDResponsibility
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobApplications", "IdJobWorkType", "dbo.JobWorkTypes");
            DropForeignKey("dbo.Companies", "IDResponsibility", "dbo.Responsibilities");
            DropForeignKey("dbo.Responsibilities", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Responsibilities", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyLicenses", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyLicenses", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyLicenses", "IdCompanyLicenseType", "dbo.CompanyLicenseTypes");
            DropForeignKey("dbo.CompanyLicenseTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyLicenseTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CompanyLicenses", "IdCompany", "dbo.Companies");
            DropIndex("dbo.JobApplications", new[] { "IdJobWorkType" });
            DropIndex("dbo.Responsibilities", new[] { "LastModifiedBy" });
            DropIndex("dbo.Responsibilities", new[] { "CreatedBy" });
            DropIndex("dbo.CompanyLicenseTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.CompanyLicenseTypes", new[] { "CreatedBy" });
            DropIndex("dbo.CompanyLicenseTypes", "IX_CompanyLicenseTypeName");
            DropIndex("dbo.CompanyLicenses", new[] { "LastModifiedBy" });
            DropIndex("dbo.CompanyLicenses", new[] { "CreatedBy" });
            DropIndex("dbo.CompanyLicenses", new[] { "IdCompany" });
            DropIndex("dbo.CompanyLicenses", new[] { "IdCompanyLicenseType" });
            DropIndex("dbo.Companies", new[] { "IDResponsibility" });
            DropColumn("dbo.JobApplications", "IdJobWorkType");
            DropColumn("dbo.Companies", "IDResponsibility");
            DropColumn("dbo.Companies", "EmailPassword");
            DropColumn("dbo.Companies", "EmailAddress");
            DropTable("dbo.Responsibilities");
            DropTable("dbo.CompanyLicenseTypes");
            DropTable("dbo.CompanyLicenses");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
