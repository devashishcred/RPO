namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayOrderFieldInDocumentField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentFields", "DisplayOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentFields", "DisplayOrder");
        }
    }
}
