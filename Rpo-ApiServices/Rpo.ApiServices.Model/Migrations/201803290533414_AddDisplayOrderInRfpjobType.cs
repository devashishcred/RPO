namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayOrderInRfpjobType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpJobTypes", "DisplayOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RfpJobTypes", "DisplayOrder");
        }
    }
}
