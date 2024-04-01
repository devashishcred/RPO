namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesfordefaultcc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransmissionTypeDefaultCCs", "IdEamilType", c => c.Int());
            CreateIndex("dbo.TransmissionTypeDefaultCCs", "IdEamilType");
            AddForeignKey("dbo.TransmissionTypeDefaultCCs", "IdEamilType", "dbo.EmailTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransmissionTypeDefaultCCs", "IdEamilType", "dbo.EmailTypes");
            DropIndex("dbo.TransmissionTypeDefaultCCs", new[] { "IdEamilType" });
            DropColumn("dbo.TransmissionTypeDefaultCCs", "IdEamilType");
        }
    }
}
