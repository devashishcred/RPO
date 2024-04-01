namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GiveForeignKeyEmployee : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Employees", "LastModifiedBy");
            AddForeignKey("dbo.Employees", "LastModifiedBy", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "LastModifiedBy", "dbo.Employees");
            DropIndex("dbo.Employees", new[] { "LastModifiedBy" });
        }
    }
}
