namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupIdinTheJobTransmittalCC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTransmittalCCs", "IdGroup", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobTransmittalCCs", "IdGroup");
        }
    }
}
