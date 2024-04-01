namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeunqienessfromemployeetable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Employees", "IX_EmployeeName");
            CreateIndex("dbo.Employees", new[] { "FirstName", "LastName" }, name: "IX_EmployeeName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Employees", "IX_EmployeeName");
            CreateIndex("dbo.Employees", new[] { "FirstName", "LastName" }, unique: true, name: "IX_EmployeeName");
        }
    }
}
