namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobDocumentsInTask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskJobDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTask = c.Int(nullable: false),
                        IdJobDocument = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.JobDocuments", t => t.IdJobDocument)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.Tasks", t => t.IdTask)
                .Index(t => t.IdTask)
                .Index(t => t.IdJobDocument)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskJobDocuments", "IdTask", "dbo.Tasks");
            DropForeignKey("dbo.TaskJobDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.TaskJobDocuments", "IdJobDocument", "dbo.JobDocuments");
            DropForeignKey("dbo.TaskJobDocuments", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.TaskJobDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.TaskJobDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.TaskJobDocuments", new[] { "IdJobDocument" });
            DropIndex("dbo.TaskJobDocuments", new[] { "IdTask" });
            DropTable("dbo.TaskJobDocuments");
        }
    }
}
