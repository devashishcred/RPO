namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFaxinCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Fax", c => c.String(maxLength: 15));
            AlterStoredProcedure(
                "dbo.Address_Insert",
                p => new
                    {
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        Fax = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        IsMainAddress = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Addresses]([IdAddressType], [Address1], [Address2], [City], [IdState], [ZipCode], [Phone], [Fax], [IdCompany], [IdContact], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate], [IsMainAddress])
                      VALUES (@IdAddressType, @Address1, @Address2, @City, @IdState, @ZipCode, @Phone, @Fax, @IdCompany, @IdContact, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @IsMainAddress)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Addresses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Addresses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Address_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdAddressType = p.Int(),
                        Address1 = p.String(maxLength: 50),
                        Address2 = p.String(maxLength: 50),
                        City = p.String(),
                        IdState = p.Int(),
                        ZipCode = p.String(maxLength: 10),
                        Phone = p.String(maxLength: 15),
                        Fax = p.String(maxLength: 15),
                        IdCompany = p.Int(),
                        IdContact = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                        IsMainAddress = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Addresses]
                      SET [IdAddressType] = @IdAddressType, [Address1] = @Address1, [Address2] = @Address2, [City] = @City, [IdState] = @IdState, [ZipCode] = @ZipCode, [Phone] = @Phone, [Fax] = @Fax, [IdCompany] = @IdCompany, [IdContact] = @IdContact, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate, [IsMainAddress] = @IsMainAddress
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "Fax");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
