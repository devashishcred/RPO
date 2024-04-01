namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IDRFP_JobMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobMilestones", "IdRfp", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobMilestones", "IdRfp");
        }
    }
}
