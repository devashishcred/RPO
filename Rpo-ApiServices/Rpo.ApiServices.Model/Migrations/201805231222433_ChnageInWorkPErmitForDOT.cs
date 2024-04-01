namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChnageInWorkPErmitForDOT : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdJobWorkType" });
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType", c => c.Int());
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdJobWorkType" });
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType", c => c.Int(nullable: false));
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "IdJobWorkType");
        }
    }
}
