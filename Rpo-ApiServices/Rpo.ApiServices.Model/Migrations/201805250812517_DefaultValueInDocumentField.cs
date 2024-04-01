namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultValueInDocumentField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentFields", "DefaultValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentFields", "DefaultValue");
        }
    }
}
