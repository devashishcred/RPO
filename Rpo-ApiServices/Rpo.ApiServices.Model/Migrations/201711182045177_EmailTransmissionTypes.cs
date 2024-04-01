namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailTransmissionTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransmissionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.EmailType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[EmailTypes]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EmailTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EmailTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.EmailType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[EmailTypes]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EmailType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[EmailTypes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TransmissionType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[TransmissionTypes]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TransmissionTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TransmissionTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.TransmissionType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[TransmissionTypes]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TransmissionType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[TransmissionTypes]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.TransmissionType_Delete");
            DropStoredProcedure("dbo.TransmissionType_Update");
            DropStoredProcedure("dbo.TransmissionType_Insert");
            DropStoredProcedure("dbo.EmailType_Delete");
            DropStoredProcedure("dbo.EmailType_Update");
            DropStoredProcedure("dbo.EmailType_Insert");
            DropTable("dbo.TransmissionTypes");
            DropTable("dbo.EmailTypes");
        }
    }
}
