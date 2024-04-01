namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScopeReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rfps", "ScopeReview_Description", c => c.String());
            AddColumn("dbo.Rfps", "ScopeReview_GeneralNotes", c => c.String());
            AddColumn("dbo.Rfps", "ScopeReview_ScopeReviewRecientsType", c => c.Int(nullable: false));
            AlterStoredProcedure(
                "dbo.Rfp_Insert",
                p => new
                    {
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
                        ScopeReview_Description = p.String(),
                        ScopeReview_GeneralNotes = p.String(),
                        ScopeReview_ScopeReviewRecientsType = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Rfps]([IdRfpAddress], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdContact], [Address1], [Address2], [Phone], [Email], [IdReferredByCompany], [IdReferredByContact], [ScopeReview_Description], [ScopeReview_GeneralNotes], [ScopeReview_ScopeReviewRecientsType])
                      VALUES (@IdRfpAddress, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdContact, @Address1, @Address2, @Phone, @Email, @IdReferredByCompany, @IdReferredByContact, @ScopeReview_Description, @ScopeReview_GeneralNotes, @ScopeReview_ScopeReviewRecientsType)
                      
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
                        ScopeReview_Description = p.String(),
                        ScopeReview_GeneralNotes = p.String(),
                        ScopeReview_ScopeReviewRecientsType = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Rfps]
                      SET [IdRfpAddress] = @IdRfpAddress, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [Address1] = @Address1, [Address2] = @Address2, [Phone] = @Phone, [Email] = @Email, [IdReferredByCompany] = @IdReferredByCompany, [IdReferredByContact] = @IdReferredByContact, [ScopeReview_Description] = @ScopeReview_Description, [ScopeReview_GeneralNotes] = @ScopeReview_GeneralNotes, [ScopeReview_ScopeReviewRecientsType] = @ScopeReview_ScopeReviewRecientsType
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rfps", "ScopeReview_ScopeReviewRecientsType");
            DropColumn("dbo.Rfps", "ScopeReview_GeneralNotes");
            DropColumn("dbo.Rfps", "ScopeReview_Description");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
