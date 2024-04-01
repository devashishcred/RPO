namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PendingChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobViolationExplanationOfCharges", "PaneltyAmount", c => c.Double());
            DropColumn("dbo.JobViolationExplanationOfCharges", "FaceAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobViolationExplanationOfCharges", "FaceAmount", c => c.String());
            DropColumn("dbo.JobViolationExplanationOfCharges", "PaneltyAmount");
        }
    }
}
