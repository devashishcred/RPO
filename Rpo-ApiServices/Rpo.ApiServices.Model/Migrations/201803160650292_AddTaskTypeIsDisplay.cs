namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaskTypeIsDisplay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskTypes", "IsDisplayTime", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskTypes", "IsDisplayTime");
        }
    }
}
