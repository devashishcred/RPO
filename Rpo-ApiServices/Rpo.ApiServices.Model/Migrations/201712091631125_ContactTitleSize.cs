namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactTitleSize : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ContactTitles", "IX_ContactTitleName");
            AlterColumn("dbo.ContactTitles", "Name", c => c.String(nullable: false, maxLength: 100));
            Sql("Alter Table dbo.ContactTitles Alter Column Name nvarchar(100) not null");
            CreateIndex("dbo.ContactTitles", "Name", unique: true, name: "IX_ContactTitleName");
            AlterStoredProcedure(
                "dbo.ContactTitle_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
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
            
            AlterStoredProcedure(
                "dbo.ContactTitle_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 100),
                    },
                body:
                    @"UPDATE [dbo].[ContactTitles]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ContactTitles", "IX_ContactTitleName");
            AlterColumn("dbo.ContactTitles", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.ContactTitles", "Name", unique: true, name: "IX_ContactTitleName");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
