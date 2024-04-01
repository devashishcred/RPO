namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFieldsForWorkPermit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplicationWorkPermitTypes", "PermitNumber", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "PreviousPermitNumber", c => c.String());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "RenewalFee", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplicationWorkPermitTypes", "RenewalFee");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "PreviousPermitNumber");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "PermitNumber");
        }
    }
}
