namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTheContentColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contacts", "Image");
            DropColumn("dbo.ContactDocuments", "Content");
            DropColumn("dbo.EmployeeDocuments", "Content");
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
                    },
                body:
                    @"INSERT [dbo].[Contacts]([PersonalType], [IdPrefix], [IdSuffix], [FirstName], [MiddleName], [LastName], [IdCompany], [IdContactTitle], [BirthDate], [WorkPhone], [WorkPhoneExt], [MobilePhone], [OtherPhone], [Email], [ContactImagePath], [ContactImageThumbPath], [Notes])
                      VALUES (@PersonalType, @IdPrefix, @IdSuffix, @FirstName, @MiddleName, @LastName, @IdCompany, @IdContactTitle, @BirthDate, @WorkPhone, @WorkPhoneExt, @MobilePhone, @OtherPhone, @Email, @ContactImagePath, @ContactImageThumbPath, @Notes)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Contacts]
                      SET [PersonalType] = @PersonalType, [IdPrefix] = @IdPrefix, [IdSuffix] = @IdSuffix, [FirstName] = @FirstName, [MiddleName] = @MiddleName, [LastName] = @LastName, [IdCompany] = @IdCompany, [IdContactTitle] = @IdContactTitle, [BirthDate] = @BirthDate, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [OtherPhone] = @OtherPhone, [Email] = @Email, [ContactImagePath] = @ContactImagePath, [ContactImageThumbPath] = @ContactImageThumbPath, [Notes] = @Notes
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ContactDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        IdContact = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                    },
                body:
                    @"INSERT [dbo].[ContactDocuments]([Name], [IdContact], [DocumentPath])
                      VALUES (@Name, @IdContact, @DocumentPath)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ContactDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 255),
                        IdContact = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                    },
                body:
                    @"UPDATE [dbo].[ContactDocuments]
                      SET [Name] = @Name, [IdContact] = @IdContact, [DocumentPath] = @DocumentPath
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.EmployeeDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        IdEmployee = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                    },
                body:
                    @"INSERT [dbo].[EmployeeDocuments]([Name], [IdEmployee], [DocumentPath])
                      VALUES (@Name, @IdEmployee, @DocumentPath)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EmployeeDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EmployeeDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.EmployeeDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 255),
                        IdEmployee = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                    },
                body:
                    @"UPDATE [dbo].[EmployeeDocuments]
                      SET [Name] = @Name, [IdEmployee] = @IdEmployee, [DocumentPath] = @DocumentPath
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmployeeDocuments", "Content", c => c.Binary());
            AddColumn("dbo.ContactDocuments", "Content", c => c.Binary());
            AddColumn("dbo.Contacts", "Image", c => c.Binary());
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
