namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColumnsInRFPStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpStatus", "DisplayName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RfpStatus", "DisplayName");
        }
    }
}
