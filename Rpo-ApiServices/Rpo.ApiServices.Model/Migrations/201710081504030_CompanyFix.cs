namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CompanyFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Companies", "IdTaxidType", "dbo.TaxIdTypes");
            DropIndex("dbo.Companies", new[] { "IdTaxidType" });
            AddColumn("dbo.Companies", "IdCompanyType", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "TrackingExpiry", c => c.DateTime());
            AddColumn("dbo.Companies", "IBMNumber", c => c.String(maxLength: 10));
            AddColumn("dbo.Companies", "HICExpiry", c => c.DateTime());
            AlterColumn("dbo.Companies", "TrakingNumber", c => c.String(maxLength: 10));
            CreateIndex("dbo.Companies", "IdCompanyType");
            AddForeignKey("dbo.Companies", "IdCompanyType", "dbo.CompanyTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Companies", "IdTaxidType");
            DropColumn("dbo.Companies", "CorporateTestingLabNamber");
            DropColumn("dbo.Companies", "SpecialInpector");
            AlterStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        IdCompanyType = p.Int(),
                        TrakingNumber = p.String(maxLength: 10),
                        TrackingExpiry = p.DateTime(),
                        IBMNumber = p.String(maxLength: 10),
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 10),
                        InsuranceWorkCompesention = p.DateTime(),
                        InsuranceDisbility = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [IdCompanyType], [TrakingNumber], [TrackingExpiry], [IBMNumber], [HICNumber], [HICExpiry], [TaxIdNumber], [InsuranceWorkCompesention], [InsuranceDisbility], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes])
                      VALUES (@Name, @IdCompanyType, @TrakingNumber, @TrackingExpiry, @IBMNumber, @HICNumber, @HICExpiry, @TaxIdNumber, @InsuranceWorkCompesention, @InsuranceDisbility, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes)
                      
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
                        IdCompanyType = p.Int(),
                        TrakingNumber = p.String(maxLength: 10),
                        TrackingExpiry = p.DateTime(),
                        IBMNumber = p.String(maxLength: 10),
                        HICNumber = p.String(maxLength: 10),
                        HICExpiry = p.DateTime(),
                        TaxIdNumber = p.String(maxLength: 10),
                        InsuranceWorkCompesention = p.DateTime(),
                        InsuranceDisbility = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [IdCompanyType] = @IdCompanyType, [TrakingNumber] = @TrakingNumber, [TrackingExpiry] = @TrackingExpiry, [IBMNumber] = @IBMNumber, [HICNumber] = @HICNumber, [HICExpiry] = @HICExpiry, [TaxIdNumber] = @TaxIdNumber, [InsuranceWorkCompesention] = @InsuranceWorkCompesention, [InsuranceDisbility] = @InsuranceDisbility, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "SpecialInpector", c => c.String(maxLength: 10));
            AddColumn("dbo.Companies", "CorporateTestingLabNamber", c => c.String(maxLength: 10));
            AddColumn("dbo.Companies", "IdTaxidType", c => c.Int(nullable: false));
            DropForeignKey("dbo.Companies", "IdCompanyType", "dbo.CompanyTypes");
            DropIndex("dbo.Companies", new[] { "IdCompanyType" });
            AlterColumn("dbo.Companies", "TrakingNumber", c => c.String());
            DropColumn("dbo.Companies", "HICExpiry");
            DropColumn("dbo.Companies", "IBMNumber");
            DropColumn("dbo.Companies", "TrackingExpiry");
            DropColumn("dbo.Companies", "IdCompanyType");
            CreateIndex("dbo.Companies", "IdTaxidType");
            AddForeignKey("dbo.Companies", "IdTaxidType", "dbo.TaxIdTypes", "Id", cascadeDelete: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
