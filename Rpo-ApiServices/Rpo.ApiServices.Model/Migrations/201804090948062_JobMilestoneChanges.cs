namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobMilestoneChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobFeeSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJob = c.Int(nullable: false),
                        IdRfpWorkType = c.Int(),
                        Cost = c.Double(),
                        Quantity = c.Double(),
                        TotalCost = c.Double(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RfpJobTypes", t => t.IdRfpWorkType)
                .ForeignKey("dbo.Jobs", t => t.IdJob)
                .Index(t => t.IdJob)
                .Index(t => t.IdRfpWorkType);
            
            CreateTable(
                "dbo.JobMilestoneServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdMilestone = c.Int(nullable: false),
                        IdJobFeeSchedule = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobFeeSchedules", t => t.IdJobFeeSchedule)
                .ForeignKey("dbo.JobMilestones", t => t.IdMilestone)
                .Index(t => t.IdMilestone)
                .Index(t => t.IdJobFeeSchedule);
            
            DropColumn("dbo.JobMilestones", "Units");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobMilestones", "Units", c => c.Int());
            DropForeignKey("dbo.JobMilestoneServices", "IdMilestone", "dbo.JobMilestones");
            DropForeignKey("dbo.JobMilestoneServices", "IdJobFeeSchedule", "dbo.JobFeeSchedules");
            DropForeignKey("dbo.JobFeeSchedules", "IdJob", "dbo.Jobs");
            DropForeignKey("dbo.JobFeeSchedules", "IdRfpWorkType", "dbo.RfpJobTypes");
            DropIndex("dbo.JobMilestoneServices", new[] { "IdJobFeeSchedule" });
            DropIndex("dbo.JobMilestoneServices", new[] { "IdMilestone" });
            DropIndex("dbo.JobFeeSchedules", new[] { "IdRfpWorkType" });
            DropIndex("dbo.JobFeeSchedules", new[] { "IdJob" });
            DropTable("dbo.JobMilestoneServices");
            DropTable("dbo.JobFeeSchedules");
        }
    }
}
