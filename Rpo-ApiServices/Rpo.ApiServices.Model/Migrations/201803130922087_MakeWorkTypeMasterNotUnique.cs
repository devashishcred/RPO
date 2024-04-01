namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeWorkTypeMasterNotUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RfpWorkTypes", "IX_RfpWorkTypeName");
            DropIndex("dbo.RfpWorkTypeCategories", "IX_RfpWorkTypeCategoryName");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.RfpWorkTypeCategories", "Name", unique: true, name: "IX_RfpWorkTypeCategoryName");
            CreateIndex("dbo.RfpWorkTypes", "Name", unique: true, name: "IX_RfpWorkTypeName");
        }
    }
}
