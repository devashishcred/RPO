namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobContactChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobContacts", "IdRfpAddress", c => c.Int(nullable: false));
            AddColumn("dbo.JobContacts", "IsBilling", c => c.Boolean(nullable: false));
            AddColumn("dbo.JobContacts", "IsMainCompany", c => c.Boolean(nullable: false));
            CreateIndex("dbo.JobContacts", "IdRfpAddress");
            AddForeignKey("dbo.JobContacts", "IdRfpAddress", "dbo.RfpAddresses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobContacts", "IdRfpAddress", "dbo.RfpAddresses");
            DropIndex("dbo.JobContacts", new[] { "IdRfpAddress" });
            DropColumn("dbo.JobContacts", "IsMainCompany");
            DropColumn("dbo.JobContacts", "IsBilling");
            DropColumn("dbo.JobContacts", "IdRfpAddress");
        }
    }
}
