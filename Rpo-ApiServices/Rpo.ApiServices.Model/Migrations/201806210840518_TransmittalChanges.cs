namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransmittalChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTransmittalCCs", "IdEmployee", c => c.Int());
            CreateIndex("dbo.JobTransmittalCCs", "IdEmployee");
            AddForeignKey("dbo.JobTransmittalCCs", "IdEmployee", "dbo.Contacts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTransmittalCCs", "IdEmployee", "dbo.Contacts");
            DropIndex("dbo.JobTransmittalCCs", new[] { "IdEmployee" });
            DropColumn("dbo.JobTransmittalCCs", "IdEmployee");
        }
    }
}
