namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecondOfficerInRfpAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RfpAddresses", "IdSecondOfficerCompany", c => c.Int());
            AddColumn("dbo.RfpAddresses", "IdSecondOfficer", c => c.Int());
            AddColumn("dbo.OwnerTypes", "IsSecondOfficerRequire", c => c.Boolean());
            CreateIndex("dbo.RfpAddresses", "IdSecondOfficerCompany");
            CreateIndex("dbo.RfpAddresses", "IdSecondOfficer");
            AddForeignKey("dbo.RfpAddresses", "IdSecondOfficer", "dbo.Contacts", "Id");
            AddForeignKey("dbo.RfpAddresses", "IdSecondOfficerCompany", "dbo.Companies", "Id");
            AlterStoredProcedure(
                "dbo.RfpAddress_Insert",
                p => new
                    {
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 10),
                        Street = p.String(maxLength: 50),
                        ZipCode = p.String(maxLength: 5),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        BinNumber = p.String(maxLength: 50),
                        ComunityBoardNumber = p.String(maxLength: 50),
                        ZoneDistrict = p.String(maxLength: 50),
                        Overlay = p.String(maxLength: 50),
                        SpecialDistrict = p.String(maxLength: 50),
                        Map = p.String(maxLength: 50),
                        IdOwnerType = p.Int(),
                        IdCompany = p.Int(),
                        NonProfit = p.Boolean(),
                        IdOwnerContact = p.Int(),
                        IdSecondOfficerCompany = p.Int(),
                        IdSecondOfficer = p.Int(),
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
                        GrossArea = p.String(),
                        StreetLegalWidth = p.Int(),
                        IsLandmark = p.Boolean(),
                        IsLittleE = p.Boolean(),
                        TidalWetlandsMapCheck = p.Boolean(),
                        FreshwaterWetlandsMapCheck = p.Boolean(),
                        CoastalErosionHazardAreaMapCheck = p.Boolean(),
                        SpecialFloodHazardAreaCheck = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[RfpAddresses]([IdBorough], [HouseNumber], [Street], [ZipCode], [Block], [Lot], [BinNumber], [ComunityBoardNumber], [ZoneDistrict], [Overlay], [SpecialDistrict], [Map], [IdOwnerType], [IdCompany], [NonProfit], [IdOwnerContact], [IdSecondOfficerCompany], [IdSecondOfficer], [Title], [IdOccupancyClassification], [IsOcupancyClassification20082014], [IdConstructionClassification], [IsConstructionClassification20082014], [IdMultipleDwellingClassification], [IdPrimaryStructuralSystem], [IdStructureOccupancyCategory], [IdSeismicDesignCategory], [Stories], [Height], [Feet], [DwellingUnits], [GrossArea], [StreetLegalWidth], [IsLandmark], [IsLittleE], [TidalWetlandsMapCheck], [FreshwaterWetlandsMapCheck], [CoastalErosionHazardAreaMapCheck], [SpecialFloodHazardAreaCheck], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@IdBorough, @HouseNumber, @Street, @ZipCode, @Block, @Lot, @BinNumber, @ComunityBoardNumber, @ZoneDistrict, @Overlay, @SpecialDistrict, @Map, @IdOwnerType, @IdCompany, @NonProfit, @IdOwnerContact, @IdSecondOfficerCompany, @IdSecondOfficer, @Title, @IdOccupancyClassification, @IsOcupancyClassification20082014, @IdConstructionClassification, @IsConstructionClassification20082014, @IdMultipleDwellingClassification, @IdPrimaryStructuralSystem, @IdStructureOccupancyCategory, @IdSeismicDesignCategory, @Stories, @Height, @Feet, @DwellingUnits, @GrossArea, @StreetLegalWidth, @IsLandmark, @IsLittleE, @TidalWetlandsMapCheck, @FreshwaterWetlandsMapCheck, @CoastalErosionHazardAreaMapCheck, @SpecialFloodHazardAreaCheck, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        BinNumber = p.String(maxLength: 50),
                        ComunityBoardNumber = p.String(maxLength: 50),
                        ZoneDistrict = p.String(maxLength: 50),
                        Overlay = p.String(maxLength: 50),
                        SpecialDistrict = p.String(maxLength: 50),
                        Map = p.String(maxLength: 50),
                        IdOwnerType = p.Int(),
                        IdCompany = p.Int(),
                        NonProfit = p.Boolean(),
                        IdOwnerContact = p.Int(),
                        IdSecondOfficerCompany = p.Int(),
                        IdSecondOfficer = p.Int(),
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
                        GrossArea = p.String(),
                        StreetLegalWidth = p.Int(),
                        IsLandmark = p.Boolean(),
                        IsLittleE = p.Boolean(),
                        TidalWetlandsMapCheck = p.Boolean(),
                        FreshwaterWetlandsMapCheck = p.Boolean(),
                        CoastalErosionHazardAreaMapCheck = p.Boolean(),
                        SpecialFloodHazardAreaCheck = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[RfpAddresses]
                      SET [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [Street] = @Street, [ZipCode] = @ZipCode, [Block] = @Block, [Lot] = @Lot, [BinNumber] = @BinNumber, [ComunityBoardNumber] = @ComunityBoardNumber, [ZoneDistrict] = @ZoneDistrict, [Overlay] = @Overlay, [SpecialDistrict] = @SpecialDistrict, [Map] = @Map, [IdOwnerType] = @IdOwnerType, [IdCompany] = @IdCompany, [NonProfit] = @NonProfit, [IdOwnerContact] = @IdOwnerContact, [IdSecondOfficerCompany] = @IdSecondOfficerCompany, [IdSecondOfficer] = @IdSecondOfficer, [Title] = @Title, [IdOccupancyClassification] = @IdOccupancyClassification, [IsOcupancyClassification20082014] = @IsOcupancyClassification20082014, [IdConstructionClassification] = @IdConstructionClassification, [IsConstructionClassification20082014] = @IsConstructionClassification20082014, [IdMultipleDwellingClassification] = @IdMultipleDwellingClassification, [IdPrimaryStructuralSystem] = @IdPrimaryStructuralSystem, [IdStructureOccupancyCategory] = @IdStructureOccupancyCategory, [IdSeismicDesignCategory] = @IdSeismicDesignCategory, [Stories] = @Stories, [Height] = @Height, [Feet] = @Feet, [DwellingUnits] = @DwellingUnits, [GrossArea] = @GrossArea, [StreetLegalWidth] = @StreetLegalWidth, [IsLandmark] = @IsLandmark, [IsLittleE] = @IsLittleE, [TidalWetlandsMapCheck] = @TidalWetlandsMapCheck, [FreshwaterWetlandsMapCheck] = @FreshwaterWetlandsMapCheck, [CoastalErosionHazardAreaMapCheck] = @CoastalErosionHazardAreaMapCheck, [SpecialFloodHazardAreaCheck] = @SpecialFloodHazardAreaCheck, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RfpAddresses", "IdSecondOfficerCompany", "dbo.Companies");
            DropForeignKey("dbo.RfpAddresses", "IdSecondOfficer", "dbo.Contacts");
            DropIndex("dbo.RfpAddresses", new[] { "IdSecondOfficer" });
            DropIndex("dbo.RfpAddresses", new[] { "IdSecondOfficerCompany" });
            DropColumn("dbo.OwnerTypes", "IsSecondOfficerRequire");
            DropColumn("dbo.RfpAddresses", "IdSecondOfficer");
            DropColumn("dbo.RfpAddresses", "IdSecondOfficerCompany");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
