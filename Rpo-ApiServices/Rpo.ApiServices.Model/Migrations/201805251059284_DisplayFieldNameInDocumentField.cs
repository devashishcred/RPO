namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisplayFieldNameInDocumentField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fields", "DisplayFieldName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "DisplayFieldName");
        }
    }
}
