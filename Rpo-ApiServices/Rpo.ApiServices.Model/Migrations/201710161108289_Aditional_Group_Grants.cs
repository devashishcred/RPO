namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Aditional_Group_Grants : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "RFP", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "Tasks", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "Reports", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "ReferenceLinks", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "ReferenceDocuments", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "UserGroup", c => c.Int(nullable: false));
            AlterStoredProcedure(
                "dbo.Group_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 100),
                        Description = p.String(),
                        EmployeeEmployeeInfo = p.Int(),
                        EmployeeContactInfo = p.Int(),
                        EmployeePersonalInfo = p.Int(),
                        EmployeeAgentCertificates = p.Int(),
                        EmployeeSystemAccessInformation = p.Int(),
                        EmployeeUserGroup = p.Int(),
                        EmployeeDocuments = p.Int(),
                        EmployeeStatus = p.Int(),
                        Jobs = p.Int(),
                        Contacts = p.Int(),
                        Company = p.Int(),
                        RFP = p.Int(),
                        Tasks = p.Int(),
                        Reports = p.Int(),
                        ReferenceLinks = p.Int(),
                        ReferenceDocuments = p.Int(),
                        UserGroup = p.Int(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Groups]([Name], [Description], [EmployeeEmployeeInfo], [EmployeeContactInfo], [EmployeePersonalInfo], [EmployeeAgentCertificates], [EmployeeSystemAccessInformation], [EmployeeUserGroup], [EmployeeDocuments], [EmployeeStatus], [Jobs], [Contacts], [Company], [RFP], [Tasks], [Reports], [ReferenceLinks], [ReferenceDocuments], [UserGroup], [IsActive])
                      VALUES (@Name, @Description, @EmployeeEmployeeInfo, @EmployeeContactInfo, @EmployeePersonalInfo, @EmployeeAgentCertificates, @EmployeeSystemAccessInformation, @EmployeeUserGroup, @EmployeeDocuments, @EmployeeStatus, @Jobs, @Contacts, @Company, @RFP, @Tasks, @Reports, @ReferenceLinks, @ReferenceDocuments, @UserGroup, @IsActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Groups]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Groups] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Group_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 100),
                        Description = p.String(),
                        EmployeeEmployeeInfo = p.Int(),
                        EmployeeContactInfo = p.Int(),
                        EmployeePersonalInfo = p.Int(),
                        EmployeeAgentCertificates = p.Int(),
                        EmployeeSystemAccessInformation = p.Int(),
                        EmployeeUserGroup = p.Int(),
                        EmployeeDocuments = p.Int(),
                        EmployeeStatus = p.Int(),
                        Jobs = p.Int(),
                        Contacts = p.Int(),
                        Company = p.Int(),
                        RFP = p.Int(),
                        Tasks = p.Int(),
                        Reports = p.Int(),
                        ReferenceLinks = p.Int(),
                        ReferenceDocuments = p.Int(),
                        UserGroup = p.Int(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Groups]
                      SET [Name] = @Name, [Description] = @Description, [EmployeeEmployeeInfo] = @EmployeeEmployeeInfo, [EmployeeContactInfo] = @EmployeeContactInfo, [EmployeePersonalInfo] = @EmployeePersonalInfo, [EmployeeAgentCertificates] = @EmployeeAgentCertificates, [EmployeeSystemAccessInformation] = @EmployeeSystemAccessInformation, [EmployeeUserGroup] = @EmployeeUserGroup, [EmployeeDocuments] = @EmployeeDocuments, [EmployeeStatus] = @EmployeeStatus, [Jobs] = @Jobs, [Contacts] = @Contacts, [Company] = @Company, [RFP] = @RFP, [Tasks] = @Tasks, [Reports] = @Reports, [ReferenceLinks] = @ReferenceLinks, [ReferenceDocuments] = @ReferenceDocuments, [UserGroup] = @UserGroup, [IsActive] = @IsActive
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "UserGroup");
            DropColumn("dbo.Groups", "ReferenceDocuments");
            DropColumn("dbo.Groups", "ReferenceLinks");
            DropColumn("dbo.Groups", "Reports");
            DropColumn("dbo.Groups", "Tasks");
            DropColumn("dbo.Groups", "RFP");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
