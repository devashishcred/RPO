namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldInVerbiagies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Verbiages", "IsEditable", c => c.Boolean(nullable: false));
            AlterStoredProcedure(
                "dbo.Verbiage_Insert",
                p => new
                    {
                        Name = p.String(),
                        Content = p.String(),
                        IsDefault = p.Boolean(),
                        IsEditable = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Verbiages]([Name], [Content], [IsDefault], [IsEditable], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Content, @IsDefault, @IsEditable, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Verbiages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Verbiages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Verbiage_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        Content = p.String(),
                        IsDefault = p.Boolean(),
                        IsEditable = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Verbiages]
                      SET [Name] = @Name, [Content] = @Content, [IsDefault] = @IsDefault, [IsEditable] = @IsEditable, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Verbiages", "IsEditable");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
