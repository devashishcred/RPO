namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsDevelopementCompleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentMasters", "IsDevelopementCompleted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentMasters", "IsDevelopementCompleted");
        }
    }
}
