namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsCurrentStatusOfFiling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpJobTypes", "IsCurrentStatusOfFiling", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RfpJobTypes", "IsCurrentStatusOfFiling");
        }
    }
}
