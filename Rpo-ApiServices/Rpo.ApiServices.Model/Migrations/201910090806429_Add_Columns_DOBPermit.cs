namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Columns_DOBPermit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DOBPermitMappings", "PermitType", c => c.String());
            AddColumn("dbo.DOBPermitMappings", "PermitSubType", c => c.String());
            AddColumn("dbo.DOBPermitMappings", "EntryDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DOBPermitMappings", "EntryDate");
            DropColumn("dbo.DOBPermitMappings", "PermitSubType");
            DropColumn("dbo.DOBPermitMappings", "PermitType");
        }
    }
}
