namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInTheAddressMasters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConstructionClassifications", "Code", c => c.String(maxLength: 10));
            AddColumn("dbo.ConstructionClassifications", "Is_2008_2014", c => c.Boolean());
            AddColumn("dbo.MultipleDwellingClassifications", "Code", c => c.String(maxLength: 10));
            AddColumn("dbo.OccupancyClassifications", "Code", c => c.String(maxLength: 10));
            AddColumn("dbo.OccupancyClassifications", "Is_2008_2014", c => c.Boolean());
            AlterStoredProcedure(
                "dbo.ConstructionClassification_Insert",
                p => new
                    {
                        Code = p.String(maxLength: 10),
                        Is_2008_2014 = p.Boolean(),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ConstructionClassifications]([Code], [Is_2008_2014], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Code, @Is_2008_2014, @Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ConstructionClassifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ConstructionClassifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ConstructionClassification_Update",
                p => new
                    {
                        Id = p.Int(),
                        Code = p.String(maxLength: 10),
                        Is_2008_2014 = p.Boolean(),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ConstructionClassifications]
                      SET [Code] = @Code, [Is_2008_2014] = @Is_2008_2014, [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.MultipleDwellingClassification_Insert",
                p => new
                    {
                        Code = p.String(maxLength: 10),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[MultipleDwellingClassifications]([Code], [Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Code, @Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[MultipleDwellingClassifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[MultipleDwellingClassifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.MultipleDwellingClassification_Update",
                p => new
                    {
                        Id = p.Int(),
                        Code = p.String(maxLength: 10),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[MultipleDwellingClassifications]
                      SET [Code] = @Code, [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.OccupancyClassification_Insert",
                p => new
                    {
                        Code = p.String(maxLength: 10),
                        Description = p.String(maxLength: 50),
                        Is_2008_2014 = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[OccupancyClassifications]([Code], [Description], [Is_2008_2014], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Code, @Description, @Is_2008_2014, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[OccupancyClassifications]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[OccupancyClassifications] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.OccupancyClassification_Update",
                p => new
                    {
                        Id = p.Int(),
                        Code = p.String(maxLength: 10),
                        Description = p.String(maxLength: 50),
                        Is_2008_2014 = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[OccupancyClassifications]
                      SET [Code] = @Code, [Description] = @Description, [Is_2008_2014] = @Is_2008_2014, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.OccupancyClassifications", "Is_2008_2014");
            DropColumn("dbo.OccupancyClassifications", "Code");
            DropColumn("dbo.MultipleDwellingClassifications", "Code");
            DropColumn("dbo.ConstructionClassifications", "Is_2008_2014");
            DropColumn("dbo.ConstructionClassifications", "Code");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
