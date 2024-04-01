namespace Rpo.ApiServices.Model.Migrations
{ 
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_IsAdditionalService_JobfeeSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "IsAdditionalService", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFeeSchedules", "IsAdditionalService");
        }
    }
}
