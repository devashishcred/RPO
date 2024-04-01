namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecompanycolumnsize : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Companies", "IX_CompanyName");
            AlterColumn("dbo.Companies", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Companies", "Name", unique: true, name: "IX_CompanyName");
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
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [TrackingNumber], [TrackingExpiry], [IBMNumber], [SpecialInspectionAgencyNumber], [SpecialInspectionAgencyExpiry], [HICNumber], [HICExpiry], [CTLicenseNumber], [CTExpirationDate], [TaxIdNumber], [InsuranceWorkCompensation], [InsuranceDisability], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes], [Url], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [DOTInsuranceWorkCompensation], [DOTInsuranceGeneralLiability])
                      VALUES (@Name, @TrackingNumber, @TrackingExpiry, @IBMNumber, @SpecialInspectionAgencyNumber, @SpecialInspectionAgencyExpiry, @HICNumber, @HICExpiry, @CTLicenseNumber, @CTExpirationDate, @TaxIdNumber, @InsuranceWorkCompensation, @InsuranceDisability, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes, @Url, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @DOTInsuranceWorkCompensation, @DOTInsuranceGeneralLiability)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [TrackingNumber] = @TrackingNumber, [TrackingExpiry] = @TrackingExpiry, [IBMNumber] = @IBMNumber, [SpecialInspectionAgencyNumber] = @SpecialInspectionAgencyNumber, [SpecialInspectionAgencyExpiry] = @SpecialInspectionAgencyExpiry, [HICNumber] = @HICNumber, [HICExpiry] = @HICExpiry, [CTLicenseNumber] = @CTLicenseNumber, [CTExpirationDate] = @CTExpirationDate, [TaxIdNumber] = @TaxIdNumber, [InsuranceWorkCompensation] = @InsuranceWorkCompensation, [InsuranceDisability] = @InsuranceDisability, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes, [Url] = @Url, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [DOTInsuranceWorkCompensation] = @DOTInsuranceWorkCompensation, [DOTInsuranceGeneralLiability] = @DOTInsuranceGeneralLiability
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Companies", "IX_CompanyName");
            AlterColumn("dbo.Companies", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Companies", "Name", unique: true, name: "IX_CompanyName");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
