namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsMigration_JobFeeSchedules : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "IsMilestoneService", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFeeSchedules", "IsMilestoneService");
        }
    }
}
