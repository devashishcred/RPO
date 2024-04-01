namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommonColumnsInMasterTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConstructionClassifications", "CreatedBy", c => c.Int());
            AddColumn("dbo.ConstructionClassifications", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ConstructionClassifications", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.ConstructionClassifications", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.PrimaryStructuralSystems", "CreatedBy", c => c.Int());
            AddColumn("dbo.PrimaryStructuralSystems", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.PrimaryStructuralSystems", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.PrimaryStructuralSystems", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.StructureOccupancyCategories", "CreatedBy", c => c.Int());
            AddColumn("dbo.StructureOccupancyCategories", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.StructureOccupancyCategories", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.StructureOccupancyCategories", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.TaxIdTypes", "CreatedBy", c => c.Int());
            AddColumn("dbo.TaxIdTypes", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.TaxIdTypes", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.TaxIdTypes", "LastModifiedDate", c => c.DateTime());
            CreateIndex("dbo.ConstructionClassifications", "CreatedBy");
            CreateIndex("dbo.ConstructionClassifications", "LastModifiedBy");
            CreateIndex("dbo.PrimaryStructuralSystems", "CreatedBy");
            CreateIndex("dbo.PrimaryStructuralSystems", "LastModifiedBy");
            CreateIndex("dbo.StructureOccupancyCategories", "CreatedBy");
            CreateIndex("dbo.StructureOccupancyCategories", "LastModifiedBy");
            CreateIndex("dbo.TaxIdTypes", "CreatedBy");
            CreateIndex("dbo.TaxIdTypes", "LastModifiedBy");
            AddForeignKey("dbo.ConstructionClassifications", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.ConstructionClassifications", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.PrimaryStructuralSystems", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.PrimaryStructuralSystems", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.StructureOccupancyCategories", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.StructureOccupancyCategories", "LastModifiedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TaxIdTypes", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.TaxIdTypes", "LastModifiedBy", "dbo.Employees", "Id");
            AlterStoredProcedure(
                "dbo.ConstructionClassification_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ConstructionClassifications]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ConstructionClassifications]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.PrimaryStructuralSystem_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[PrimaryStructuralSystems]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PrimaryStructuralSystems]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PrimaryStructuralSystems] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.PrimaryStructuralSystem_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[PrimaryStructuralSystems]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.StructureOccupancyCategory_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[StructureOccupancyCategories]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[StructureOccupancyCategories]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[StructureOccupancyCategories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.StructureOccupancyCategory_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[StructureOccupancyCategories]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.TaxIdType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[TaxIdTypes]([Name], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TaxIdTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TaxIdTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TaxIdType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[TaxIdTypes]
                      SET [Name] = @Name, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaxIdTypes", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.TaxIdTypes", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.StructureOccupancyCategories", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.StructureOccupancyCategories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.PrimaryStructuralSystems", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.PrimaryStructuralSystems", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.ConstructionClassifications", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.ConstructionClassifications", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.TaxIdTypes", new[] { "LastModifiedBy" });
            DropIndex("dbo.TaxIdTypes", new[] { "CreatedBy" });
            DropIndex("dbo.StructureOccupancyCategories", new[] { "LastModifiedBy" });
            DropIndex("dbo.StructureOccupancyCategories", new[] { "CreatedBy" });
            DropIndex("dbo.PrimaryStructuralSystems", new[] { "LastModifiedBy" });
            DropIndex("dbo.PrimaryStructuralSystems", new[] { "CreatedBy" });
            DropIndex("dbo.ConstructionClassifications", new[] { "LastModifiedBy" });
            DropIndex("dbo.ConstructionClassifications", new[] { "CreatedBy" });
            DropColumn("dbo.TaxIdTypes", "LastModifiedDate");
            DropColumn("dbo.TaxIdTypes", "LastModifiedBy");
            DropColumn("dbo.TaxIdTypes", "CreatedDate");
            DropColumn("dbo.TaxIdTypes", "CreatedBy");
            DropColumn("dbo.StructureOccupancyCategories", "LastModifiedDate");
            DropColumn("dbo.StructureOccupancyCategories", "LastModifiedBy");
            DropColumn("dbo.StructureOccupancyCategories", "CreatedDate");
            DropColumn("dbo.StructureOccupancyCategories", "CreatedBy");
            DropColumn("dbo.PrimaryStructuralSystems", "LastModifiedDate");
            DropColumn("dbo.PrimaryStructuralSystems", "LastModifiedBy");
            DropColumn("dbo.PrimaryStructuralSystems", "CreatedDate");
            DropColumn("dbo.PrimaryStructuralSystems", "CreatedBy");
            DropColumn("dbo.ConstructionClassifications", "LastModifiedDate");
            DropColumn("dbo.ConstructionClassifications", "LastModifiedBy");
            DropColumn("dbo.ConstructionClassifications", "CreatedDate");
            DropColumn("dbo.ConstructionClassifications", "CreatedBy");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
