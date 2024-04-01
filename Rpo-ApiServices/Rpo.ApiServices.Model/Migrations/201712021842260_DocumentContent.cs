namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobDocuments", "Name", c => c.String());
            AddColumn("dbo.JobDocuments", "Content", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobDocuments", "Content");
            DropColumn("dbo.JobDocuments", "Name");
        }
    }
}
