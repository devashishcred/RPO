namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdJobType = c.Int(nullable: false),
                        WorkDescription = c.String(),
                        ArePlansNotPrepared = c.Boolean(nullable: false),
                        ArePlansCompleted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsDisaproved = c.Boolean(nullable: false),
                        IsPermitted = c.Boolean(nullable: false),
                        IdRfp = c.Int(nullable: false),
                        Rfp_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobTypes", t => t.IdJobType)
                .ForeignKey("dbo.Rfps", t => t.Rfp_Id)
                .Index(t => t.IdJobType)
                .Index(t => t.Rfp_Id);
            
            CreateTable(
                "dbo.WorkTypeNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdWorkType = c.Int(nullable: false),
                        Note = c.String(),
                        IdProjectDetail = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkTypes", t => t.IdWorkType)
                .ForeignKey("dbo.ProjectDetails", t => t.IdProjectDetail)
                .Index(t => t.IdWorkType)
                .Index(t => t.IdProjectDetail);
            
            CreateTable(
                "dbo.WorkTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.JobType_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[JobTypes]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[JobTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[JobTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.JobType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[JobTypes]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.JobType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[JobTypes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
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
                        Rfp_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ProjectDetails]([IdJobType], [WorkDescription], [ArePlansNotPrepared], [ArePlansCompleted], [IsApproved], [IsDisaproved], [IsPermitted], [IdRfp], [Rfp_Id])
                      VALUES (@IdJobType, @WorkDescription, @ArePlansNotPrepared, @ArePlansCompleted, @IsApproved, @IsDisaproved, @IsPermitted, @IdRfp, @Rfp_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ProjectDetails]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ProjectDetails] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
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
                        Rfp_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ProjectDetails]
                      SET [IdJobType] = @IdJobType, [WorkDescription] = @WorkDescription, [ArePlansNotPrepared] = @ArePlansNotPrepared, [ArePlansCompleted] = @ArePlansCompleted, [IsApproved] = @IsApproved, [IsDisaproved] = @IsDisaproved, [IsPermitted] = @IsPermitted, [IdRfp] = @IdRfp, [Rfp_Id] = @Rfp_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ProjectDetail_Delete",
                p => new
                    {
                        Id = p.Int(),
                        Rfp_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ProjectDetails]
                      WHERE (([Id] = @Id) AND (([Rfp_Id] = @Rfp_Id) OR ([Rfp_Id] IS NULL AND @Rfp_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.WorkTypeNote_Insert",
                p => new
                    {
                        IdWorkType = p.Int(),
                        Note = p.String(),
                        IdProjectDetail = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[WorkTypeNotes]([IdWorkType], [Note], [IdProjectDetail])
                      VALUES (@IdWorkType, @Note, @IdProjectDetail)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[WorkTypeNotes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[WorkTypeNotes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.WorkTypeNote_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdWorkType = p.Int(),
                        Note = p.String(),
                        IdProjectDetail = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[WorkTypeNotes]
                      SET [IdWorkType] = @IdWorkType, [Note] = @Note, [IdProjectDetail] = @IdProjectDetail
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.WorkTypeNote_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[WorkTypeNotes]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.WorkType_Insert",
                p => new
                    {
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[WorkTypes]([Description])
                      VALUES (@Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[WorkTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[WorkTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.WorkType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Description = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[WorkTypes]
                      SET [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.WorkType_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[WorkTypes]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.WorkType_Delete");
            DropStoredProcedure("dbo.WorkType_Update");
            DropStoredProcedure("dbo.WorkType_Insert");
            DropStoredProcedure("dbo.WorkTypeNote_Delete");
            DropStoredProcedure("dbo.WorkTypeNote_Update");
            DropStoredProcedure("dbo.WorkTypeNote_Insert");
            DropStoredProcedure("dbo.ProjectDetail_Delete");
            DropStoredProcedure("dbo.ProjectDetail_Update");
            DropStoredProcedure("dbo.ProjectDetail_Insert");
            DropStoredProcedure("dbo.JobType_Delete");
            DropStoredProcedure("dbo.JobType_Update");
            DropStoredProcedure("dbo.JobType_Insert");
            DropForeignKey("dbo.ProjectDetails", "Rfp_Id", "dbo.Rfps");
            DropForeignKey("dbo.WorkTypeNotes", "IdProjectDetail", "dbo.ProjectDetails");
            DropForeignKey("dbo.WorkTypeNotes", "IdWorkType", "dbo.WorkTypes");
            DropForeignKey("dbo.ProjectDetails", "IdJobType", "dbo.JobTypes");
            DropIndex("dbo.WorkTypeNotes", new[] { "IdProjectDetail" });
            DropIndex("dbo.WorkTypeNotes", new[] { "IdWorkType" });
            DropIndex("dbo.ProjectDetails", new[] { "Rfp_Id" });
            DropIndex("dbo.ProjectDetails", new[] { "IdJobType" });
            DropTable("dbo.WorkTypes");
            DropTable("dbo.WorkTypeNotes");
            DropTable("dbo.ProjectDetails");
            DropTable("dbo.JobTypes");
        }
    }
}
