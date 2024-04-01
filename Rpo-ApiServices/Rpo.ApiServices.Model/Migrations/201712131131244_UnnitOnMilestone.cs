namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnnitOnMilestone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobMilestones", "Units", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobMilestones", "Units");
        }
    }
}
