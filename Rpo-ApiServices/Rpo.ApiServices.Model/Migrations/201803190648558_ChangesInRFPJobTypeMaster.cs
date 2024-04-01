namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInRFPJobTypeMaster : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RfpFeeSchedules", "IdRfpWorkType", "dbo.RfpJobTypes");
            DropForeignKey("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory", "dbo.RfpJobTypes");
            DropForeignKey("dbo.RfpFeeSchedules", "IdProjectDetail", "dbo.ProjectDetails");
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdProjectDetail" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkTypeCategory" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkType" });
            AddColumn("dbo.ProjectDetails", "IdRfpWorkTypeCategory", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectDetails", "IdRfpWorkType", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectDetails", "Cost", c => c.Double(nullable: false));
            AddColumn("dbo.ProjectDetails", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectDetails", "TotalCost", c => c.Double(nullable: false));
            AddColumn("dbo.ProjectDetails", "Description", c => c.String());
            AddColumn("dbo.RfpJobTypes", "AdditionalUnitPrice", c => c.Double());
            AddColumn("dbo.RfpJobTypes", "Cost", c => c.Double());
            AddColumn("dbo.RfpJobTypes", "CostType", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectDetails", "IdRfpWorkTypeCategory");
            CreateIndex("dbo.ProjectDetails", "IdRfpWorkType");
            AddForeignKey("dbo.ProjectDetails", "IdRfpWorkType", "dbo.RfpJobTypes", "Id");
            AddForeignKey("dbo.ProjectDetails", "IdRfpWorkTypeCategory", "dbo.RfpJobTypes", "Id");
            DropColumn("dbo.RfpJobTypes", "AdditionalUnitCost");
            DropTable("dbo.RfpFeeSchedules");
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
                        IdRfpWorkTypeCategory = p.Int(),
                        IdRfpWorkType = p.Int(),
                        Cost = p.Double(),
                        Quantity = p.Int(),
                        TotalCost = p.Double(),
                        Description = p.String(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ProjectDetails]([WorkDescription], [ArePlansNotPrepared], [ArePlansCompleted], [IsApproved], [IsDisaproved], [IsPermitted], [IdRfp], [IdRfpJobType], [IdRfpSubJobTypeCategory], [IdRfpSubJobType], [IdRfpWorkTypeCategory], [IdRfpWorkType], [Cost], [Quantity], [TotalCost], [Description], [DisplayOrder])
                      VALUES (@WorkDescription, @ArePlansNotPrepared, @ArePlansCompleted, @IsApproved, @IsDisaproved, @IsPermitted, @IdRfp, @IdRfpJobType, @IdRfpSubJobTypeCategory, @IdRfpSubJobType, @IdRfpWorkTypeCategory, @IdRfpWorkType, @Cost, @Quantity, @TotalCost, @Description, @DisplayOrder)
                      
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
                        IdRfpWorkTypeCategory = p.Int(),
                        IdRfpWorkType = p.Int(),
                        Cost = p.Double(),
                        Quantity = p.Int(),
                        TotalCost = p.Double(),
                        Description = p.String(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ProjectDetails]
                      SET [WorkDescription] = @WorkDescription, [ArePlansNotPrepared] = @ArePlansNotPrepared, [ArePlansCompleted] = @ArePlansCompleted, [IsApproved] = @IsApproved, [IsDisaproved] = @IsDisaproved, [IsPermitted] = @IsPermitted, [IdRfp] = @IdRfp, [IdRfpJobType] = @IdRfpJobType, [IdRfpSubJobTypeCategory] = @IdRfpSubJobTypeCategory, [IdRfpSubJobType] = @IdRfpSubJobType, [IdRfpWorkTypeCategory] = @IdRfpWorkTypeCategory, [IdRfpWorkType] = @IdRfpWorkType, [Cost] = @Cost, [Quantity] = @Quantity, [TotalCost] = @TotalCost, [Description] = @Description, [DisplayOrder] = @DisplayOrder
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RfpJobTypes", "AdditionalUnitCost", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.ProjectDetails", "IdRfpWorkTypeCategory", "dbo.RfpJobTypes");
            DropForeignKey("dbo.ProjectDetails", "IdRfpWorkType", "dbo.RfpJobTypes");
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpWorkType" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpWorkTypeCategory" });
            DropColumn("dbo.RfpJobTypes", "CostType");
            DropColumn("dbo.RfpJobTypes", "Cost");
            DropColumn("dbo.RfpJobTypes", "AdditionalUnitPrice");
            DropColumn("dbo.ProjectDetails", "Description");
            DropColumn("dbo.ProjectDetails", "TotalCost");
            DropColumn("dbo.ProjectDetails", "Quantity");
            DropColumn("dbo.ProjectDetails", "Cost");
            DropColumn("dbo.ProjectDetails", "IdRfpWorkType");
            DropColumn("dbo.ProjectDetails", "IdRfpWorkTypeCategory");
            CreateIndex("dbo.RfpFeeSchedules", "IdRfpWorkType");
            CreateIndex("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory");
            CreateIndex("dbo.RfpFeeSchedules", "IdProjectDetail");
            AddForeignKey("dbo.RfpFeeSchedules", "IdProjectDetail", "dbo.ProjectDetails", "Id");
            AddForeignKey("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory", "dbo.RfpJobTypes", "Id");
            AddForeignKey("dbo.RfpFeeSchedules", "IdRfpWorkType", "dbo.RfpJobTypes", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
