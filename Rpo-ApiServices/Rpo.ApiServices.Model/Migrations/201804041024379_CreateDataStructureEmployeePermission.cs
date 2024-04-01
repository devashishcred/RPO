namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDataStructureEmployeePermission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeePermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdEmployee = c.Int(nullable: false),
                        Permissions = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.IdEmployee)
                .Index(t => t.IdEmployee);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                        GroupName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserGroupPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdUserGroup = c.Int(nullable: false),
                        Permissions = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.IdUserGroup)
                .Index(t => t.IdUserGroup);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroupPermissions", "IdUserGroup", "dbo.Groups");
            DropForeignKey("dbo.EmployeePermissions", "IdEmployee", "dbo.Employees");
            DropIndex("dbo.UserGroupPermissions", new[] { "IdUserGroup" });
            DropIndex("dbo.EmployeePermissions", new[] { "IdEmployee" });
            DropTable("dbo.UserGroupPermissions");
            DropTable("dbo.Permissions");
            DropTable("dbo.EmployeePermissions");
        }
    }
}
