namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalCostInTheJobFeeSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "Cost", c => c.Double());
            AddColumn("dbo.JobFeeSchedules", "TotalCost", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFeeSchedules", "TotalCost");
            DropColumn("dbo.JobFeeSchedules", "Cost");
        }
    }
}
