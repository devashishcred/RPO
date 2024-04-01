namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScopeReviewSection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProposalReviewSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProposalReview = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RfpProposalReviews", t => t.IdProposalReview)
                .Index(t => t.IdProposalReview);
            
            DropColumn("dbo.RfpProposalReviews", "Introduction");
            DropColumn("dbo.RfpProposalReviews", "Scope");
            DropColumn("dbo.RfpProposalReviews", "AdditionalService");
            CreateStoredProcedure(
                "dbo.ProposalReviewSection_Insert",
                p => new
                    {
                        IdProposalReview = p.Int(),
                        Type = p.Int(),
                        Content = p.String(),
                    },
                body:
                    @"INSERT [dbo].[ProposalReviewSections]([IdProposalReview], [Type], [Content])
                      VALUES (@IdProposalReview, @Type, @Content)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ProposalReviewSections]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ProposalReviewSections] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ProposalReviewSection_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdProposalReview = p.Int(),
                        Type = p.Int(),
                        Content = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[ProposalReviewSections]
                      SET [IdProposalReview] = @IdProposalReview, [Type] = @Type, [Content] = @Content
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ProposalReviewSection_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ProposalReviewSections]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.RfpProposalReview_Insert",
                p => new
                    {
                        Id = p.Int(),
                        Cost = p.Double(),
                    },
                body:
                    @"INSERT [dbo].[RfpProposalReviews]([Id], [Cost])
                      VALUES (@Id, @Cost)"
            );
            
            AlterStoredProcedure(
                "dbo.RfpProposalReview_Update",
                p => new
                    {
                        Id = p.Int(),
                        Cost = p.Double(),
                    },
                body:
                    @"UPDATE [dbo].[RfpProposalReviews]
                      SET [Cost] = @Cost
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ProposalReviewSection_Delete");
            DropStoredProcedure("dbo.ProposalReviewSection_Update");
            DropStoredProcedure("dbo.ProposalReviewSection_Insert");
            AddColumn("dbo.RfpProposalReviews", "AdditionalService", c => c.String());
            AddColumn("dbo.RfpProposalReviews", "Scope", c => c.String());
            AddColumn("dbo.RfpProposalReviews", "Introduction", c => c.String());
            DropForeignKey("dbo.ProposalReviewSections", "IdProposalReview", "dbo.RfpProposalReviews");
            DropIndex("dbo.ProposalReviewSections", new[] { "IdProposalReview" });
            DropTable("dbo.ProposalReviewSections");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
