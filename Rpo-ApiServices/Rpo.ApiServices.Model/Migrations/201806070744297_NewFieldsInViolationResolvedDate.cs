namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFieldsInViolationResolvedDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.JobViolations", "ResolvedDate");
            AddColumn("dbo.JobViolations", "ResolvedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobViolations", "ResolvedDate");
            AddColumn("dbo.JobViolations", "ResolvedDate", c => c.Boolean(nullable: false));
        }
    }
}
