namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeUnitandValueNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobMilestones", "Value", c => c.Double());
            AlterColumn("dbo.JobMilestones", "Units", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobMilestones", "Units", c => c.Int(nullable: false));
            AlterColumn("dbo.JobMilestones", "Value", c => c.Double(nullable: false));
        }
    }
}
