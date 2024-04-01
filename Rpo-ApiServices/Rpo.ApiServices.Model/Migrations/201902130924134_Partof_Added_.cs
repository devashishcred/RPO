namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Partof_Added_ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpFeeSchedules", "IdPartof", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RfpFeeSchedules", "IdPartof");
        }
    }
}
