namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnIsSignatureNewPageInRFP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rfps", "IsSignatureNewPage", c => c.Boolean());
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
                        IdRfpScopeReview = p.Int(),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Cost = p.Double(),
                        IsSignatureNewPage = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Rfps]([RfpNumber], [IdRfpStatus], [IdRfpAddress], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdContact], [Address1], [Address2], [Phone], [Email], [IdReferredByCompany], [IdReferredByContact], [LastModifiedDate], [IdLastModifiedBy], [StatusChangedDate], [CreatedDate], [IdCreatedBy], [GoNextStep], [LastUpdatedStep], [CompletedStep], [IdRfpScopeReview], [City], [IdState], [ZipCode], [Cost], [IsSignatureNewPage])
                      VALUES (@RfpNumber, @IdRfpStatus, @IdRfpAddress, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdContact, @Address1, @Address2, @Phone, @Email, @IdReferredByCompany, @IdReferredByContact, @LastModifiedDate, @IdLastModifiedBy, @StatusChangedDate, @CreatedDate, @IdCreatedBy, @GoNextStep, @LastUpdatedStep, @CompletedStep, @IdRfpScopeReview, @City, @IdState, @ZipCode, @Cost, @IsSignatureNewPage)
                      
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
                        IdRfpScopeReview = p.Int(),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Cost = p.Double(),
                        IsSignatureNewPage = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Rfps]
                      SET [RfpNumber] = @RfpNumber, [IdRfpStatus] = @IdRfpStatus, [IdRfpAddress] = @IdRfpAddress, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [Address1] = @Address1, [Address2] = @Address2, [Phone] = @Phone, [Email] = @Email, [IdReferredByCompany] = @IdReferredByCompany, [IdReferredByContact] = @IdReferredByContact, [LastModifiedDate] = @LastModifiedDate, [IdLastModifiedBy] = @IdLastModifiedBy, [StatusChangedDate] = @StatusChangedDate, [CreatedDate] = @CreatedDate, [IdCreatedBy] = @IdCreatedBy, [GoNextStep] = @GoNextStep, [LastUpdatedStep] = @LastUpdatedStep, [CompletedStep] = @CompletedStep, [IdRfpScopeReview] = @IdRfpScopeReview, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [Cost] = @Cost, [IsSignatureNewPage] = @IsSignatureNewPage
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rfps", "IsSignatureNewPage");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
