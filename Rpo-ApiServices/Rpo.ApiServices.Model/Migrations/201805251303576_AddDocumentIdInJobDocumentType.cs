namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentIdInJobDocumentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobDocumentTypes", "IdDocument", c => c.Int());
            CreateIndex("dbo.JobDocumentTypes", "IdDocument");
            AddForeignKey("dbo.JobDocumentTypes", "IdDocument", "dbo.DocumentMasters", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobDocumentTypes", "IdDocument", "dbo.DocumentMasters");
            DropIndex("dbo.JobDocumentTypes", new[] { "IdDocument" });
            DropColumn("dbo.JobDocumentTypes", "IdDocument");
        }
    }
}
