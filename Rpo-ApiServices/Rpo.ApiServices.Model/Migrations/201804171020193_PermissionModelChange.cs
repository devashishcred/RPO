namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermissionModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "CreatedBy", c => c.Int());
            AddColumn("dbo.Permissions", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.Permissions", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.Permissions", "LastModifiedDate", c => c.DateTime());
            CreateIndex("dbo.Permissions", "CreatedBy");
            CreateIndex("dbo.Permissions", "LastModifiedBy");
            AddForeignKey("dbo.Permissions", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.Permissions", "LastModifiedBy", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.Permissions", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.Permissions", new[] { "LastModifiedBy" });
            DropIndex("dbo.Permissions", new[] { "CreatedBy" });
            DropColumn("dbo.Permissions", "LastModifiedDate");
            DropColumn("dbo.Permissions", "LastModifiedBy");
            DropColumn("dbo.Permissions", "CreatedDate");
            DropColumn("dbo.Permissions", "CreatedBy");
        }
    }
}
