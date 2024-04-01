namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColumnsInRFP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rfps", "IdLastModifiedBy", c => c.Int());
            AddColumn("dbo.Rfps", "StatusChangedDate", c => c.DateTime());
            AddColumn("dbo.Rfps", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Rfps", "IdCreatedBy", c => c.Int());
            CreateIndex("dbo.Rfps", "IdLastModifiedBy");
            CreateIndex("dbo.Rfps", "IdCreatedBy");
            AddForeignKey("dbo.Rfps", "IdCreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Rfps", "IdLastModifiedBy", "dbo.Employees", "Id");
            AlterStoredProcedure(
                "dbo.Rfp_Insert",
                p => new
                    {
                        RfpNumber = p.String(),
                        IdRfpStatus = p.Int(),
                        IdRfpAddress = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        Phone = p.String(maxLength: 14),
                        Email = p.String(maxLength: 250),
                        IdReferredByCompany = p.Int(),
                        IdReferredByContact = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        IdLastModifiedBy = p.Int(),
                        StatusChangedDate = p.DateTime(),
                        CreatedDate = p.DateTime(),
                        IdCreatedBy = p.Int(),
                        GoNextStep = p.Int(),
                        LastUpdatedStep = p.Int(),
                        CompletedStep = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Rfps]([RfpNumber], [IdRfpStatus], [IdRfpAddress], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdContact], [Address1], [Address2], [Phone], [Email], [IdReferredByCompany], [IdReferredByContact], [LastModifiedDate], [IdLastModifiedBy], [StatusChangedDate], [CreatedDate], [IdCreatedBy], [GoNextStep], [LastUpdatedStep], [CompletedStep])
                      VALUES (@RfpNumber, @IdRfpStatus, @IdRfpAddress, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdContact, @Address1, @Address2, @Phone, @Email, @IdReferredByCompany, @IdReferredByContact, @LastModifiedDate, @IdLastModifiedBy, @StatusChangedDate, @CreatedDate, @IdCreatedBy, @GoNextStep, @LastUpdatedStep, @CompletedStep)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Rfps]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Rfps] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Rfp_Update",
                p => new
                    {
                        Id = p.Int(),
                        RfpNumber = p.String(),
                        IdRfpStatus = p.Int(),
                        IdRfpAddress = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        Phone = p.String(maxLength: 14),
                        Email = p.String(maxLength: 250),
                        IdReferredByCompany = p.Int(),
                        IdReferredByContact = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        IdLastModifiedBy = p.Int(),
                        StatusChangedDate = p.DateTime(),
                        CreatedDate = p.DateTime(),
                        IdCreatedBy = p.Int(),
                        GoNextStep = p.Int(),
                        LastUpdatedStep = p.Int(),
                        CompletedStep = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Rfps]
                      SET [RfpNumber] = @RfpNumber, [IdRfpStatus] = @IdRfpStatus, [IdRfpAddress] = @IdRfpAddress, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [Address1] = @Address1, [Address2] = @Address2, [Phone] = @Phone, [Email] = @Email, [IdReferredByCompany] = @IdReferredByCompany, [IdReferredByContact] = @IdReferredByContact, [LastModifiedDate] = @LastModifiedDate, [IdLastModifiedBy] = @IdLastModifiedBy, [StatusChangedDate] = @StatusChangedDate, [CreatedDate] = @CreatedDate, [IdCreatedBy] = @IdCreatedBy, [GoNextStep] = @GoNextStep, [LastUpdatedStep] = @LastUpdatedStep, [CompletedStep] = @CompletedStep
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rfps", "IdLastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Rfps", "IdCreatedBy", "dbo.Employees");
            DropIndex("dbo.Rfps", new[] { "IdCreatedBy" });
            DropIndex("dbo.Rfps", new[] { "IdLastModifiedBy" });
            DropColumn("dbo.Rfps", "IdCreatedBy");
            DropColumn("dbo.Rfps", "CreatedDate");
            DropColumn("dbo.Rfps", "StatusChangedDate");
            DropColumn("dbo.Rfps", "IdLastModifiedBy");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
