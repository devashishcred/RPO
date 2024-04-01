namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeFeeScheduleQuantityDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RfpFeeSchedules", "Quantity", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RfpFeeSchedules", "Quantity", c => c.Int());
        }
    }
}
