namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInFeeScheduleDataStrcture : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectDetails", "IdRfpWorkType", "dbo.RfpJobTypes");
            DropForeignKey("dbo.ProjectDetails", "IdRfpWorkTypeCategory", "dbo.RfpJobTypes");
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpWorkTypeCategory" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpWorkType" });
            CreateTable(
                "dbo.RfpFeeSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProjectDetail = c.Int(nullable: false),
                        IdRfpWorkTypeCategory = c.Int(nullable: false),
                        IdRfpWorkType = c.Int(nullable: false),
                        Cost = c.Double(),
                        Quantity = c.Int(),
                        TotalCost = c.Double(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RfpJobTypes", t => t.IdRfpWorkType)
                .ForeignKey("dbo.RfpJobTypes", t => t.IdRfpWorkTypeCategory)
                .ForeignKey("dbo.ProjectDetails", t => t.IdProjectDetail)
                .Index(t => t.IdProjectDetail)
                .Index(t => t.IdRfpWorkTypeCategory)
                .Index(t => t.IdRfpWorkType);
            
            DropColumn("dbo.ProjectDetails", "IdRfpWorkTypeCategory");
            DropColumn("dbo.ProjectDetails", "IdRfpWorkType");
            DropColumn("dbo.ProjectDetails", "Cost");
            DropColumn("dbo.ProjectDetails", "Quantity");
            DropColumn("dbo.ProjectDetails", "TotalCost");
            DropColumn("dbo.ProjectDetails", "Description");
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
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectDetails", "Description", c => c.String());
            AddColumn("dbo.ProjectDetails", "TotalCost", c => c.Double(nullable: false));
            AddColumn("dbo.ProjectDetails", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectDetails", "Cost", c => c.Double(nullable: false));
            AddColumn("dbo.ProjectDetails", "IdRfpWorkType", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectDetails", "IdRfpWorkTypeCategory", c => c.Int(nullable: false));
            DropForeignKey("dbo.RfpFeeSchedules", "IdProjectDetail", "dbo.ProjectDetails");
            DropForeignKey("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory", "dbo.RfpJobTypes");
            DropForeignKey("dbo.RfpFeeSchedules", "IdRfpWorkType", "dbo.RfpJobTypes");
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkType" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkTypeCategory" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdProjectDetail" });
            DropTable("dbo.RfpFeeSchedules");
            CreateIndex("dbo.ProjectDetails", "IdRfpWorkType");
            CreateIndex("dbo.ProjectDetails", "IdRfpWorkTypeCategory");
            AddForeignKey("dbo.ProjectDetails", "IdRfpWorkTypeCategory", "dbo.RfpJobTypes", "Id");
            AddForeignKey("dbo.ProjectDetails", "IdRfpWorkType", "dbo.RfpJobTypes", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
