namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Content = c.Binary(),
                        IdContact = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.IdContact)
                .Index(t => t.IdContact);
            
            CreateStoredProcedure(
                "dbo.ContactDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        Content = p.Binary(),
                        IdContact = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ContactDocuments]([Name], [Content], [IdContact])
                      VALUES (@Name, @Content, @IdContact)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactDocuments]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactDocuments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ContactDocument_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 255),
                        Content = p.Binary(),
                        IdContact = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ContactDocuments]
                      SET [Name] = @Name, [Content] = @Content, [IdContact] = @IdContact
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactDocument_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ContactDocuments]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ContactDocument_Delete");
            DropStoredProcedure("dbo.ContactDocument_Update");
            DropStoredProcedure("dbo.ContactDocument_Insert");
            DropForeignKey("dbo.ContactDocuments", "IdContact", "dbo.Contacts");
            DropIndex("dbo.ContactDocuments", new[] { "IdContact" });
            DropTable("dbo.ContactDocuments");
        }
    }
}
