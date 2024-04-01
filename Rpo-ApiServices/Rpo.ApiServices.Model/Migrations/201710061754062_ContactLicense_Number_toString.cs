namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ContactLicense_Number_toString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContactLicenses", "Number", c => c.String(maxLength: 15));
            AlterStoredProcedure(
                "dbo.ContactLicense_Insert",
                p => new
                    {
                        IdContactLicenseType = p.Int(),
                        Number = p.String(maxLength: 15),
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
            
            AlterStoredProcedure(
                "dbo.ContactLicense_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdContactLicenseType = p.Int(),
                        Number = p.String(maxLength: 15),
                        ExpirationLicenseDate = p.DateTime(),
                        IdContact = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ContactLicenses]
                      SET [IdContactLicenseType] = @IdContactLicenseType, [Number] = @Number, [ExpirationLicenseDate] = @ExpirationLicenseDate, [IdContact] = @IdContact
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContactLicenses", "Number", c => c.Int(nullable: false));
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
