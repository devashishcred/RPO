namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFieldsInViolation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobViolations", "IsFullyResolved", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobViolations", "ResolvedDate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobViolations", "ResolvedDate");
            DropColumn("dbo.JobViolations", "IsFullyResolved");
        }
    }
}
