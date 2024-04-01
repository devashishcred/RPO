namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentPathInEmployeeDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeDocuments", "DocumentPath", c => c.String(maxLength: 200));
            AlterStoredProcedure(
                "dbo.EmployeeDocument_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 255),
                        Content = p.Binary(),
                        IdEmployee = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                    },
                body:
                    @"INSERT [dbo].[EmployeeDocuments]([Name], [Content], [IdEmployee], [DocumentPath])
                      VALUES (@Name, @Content, @IdEmployee, @DocumentPath)
                      
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
                        Content = p.Binary(),
                        IdEmployee = p.Int(),
                        DocumentPath = p.String(maxLength: 200),
                    },
                body:
                    @"UPDATE [dbo].[EmployeeDocuments]
                      SET [Name] = @Name, [Content] = @Content, [IdEmployee] = @IdEmployee, [DocumentPath] = @DocumentPath
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeDocuments", "DocumentPath");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
