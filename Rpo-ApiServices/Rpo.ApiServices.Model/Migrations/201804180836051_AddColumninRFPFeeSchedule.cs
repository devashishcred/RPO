namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumninRFPFeeSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpFeeSchedules", "IdOldRfpFeeSchedule", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RfpFeeSchedules", "IdOldRfpFeeSchedule");
        }
    }
}
