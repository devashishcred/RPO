namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultUrl_JobApplicationWorkPermitTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplicationWorkPermitTypes", "DefaultUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplicationWorkPermitTypes", "DefaultUrl");
        }
    }
}
