namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Correcting_Companies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "TrackingNumber", c => c.String(maxLength: 10));
            AddColumn("dbo.Companies", "InsuranceWorkCompensation", c => c.DateTime());
            AddColumn("dbo.Companies", "InsuranceDisability", c => c.DateTime());
            DropColumn("dbo.Companies", "TrakingNumber");
            DropColumn("dbo.Companies", "InsuranceWorkCompesention");
            DropColumn("dbo.Companies", "InsuranceDisbility");
            AlterStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        IdCompanyType = p.Int(),
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
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [IdCompanyType], [TrackingNumber], [TrackingExpiry], [IBMNumber], [HICNumber], [HICExpiry], [TaxIdNumber], [InsuranceWorkCompensation], [InsuranceDisability], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes])
                      VALUES (@Name, @IdCompanyType, @TrackingNumber, @TrackingExpiry, @IBMNumber, @HICNumber, @HICExpiry, @TaxIdNumber, @InsuranceWorkCompensation, @InsuranceDisability, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [IdCompanyType] = @IdCompanyType, [TrackingNumber] = @TrackingNumber, [TrackingExpiry] = @TrackingExpiry, [IBMNumber] = @IBMNumber, [HICNumber] = @HICNumber, [HICExpiry] = @HICExpiry, [TaxIdNumber] = @TaxIdNumber, [InsuranceWorkCompensation] = @InsuranceWorkCompensation, [InsuranceDisability] = @InsuranceDisability, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "InsuranceDisbility", c => c.DateTime());
            AddColumn("dbo.Companies", "InsuranceWorkCompesention", c => c.DateTime());
            AddColumn("dbo.Companies", "TrakingNumber", c => c.String(maxLength: 10));
            DropColumn("dbo.Companies", "InsuranceDisability");
            DropColumn("dbo.Companies", "InsuranceWorkCompensation");
            DropColumn("dbo.Companies", "TrackingNumber");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
