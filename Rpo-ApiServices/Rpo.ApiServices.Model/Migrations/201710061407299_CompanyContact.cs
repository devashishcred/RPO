namespace Rpo.ApiServices.Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CompanyContact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdAddressType = c.Int(nullable: false),
                        Address1 = c.String(maxLength: 50),
                        Address2 = c.String(maxLength: 50),
                        IdCity = c.Int(nullable: false),
                        ZipCode = c.String(maxLength: 10),
                        Phone = c.String(maxLength: 15),
                        IdCompany = c.Int(),
                        IdContact = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressTypes", t => t.IdAddressType, cascadeDelete: true)
                .ForeignKey("dbo.Cities", t => t.IdCity, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.IdCompany)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .Index(t => t.IdAddressType)
                .Index(t => t.IdCity)
                .Index(t => t.IdCompany)
                .Index(t => t.IdContact);
            
            CreateTable(
                "dbo.AddressTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_AddressTypeName");
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        TrakingNumber = c.String(),
                        IdTaxidType = c.Int(nullable: false),
                        TaxIdNumber = c.String(maxLength: 10),
                        HICNumber = c.String(maxLength: 10),
                        CorporateTestingLabNamber = c.String(maxLength: 10),
                        SpecialInpector = c.String(maxLength: 10),
                        InsuranceWorkCompesention = c.DateTime(),
                        InsuranceDisbility = c.DateTime(),
                        InsuranceGeneralLiability = c.DateTime(),
                        InsuranceObstructionBond = c.DateTime(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaxIdTypes", t => t.IdTaxidType, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "IX_CompanyName")
                .Index(t => t.IdTaxidType);
            
            CreateTable(
                "dbo.TaxIdTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_TaxIdTypeName");
            
            CreateTable(
                "dbo.CompanyTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IdParent = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyTypes", t => t.IdParent)
                .Index(t => t.Name, unique: true, name: "IX_CompanyTypeName")
                .Index(t => t.IdParent);
            
            CreateTable(
                "dbo.ContactLicenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdContactLicenseType = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        ExpirationLicenseDate = c.DateTime(nullable: false),
                        IdContact = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactLicenseTypes", t => t.IdContactLicenseType, cascadeDelete: true)
                .ForeignKey("dbo.Contacts", t => t.IdContact, cascadeDelete: true)
                .Index(t => t.IdContactLicenseType)
                .Index(t => t.IdContact);
            
            CreateTable(
                "dbo.ContactLicenseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_ContactLicenseTypeName");
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.Binary(),
                        PersonalType = c.Int(nullable: false),
                        IdPrefix = c.Int(),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(maxLength: 2),
                        LastName = c.String(nullable: false, maxLength: 50),
                        CompanyName = c.String(maxLength: 50),
                        IdContactTitle = c.Int(),
                        BirthDate = c.DateTime(),
                        WorkPhone = c.String(maxLength: 15),
                        WorkPhoneExt = c.String(maxLength: 5),
                        MobilePhone = c.String(maxLength: 15),
                        Email = c.String(nullable: false, maxLength: 255),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactTitles", t => t.IdContactTitle)
                .ForeignKey("dbo.Prefixes", t => t.IdPrefix)
                .Index(t => t.IdPrefix)
                .Index(t => t.IdContactTitle)
                .Index(t => t.Email, unique: true, name: "IX_ContactEmail");
            
            CreateTable(
                "dbo.ContactTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_ContactTitleName");
            
            CreateTable(
                "dbo.Prefixes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_PrefixName");
            
            CreateStoredProcedure(
                "dbo.Address_Insert",
                p => new
                    {
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        IdCity = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Addresses]([IdAddressType], [Address1], [Address2], [IdCity], [ZipCode], [Phone], [IdCompany], [IdContact])
                      VALUES (@IdAddressType, @Address1, @Address2, @IdCity, @ZipCode, @Phone, @IdCompany, @IdContact)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Addresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Addresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Address_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        IdCity = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Addresses]
                      SET [IdAddressType] = @IdAddressType, [Address1] = @Address1, [Address2] = @Address2, [IdCity] = @IdCity, [ZipCode] = @ZipCode, [Phone] = @Phone, [IdCompany] = @IdCompany, [IdContact] = @IdContact
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Address_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Addresses]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AddressType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[AddressTypes]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AddressTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AddressTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.AddressType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[AddressTypes]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AddressType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AddressTypes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        TrakingNumber = p.String(),
                        IdTaxidType = p.Int(),
                        TaxIdNumber = p.String(maxLength: 10),
                        HICNumber = p.String(maxLength: 10),
                        CorporateTestingLabNamber = p.String(maxLength: 10),
                        SpecialInpector = p.String(maxLength: 10),
                        InsuranceWorkCompesention = p.DateTime(),
                        InsuranceDisbility = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Companies]([Name], [TrakingNumber], [IdTaxidType], [TaxIdNumber], [HICNumber], [CorporateTestingLabNamber], [SpecialInpector], [InsuranceWorkCompesention], [InsuranceDisbility], [InsuranceGeneralLiability], [InsuranceObstructionBond], [Notes])
                      VALUES (@Name, @TrakingNumber, @IdTaxidType, @TaxIdNumber, @HICNumber, @CorporateTestingLabNamber, @SpecialInpector, @InsuranceWorkCompesention, @InsuranceDisbility, @InsuranceGeneralLiability, @InsuranceObstructionBond, @Notes)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Companies]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Companies] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Company_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        TrakingNumber = p.String(),
                        IdTaxidType = p.Int(),
                        TaxIdNumber = p.String(maxLength: 10),
                        HICNumber = p.String(maxLength: 10),
                        CorporateTestingLabNamber = p.String(maxLength: 10),
                        SpecialInpector = p.String(maxLength: 10),
                        InsuranceWorkCompesention = p.DateTime(),
                        InsuranceDisbility = p.DateTime(),
                        InsuranceGeneralLiability = p.DateTime(),
                        InsuranceObstructionBond = p.DateTime(),
                        Notes = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Companies]
                      SET [Name] = @Name, [TrakingNumber] = @TrakingNumber, [IdTaxidType] = @IdTaxidType, [TaxIdNumber] = @TaxIdNumber, [HICNumber] = @HICNumber, [CorporateTestingLabNamber] = @CorporateTestingLabNamber, [SpecialInpector] = @SpecialInpector, [InsuranceWorkCompesention] = @InsuranceWorkCompesention, [InsuranceDisbility] = @InsuranceDisbility, [InsuranceGeneralLiability] = @InsuranceGeneralLiability, [InsuranceObstructionBond] = @InsuranceObstructionBond, [Notes] = @Notes
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Company_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Companies]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TaxIdType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[TaxIdTypes]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TaxIdTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TaxIdTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.TaxIdType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[TaxIdTypes]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TaxIdType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[TaxIdTypes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CompanyType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        IdParent = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[CompanyTypes]([Name], [IdParent])
                      VALUES (@Name, @IdParent)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CompanyTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CompanyTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CompanyType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        IdParent = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[CompanyTypes]
                      SET [Name] = @Name, [IdParent] = @IdParent
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CompanyType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CompanyTypes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactLicense_Insert",
                p => new
                    {
                        IdContactLicenseType = p.Int(),
                        Number = p.Int(),
                        ExpirationLicenseDate = p.DateTime(),
                        IdContact = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ContactLicenses]([IdContactLicenseType], [Number], [ExpirationLicenseDate], [IdContact])
                      VALUES (@IdContactLicenseType, @Number, @ExpirationLicenseDate, @IdContact)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactLicenses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactLicenses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ContactLicense_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdContactLicenseType = p.Int(),
                        Number = p.Int(),
                        ExpirationLicenseDate = p.DateTime(),
                        IdContact = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ContactLicenses]
                      SET [IdContactLicenseType] = @IdContactLicenseType, [Number] = @Number, [ExpirationLicenseDate] = @ExpirationLicenseDate, [IdContact] = @IdContact
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactLicense_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ContactLicenses]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactLicenseType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[ContactLicenseTypes]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactLicenseTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactLicenseTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ContactLicenseType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[ContactLicenseTypes]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactLicenseType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ContactLicenseTypes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Contact_Insert",
                p => new
                    {
                        Image = p.Binary(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        CompanyName = p.String(maxLength: 50),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Notes = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Contacts]([Image], [PersonalType], [IdPrefix], [FirstName], [MiddleName], [LastName], [CompanyName], [IdContactTitle], [BirthDate], [WorkPhone], [WorkPhoneExt], [MobilePhone], [Email], [Notes])
                      VALUES (@Image, @PersonalType, @IdPrefix, @FirstName, @MiddleName, @LastName, @CompanyName, @IdContactTitle, @BirthDate, @WorkPhone, @WorkPhoneExt, @MobilePhone, @Email, @Notes)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Contacts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Contacts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Contact_Update",
                p => new
                    {
                        Id = p.Int(),
                        Image = p.Binary(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        CompanyName = p.String(maxLength: 50),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Notes = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Contacts]
                      SET [Image] = @Image, [PersonalType] = @PersonalType, [IdPrefix] = @IdPrefix, [FirstName] = @FirstName, [MiddleName] = @MiddleName, [LastName] = @LastName, [CompanyName] = @CompanyName, [IdContactTitle] = @IdContactTitle, [BirthDate] = @BirthDate, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [Email] = @Email, [Notes] = @Notes
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Contact_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Contacts]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactTitle_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[ContactTitles]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactTitles]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactTitles] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ContactTitle_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[ContactTitles]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactTitle_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ContactTitles]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ContactTitle_Delete");
            DropStoredProcedure("dbo.ContactTitle_Update");
            DropStoredProcedure("dbo.ContactTitle_Insert");
            DropStoredProcedure("dbo.Contact_Delete");
            DropStoredProcedure("dbo.Contact_Update");
            DropStoredProcedure("dbo.Contact_Insert");
            DropStoredProcedure("dbo.ContactLicenseType_Delete");
            DropStoredProcedure("dbo.ContactLicenseType_Update");
            DropStoredProcedure("dbo.ContactLicenseType_Insert");
            DropStoredProcedure("dbo.ContactLicense_Delete");
            DropStoredProcedure("dbo.ContactLicense_Update");
            DropStoredProcedure("dbo.ContactLicense_Insert");
            DropStoredProcedure("dbo.CompanyType_Delete");
            DropStoredProcedure("dbo.CompanyType_Update");
            DropStoredProcedure("dbo.CompanyType_Insert");
            DropStoredProcedure("dbo.TaxIdType_Delete");
            DropStoredProcedure("dbo.TaxIdType_Update");
            DropStoredProcedure("dbo.TaxIdType_Insert");
            DropStoredProcedure("dbo.Company_Delete");
            DropStoredProcedure("dbo.Company_Update");
            DropStoredProcedure("dbo.Company_Insert");
            DropStoredProcedure("dbo.AddressType_Delete");
            DropStoredProcedure("dbo.AddressType_Update");
            DropStoredProcedure("dbo.AddressType_Insert");
            DropStoredProcedure("dbo.Address_Delete");
            DropStoredProcedure("dbo.Address_Update");
            DropStoredProcedure("dbo.Address_Insert");
            DropForeignKey("dbo.Contacts", "IdPrefix", "dbo.Prefixes");
            DropForeignKey("dbo.Contacts", "IdContactTitle", "dbo.ContactTitles");
            DropForeignKey("dbo.ContactLicenses", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.Addresses", "IdContact", "dbo.Contacts");
            DropForeignKey("dbo.ContactLicenses", "IdContactLicenseType", "dbo.ContactLicenseTypes");
            DropForeignKey("dbo.CompanyTypes", "IdParent", "dbo.CompanyTypes");
            DropForeignKey("dbo.Companies", "IdTaxidType", "dbo.TaxIdTypes");
            DropForeignKey("dbo.Addresses", "IdCompany", "dbo.Companies");
            DropForeignKey("dbo.Addresses", "IdCity", "dbo.Cities");
            DropForeignKey("dbo.Addresses", "IdAddressType", "dbo.AddressTypes");
            DropIndex("dbo.Prefixes", "IX_PrefixName");
            DropIndex("dbo.ContactTitles", "IX_ContactTitleName");
            DropIndex("dbo.Contacts", "IX_ContactEmail");
            DropIndex("dbo.Contacts", new[] { "IdContactTitle" });
            DropIndex("dbo.Contacts", new[] { "IdPrefix" });
            DropIndex("dbo.ContactLicenseTypes", "IX_ContactLicenseTypeName");
            DropIndex("dbo.ContactLicenses", new[] { "IdContact" });
            DropIndex("dbo.ContactLicenses", new[] { "IdContactLicenseType" });
            DropIndex("dbo.CompanyTypes", new[] { "IdParent" });
            DropIndex("dbo.CompanyTypes", "IX_CompanyTypeName");
            DropIndex("dbo.TaxIdTypes", "IX_TaxIdTypeName");
            DropIndex("dbo.Companies", new[] { "IdTaxidType" });
            DropIndex("dbo.Companies", "IX_CompanyName");
            DropIndex("dbo.AddressTypes", "IX_AddressTypeName");
            DropIndex("dbo.Addresses", new[] { "IdContact" });
            DropIndex("dbo.Addresses", new[] { "IdCompany" });
            DropIndex("dbo.Addresses", new[] { "IdCity" });
            DropIndex("dbo.Addresses", new[] { "IdAddressType" });
            DropTable("dbo.Prefixes");
            DropTable("dbo.ContactTitles");
            DropTable("dbo.Contacts");
            DropTable("dbo.ContactLicenseTypes");
            DropTable("dbo.ContactLicenses");
            DropTable("dbo.CompanyTypes");
            DropTable("dbo.TaxIdTypes");
            DropTable("dbo.Companies");
            DropTable("dbo.AddressTypes");
            DropTable("dbo.Addresses");
        }
    }
}
