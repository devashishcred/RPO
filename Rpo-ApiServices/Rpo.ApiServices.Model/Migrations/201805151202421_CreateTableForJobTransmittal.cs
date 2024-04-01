namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableForJobTransmittal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTransmittalJobDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobTransmittal = c.Int(),
                        IdJobDocument = c.Int(),
                        DocumentPath = c.String(maxLength: 500),
                        Copies = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobDocuments", t => t.IdJobDocument)
                .ForeignKey("dbo.JobTransmittals", t => t.IdJobTransmittal)
                .Index(t => t.IdJobTransmittal)
                .Index(t => t.IdJobDocument);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTransmittalJobDocuments", "IdJobTransmittal", "dbo.JobTransmittals");
            DropForeignKey("dbo.JobTransmittalJobDocuments", "IdJobDocument", "dbo.JobDocuments");
            DropIndex("dbo.JobTransmittalJobDocuments", new[] { "IdJobDocument" });
            DropIndex("dbo.JobTransmittalJobDocuments", new[] { "IdJobTransmittal" });
            DropTable("dbo.JobTransmittalJobDocuments");
        }
    }
}
