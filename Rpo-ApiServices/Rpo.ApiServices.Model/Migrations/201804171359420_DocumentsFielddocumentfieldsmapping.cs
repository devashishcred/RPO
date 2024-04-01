namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentsFielddocumentfieldsmapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDocument = c.Int(),
                        IdField = c.Int(),
                        IsRequired = c.Boolean(),
                        Length = c.Int(),
                        APIUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentMasters", t => t.IdDocument)
                .ForeignKey("dbo.Fields", t => t.IdField)
                .Index(t => t.IdDocument)
                .Index(t => t.IdField);
            
            CreateTable(
                "dbo.DocumentMasters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentName = c.String(),
                        Code = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldName = c.String(),
                        ControlType = c.Int(nullable: false),
                        DataType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentFields", "IdField", "dbo.Fields");
            DropForeignKey("dbo.DocumentFields", "IdDocument", "dbo.DocumentMasters");
            DropIndex("dbo.DocumentFields", new[] { "IdField" });
            DropIndex("dbo.DocumentFields", new[] { "IdDocument" });
            DropTable("dbo.Fields");
            DropTable("dbo.DocumentMasters");
            DropTable("dbo.DocumentFields");
        }
    }
}
