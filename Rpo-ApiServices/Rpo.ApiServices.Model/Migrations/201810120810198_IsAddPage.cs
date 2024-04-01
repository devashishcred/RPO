namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsAddPage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentMasters", "IsAddPage", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentMasters", "IsAddPage");
        }
    }
}
