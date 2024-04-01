namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReminderDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskReminders", "ReminderDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskReminders", "ReminderDate");
        }
    }
}
