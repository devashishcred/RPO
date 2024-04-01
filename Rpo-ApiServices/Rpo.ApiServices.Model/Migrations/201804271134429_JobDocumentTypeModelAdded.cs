namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobDocumentTypeModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobDocumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobDocumentTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.JobDocumentTypes", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.JobDocumentTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.JobDocumentTypes", new[] { "CreatedBy" });
            DropTable("dbo.JobDocumentTypes");
        }
    }
}
