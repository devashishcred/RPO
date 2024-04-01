namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCOCFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobViolations", "IsCOC", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobViolations", "COCDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobViolations", "COCDate");
            DropColumn("dbo.JobViolations", "IsCOC");
        }
    }
}
