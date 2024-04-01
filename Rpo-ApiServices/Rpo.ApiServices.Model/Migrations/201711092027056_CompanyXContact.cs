namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyXContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "IdCompany", c => c.Int());
            CreateIndex("dbo.Contacts", "IdCompany");
            AddForeignKey("dbo.Contacts", "IdCompany", "dbo.Companies", "Id");
            DropColumn("dbo.Contacts", "CompanyName");
            AlterStoredProcedure(
                "dbo.Contact_Insert",
                p => new
                    {
                        Image = p.Binary(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
                        IdContactTitle = p.Int(),
                        BirthDate = p.DateTime(),
                        WorkPhone = p.String(maxLength: 15),
                        WorkPhoneExt = p.String(maxLength: 5),
                        MobilePhone = p.String(maxLength: 15),
                        Email = p.String(maxLength: 255),
                        Notes = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Contacts]([Image], [PersonalType], [IdPrefix], [FirstName], [MiddleName], [LastName], [IdCompany], [IdContactTitle], [BirthDate], [WorkPhone], [WorkPhoneExt], [MobilePhone], [Email], [Notes])
                      VALUES (@Image, @PersonalType, @IdPrefix, @FirstName, @MiddleName, @LastName, @IdCompany, @IdContactTitle, @BirthDate, @WorkPhone, @WorkPhoneExt, @MobilePhone, @Email, @Notes)
                      
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
                        Image = p.Binary(),
                        PersonalType = p.Int(),
                        IdPrefix = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        MiddleName = p.String(maxLength: 2),
                        LastName = p.String(maxLength: 50),
                        IdCompany = p.Int(),
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
                      SET [Image] = @Image, [PersonalType] = @PersonalType, [IdPrefix] = @IdPrefix, [FirstName] = @FirstName, [MiddleName] = @MiddleName, [LastName] = @LastName, [IdCompany] = @IdCompany, [IdContactTitle] = @IdContactTitle, [BirthDate] = @BirthDate, [WorkPhone] = @WorkPhone, [WorkPhoneExt] = @WorkPhoneExt, [MobilePhone] = @MobilePhone, [Email] = @Email, [Notes] = @Notes
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "CompanyName", c => c.String(maxLength: 50));
            DropForeignKey("dbo.Contacts", "IdCompany", "dbo.Companies");
            DropIndex("dbo.Contacts", new[] { "IdCompany" });
            DropColumn("dbo.Contacts", "IdCompany");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
