namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInRFPDocuements : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RfpAttachedDocuments", newName: "RfpDocuments");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.RfpDocuments", newName: "RfpAttachedDocuments");
        }
    }
}
