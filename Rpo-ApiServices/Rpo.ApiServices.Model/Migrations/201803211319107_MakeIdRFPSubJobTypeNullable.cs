namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdRFPSubJobTypeNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpJobType" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpSubJobTypeCategory" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpSubJobType" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkTypeCategory" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkType" });
            AlterColumn("dbo.ProjectDetails", "IdRfpJobType", c => c.Int());
            AlterColumn("dbo.ProjectDetails", "IdRfpSubJobTypeCategory", c => c.Int());
            AlterColumn("dbo.ProjectDetails", "IdRfpSubJobType", c => c.Int());
            AlterColumn("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory", c => c.Int());
            AlterColumn("dbo.RfpFeeSchedules", "IdRfpWorkType", c => c.Int());
            CreateIndex("dbo.ProjectDetails", "IdRfpJobType");
            CreateIndex("dbo.ProjectDetails", "IdRfpSubJobTypeCategory");
            CreateIndex("dbo.ProjectDetails", "IdRfpSubJobType");
            CreateIndex("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory");
            CreateIndex("dbo.RfpFeeSchedules", "IdRfpWorkType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkType" });
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfpWorkTypeCategory" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpSubJobType" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpSubJobTypeCategory" });
            DropIndex("dbo.ProjectDetails", new[] { "IdRfpJobType" });
            AlterColumn("dbo.RfpFeeSchedules", "IdRfpWorkType", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectDetails", "IdRfpSubJobType", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectDetails", "IdRfpSubJobTypeCategory", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectDetails", "IdRfpJobType", c => c.Int(nullable: false));
            CreateIndex("dbo.RfpFeeSchedules", "IdRfpWorkType");
            CreateIndex("dbo.RfpFeeSchedules", "IdRfpWorkTypeCategory");
            CreateIndex("dbo.ProjectDetails", "IdRfpSubJobType");
            CreateIndex("dbo.ProjectDetails", "IdRfpSubJobTypeCategory");
            CreateIndex("dbo.ProjectDetails", "IdRfpJobType");
        }
    }
}
