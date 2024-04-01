namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddControlFieldsInRfpWorkType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpWorkTypes", "IsDescriptionControl", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpWorkTypes", "IsQuantityControl", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RfpWorkTypes", "IsQuantityControl");
            DropColumn("dbo.RfpWorkTypes", "IsDescriptionControl");
        }
    }
}
