namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColumnsInUserNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "IsView", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserNotifications", "RedirectionUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "RedirectionUrl");
            DropColumn("dbo.UserNotifications", "IsView");
        }
    }
}
