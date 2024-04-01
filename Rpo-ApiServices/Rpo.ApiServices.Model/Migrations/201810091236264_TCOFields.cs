namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TCOFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplicationWorkPermitTypes", "PlumbingSignedOff", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "FinalElevator", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "TempElevator", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "ConstructionSignedOff", c => c.DateTime());
            AddColumn("dbo.JobApplicationWorkPermitTypes", "Permittee", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplicationWorkPermitTypes", "Permittee");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "ConstructionSignedOff");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "TempElevator");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "FinalElevator");
            DropColumn("dbo.JobApplicationWorkPermitTypes", "PlumbingSignedOff");
        }
    }
}
