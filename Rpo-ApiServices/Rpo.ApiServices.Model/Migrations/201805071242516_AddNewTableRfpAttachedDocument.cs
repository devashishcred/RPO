namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTableRfpAttachedDocument : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RfpAttachedDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        IdRfp = c.Int(nullable: false),
                        DocumentPath = c.String(maxLength: 200),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        LastModifiedBy = c.Int(),
                        LastModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.CreatedBy)
                .ForeignKey("dbo.Employees", t => t.LastModifiedBy)
                .ForeignKey("dbo.Rfps", t => t.IdRfp)
                .Index(t => t.IdRfp)
                .Index(t => t.CreatedBy)
                .Index(t => t.LastModifiedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RfpAttachedDocuments", "IdRfp", "dbo.Rfps");
            DropForeignKey("dbo.RfpAttachedDocuments", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.RfpAttachedDocuments", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.RfpAttachedDocuments", new[] { "LastModifiedBy" });
            DropIndex("dbo.RfpAttachedDocuments", new[] { "CreatedBy" });
            DropIndex("dbo.RfpAttachedDocuments", new[] { "IdRfp" });
            DropTable("dbo.RfpAttachedDocuments");
        }
    }
}
