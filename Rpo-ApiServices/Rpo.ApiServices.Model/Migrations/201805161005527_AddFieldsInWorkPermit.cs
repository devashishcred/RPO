namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsInWorkPermit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobApplicationWorkPermitTypes", "DocumentPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobApplicationWorkPermitTypes", "DocumentPath");
        }
    }
}
