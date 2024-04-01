namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoroughAddCounty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boroughs", "County", c => c.String(maxLength: 50));
            AlterStoredProcedure(
                "dbo.Borough_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        County = p.String(maxLength: 50),
                        BisCode = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Boroughs]([Description], [County], [BisCode], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @County, @BisCode, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Boroughs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Boroughs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Borough_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        County = p.String(maxLength: 50),
                        BisCode = p.Int(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Boroughs]
                      SET [Description] = @Description, [County] = @County, [BisCode] = @BisCode, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boroughs", "County");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
