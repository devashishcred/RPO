namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeJobContactAddress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobContacts", "IdRfpAddress", "dbo.RfpAddresses");
            DropIndex("dbo.JobContacts", new[] { "IdRfpAddress" });
            AddColumn("dbo.JobContacts", "IdAddress", c => c.Int());
            CreateIndex("dbo.JobContacts", "IdAddress");
            AddForeignKey("dbo.JobContacts", "IdAddress", "dbo.Addresses", "Id");
            DropColumn("dbo.JobContacts", "IdRfpAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobContacts", "IdRfpAddress", c => c.Int());
            DropForeignKey("dbo.JobContacts", "IdAddress", "dbo.Addresses");
            DropIndex("dbo.JobContacts", new[] { "IdAddress" });
            DropColumn("dbo.JobContacts", "IdAddress");
            CreateIndex("dbo.JobContacts", "IdRfpAddress");
            AddForeignKey("dbo.JobContacts", "IdRfpAddress", "dbo.RfpAddresses", "Id");
        }
    }
}
