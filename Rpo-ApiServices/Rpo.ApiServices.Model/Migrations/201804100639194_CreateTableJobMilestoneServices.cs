namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableJobMilestoneServices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "IdRfpFeeSchedule", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFeeSchedules", "IdRfpFeeSchedule");
        }
    }
}
