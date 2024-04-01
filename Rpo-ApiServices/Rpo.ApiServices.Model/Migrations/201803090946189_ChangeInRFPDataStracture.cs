namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInRFPDataStracture : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectDetails", "IdJobType", "dbo.JobTypes");
            DropForeignKey("dbo.WorkTypeNotes", "IdProjectDetail", "dbo.ProjectDetails");
            DropForeignKey("dbo.Milestones", "IdRfp", "dbo.RfpProposalReviews");
            DropForeignKey("dbo.ProposalReviewSections", "IdProposalReview", "dbo.RfpProposalReviews");
            DropForeignKey("dbo.RfpScopeReviewContacts", "RfpScopeReview_Id", "dbo.RfpScopeReviews");
            DropForeignKey("dbo.RfpScopeReviewContacts", "Contact_Id", "dbo.Contacts");
            DropForeignKey("dbo.RfpProposalReviews", "Id", "dbo.Rfps");
            DropForeignKey("dbo.RfpScopeReviews", "Id", "dbo.Rfps");
            DropIndex("dbo.ProjectDetails", new[] { "IdJobType" });
            DropIndex("dbo.WorkTypeNotes", new[] { "IdProjectDetail" });
            DropIndex("dbo.RfpProposalReviews", new[] { "Id" });
            DropIndex("dbo.ProposalReviewSections", new[] { "IdProposalReview" });
            DropIndex("dbo.RfpScopeReviews", new[] { "Id" });
            DropIndex("dbo.RfpScopeReviewContacts", new[] { "RfpScopeReview_Id" });
            DropIndex("dbo.RfpScopeReviewContacts", new[] { "Contact_Id" });
            DropPrimaryKey("dbo.RfpProposalReviews");
            DropPrimaryKey("dbo.RfpScopeReviews");
            CreateTable(
                "dbo.RfpFeeSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProjectDetail = c.Int(nullable: false),
                        IdRfpWorkTypeCategory = c.Int(nullable: false),
                        IdRfpWorkType = c.Int(nullable: false),
                        Cost = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        TotalCost = c.Double(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RfpWorkTypes", t => t.IdRfpWorkType)
                .ForeignKey("dbo.RfpWorkTypeCategories", t => t.IdRfpWorkTypeCategory)
                .ForeignKey("dbo.ProjectDetails", t => t.IdProjectDetail)
                .Index(t => t.IdProjectDetail)
                .Index(t => t.IdRfpWorkTypeCategory)
                .Index(t => t.IdRfpWorkType);
            
            AddColumn("dbo.Rfps", "IdRfpScopeReview", c => c.Int());
            AddColumn("dbo.Rfps", "City", c => c.String());
            AddColumn("dbo.Rfps", "IdState", c => c.Int());
            AddColumn("dbo.Rfps", "ZipCode", c => c.String(maxLength: 10));
            AddColumn("dbo.Rfps", "Cost", c => c.Double(nullable: false));
            AddColumn("dbo.ProjectDetails", "IdRfpJobType", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectDetails", "IdRfpSubJobTypeCategory", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectDetails", "IdRfpSubJobType", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectDetails", "DisplayOrder", c => c.Int(nullable: false));
            AddColumn("dbo.RfpProposalReviews", "IdRfp", c => c.Int(nullable: false));
            AddColumn("dbo.RfpProposalReviews", "Content", c => c.String());
            AddColumn("dbo.RfpProposalReviews", "IdVerbiage", c => c.Int(nullable: false));
            AddColumn("dbo.RfpProposalReviews", "DisplayOrder", c => c.Int(nullable: false));
            AddColumn("dbo.RfpScopeReviews", "ContactsCc", c => c.String());
            AlterColumn("dbo.RfpProposalReviews", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.RfpScopeReviews", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.RfpProposalReviews", "Id");
            AddPrimaryKey("dbo.RfpScopeReviews", "Id");
            CreateIndex("dbo.Rfps", "IdRfpScopeReview");
            CreateIndex("dbo.Rfps", "IdState");
            CreateIndex("dbo.ProjectDetails", "IdRfpJobType");
            CreateIndex("dbo.ProjectDetails", "IdRfpSubJobTypeCategory");
            CreateIndex("dbo.ProjectDetails", "IdRfpSubJobType");
            CreateIndex("dbo.RfpProposalReviews", "IdRfp");
            CreateIndex("dbo.RfpProposalReviews", "IdVerbiage");
            AddForeignKey("dbo.Milestones", "IdRfp", "dbo.Rfps", "Id");
            AddForeignKey("dbo.ProjectDetails", "IdRfpJobType", "dbo.RfpJobTypes", "Id");
            AddForeignKey("dbo.ProjectDetails", "IdRfpSubJobType", "dbo.RfpSubJobTypes", "Id");
            AddForeignKey("dbo.ProjectDetails", "IdRfpSubJobTypeCategory", "dbo.RfpSubJobTypeCategories", "Id");
            AddForeignKey("dbo.RfpProposalReviews", "IdVerbiage", "dbo.Verbiages", "Id");
            AddForeignKey("dbo.Rfps", "IdState", "dbo.States", "Id");
            AddForeignKey("dbo.RfpProposalReviews", "IdRfp", "dbo.Rfps", "Id");
            AddForeignKey("dbo.Rfps", "IdRfpScopeReview", "dbo.RfpScopeReviews", "Id");
            DropColumn("dbo.ProjectDetails", "IdJobType");
            DropColumn("dbo.RfpProposalReviews", "Cost");
            DropTable("dbo.ProposalReviewSections");
            DropTable("dbo.RfpScopeReviewContacts");
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
                    },
                body:
                    @"INSERT [dbo].[Rfps]([RfpNumber], [IdRfpStatus], [IdRfpAddress], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdContact], [Address1], [Address2], [Phone], [Email], [IdReferredByCompany], [IdReferredByContact], [LastModifiedDate], [IdLastModifiedBy], [StatusChangedDate], [CreatedDate], [IdCreatedBy], [GoNextStep], [LastUpdatedStep], [CompletedStep], [IdRfpScopeReview], [City], [IdState], [ZipCode], [Cost])
                      VALUES (@RfpNumber, @IdRfpStatus, @IdRfpAddress, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdContact, @Address1, @Address2, @Phone, @Email, @IdReferredByCompany, @IdReferredByContact, @LastModifiedDate, @IdLastModifiedBy, @StatusChangedDate, @CreatedDate, @IdCreatedBy, @GoNextStep, @LastUpdatedStep, @CompletedStep, @IdRfpScopeReview, @City, @IdState, @ZipCode, @Cost)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Rfps]
                      SET [RfpNumber] = @RfpNumber, [IdRfpStatus] = @IdRfpStatus, [IdRfpAddress] = @IdRfpAddress, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [Address1] = @Address1, [Address2] = @Address2, [Phone] = @Phone, [Email] = @Email, [IdReferredByCompany] = @IdReferredByCompany, [IdReferredByContact] = @IdReferredByContact, [LastModifiedDate] = @LastModifiedDate, [IdLastModifiedBy] = @IdLastModifiedBy, [StatusChangedDate] = @StatusChangedDate, [CreatedDate] = @CreatedDate, [IdCreatedBy] = @IdCreatedBy, [GoNextStep] = @GoNextStep, [LastUpdatedStep] = @LastUpdatedStep, [CompletedStep] = @CompletedStep, [IdRfpScopeReview] = @IdRfpScopeReview, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [Cost] = @Cost
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ProjectDetail_Insert",
                p => new
                    {
                        WorkDescription = p.String(),
                        ArePlansNotPrepared = p.Boolean(),
                        ArePlansCompleted = p.Boolean(),
                        IsApproved = p.Boolean(),
                        IsDisaproved = p.Boolean(),
                        IsPermitted = p.Boolean(),
                        IdRfp = p.Int(),
                        IdRfpJobType = p.Int(),
                        IdRfpSubJobTypeCategory = p.Int(),
                        IdRfpSubJobType = p.Int(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ProjectDetails]([WorkDescription], [ArePlansNotPrepared], [ArePlansCompleted], [IsApproved], [IsDisaproved], [IsPermitted], [IdRfp], [IdRfpJobType], [IdRfpSubJobTypeCategory], [IdRfpSubJobType], [DisplayOrder])
                      VALUES (@WorkDescription, @ArePlansNotPrepared, @ArePlansCompleted, @IsApproved, @IsDisaproved, @IsPermitted, @IdRfp, @IdRfpJobType, @IdRfpSubJobTypeCategory, @IdRfpSubJobType, @DisplayOrder)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ProjectDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ProjectDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ProjectDetail_Update",
                p => new
                    {
                        Id = p.Int(),
                        WorkDescription = p.String(),
                        ArePlansNotPrepared = p.Boolean(),
                        ArePlansCompleted = p.Boolean(),
                        IsApproved = p.Boolean(),
                        IsDisaproved = p.Boolean(),
                        IsPermitted = p.Boolean(),
                        IdRfp = p.Int(),
                        IdRfpJobType = p.Int(),
                        IdRfpSubJobTypeCategory = p.Int(),
                        IdRfpSubJobType = p.Int(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ProjectDetails]
                      SET [WorkDescription] = @WorkDescription, [ArePlansNotPrepared] = @ArePlansNotPrepared, [ArePlansCompleted] = @ArePlansCompleted, [IsApproved] = @IsApproved, [IsDisaproved] = @IsDisaproved, [IsPermitted] = @IsPermitted, [IdRfp] = @IdRfp, [IdRfpJobType] = @IdRfpJobType, [IdRfpSubJobTypeCategory] = @IdRfpSubJobTypeCategory, [IdRfpSubJobType] = @IdRfpSubJobType, [DisplayOrder] = @DisplayOrder
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.RfpProposalReview_Insert",
                p => new
                    {
                        IdRfp = p.Int(),
                        Content = p.String(),
                        IdVerbiage = p.Int(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[RfpProposalReviews]([IdRfp], [Content], [IdVerbiage], [DisplayOrder])
                      VALUES (@IdRfp, @Content, @IdVerbiage, @DisplayOrder)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[RfpProposalReviews]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RfpProposalReviews] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.RfpProposalReview_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdRfp = p.Int(),
                        Content = p.String(),
                        IdVerbiage = p.Int(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[RfpProposalReviews]
                      SET [IdRfp] = @IdRfp, [Content] = @Content, [IdVerbiage] = @IdVerbiage, [DisplayOrder] = @DisplayOrder
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.RfpScopeReview_Insert",
                p => new
                    {
                        Description = p.String(),
                        GeneralNotes = p.String(),
                        ContactsCc = p.String(),
                    },
                body:
                    @"INSERT [dbo].[RfpScopeReviews]([Description], [GeneralNotes], [ContactsCc])
                      VALUES (@Description, @GeneralNotes, @ContactsCc)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[RfpScopeReviews]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RfpScopeReviews] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.RfpScopeReview_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(),
                        GeneralNotes = p.String(),
                        ContactsCc = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[RfpScopeReviews]
                      SET [Description] = @Description, [GeneralNotes] = @GeneralNotes, [ContactsCc] = @ContactsCc
                      WHERE ([Id] = @Id)"
            );
            
            DropStoredProcedure("dbo.ProposalReviewSection_Insert");
            DropStoredProcedure("dbo.ProposalReviewSection_Update");
            DropStoredProcedure("dbo.ProposalReviewSection_Delete");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RfpScopeReviewContacts",
                c => new
                    {
                        RfpScopeReview_Id = c.Int(nullable: false),
                        Contact_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RfpScopeReview_Id, t.Contact_Id });
            
            CreateTable(
                "dbo.ProposalReviewSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProposalReview = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RfpProposalReviews", "Cost", c => c.Double(nullable: false));
            AddColumn("dbo.ProjectDetails", "IdJobType", c => c.Int(nullable: false));
            DropForeignKey("dbo.Rfps", "IdRfpScopeReview", "dbo.RfpScopeReviews");
            DropForeignKey("dbo.RfpProposalReviews", "IdRfp", "dbo.Rfps");
            DropForeignKey("dbo.Rfps", "IdState", "dbo.States");
            DropForeignKey("dbo.RfpProposalReviews", "IdVerbiage", "dbo.Verbiages");
            DropForeignKey("dbo.ProjectDetails", "IdRfpSubJobTypeCategory", "dbo.RfpSubJobTypeCategories");
            DropForeignKey("dbo.ProjectDetails", "IdRfpSubJobType", "dbo.RfpSubJobTypes");
            DropForeignKey("dbo.ProjectDetails", "IdRfpJobType", "dbo.RfpJobTypes");
            DropForeignKey("dbo.RfpFeeSchedules", "IdProjectDetail", "dbo.ProjectDetails");
            DropForeignKey("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory", "dbo.RfpWorkTypeCategories");
            DropForeignKey("dbo.RfpFeeSchedules", "IdRfpWorkType", "dbo.RfpWorkTypes");
            DropForeignKey("dbo.Milestones", "IdRfp", "dbo.Rfps");
            DropIndex("dbo.RfpProposalReviews", new[] { "IdVerbiage" });
            DropIndex("dbo.RfpProposalReviews", new[] { "IdRfp" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkType" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkTypeCategory" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdProjectDetail" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpSubJobType" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpSubJobTypeCategory" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpJobType" });
            DropIndex("dbo.Rfps", new[] { "IdState" });
            DropIndex("dbo.Rfps", new[] { "IdRfpScopeReview" });
            DropPrimaryKey("dbo.RfpScopeReviews");
            DropPrimaryKey("dbo.RfpProposalReviews");
            AlterColumn("dbo.RfpScopeReviews", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpProposalReviews", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.RfpScopeReviews", "ContactsCc");
            DropColumn("dbo.RfpProposalReviews", "DisplayOrder");
            DropColumn("dbo.RfpProposalReviews", "IdVerbiage");
            DropColumn("dbo.RfpProposalReviews", "Content");
            DropColumn("dbo.RfpProposalReviews", "IdRfp");
            DropColumn("dbo.ProjectDetails", "DisplayOrder");
            DropColumn("dbo.ProjectDetails", "IdRfpSubJobType");
            DropColumn("dbo.ProjectDetails", "IdRfpSubJobTypeCategory");
            DropColumn("dbo.ProjectDetails", "IdRfpJobType");
            DropColumn("dbo.Rfps", "Cost");
            DropColumn("dbo.Rfps", "ZipCode");
            DropColumn("dbo.Rfps", "IdState");
            DropColumn("dbo.Rfps", "City");
            DropColumn("dbo.Rfps", "IdRfpScopeReview");
            DropTable("dbo.RfpFeeSchedules");
            AddPrimaryKey("dbo.RfpScopeReviews", "Id");
            AddPrimaryKey("dbo.RfpProposalReviews", "Id");
            CreateIndex("dbo.RfpScopeReviewContacts", "Contact_Id");
            CreateIndex("dbo.RfpScopeReviewContacts", "RfpScopeReview_Id");
            CreateIndex("dbo.RfpScopeReviews", "Id");
            CreateIndex("dbo.ProposalReviewSections", "IdProposalReview");
            CreateIndex("dbo.RfpProposalReviews", "Id");
            CreateIndex("dbo.WorkTypeNotes", "IdProjectDetail");
            CreateIndex("dbo.ProjectDetails", "IdJobType");
            AddForeignKey("dbo.RfpScopeReviews", "Id", "dbo.Rfps", "Id");
            AddForeignKey("dbo.RfpProposalReviews", "Id", "dbo.Rfps", "Id");
            AddForeignKey("dbo.RfpScopeReviewContacts", "Contact_Id", "dbo.Contacts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RfpScopeReviewContacts", "RfpScopeReview_Id", "dbo.RfpScopeReviews", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProposalReviewSections", "IdProposalReview", "dbo.RfpProposalReviews", "Id");
            AddForeignKey("dbo.Milestones", "IdRfp", "dbo.RfpProposalReviews", "Id");
            AddForeignKey("dbo.WorkTypeNotes", "IdProjectDetail", "dbo.ProjectDetails", "Id");
            AddForeignKey("dbo.ProjectDetails", "IdJobType", "dbo.JobTypes", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
