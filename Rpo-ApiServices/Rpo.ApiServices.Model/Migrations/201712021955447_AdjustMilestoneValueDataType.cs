namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustMilestoneValueDataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobMilestones", "Value", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobMilestones", "Value", c => c.String());
        }
    }
}
