namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ChangeIdentityRfpScopeReview : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RfpFeeSchedules", "Id", c => c.Int(nullable: false, identity: true));
        }

        public override void Down()
        {
            AlterColumn("dbo.RfpFeeSchedules", "Id", c => c.Int(nullable: false, identity: false));
        }
    }
}
