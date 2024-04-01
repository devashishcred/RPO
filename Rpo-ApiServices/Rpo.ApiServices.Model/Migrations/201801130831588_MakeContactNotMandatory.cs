namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeContactNotMandatory : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdContactResponsible" });
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible", c => c.Int());
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "IdResponsibility", c => c.Int());
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobApplicationWorkPermitTypes", new[] { "IdContactResponsible" });
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "IdResponsibility", c => c.Int(nullable: false));
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible", c => c.Int(nullable: false));
            CreateIndex("dbo.JobApplicationWorkPermitTypes", "IdContactResponsible");
        }
    }
}
