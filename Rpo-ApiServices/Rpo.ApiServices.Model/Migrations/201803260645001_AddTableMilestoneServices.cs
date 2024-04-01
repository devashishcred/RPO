namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableMilestoneServices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MilestoneServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdMilestone = c.Int(nullable: false),
                        IdRfpFeeSchedule = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Milestones", t => t.IdMilestone)
                .ForeignKey("dbo.RfpFeeSchedules", t => t.IdRfpFeeSchedule)
                .Index(t => t.IdMilestone)
                .Index(t => t.IdRfpFeeSchedule);
            
            DropColumn("dbo.Milestones", "Units");
            AlterStoredProcedure(
                "dbo.Milestone_Insert",
                p => new
                    {
                        Name = p.String(),
                        Value = p.Double(),
                        IdRfp = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Milestones]([Name], [Value], [IdRfp], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Value, @IdRfp, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Milestones]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Milestones] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Milestone_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Value = p.Double(),
                        IdRfp = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Milestones]
                      SET [Name] = @Name, [Value] = @Value, [IdRfp] = @IdRfp, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Milestones", "Units", c => c.Int(nullable: false));
            DropForeignKey("dbo.MilestoneServices", "IdRfpFeeSchedule", "dbo.RfpFeeSchedules");
            DropForeignKey("dbo.MilestoneServices", "IdMilestone", "dbo.Milestones");
            DropIndex("dbo.MilestoneServices", new[] { "IdRfpFeeSchedule" });
            DropIndex("dbo.MilestoneServices", new[] { "IdMilestone" });
            DropTable("dbo.MilestoneServices");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
