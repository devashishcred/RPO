namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RfpAdressNonMandatoryFields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RfpAddresses", new[] { "IdOwnerContact" });
            DropIndex("dbo.RfpAddresses", new[] { "IdOccupancyClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdConstructionClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdMultipleDwellingClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdPrimaryStructuralSystem" });
            DropIndex("dbo.RfpAddresses", new[] { "IdStructureOccupancyCategory" });
            DropIndex("dbo.RfpAddresses", new[] { "IdSeismicDesignCategory" });
            AlterColumn("dbo.RfpAddresses", "IdOwnerContact", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "IdOccupancyClassification", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "IdConstructionClassification", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "IdMultipleDwellingClassification", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "IdPrimaryStructuralSystem", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "IdStructureOccupancyCategory", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "IdSeismicDesignCategory", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "Stories", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "Height", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "Feet", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "DwellingUnits", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "GrossArea", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "StreetLegalWidth", c => c.Int());
            CreateIndex("dbo.RfpAddresses", "IdOwnerContact");
            CreateIndex("dbo.RfpAddresses", "IdOccupancyClassification");
            CreateIndex("dbo.RfpAddresses", "IdConstructionClassification");
            CreateIndex("dbo.RfpAddresses", "IdMultipleDwellingClassification");
            CreateIndex("dbo.RfpAddresses", "IdPrimaryStructuralSystem");
            CreateIndex("dbo.RfpAddresses", "IdStructureOccupancyCategory");
            CreateIndex("dbo.RfpAddresses", "IdSeismicDesignCategory");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RfpAddresses", new[] { "IdSeismicDesignCategory" });
            DropIndex("dbo.RfpAddresses", new[] { "IdStructureOccupancyCategory" });
            DropIndex("dbo.RfpAddresses", new[] { "IdPrimaryStructuralSystem" });
            DropIndex("dbo.RfpAddresses", new[] { "IdMultipleDwellingClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdConstructionClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdOccupancyClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdOwnerContact" });
            AlterColumn("dbo.RfpAddresses", "StreetLegalWidth", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "GrossArea", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "DwellingUnits", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "Feet", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "Height", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "Stories", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "IdSeismicDesignCategory", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "IdStructureOccupancyCategory", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "IdPrimaryStructuralSystem", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "IdMultipleDwellingClassification", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "IdConstructionClassification", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "IdOccupancyClassification", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "IdOwnerContact", c => c.Int(nullable: false));
            CreateIndex("dbo.RfpAddresses", "IdSeismicDesignCategory");
            CreateIndex("dbo.RfpAddresses", "IdStructureOccupancyCategory");
            CreateIndex("dbo.RfpAddresses", "IdPrimaryStructuralSystem");
            CreateIndex("dbo.RfpAddresses", "IdMultipleDwellingClassification");
            CreateIndex("dbo.RfpAddresses", "IdConstructionClassification");
            CreateIndex("dbo.RfpAddresses", "IdOccupancyClassification");
            CreateIndex("dbo.RfpAddresses", "IdOwnerContact");
        }
    }
}
