namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColumnInJobtransmittalAttachement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTransmittalAttachments", "Copies", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobTransmittalAttachments", "Copies");
        }
    }
}
