namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobDocumentAddedFieldformodifiedempdetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobDocuments", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.JobDocuments", "LastModifiedDate", c => c.DateTime());
            CreateIndex("dbo.JobDocuments", "LastModifiedBy");
            AddForeignKey("dbo.JobDocuments", "LastModifiedBy", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobDocuments", "LastModifiedBy", "dbo.Employees");
            DropIndex("dbo.JobDocuments", new[] { "LastModifiedBy" });
            DropColumn("dbo.JobDocuments", "LastModifiedDate");
            DropColumn("dbo.JobDocuments", "LastModifiedBy");
        }
    }
}
