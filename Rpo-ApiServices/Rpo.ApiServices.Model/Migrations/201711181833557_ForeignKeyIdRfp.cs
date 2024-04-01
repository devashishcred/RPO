namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyIdRfp : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ProjectDetails", new[] { "Rfp_Id" });
            DropColumn("dbo.ProjectDetails", "IdRfp");
            RenameColumn(table: "dbo.ProjectDetails", name: "Rfp_Id", newName: "IdRfp");
            AlterColumn("dbo.ProjectDetails", "IdRfp", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectDetails", "IdRfp");
            AlterStoredProcedure(
                "dbo.ProjectDetail_Insert",
                p => new
                    {
                        IdJobType = p.Int(),
                        WorkDescription = p.String(),
                        ArePlansNotPrepared = p.Boolean(),
                        ArePlansCompleted = p.Boolean(),
                        IsApproved = p.Boolean(),
                        IsDisaproved = p.Boolean(),
                        IsPermitted = p.Boolean(),
                        IdRfp = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ProjectDetails]([IdJobType], [WorkDescription], [ArePlansNotPrepared], [ArePlansCompleted], [IsApproved], [IsDisaproved], [IsPermitted], [IdRfp])
                      VALUES (@IdJobType, @WorkDescription, @ArePlansNotPrepared, @ArePlansCompleted, @IsApproved, @IsDisaproved, @IsPermitted, @IdRfp)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ProjectDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ProjectDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.ProjectDetail_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobType = p.Int(),
                        WorkDescription = p.String(),
                        ArePlansNotPrepared = p.Boolean(),
                        ArePlansCompleted = p.Boolean(),
                        IsApproved = p.Boolean(),
                        IsDisaproved = p.Boolean(),
                        IsPermitted = p.Boolean(),
                        IdRfp = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ProjectDetails]
                      SET [IdJobType] = @IdJobType, [WorkDescription] = @WorkDescription, [ArePlansNotPrepared] = @ArePlansNotPrepared, [ArePlansCompleted] = @ArePlansCompleted, [IsApproved] = @IsApproved, [IsDisaproved] = @IsDisaproved, [IsPermitted] = @IsPermitted, [IdRfp] = @IdRfp
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.ProjectDetail_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ProjectDetails]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProjectDetails", new[] { "IdRfp" });
            AlterColumn("dbo.ProjectDetails", "IdRfp", c => c.Int());
            RenameColumn(table: "dbo.ProjectDetails", name: "IdRfp", newName: "Rfp_Id");
            AddColumn("dbo.ProjectDetails", "IdRfp", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectDetails", "Rfp_Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
