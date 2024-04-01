namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTheEmailHistoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RFPEmailAttachmentHistories", "Name", c => c.String(maxLength: 500));
            AddColumn("dbo.RFPEmailHistories", "IdRfp", c => c.Int());
            CreateIndex("dbo.RFPEmailHistories", "IdRfp");
            AddForeignKey("dbo.RFPEmailHistories", "IdRfp", "dbo.Rfps", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RFPEmailHistories", "IdRfp", "dbo.Rfps");
            DropIndex("dbo.RFPEmailHistories", new[] { "IdRfp" });
            DropColumn("dbo.RFPEmailHistories", "IdRfp");
            DropColumn("dbo.RFPEmailAttachmentHistories", "Name");
        }
    }
}
