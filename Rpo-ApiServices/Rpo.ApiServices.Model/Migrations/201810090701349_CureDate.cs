namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CureDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobViolations", "CureDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobViolations", "CureDate");
        }
    }
}
