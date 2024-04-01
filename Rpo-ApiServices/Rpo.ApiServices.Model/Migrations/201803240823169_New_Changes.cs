namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectDetails", "DisApprovedJobNumber", c => c.String(maxLength: 9));
            DropColumn("dbo.ProjectDetails", "DisprovedJobNumber");
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
                        DisApprovedJobNumber = p.String(maxLength: 9),
                        PermittedJobNumber = p.String(maxLength: 9),
                        IdRfp = p.Int(),
                        IdRfpJobType = p.Int(),
                        IdRfpSubJobTypeCategory = p.Int(),
                        IdRfpSubJobType = p.Int(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ProjectDetails]([WorkDescription], [ArePlansNotPrepared], [ArePlansCompleted], [IsApproved], [IsDisaproved], [IsPermitted], [ApprovedJobNumber], [DisApprovedJobNumber], [PermittedJobNumber], [IdRfp], [IdRfpJobType], [IdRfpSubJobTypeCategory], [IdRfpSubJobType], [DisplayOrder])
                      VALUES (@WorkDescription, @ArePlansNotPrepared, @ArePlansCompleted, @IsApproved, @IsDisaproved, @IsPermitted, @ApprovedJobNumber, @DisApprovedJobNumber, @PermittedJobNumber, @IdRfp, @IdRfpJobType, @IdRfpSubJobTypeCategory, @IdRfpSubJobType, @DisplayOrder)
                      
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
                        DisApprovedJobNumber = p.String(maxLength: 9),
                        PermittedJobNumber = p.String(maxLength: 9),
                        IdRfp = p.Int(),
                        IdRfpJobType = p.Int(),
                        IdRfpSubJobTypeCategory = p.Int(),
                        IdRfpSubJobType = p.Int(),
                        DisplayOrder = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ProjectDetails]
                      SET [WorkDescription] = @WorkDescription, [ArePlansNotPrepared] = @ArePlansNotPrepared, [ArePlansCompleted] = @ArePlansCompleted, [IsApproved] = @IsApproved, [IsDisaproved] = @IsDisaproved, [IsPermitted] = @IsPermitted, [ApprovedJobNumber] = @ApprovedJobNumber, [DisApprovedJobNumber] = @DisApprovedJobNumber, [PermittedJobNumber] = @PermittedJobNumber, [IdRfp] = @IdRfp, [IdRfpJobType] = @IdRfpJobType, [IdRfpSubJobTypeCategory] = @IdRfpSubJobTypeCategory, [IdRfpSubJobType] = @IdRfpSubJobType, [DisplayOrder] = @DisplayOrder
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectDetails", "DisprovedJobNumber", c => c.String(maxLength: 9));
            DropColumn("dbo.ProjectDetails", "DisApprovedJobNumber");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
