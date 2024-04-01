namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_column_Serviceitem_contact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.RfpJobTypes", "IsActive", c => c.Boolean(nullable: false));
            AlterStoredProcedure(
                "dbo.Contact_Insert",
                p => new
                    {
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        IdSuffix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        OtherPhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        ContactImagePath = p.String(maxLength: 200),
                        ContactImageThumbPath = p.String(maxLength: 200),
                        Notes = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        FaxNumber = p.String(),
                        IdPrimaryCompanyAddress = p.Int(),
                        IsPrimaryCompanyAddress = p.Boolean(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Contacts]([PersonalType], [IdPrefix], [IdSuffix], [FirstName], [MiddleName], [LastName], [IdCompany], [IdContactTitle], [BirthDate], [WorkPhone], [WorkPhoneExt], [MobilePhone], [OtherPhone], [Email], [ContactImagePath], [ContactImageThumbPath], [Notes], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [FaxNumber], [IdPrimaryCompanyAddress], [IsPrimaryCompanyAddress], [IsActive])
                      VALUES (@PersonalType, @IdPrefix, @IdSuffix, @FirstName, @MiddleName, @LastName, @IdCompany, @IdContactTitle, @BirthDate, @WorkPhone, @WorkPhoneExt, @MobilePhone, @OtherPhone, @Email, @ContactImagePath, @ContactImageThumbPath, @Notes, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @FaxNumber, @IdPrimaryCompanyAddress, @IsPrimaryCompanyAddress, @IsActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Contacts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Contacts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Contact_Update",
                p => new
                    {
                        Id = p.Int(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        IdSuffix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        OtherPhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        ContactImagePath = p.String(maxLength: 200),
                        ContactImageThumbPath = p.String(maxLength: 200),
                        Notes = p.String(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        FaxNumber = p.String(),
                        IdPrimaryCompanyAddress = p.Int(),
                        IsPrimaryCompanyAddress = p.Boolean(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Contacts]
                      SET [PersonalType] = @PersonalType, [IdPrefix] = @IdPrefix, [IdSuffix] = @IdSuffix, [FirstName] = @FirstName, [MiddleName] = @MiddleName, [LastName] = @LastName, [IdCompany] = @IdCompany, [IdContactTitle] = @IdContactTitle, [BirthDate] = @BirthDate, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [OtherPhone] = @OtherPhone, [Email] = @Email, [ContactImagePath] = @ContactImagePath, [ContactImageThumbPath] = @ContactImageThumbPath, [Notes] = @Notes, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [FaxNumber] = @FaxNumber, [IdPrimaryCompanyAddress] = @IdPrimaryCompanyAddress, [IsPrimaryCompanyAddress] = @IsPrimaryCompanyAddress, [IsActive] = @IsActive
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.RfpJobTypes", "IsActive");
            DropColumn("dbo.Contacts", "IsActive");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
