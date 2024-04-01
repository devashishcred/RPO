namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Column_Signoff_Jobapplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplications", "SignOff", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplications", "SignOff");
        }
    }
}
