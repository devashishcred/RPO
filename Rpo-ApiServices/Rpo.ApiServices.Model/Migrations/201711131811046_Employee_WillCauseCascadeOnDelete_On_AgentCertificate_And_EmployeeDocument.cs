namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Employee_WillCauseCascadeOnDelete_On_AgentCertificate_And_EmployeeDocument : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees");
            AddForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees");
            AddForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees", "Id");
            AddForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees", "Id");
        }
    }
}
