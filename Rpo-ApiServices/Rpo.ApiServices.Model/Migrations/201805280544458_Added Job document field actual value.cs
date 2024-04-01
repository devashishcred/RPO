namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedJobdocumentfieldactualvalue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobDocumentFields", "ActualValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobDocumentFields", "ActualValue");
        }
    }
}
