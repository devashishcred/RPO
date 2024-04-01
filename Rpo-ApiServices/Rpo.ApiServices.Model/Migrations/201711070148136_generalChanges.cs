namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class generalChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RfpScopeReviews",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(),
                        GeneralNotes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rfps", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.JobTypeWorkTypes",
                c => new
                    {
                        JobType_Id = c.Int(nullable: false),
                        WorkType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.JobType_Id, t.WorkType_Id })
                .ForeignKey("dbo.JobTypes", t => t.JobType_Id, cascadeDelete: true)
                .ForeignKey("dbo.WorkTypes", t => t.WorkType_Id, cascadeDelete: true)
                .Index(t => t.JobType_Id)
                .Index(t => t.WorkType_Id);
            
            CreateTable(
                "dbo.RfpScopeReviewContacts",
                c => new
                    {
                        RfpScopeReview_Id = c.Int(nullable: false),
                        Contact_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RfpScopeReview_Id, t.Contact_Id })
                .ForeignKey("dbo.RfpScopeReviews", t => t.RfpScopeReview_Id, cascadeDelete: true)
                .ForeignKey("dbo.Contacts", t => t.Contact_Id, cascadeDelete: true)
                .Index(t => t.RfpScopeReview_Id)
                .Index(t => t.Contact_Id);
            
            AddColumn("dbo.JobTypes", "Number", c => c.String(maxLength: 5));
            AddColumn("dbo.JobTypes", "IdParent", c => c.Int());
            AddColumn("dbo.WorkTypes", "Number", c => c.String(maxLength: 5));
            AddColumn("dbo.Rfps", "RfpNumber", c => c.String());
            AddColumn("dbo.Rfps", "Status", c => c.Int(nullable: false));
            CreateIndex("dbo.JobTypes", "IdParent");
            AddForeignKey("dbo.JobTypes", "IdParent", "dbo.JobTypes", "Id");
            DropColumn("dbo.Rfps", "ScopeReview_Description");
            DropColumn("dbo.Rfps", "ScopeReview_GeneralNotes");
            DropColumn("dbo.Rfps", "ScopeReview_ScopeReviewRecientsType");
            CreateStoredProcedure(
                "dbo.RfpScopeReview_Insert",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(),
                        GeneralNotes = p.String(),
                    },
                body:
                    @"INSERT [dbo].[RfpScopeReviews]([Id], [Description], [GeneralNotes])
                      VALUES (@Id, @Description, @GeneralNotes)"
            );
            
            CreateStoredProcedure(
                "dbo.RfpScopeReview_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(),
                        GeneralNotes = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[RfpScopeReviews]
                      SET [Description] = @Description, [GeneralNotes] = @GeneralNotes
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RfpScopeReview_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[RfpScopeReviews]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.JobType_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 100),
                        Number = p.String(maxLength: 5),
                        IdParent = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[JobTypes]([Description], [Number], [IdParent])
                      VALUES (@Description, @Number, @IdParent)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.JobType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 100),
                        Number = p.String(maxLength: 5),
                        IdParent = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[JobTypes]
                      SET [Description] = @Description, [Number] = @Number, [IdParent] = @IdParent
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.WorkType_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        Number = p.String(maxLength: 5),
                    },
                body:
                    @"INSERT [dbo].[WorkTypes]([Description], [Number])
                      VALUES (@Description, @Number)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[WorkTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[WorkTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.WorkType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        Number = p.String(maxLength: 5),
                    },
                body:
                    @"UPDATE [dbo].[WorkTypes]
                      SET [Description] = @Description, [Number] = @Number
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Rfp_Insert",
                p => new
                    {
                        RfpNumber = p.String(),
                        Status = p.Int(),
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
                    },
                body:
                    @"INSERT [dbo].[Rfps]([RfpNumber], [Status], [IdRfpAddress], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdContact], [Address1], [Address2], [Phone], [Email], [IdReferredByCompany], [IdReferredByContact])
                      VALUES (@RfpNumber, @Status, @IdRfpAddress, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdContact, @Address1, @Address2, @Phone, @Email, @IdReferredByCompany, @IdReferredByContact)
                      
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
                        Status = p.Int(),
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
                    },
                body:
                    @"UPDATE [dbo].[Rfps]
                      SET [RfpNumber] = @RfpNumber, [Status] = @Status, [IdRfpAddress] = @IdRfpAddress, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [Address1] = @Address1, [Address2] = @Address2, [Phone] = @Phone, [Email] = @Email, [IdReferredByCompany] = @IdReferredByCompany, [IdReferredByContact] = @IdReferredByContact
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.RfpScopeReview_Delete");
            DropStoredProcedure("dbo.RfpScopeReview_Update");
            DropStoredProcedure("dbo.RfpScopeReview_Insert");
            AddColumn("dbo.Rfps", "ScopeReview_ScopeReviewRecientsType", c => c.Int(nullable: false));
            AddColumn("dbo.Rfps", "ScopeReview_GeneralNotes", c => c.String());
            AddColumn("dbo.Rfps", "ScopeReview_Description", c => c.String());
            DropForeignKey("dbo.RfpScopeReviews", "Id", "dbo.Rfps");
            DropForeignKey("dbo.RfpScopeReviewContacts", "Contact_Id", "dbo.Contacts");
            DropForeignKey("dbo.RfpScopeReviewContacts", "RfpScopeReview_Id", "dbo.RfpScopeReviews");
            DropForeignKey("dbo.JobTypeWorkTypes", "WorkType_Id", "dbo.WorkTypes");
            DropForeignKey("dbo.JobTypeWorkTypes", "JobType_Id", "dbo.JobTypes");
            DropForeignKey("dbo.JobTypes", "IdParent", "dbo.JobTypes");
            DropIndex("dbo.RfpScopeReviewContacts", new[] { "Contact_Id" });
            DropIndex("dbo.RfpScopeReviewContacts", new[] { "RfpScopeReview_Id" });
            DropIndex("dbo.JobTypeWorkTypes", new[] { "WorkType_Id" });
            DropIndex("dbo.JobTypeWorkTypes", new[] { "JobType_Id" });
            DropIndex("dbo.RfpScopeReviews", new[] { "Id" });
            DropIndex("dbo.JobTypes", new[] { "IdParent" });
            DropColumn("dbo.Rfps", "Status");
            DropColumn("dbo.Rfps", "RfpNumber");
            DropColumn("dbo.WorkTypes", "Number");
            DropColumn("dbo.JobTypes", "IdParent");
            DropColumn("dbo.JobTypes", "Number");
            DropTable("dbo.RfpScopeReviewContacts");
            DropTable("dbo.JobTypeWorkTypes");
            DropTable("dbo.RfpScopeReviews");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
