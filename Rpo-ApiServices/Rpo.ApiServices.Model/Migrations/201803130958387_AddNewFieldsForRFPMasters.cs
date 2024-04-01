namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldsForRFPMasters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpWorkTypes", "IsUnitCost", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpWorkTypes", "IsAdditionalUnitCost", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpWorkTypes", "IsDescription", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpWorkTypes", "IsWorkDescription", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpWorkTypes", "AdditionalUnitCost", c => c.Double());
            DropColumn("dbo.RfpWorkTypes", "IsFixPrice");
            DropColumn("dbo.RfpWorkTypes", "IsDescriptionControl");
            DropColumn("dbo.RfpWorkTypes", "IsQuantityControl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RfpWorkTypes", "IsQuantityControl", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpWorkTypes", "IsDescriptionControl", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpWorkTypes", "IsFixPrice", c => c.Boolean(nullable: false));
            DropColumn("dbo.RfpWorkTypes", "AdditionalUnitCost");
            DropColumn("dbo.RfpWorkTypes", "IsWorkDescription");
            DropColumn("dbo.RfpWorkTypes", "IsDescription");
            DropColumn("dbo.RfpWorkTypes", "IsAdditionalUnitCost");
            DropColumn("dbo.RfpWorkTypes", "IsUnitCost");
        }
    }
}
