namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisprovedJobNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectDetails", "ApprovedJobNumber", c => c.String(maxLength: 9));
            AddColumn("dbo.ProjectDetails", "DisprovedJobNumber", c => c.String(maxLength: 9));
            AddColumn("dbo.ProjectDetails", "PermittedJobNumber", c => c.String(maxLength: 9));
            AlterStoredProcedure(
                "dbo.ProjectDetail_Insert",
                p => new
                    {
                        WorkDescription = p.String(),
                        ArePlansNotPrepared = p.Boolean(),
                        ArePlansCompleted = p.Boolean(),
                        IsApproved = p.Boolean(),
                        IsDisaproved = p.Boolean(),
                        IsPermitted = p.Boolean(),
                        ApprovedJobNumber = p.String(maxLength: 9),
                        DisprovedJobNumber = p.String(maxLength: 9),
                        PermittedJobNumber = p.String(maxLength: 9),
                        IdRfp = p.Int(),
                        IdRfpJobType = p.Int(),
                        IdRfpSubJobTypeCategory = p.Int(),
                        IdRfpSubJobType = p.Int(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ProjectDetails]([WorkDescription], [ArePlansNotPrepared], [ArePlansCompleted], [IsApproved], [IsDisaproved], [IsPermitted], [ApprovedJobNumber], [DisprovedJobNumber], [PermittedJobNumber], [IdRfp], [IdRfpJobType], [IdRfpSubJobTypeCategory], [IdRfpSubJobType], [DisplayOrder])
                      VALUES (@WorkDescription, @ArePlansNotPrepared, @ArePlansCompleted, @IsApproved, @IsDisaproved, @IsPermitted, @ApprovedJobNumber, @DisprovedJobNumber, @PermittedJobNumber, @IdRfp, @IdRfpJobType, @IdRfpSubJobTypeCategory, @IdRfpSubJobType, @DisplayOrder)
                      
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
                        WorkDescription = p.String(),
                        ArePlansNotPrepared = p.Boolean(),
                        ArePlansCompleted = p.Boolean(),
                        IsApproved = p.Boolean(),
                        IsDisaproved = p.Boolean(),
                        IsPermitted = p.Boolean(),
                        ApprovedJobNumber = p.String(maxLength: 9),
                        DisprovedJobNumber = p.String(maxLength: 9),
                        PermittedJobNumber = p.String(maxLength: 9),
                        IdRfp = p.Int(),
                        IdRfpJobType = p.Int(),
                        IdRfpSubJobTypeCategory = p.Int(),
                        IdRfpSubJobType = p.Int(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ProjectDetails]
                      SET [WorkDescription] = @WorkDescription, [ArePlansNotPrepared] = @ArePlansNotPrepared, [ArePlansCompleted] = @ArePlansCompleted, [IsApproved] = @IsApproved, [IsDisaproved] = @IsDisaproved, [IsPermitted] = @IsPermitted, [ApprovedJobNumber] = @ApprovedJobNumber, [DisprovedJobNumber] = @DisprovedJobNumber, [PermittedJobNumber] = @PermittedJobNumber, [IdRfp] = @IdRfp, [IdRfpJobType] = @IdRfpJobType, [IdRfpSubJobTypeCategory] = @IdRfpSubJobTypeCategory, [IdRfpSubJobType] = @IdRfpSubJobType, [DisplayOrder] = @DisplayOrder
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectDetails", "PermittedJobNumber");
            DropColumn("dbo.ProjectDetails", "DisprovedJobNumber");
            DropColumn("dbo.ProjectDetails", "ApprovedJobNumber");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
