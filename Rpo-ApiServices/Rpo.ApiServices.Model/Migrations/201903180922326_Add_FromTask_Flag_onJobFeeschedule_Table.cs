namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_FromTask_Flag_onJobFeeschedule_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobFeeSchedules", "FromTask", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobFeeSchedules", "FromTask");
        }
    }
}
