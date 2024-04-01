namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsRemovedFlagInJobFeeSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "IsRemoved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFeeSchedules", "IsRemoved");
        }
    }
}
