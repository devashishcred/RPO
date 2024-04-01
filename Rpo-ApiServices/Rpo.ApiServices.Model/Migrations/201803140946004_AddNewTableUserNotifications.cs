namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTableUserNotifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationMessage = c.String(),
                        NotificationDate = c.DateTime(nullable: false),
                        IdUserNotified = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdUserNotified)
                .Index(t => t.IdUserNotified);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "IdUserNotified", "dbo.Employees");
            DropIndex("dbo.UserNotifications", new[] { "IdUserNotified" });
            DropTable("dbo.UserNotifications");
        }
    }
}
