namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplyforeignKeyToIdBorough : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RfpAddresses", new[] { "IdCompany" });
            DropIndex("dbo.RfpAddresses", new[] { "Borough_Id" });
            DropColumn("dbo.RfpAddresses", "IdBorough");
            RenameColumn(table: "dbo.RfpAddresses", name: "Borough_Id", newName: "IdBorough");
            AlterColumn("dbo.RfpAddresses", "IdBorough", c => c.Int());
            AlterColumn("dbo.RfpAddresses", "IdCompany", c => c.Int());
            CreateIndex("dbo.RfpAddresses", "IdBorough");
            CreateIndex("dbo.RfpAddresses", "IdCompany");
            AlterStoredProcedure(
                "dbo.RfpAddress_Insert",
                p => new
                    {
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 10),
                        Street = p.String(maxLength: 50),
                        ZipCode = p.String(maxLength: 5),
                        Block = p.String(maxLength: 5),
                        Lot = p.String(maxLength: 5),
                        BinNumber = p.String(maxLength: 5),
                        ComunityBoardNumber = p.String(maxLength: 5),
                        ZoneDistrict = p.String(maxLength: 50),
                        Overlay = p.String(maxLength: 50),
                        SpecialDistrict = p.String(maxLength: 50),
                        Map = p.String(maxLength: 50),
                        IdAddressType = p.Int(),
                        IdCompany = p.Int(),
                        NonProfit = p.Boolean(),
                        IdOwnerContact = p.Int(),
                        Title = p.String(maxLength: 50),
                        IdOccupancyClassification = p.Int(),
                        IsOcupancyClassification20082014 = p.Boolean(),
                        IdConstructionClassification = p.Int(),
                        IsConstructionClassification20082014 = p.Boolean(),
                        IdMultipleDwellingClassification = p.Int(),
                        IdPrimaryStructuralSystem = p.Int(),
                        IdStructureOccupancyCategory = p.Int(),
                        IdSeismicDesignCategory = p.Int(),
                        Stories = p.Int(),
                        Height = p.Int(),
                        Feet = p.Int(),
                        DwellingUnits = p.Int(),
                        GrossArea = p.Int(),
                        StreetLegalWidth = p.Int(),
                        IsLandmark = p.Boolean(),
                        IsLittleE = p.Boolean(),
                        TidalWetlandsMapCheck = p.Boolean(),
                        FreshwaterWetlandsMapCheck = p.Boolean(),
                        CoastalErosionHazardAreaMapCheck = p.Boolean(),
                        SpecialFloodHazardAreaCheck = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[RfpAddresses]([IdBorough], [HouseNumber], [Street], [ZipCode], [Block], [Lot], [BinNumber], [ComunityBoardNumber], [ZoneDistrict], [Overlay], [SpecialDistrict], [Map], [IdAddressType], [IdCompany], [NonProfit], [IdOwnerContact], [Title], [IdOccupancyClassification], [IsOcupancyClassification20082014], [IdConstructionClassification], [IsConstructionClassification20082014], [IdMultipleDwellingClassification], [IdPrimaryStructuralSystem], [IdStructureOccupancyCategory], [IdSeismicDesignCategory], [Stories], [Height], [Feet], [DwellingUnits], [GrossArea], [StreetLegalWidth], [IsLandmark], [IsLittleE], [TidalWetlandsMapCheck], [FreshwaterWetlandsMapCheck], [CoastalErosionHazardAreaMapCheck], [SpecialFloodHazardAreaCheck])
                      VALUES (@IdBorough, @HouseNumber, @Street, @ZipCode, @Block, @Lot, @BinNumber, @ComunityBoardNumber, @ZoneDistrict, @Overlay, @SpecialDistrict, @Map, @IdAddressType, @IdCompany, @NonProfit, @IdOwnerContact, @Title, @IdOccupancyClassification, @IsOcupancyClassification20082014, @IdConstructionClassification, @IsConstructionClassification20082014, @IdMultipleDwellingClassification, @IdPrimaryStructuralSystem, @IdStructureOccupancyCategory, @IdSeismicDesignCategory, @Stories, @Height, @Feet, @DwellingUnits, @GrossArea, @StreetLegalWidth, @IsLandmark, @IsLittleE, @TidalWetlandsMapCheck, @FreshwaterWetlandsMapCheck, @CoastalErosionHazardAreaMapCheck, @SpecialFloodHazardAreaCheck)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[RfpAddresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RfpAddresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.RfpAddress_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 10),
                        Street = p.String(maxLength: 50),
                        ZipCode = p.String(maxLength: 5),
                        Block = p.String(maxLength: 5),
                        Lot = p.String(maxLength: 5),
                        BinNumber = p.String(maxLength: 5),
                        ComunityBoardNumber = p.String(maxLength: 5),
                        ZoneDistrict = p.String(maxLength: 50),
                        Overlay = p.String(maxLength: 50),
                        SpecialDistrict = p.String(maxLength: 50),
                        Map = p.String(maxLength: 50),
                        IdAddressType = p.Int(),
                        IdCompany = p.Int(),
                        NonProfit = p.Boolean(),
                        IdOwnerContact = p.Int(),
                        Title = p.String(maxLength: 50),
                        IdOccupancyClassification = p.Int(),
                        IsOcupancyClassification20082014 = p.Boolean(),
                        IdConstructionClassification = p.Int(),
                        IsConstructionClassification20082014 = p.Boolean(),
                        IdMultipleDwellingClassification = p.Int(),
                        IdPrimaryStructuralSystem = p.Int(),
                        IdStructureOccupancyCategory = p.Int(),
                        IdSeismicDesignCategory = p.Int(),
                        Stories = p.Int(),
                        Height = p.Int(),
                        Feet = p.Int(),
                        DwellingUnits = p.Int(),
                        GrossArea = p.Int(),
                        StreetLegalWidth = p.Int(),
                        IsLandmark = p.Boolean(),
                        IsLittleE = p.Boolean(),
                        TidalWetlandsMapCheck = p.Boolean(),
                        FreshwaterWetlandsMapCheck = p.Boolean(),
                        CoastalErosionHazardAreaMapCheck = p.Boolean(),
                        SpecialFloodHazardAreaCheck = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[RfpAddresses]
                      SET [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [Street] = @Street, [ZipCode] = @ZipCode, [Block] = @Block, [Lot] = @Lot, [BinNumber] = @BinNumber, [ComunityBoardNumber] = @ComunityBoardNumber, [ZoneDistrict] = @ZoneDistrict, [Overlay] = @Overlay, [SpecialDistrict] = @SpecialDistrict, [Map] = @Map, [IdAddressType] = @IdAddressType, [IdCompany] = @IdCompany, [NonProfit] = @NonProfit, [IdOwnerContact] = @IdOwnerContact, [Title] = @Title, [IdOccupancyClassification] = @IdOccupancyClassification, [IsOcupancyClassification20082014] = @IsOcupancyClassification20082014, [IdConstructionClassification] = @IdConstructionClassification, [IsConstructionClassification20082014] = @IsConstructionClassification20082014, [IdMultipleDwellingClassification] = @IdMultipleDwellingClassification, [IdPrimaryStructuralSystem] = @IdPrimaryStructuralSystem, [IdStructureOccupancyCategory] = @IdStructureOccupancyCategory, [IdSeismicDesignCategory] = @IdSeismicDesignCategory, [Stories] = @Stories, [Height] = @Height, [Feet] = @Feet, [DwellingUnits] = @DwellingUnits, [GrossArea] = @GrossArea, [StreetLegalWidth] = @StreetLegalWidth, [IsLandmark] = @IsLandmark, [IsLittleE] = @IsLittleE, [TidalWetlandsMapCheck] = @TidalWetlandsMapCheck, [FreshwaterWetlandsMapCheck] = @FreshwaterWetlandsMapCheck, [CoastalErosionHazardAreaMapCheck] = @CoastalErosionHazardAreaMapCheck, [SpecialFloodHazardAreaCheck] = @SpecialFloodHazardAreaCheck
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.RfpAddress_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[RfpAddresses]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RfpAddresses", new[] { "IdCompany" });
            DropIndex("dbo.RfpAddresses", new[] { "IdBorough" });
            AlterColumn("dbo.RfpAddresses", "IdCompany", c => c.Int(nullable: false));
            AlterColumn("dbo.RfpAddresses", "IdBorough", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.RfpAddresses", name: "IdBorough", newName: "Borough_Id");
            AddColumn("dbo.RfpAddresses", "IdBorough", c => c.Int(nullable: false));
            CreateIndex("dbo.RfpAddresses", "Borough_Id");
            CreateIndex("dbo.RfpAddresses", "IdCompany");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
