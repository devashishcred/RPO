namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultContactInTaskType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskTypes", "IsDisplayContact", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskTypes", "IsDisplayDuration", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskTypes", "IdDefaultContact", c => c.Int());
            CreateIndex("dbo.TaskTypes", "IdDefaultContact");
            AddForeignKey("dbo.TaskTypes", "IdDefaultContact", "dbo.Contacts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskTypes", "IdDefaultContact", "dbo.Contacts");
            DropIndex("dbo.TaskTypes", new[] { "IdDefaultContact" });
            DropColumn("dbo.TaskTypes", "IdDefaultContact");
            DropColumn("dbo.TaskTypes", "IsDisplayDuration");
            DropColumn("dbo.TaskTypes", "IsDisplayContact");
        }
    }
}
