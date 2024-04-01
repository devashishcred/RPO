namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableTaskDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        IdTask = c.Int(nullable: false),
                        DocumentPath = c.String(maxLength: 200),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.Tasks", t => t.IdTask)
                .Index(t => t.IdTask)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskDocuments", "IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.TaskDocuments", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.TaskDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.TaskDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.TaskDocuments", new[] { "IdTask" });
            DropTable("dbo.TaskDocuments");
        }
    }
}
