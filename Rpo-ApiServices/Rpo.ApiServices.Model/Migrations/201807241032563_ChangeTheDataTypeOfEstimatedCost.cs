namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTheDataTypeOfEstimatedCost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "EstimatedCost", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobApplicationWorkPermitTypes", "EstimatedCost", c => c.String());
        }
    }
}
