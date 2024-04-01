namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProposalReview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RfpProposalReviews",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Introduction = c.String(),
                        Scope = c.String(),
                        Cost = c.Double(nullable: false),
                        AdditionalService = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rfps", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Milestones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Double(nullable: false),
                        IdRfp = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RfpProposalReviews", t => t.IdRfp)
                .Index(t => t.IdRfp);
            
            CreateStoredProcedure(
                "dbo.RfpProposalReview_Insert",
                p => new
                    {
                        Id = p.Int(),
                        Introduction = p.String(),
                        Scope = p.String(),
                        Cost = p.Double(),
                        AdditionalService = p.String(),
                    },
                body:
                    @"INSERT [dbo].[RfpProposalReviews]([Id], [Introduction], [Scope], [Cost], [AdditionalService])
                      VALUES (@Id, @Introduction, @Scope, @Cost, @AdditionalService)"
            );
            
            CreateStoredProcedure(
                "dbo.RfpProposalReview_Update",
                p => new
                    {
                        Id = p.Int(),
                        Introduction = p.String(),
                        Scope = p.String(),
                        Cost = p.Double(),
                        AdditionalService = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[RfpProposalReviews]
                      SET [Introduction] = @Introduction, [Scope] = @Scope, [Cost] = @Cost, [AdditionalService] = @AdditionalService
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RfpProposalReview_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[RfpProposalReviews]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Milestone_Insert",
                p => new
                    {
                        Name = p.String(),
                        Value = p.Double(),
                        IdRfp = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Milestones]([Name], [Value], [IdRfp])
                      VALUES (@Name, @Value, @IdRfp)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Milestones]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Milestones] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Milestone_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Value = p.Double(),
                        IdRfp = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Milestones]
                      SET [Name] = @Name, [Value] = @Value, [IdRfp] = @IdRfp
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Milestone_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Milestones]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Milestone_Delete");
            DropStoredProcedure("dbo.Milestone_Update");
            DropStoredProcedure("dbo.Milestone_Insert");
            DropStoredProcedure("dbo.RfpProposalReview_Delete");
            DropStoredProcedure("dbo.RfpProposalReview_Update");
            DropStoredProcedure("dbo.RfpProposalReview_Insert");
            DropForeignKey("dbo.RfpProposalReviews", "Id", "dbo.Rfps");
            DropForeignKey("dbo.Milestones", "IdRfp", "dbo.RfpProposalReviews");
            DropIndex("dbo.Milestones", new[] { "IdRfp" });
            DropIndex("dbo.RfpProposalReviews", new[] { "Id" });
            DropTable("dbo.Milestones");
            DropTable("dbo.RfpProposalReviews");
        }
    }
}
