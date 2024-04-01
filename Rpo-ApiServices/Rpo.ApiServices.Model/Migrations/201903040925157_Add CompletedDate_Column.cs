namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompletedDate_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "CompletedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFeeSchedules", "CompletedDate");
        }
    }
}
