namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RfpInitial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "IdAddressType", "dbo.AddressTypes");
            DropForeignKey("dbo.Addresses", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.Cities", "IdState", "dbo.States");
            DropForeignKey("dbo.AgentCertificates", "IdDocumentType", "dbo.DocumentTypes");
            DropForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.Employees", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.Employees", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.ContactLicenses", "IdContactLicenseType", "dbo.ContactLicenseTypes");
            DropForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            CreateTable(
                "dbo.Boroughs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstructionClassifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MultipleDwellingClassifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OccupancyClassifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrimaryStructuralSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RfpAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HouseNumber = c.String(maxLength: 10),
                        Street = c.String(maxLength: 50),
                        ZipCode = c.String(maxLength: 5),
                        Block = c.String(maxLength: 5),
                        Lot = c.String(maxLength: 5),
                        BinNumber = c.String(maxLength: 5),
                        ComunityBoardNumber = c.String(maxLength: 5),
                        ZoneDistrict = c.String(maxLength: 50),
                        Overlay = c.String(maxLength: 50),
                        SpecialDistrict = c.String(maxLength: 50),
                        Map = c.String(maxLength: 50),
                        IdAddressType = c.Int(nullable: false),
                        Company = c.String(maxLength: 50),
                        NonProfit = c.Boolean(nullable: false),
                        IdOwnerContact = c.Int(nullable: false),
                        Title = c.String(maxLength: 50),
                        IdOccupancyClassification = c.Int(nullable: false),
                        IsOcupancyClassification20082014 = c.Boolean(nullable: false),
                        IdConstructionClassification = c.Int(nullable: false),
                        IsConstructionClassification20082014 = c.Boolean(nullable: false),
                        IdMultipleDwellingClassification = c.Int(nullable: false),
                        IdPrimaryStructuralSystem = c.Int(nullable: false),
                        IdStructureOccupancyCategory = c.Int(nullable: false),
                        IdSeismicDesignCategory = c.Int(nullable: false),
                        Stories = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Feet = c.Int(nullable: false),
                        DwellingUnits = c.Int(nullable: false),
                        GrossArea = c.Int(nullable: false),
                        StreetLegalWidth = c.Int(nullable: false),
                        IsLandmark = c.Boolean(nullable: false),
                        IsLittleE = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressTypes", t => t.IdAddressType)
                .ForeignKey("dbo.ConstructionClassifications", t => t.IdConstructionClassification)
                .ForeignKey("dbo.MultipleDwellingClassifications", t => t.IdMultipleDwellingClassification)
                .ForeignKey("dbo.OccupancyClassifications", t => t.IdOccupancyClassification)
                .ForeignKey("dbo.Contacts", t => t.IdOwnerContact)
                .ForeignKey("dbo.PrimaryStructuralSystems", t => t.IdPrimaryStructuralSystem)
                .ForeignKey("dbo.SeismicDesignCategories", t => t.IdSeismicDesignCategory)
                .ForeignKey("dbo.StructureOccupancyCategories", t => t.IdStructureOccupancyCategory)
                .Index(t => t.IdAddressType)
                .Index(t => t.IdOwnerContact)
                .Index(t => t.IdOccupancyClassification)
                .Index(t => t.IdConstructionClassification)
                .Index(t => t.IdMultipleDwellingClassification)
                .Index(t => t.IdPrimaryStructuralSystem)
                .Index(t => t.IdStructureOccupancyCategory)
                .Index(t => t.IdSeismicDesignCategory);
            
            CreateTable(
                "dbo.SeismicDesignCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StructureOccupancyCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rfps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRfpAddress = c.Int(nullable: false),
                        IdBorough = c.Int(nullable: false),
                        HouseNumber = c.String(maxLength: 50),
                        StreetNumber = c.String(maxLength: 50),
                        FloorNumber = c.String(maxLength: 50),
                        Apartment = c.String(maxLength: 50),
                        SpecialPlace = c.String(maxLength: 50),
                        Block = c.String(maxLength: 50),
                        Lot = c.String(maxLength: 50),
                        HasLandMarkStatus = c.Boolean(nullable: false),
                        HasEnvironmentalRestriction = c.Boolean(nullable: false),
                        HasOpenWork = c.Boolean(nullable: false),
                        IdCompany = c.Int(nullable: false),
                        IdContact = c.Int(nullable: false),
                        Address1 = c.String(maxLength: 50),
                        Address2 = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 14),
                        Email = c.String(maxLength: 250),
                        IdReferredByCompany = c.Int(nullable: false),
                        IdReferredByContact = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boroughs", t => t.IdBorough)
                .ForeignKey("dbo.Companies", t => t.IdCompany)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .ForeignKey("dbo.Companies", t => t.IdReferredByCompany)
                .ForeignKey("dbo.Contacts", t => t.IdReferredByContact)
                .ForeignKey("dbo.RfpAddresses", t => t.IdRfpAddress)
                .Index(t => t.IdRfpAddress)
                .Index(t => t.IdBorough)
                .Index(t => t.IdCompany)
                .Index(t => t.IdContact)
                .Index(t => t.IdReferredByCompany)
                .Index(t => t.IdReferredByContact);
            
            AlterColumn("dbo.AgentCertificates", "ExpirationDate", c => c.DateTime());
            AlterColumn("dbo.Employees", "Dob", c => c.DateTime());
            AddForeignKey("dbo.Addresses", "IdAddressType", "dbo.AddressTypes", "Id");
            AddForeignKey("dbo.Addresses", "IdCity", "dbo.Cities", "Id");
            AddForeignKey("dbo.Cities", "IdState", "dbo.States", "Id");
            AddForeignKey("dbo.AgentCertificates", "IdDocumentType", "dbo.DocumentTypes", "Id");
            AddForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees", "Id");
            AddForeignKey("dbo.Employees", "IdCity", "dbo.Cities", "Id");
            AddForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees", "Id");
            AddForeignKey("dbo.Employees", "IdGroup", "dbo.Groups", "Id");
            AddForeignKey("dbo.ContactLicenses", "IdContactLicenseType", "dbo.ContactLicenseTypes", "Id");
            AddForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
            CreateStoredProcedure(
                "dbo.Borough_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[Boroughs]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Boroughs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Boroughs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Borough_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[Boroughs]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Borough_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Boroughs]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ConstructionClassification_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[ConstructionClassifications]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ConstructionClassifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ConstructionClassifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ConstructionClassification_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[ConstructionClassifications]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ConstructionClassification_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ConstructionClassifications]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.MultipleDwellingClassification_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[MultipleDwellingClassifications]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[MultipleDwellingClassifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[MultipleDwellingClassifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.MultipleDwellingClassification_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[MultipleDwellingClassifications]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.MultipleDwellingClassification_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[MultipleDwellingClassifications]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.OccupancyClassification_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[OccupancyClassifications]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[OccupancyClassifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[OccupancyClassifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.OccupancyClassification_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[OccupancyClassifications]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.OccupancyClassification_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[OccupancyClassifications]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PrimaryStructuralSystem_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[PrimaryStructuralSystems]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PrimaryStructuralSystems]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PrimaryStructuralSystems] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PrimaryStructuralSystem_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[PrimaryStructuralSystems]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PrimaryStructuralSystem_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PrimaryStructuralSystems]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RfpAddress_Insert",
                p => new
                    {
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
                        Company = p.String(maxLength: 50),
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
                    },
                body:
                    @"INSERT [dbo].[RfpAddresses]([HouseNumber], [Street], [ZipCode], [Block], [Lot], [BinNumber], [ComunityBoardNumber], [ZoneDistrict], [Overlay], [SpecialDistrict], [Map], [IdAddressType], [Company], [NonProfit], [IdOwnerContact], [Title], [IdOccupancyClassification], [IsOcupancyClassification20082014], [IdConstructionClassification], [IsConstructionClassification20082014], [IdMultipleDwellingClassification], [IdPrimaryStructuralSystem], [IdStructureOccupancyCategory], [IdSeismicDesignCategory], [Stories], [Height], [Feet], [DwellingUnits], [GrossArea], [StreetLegalWidth], [IsLandmark], [IsLittleE])
                      VALUES (@HouseNumber, @Street, @ZipCode, @Block, @Lot, @BinNumber, @ComunityBoardNumber, @ZoneDistrict, @Overlay, @SpecialDistrict, @Map, @IdAddressType, @Company, @NonProfit, @IdOwnerContact, @Title, @IdOccupancyClassification, @IsOcupancyClassification20082014, @IdConstructionClassification, @IsConstructionClassification20082014, @IdMultipleDwellingClassification, @IdPrimaryStructuralSystem, @IdStructureOccupancyCategory, @IdSeismicDesignCategory, @Stories, @Height, @Feet, @DwellingUnits, @GrossArea, @StreetLegalWidth, @IsLandmark, @IsLittleE)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[RfpAddresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RfpAddresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.RfpAddress_Update",
                p => new
                    {
                        Id = p.Int(),
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
                        Company = p.String(maxLength: 50),
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
                    },
                body:
                    @"UPDATE [dbo].[RfpAddresses]
                      SET [HouseNumber] = @HouseNumber, [Street] = @Street, [ZipCode] = @ZipCode, [Block] = @Block, [Lot] = @Lot, [BinNumber] = @BinNumber, [ComunityBoardNumber] = @ComunityBoardNumber, [ZoneDistrict] = @ZoneDistrict, [Overlay] = @Overlay, [SpecialDistrict] = @SpecialDistrict, [Map] = @Map, [IdAddressType] = @IdAddressType, [Company] = @Company, [NonProfit] = @NonProfit, [IdOwnerContact] = @IdOwnerContact, [Title] = @Title, [IdOccupancyClassification] = @IdOccupancyClassification, [IsOcupancyClassification20082014] = @IsOcupancyClassification20082014, [IdConstructionClassification] = @IdConstructionClassification, [IsConstructionClassification20082014] = @IsConstructionClassification20082014, [IdMultipleDwellingClassification] = @IdMultipleDwellingClassification, [IdPrimaryStructuralSystem] = @IdPrimaryStructuralSystem, [IdStructureOccupancyCategory] = @IdStructureOccupancyCategory, [IdSeismicDesignCategory] = @IdSeismicDesignCategory, [Stories] = @Stories, [Height] = @Height, [Feet] = @Feet, [DwellingUnits] = @DwellingUnits, [GrossArea] = @GrossArea, [StreetLegalWidth] = @StreetLegalWidth, [IsLandmark] = @IsLandmark, [IsLittleE] = @IsLittleE
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RfpAddress_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[RfpAddresses]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.SeismicDesignCategory_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[SeismicDesignCategories]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[SeismicDesignCategories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[SeismicDesignCategories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.SeismicDesignCategory_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[SeismicDesignCategories]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.SeismicDesignCategory_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[SeismicDesignCategories]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.StructureOccupancyCategory_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[StructureOccupancyCategories]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[StructureOccupancyCategories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[StructureOccupancyCategories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.StructureOccupancyCategory_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[StructureOccupancyCategories]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.StructureOccupancyCategory_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[StructureOccupancyCategories]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Rfp_Insert",
                p => new
                    {
                        IdRfpAddress = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        Phone = p.String(maxLength: 14),
                        Email = p.String(maxLength: 250),
                        IdReferredByCompany = p.Int(),
                        IdReferredByContact = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Rfps]([IdRfpAddress], [IdBorough], [HouseNumber], [StreetNumber], [FloorNumber], [Apartment], [SpecialPlace], [Block], [Lot], [HasLandMarkStatus], [HasEnvironmentalRestriction], [HasOpenWork], [IdCompany], [IdContact], [Address1], [Address2], [Phone], [Email], [IdReferredByCompany], [IdReferredByContact])
                      VALUES (@IdRfpAddress, @IdBorough, @HouseNumber, @StreetNumber, @FloorNumber, @Apartment, @SpecialPlace, @Block, @Lot, @HasLandMarkStatus, @HasEnvironmentalRestriction, @HasOpenWork, @IdCompany, @IdContact, @Address1, @Address2, @Phone, @Email, @IdReferredByCompany, @IdReferredByContact)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Rfps]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Rfps] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Rfp_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdRfpAddress = p.Int(),
                        IdBorough = p.Int(),
                        HouseNumber = p.String(maxLength: 50),
                        StreetNumber = p.String(maxLength: 50),
                        FloorNumber = p.String(maxLength: 50),
                        Apartment = p.String(maxLength: 50),
                        SpecialPlace = p.String(maxLength: 50),
                        Block = p.String(maxLength: 50),
                        Lot = p.String(maxLength: 50),
                        HasLandMarkStatus = p.Boolean(),
                        HasEnvironmentalRestriction = p.Boolean(),
                        HasOpenWork = p.Boolean(),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        Phone = p.String(maxLength: 14),
                        Email = p.String(maxLength: 250),
                        IdReferredByCompany = p.Int(),
                        IdReferredByContact = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Rfps]
                      SET [IdRfpAddress] = @IdRfpAddress, [IdBorough] = @IdBorough, [HouseNumber] = @HouseNumber, [StreetNumber] = @StreetNumber, [FloorNumber] = @FloorNumber, [Apartment] = @Apartment, [SpecialPlace] = @SpecialPlace, [Block] = @Block, [Lot] = @Lot, [HasLandMarkStatus] = @HasLandMarkStatus, [HasEnvironmentalRestriction] = @HasEnvironmentalRestriction, [HasOpenWork] = @HasOpenWork, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [Address1] = @Address1, [Address2] = @Address2, [Phone] = @Phone, [Email] = @Email, [IdReferredByCompany] = @IdReferredByCompany, [IdReferredByContact] = @IdReferredByContact
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Rfp_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Rfps]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Rfp_Delete");
            DropStoredProcedure("dbo.Rfp_Update");
            DropStoredProcedure("dbo.Rfp_Insert");
            DropStoredProcedure("dbo.StructureOccupancyCategory_Delete");
            DropStoredProcedure("dbo.StructureOccupancyCategory_Update");
            DropStoredProcedure("dbo.StructureOccupancyCategory_Insert");
            DropStoredProcedure("dbo.SeismicDesignCategory_Delete");
            DropStoredProcedure("dbo.SeismicDesignCategory_Update");
            DropStoredProcedure("dbo.SeismicDesignCategory_Insert");
            DropStoredProcedure("dbo.RfpAddress_Delete");
            DropStoredProcedure("dbo.RfpAddress_Update");
            DropStoredProcedure("dbo.RfpAddress_Insert");
            DropStoredProcedure("dbo.PrimaryStructuralSystem_Delete");
            DropStoredProcedure("dbo.PrimaryStructuralSystem_Update");
            DropStoredProcedure("dbo.PrimaryStructuralSystem_Insert");
            DropStoredProcedure("dbo.OccupancyClassification_Delete");
            DropStoredProcedure("dbo.OccupancyClassification_Update");
            DropStoredProcedure("dbo.OccupancyClassification_Insert");
            DropStoredProcedure("dbo.MultipleDwellingClassification_Delete");
            DropStoredProcedure("dbo.MultipleDwellingClassification_Update");
            DropStoredProcedure("dbo.MultipleDwellingClassification_Insert");
            DropStoredProcedure("dbo.ConstructionClassification_Delete");
            DropStoredProcedure("dbo.ConstructionClassification_Update");
            DropStoredProcedure("dbo.ConstructionClassification_Insert");
            DropStoredProcedure("dbo.Borough_Delete");
            DropStoredProcedure("dbo.Borough_Update");
            DropStoredProcedure("dbo.Borough_Insert");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.ContactLicenses", "IdContactLicenseType", "dbo.ContactLicenseTypes");
            DropForeignKey("dbo.Employees", "IdGroup", "dbo.Groups");
            DropForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.Employees", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees");
            DropForeignKey("dbo.AgentCertificates", "IdDocumentType", "dbo.DocumentTypes");
            DropForeignKey("dbo.Cities", "IdState", "dbo.States");
            DropForeignKey("dbo.Addresses", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.Addresses", "IdAddressType", "dbo.AddressTypes");
            DropForeignKey("dbo.Rfps", "IdRfpAddress", "dbo.RfpAddresses");
            DropForeignKey("dbo.Rfps", "IdReferredByContact", "dbo.Contacts");
            DropForeignKey("dbo.Rfps", "IdReferredByCompany", "dbo.Companies");
            DropForeignKey("dbo.Rfps", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.Rfps", "IdCompany", "dbo.Companies");
            DropForeignKey("dbo.Rfps", "IdBorough", "dbo.Boroughs");
            DropForeignKey("dbo.RfpAddresses", "IdStructureOccupancyCategory", "dbo.StructureOccupancyCategories");
            DropForeignKey("dbo.RfpAddresses", "IdSeismicDesignCategory", "dbo.SeismicDesignCategories");
            DropForeignKey("dbo.RfpAddresses", "IdPrimaryStructuralSystem", "dbo.PrimaryStructuralSystems");
            DropForeignKey("dbo.RfpAddresses", "IdOwnerContact", "dbo.Contacts");
            DropForeignKey("dbo.RfpAddresses", "IdOccupancyClassification", "dbo.OccupancyClassifications");
            DropForeignKey("dbo.RfpAddresses", "IdMultipleDwellingClassification", "dbo.MultipleDwellingClassifications");
            DropForeignKey("dbo.RfpAddresses", "IdConstructionClassification", "dbo.ConstructionClassifications");
            DropForeignKey("dbo.RfpAddresses", "IdAddressType", "dbo.AddressTypes");
            DropIndex("dbo.Rfps", new[] { "IdReferredByContact" });
            DropIndex("dbo.Rfps", new[] { "IdReferredByCompany" });
            DropIndex("dbo.Rfps", new[] { "IdContact" });
            DropIndex("dbo.Rfps", new[] { "IdCompany" });
            DropIndex("dbo.Rfps", new[] { "IdBorough" });
            DropIndex("dbo.Rfps", new[] { "IdRfpAddress" });
            DropIndex("dbo.RfpAddresses", new[] { "IdSeismicDesignCategory" });
            DropIndex("dbo.RfpAddresses", new[] { "IdStructureOccupancyCategory" });
            DropIndex("dbo.RfpAddresses", new[] { "IdPrimaryStructuralSystem" });
            DropIndex("dbo.RfpAddresses", new[] { "IdMultipleDwellingClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdConstructionClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdOccupancyClassification" });
            DropIndex("dbo.RfpAddresses", new[] { "IdOwnerContact" });
            DropIndex("dbo.RfpAddresses", new[] { "IdAddressType" });
            AlterColumn("dbo.Employees", "Dob", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AgentCertificates", "ExpirationDate", c => c.DateTime(nullable: false));
            DropTable("dbo.Rfps");
            DropTable("dbo.StructureOccupancyCategories");
            DropTable("dbo.SeismicDesignCategories");
            DropTable("dbo.RfpAddresses");
            DropTable("dbo.PrimaryStructuralSystems");
            DropTable("dbo.OccupancyClassifications");
            DropTable("dbo.MultipleDwellingClassifications");
            DropTable("dbo.ConstructionClassifications");
            DropTable("dbo.Boroughs");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ContactLicenses", "IdContactLicenseType", "dbo.ContactLicenseTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "IdGroup", "dbo.Groups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EmployeeDocuments", "IdEmployee", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "IdCity", "dbo.Cities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AgentCertificates", "IdEmployee", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AgentCertificates", "IdDocumentType", "dbo.DocumentTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Cities", "IdState", "dbo.States", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Addresses", "IdCity", "dbo.Cities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Addresses", "IdAddressType", "dbo.AddressTypes", "Id", cascadeDelete: true);
        }
    }
}
