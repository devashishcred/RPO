namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_columns_in_documentmaster_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentMasters", "IsNewDocument", c => c.Boolean(nullable: false));
            AddColumn("dbo.Fields", "IdDocument", c => c.Int());
            CreateIndex("dbo.Fields", "IdDocument");
            AddForeignKey("dbo.Fields", "IdDocument", "dbo.DocumentMasters", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fields", "IdDocument", "dbo.DocumentMasters");
            DropIndex("dbo.Fields", new[] { "IdDocument" });
            DropColumn("dbo.Fields", "IdDocument");
            DropColumn("dbo.DocumentMasters", "IsNewDocument");
        }
    }
}
