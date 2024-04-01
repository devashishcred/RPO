namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Employee_IdCity_nullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Employees", new[] { "IdCity" });
            AlterColumn("dbo.Employees", "IdCity", c => c.Int());
            CreateIndex("dbo.Employees", "IdCity");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Employees", new[] { "IdCity" });
            AlterColumn("dbo.Employees", "IdCity", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "IdCity");
        }
    }
}
