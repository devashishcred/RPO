namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableTrasmissionTypeDefaultCC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransmissionTypeDefaultCCs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTransmissionType = c.Int(),
                        IdEmployee = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdEmployee)
                .ForeignKey("dbo.TransmissionTypes", t => t.IdTransmissionType)
                .Index(t => t.IdTransmissionType)
                .Index(t => t.IdEmployee);
            
            AddColumn("dbo.Companies", "CTLicenseNumber", c => c.String());
            AddColumn("dbo.Companies", "CTExpirationDate", c => c.DateTime());
            AddColumn("dbo.TransmissionTypes", "IsSendEmail", c => c.Boolean(nullable: false));
            DropColumn("dbo.TransmissionTypes", "DefaultCC");
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
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [TrackingNumber], [TrackingExpiry], [IBMNumber], [SpecialInspectionAgencyNumber], [SpecialInspectionAgencyExpiry], [HICNumber], [HICExpiry], [CTLicenseNumber], [CTExpirationDate], [TaxIdNumber], [InsuranceWorkCompensation], [InsuranceDisability], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes], [Url], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @TrackingNumber, @TrackingExpiry, @IBMNumber, @SpecialInspectionAgencyNumber, @SpecialInspectionAgencyExpiry, @HICNumber, @HICExpiry, @CTLicenseNumber, @CTExpirationDate, @TaxIdNumber, @InsuranceWorkCompensation, @InsuranceDisability, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes, @Url, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [TrackingNumber] = @TrackingNumber, [TrackingExpiry] = @TrackingExpiry, [IBMNumber] = @IBMNumber, [SpecialInspectionAgencyNumber] = @SpecialInspectionAgencyNumber, [SpecialInspectionAgencyExpiry] = @SpecialInspectionAgencyExpiry, [HICNumber] = @HICNumber, [HICExpiry] = @HICExpiry, [CTLicenseNumber] = @CTLicenseNumber, [CTExpirationDate] = @CTExpirationDate, [TaxIdNumber] = @TaxIdNumber, [InsuranceWorkCompensation] = @InsuranceWorkCompensation, [InsuranceDisability] = @InsuranceDisability, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes, [Url] = @Url, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.TransmissionType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        IsSendEmail = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[TransmissionTypes]([Name], [IsSendEmail], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @IsSendEmail, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TransmissionTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TransmissionTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TransmissionType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        IsSendEmail = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[TransmissionTypes]
                      SET [Name] = @Name, [IsSendEmail] = @IsSendEmail, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransmissionTypes", "DefaultCC", c => c.String(maxLength: 1000));
            DropForeignKey("dbo.TransmissionTypeDefaultCCs", "IdTransmissionType", "dbo.TransmissionTypes");
            DropForeignKey("dbo.TransmissionTypeDefaultCCs", "IdEmployee", "dbo.Employees");
            DropIndex("dbo.TransmissionTypeDefaultCCs", new[] { "IdEmployee" });
            DropIndex("dbo.TransmissionTypeDefaultCCs", new[] { "IdTransmissionType" });
            DropColumn("dbo.TransmissionTypes", "IsSendEmail");
            DropColumn("dbo.Companies", "CTExpirationDate");
            DropColumn("dbo.Companies", "CTLicenseNumber");
            DropTable("dbo.TransmissionTypeDefaultCCs");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
