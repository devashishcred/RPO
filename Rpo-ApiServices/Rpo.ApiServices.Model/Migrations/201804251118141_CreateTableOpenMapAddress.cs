namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableOpenMapAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OpenMapAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Borough = c.String(),
                        HouseNumber = c.String(),
                        Street = c.String(),
                        ZoneDistrict = c.String(),
                        Overlay = c.String(),
                        Map = c.String(),
                        Stories = c.String(),
                        DwellingUnits = c.String(),
                        GrossArea = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OpenMapAddresses");
        }
    }
}
