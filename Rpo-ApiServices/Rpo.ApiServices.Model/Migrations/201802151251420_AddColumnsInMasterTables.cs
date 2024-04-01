namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsInMasterTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MultipleDwellingClassifications", "CreatedBy", c => c.Int());
            AddColumn("dbo.MultipleDwellingClassifications", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.MultipleDwellingClassifications", "LastModifiedBy", c => c.Int());
            AddColumn("dbo.MultipleDwellingClassifications", "LastModifiedDate", c => c.DateTime());
            CreateIndex("dbo.MultipleDwellingClassifications", "Description", unique: true, name: "IX_MultipleDwellingClassificationDescription");
            CreateIndex("dbo.MultipleDwellingClassifications", "CreatedBy");
            CreateIndex("dbo.MultipleDwellingClassifications", "LastModifiedBy");
            AddForeignKey("dbo.MultipleDwellingClassifications", "CreatedBy", "dbo.Employees", "Id");
            AddForeignKey("dbo.MultipleDwellingClassifications", "LastModifiedBy", "dbo.Employees", "Id");
            AlterStoredProcedure(
                "dbo.MultipleDwellingClassification_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[MultipleDwellingClassifications]([Description], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Description, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
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
                        Description = p.String(maxLength: 50),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[MultipleDwellingClassifications]
                      SET [Description] = @Description, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MultipleDwellingClassifications", "LastModifiedBy", "dbo.Employees");
            DropForeignKey("dbo.MultipleDwellingClassifications", "CreatedBy", "dbo.Employees");
            DropIndex("dbo.MultipleDwellingClassifications", new[] { "LastModifiedBy" });
            DropIndex("dbo.MultipleDwellingClassifications", new[] { "CreatedBy" });
            DropIndex("dbo.MultipleDwellingClassifications", "IX_MultipleDwellingClassificationDescription");
            DropColumn("dbo.MultipleDwellingClassifications", "LastModifiedDate");
            DropColumn("dbo.MultipleDwellingClassifications", "LastModifiedBy");
            DropColumn("dbo.MultipleDwellingClassifications", "CreatedDate");
            DropColumn("dbo.MultipleDwellingClassifications", "CreatedBy");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
