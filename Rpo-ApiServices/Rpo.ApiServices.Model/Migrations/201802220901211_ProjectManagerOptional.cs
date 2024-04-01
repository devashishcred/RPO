namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectManagerOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Jobs", new[] { "IdProjectManager" });
            AlterColumn("dbo.Jobs", "IdProjectManager", c => c.Int());
            CreateIndex("dbo.Jobs", "IdProjectManager");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Jobs", new[] { "IdProjectManager" });
            AlterColumn("dbo.Jobs", "IdProjectManager", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "IdProjectManager");
        }
    }
}
