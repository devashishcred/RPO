namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInVerbiageMaster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Verbiages", "VerbiageType", c => c.Int());
            DropColumn("dbo.Verbiages", "DisplayOrder");
            AlterStoredProcedure(
                "dbo.Verbiage_Insert",
                p => new
                    {
                        Name = p.String(),
                        Content = p.String(),
                        IsDefault = p.Boolean(),
                        VerbiageType = p.Int(),
                        IsEditable = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Verbiages]([Name], [Content], [IsDefault], [VerbiageType], [IsEditable], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Content, @IsDefault, @VerbiageType, @IsEditable, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                        VerbiageType = p.Int(),
                        IsEditable = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Verbiages]
                      SET [Name] = @Name, [Content] = @Content, [IsDefault] = @IsDefault, [VerbiageType] = @VerbiageType, [IsEditable] = @IsEditable, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Verbiages", "DisplayOrder", c => c.Int());
            DropColumn("dbo.Verbiages", "VerbiageType");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
