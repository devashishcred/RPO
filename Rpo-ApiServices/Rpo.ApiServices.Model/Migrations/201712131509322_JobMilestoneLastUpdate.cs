namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobMilestoneLastUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobMilestones", "LastModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.JobMilestones", "LastModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobMilestones", "LastModifiedBy");
            DropColumn("dbo.JobMilestones", "LastModified");
        }
    }
}
