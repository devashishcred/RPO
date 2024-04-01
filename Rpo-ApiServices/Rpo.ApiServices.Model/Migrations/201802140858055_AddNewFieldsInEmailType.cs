namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldsInEmailType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailTypes", "Subject", c => c.String(maxLength: 200));
            AddColumn("dbo.EmailTypes", "EmailBody", c => c.String());
            AddColumn("dbo.EmailTypes", "Description", c => c.String(maxLength: 1000));
            AddColumn("dbo.EmailTypes", "IsRfp", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmailTypes", "IsCompany", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmailTypes", "IsJob", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmailTypes", "IsContact", c => c.Boolean(nullable: false));
            AddColumn("dbo.TransmissionTypes", "DefaultCC", c => c.String(maxLength: 1000));
            AlterColumn("dbo.EmailTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.TransmissionTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.EmailTypes", "Name", unique: true, name: "IX_EmailTypeName");
            CreateIndex("dbo.TransmissionTypes", "Name", unique: true, name: "IX_TransmissionTypeName");
            AlterStoredProcedure(
                "dbo.EmailType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Subject = p.String(maxLength: 200),
                        EmailBody = p.String(),
                        Description = p.String(maxLength: 1000),
                        IsRfp = p.Boolean(),
                        IsCompany = p.Boolean(),
                        IsJob = p.Boolean(),
                        IsContact = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[EmailTypes]([Name], [Subject], [EmailBody], [Description], [IsRfp], [IsCompany], [IsJob], [IsContact], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @Subject, @EmailBody, @Description, @IsRfp, @IsCompany, @IsJob, @IsContact, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EmailTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EmailTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.EmailType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        Subject = p.String(maxLength: 200),
                        EmailBody = p.String(),
                        Description = p.String(maxLength: 1000),
                        IsRfp = p.Boolean(),
                        IsCompany = p.Boolean(),
                        IsJob = p.Boolean(),
                        IsContact = p.Boolean(),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[EmailTypes]
                      SET [Name] = @Name, [Subject] = @Subject, [EmailBody] = @EmailBody, [Description] = @Description, [IsRfp] = @IsRfp, [IsCompany] = @IsCompany, [IsJob] = @IsJob, [IsContact] = @IsContact, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.TransmissionType_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        DefaultCC = p.String(maxLength: 1000),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[TransmissionTypes]([Name], [DefaultCC], [CreatedBy], [CreatedDate], [LastModifiedBy], [LastModifiedDate])
                      VALUES (@Name, @DefaultCC, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TransmissionTypes]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TransmissionTypes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TransmissionType_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 50),
                        DefaultCC = p.String(maxLength: 1000),
                        CreatedBy = p.Int(),
                        CreatedDate = p.DateTime(),
                        LastModifiedBy = p.Int(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[TransmissionTypes]
                      SET [Name] = @Name, [DefaultCC] = @DefaultCC, [CreatedBy] = @CreatedBy, [CreatedDate] = @CreatedDate, [LastModifiedBy] = @LastModifiedBy, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TransmissionTypes", "IX_TransmissionTypeName");
            DropIndex("dbo.EmailTypes", "IX_EmailTypeName");
            AlterColumn("dbo.TransmissionTypes", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.EmailTypes", "Name", c => c.String(maxLength: 50));
            DropColumn("dbo.TransmissionTypes", "DefaultCC");
            DropColumn("dbo.EmailTypes", "IsContact");
            DropColumn("dbo.EmailTypes", "IsJob");
            DropColumn("dbo.EmailTypes", "IsCompany");
            DropColumn("dbo.EmailTypes", "IsRfp");
            DropColumn("dbo.EmailTypes", "Description");
            DropColumn("dbo.EmailTypes", "EmailBody");
            DropColumn("dbo.EmailTypes", "Subject");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
