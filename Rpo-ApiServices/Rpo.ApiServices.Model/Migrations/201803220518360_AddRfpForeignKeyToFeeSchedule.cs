namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRfpForeignKeyToFeeSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpFeeSchedules", "IdRfp", c => c.Int(nullable: false));
            CreateIndex("dbo.RfpFeeSchedules", "IdRfp");
            AddForeignKey("dbo.RfpFeeSchedules", "IdRfp", "dbo.Rfps", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RfpFeeSchedules", "IdRfp", "dbo.Rfps");
            DropIndex("dbo.RfpFeeSchedules", new[] { "IdRfp" });
            DropColumn("dbo.RfpFeeSchedules", "IdRfp");
        }
    }
}
