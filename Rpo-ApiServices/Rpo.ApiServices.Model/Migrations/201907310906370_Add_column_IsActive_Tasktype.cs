namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_column_IsActive_Tasktype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskTypes", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskTypes", "IsActive");
        }
    }
}
