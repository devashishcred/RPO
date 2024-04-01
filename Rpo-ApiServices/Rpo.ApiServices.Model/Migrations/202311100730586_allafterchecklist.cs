namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allafterchecklist : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CustomerNotes", newName: "ClientNoteCustomers");
           // DropForeignKey("dbo.JobCheckListClientNoteHistories", "CreatedBy", "dbo.Employees");
            DropForeignKey("dbo.CustomerNotes", "CreatedBy", "dbo.Customers");
            DropForeignKey("dbo.CustomerNotes", "LastModifiedBy", "dbo.Customers");
            //DropIndex("dbo.ClientNoteCustomers", new[] { "CreatedBy" });
            //DropIndex("dbo.ClientNoteCustomers", new[] { "LastModifiedBy" });
            //DropColumn("dbo.ClientNoteCustomers", "CreatedBy");
            //DropColumn("dbo.ClientNoteCustomers", "LastModifiedBy");
            CreateStoredProcedure(
                "dbo.ClientNoteCustomer_Insert",
                p => new
                    {
                        IdJobChecklistItemDetail = p.Int(),
                        Idcustomer = p.Int(),
                        Description = p.String(),
                        Isinternal = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[ClientNoteCustomers]([IdJobChecklistItemDetail], [Idcustomer], [Description], [Isinternal], [CreatedDate], [LastModifiedDate])
                      VALUES (@IdJobChecklistItemDetail, @Idcustomer, @Description, @Isinternal, @CreatedDate, @LastModifiedDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ClientNoteCustomers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ClientNoteCustomers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ClientNoteCustomer_Update",
                p => new
                    {
                        Id = p.Int(),
                        IdJobChecklistItemDetail = p.Int(),
                        Idcustomer = p.Int(),
                        Description = p.String(),
                        Isinternal = p.Boolean(),
                        CreatedDate = p.DateTime(),
                        LastModifiedDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[ClientNoteCustomers]
                      SET [IdJobChecklistItemDetail] = @IdJobChecklistItemDetail, [Idcustomer] = @Idcustomer, [Description] = @Description, [Isinternal] = @Isinternal, [CreatedDate] = @CreatedDate, [LastModifiedDate] = @LastModifiedDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ClientNoteCustomer_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ClientNoteCustomers]
                      WHERE ([Id] = @Id)"
            );
            
            DropStoredProcedure("dbo.CustomerNote_Insert");
            DropStoredProcedure("dbo.CustomerNote_Update");
            DropStoredProcedure("dbo.CustomerNote_Delete");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.ClientNoteCustomer_Delete");
            DropStoredProcedure("dbo.ClientNoteCustomer_Update");
            DropStoredProcedure("dbo.ClientNoteCustomer_Insert");
            //AddColumn("dbo.ClientNoteCustomers", "LastModifiedBy", c => c.Int());
            //AddColumn("dbo.ClientNoteCustomers", "CreatedBy", c => c.Int());
            //CreateIndex("dbo.ClientNoteCustomers", "LastModifiedBy");
            //CreateIndex("dbo.ClientNoteCustomers", "CreatedBy");
            //AddForeignKey("dbo.CustomerNotes", "LastModifiedBy", "dbo.Customers", "Id");
            //AddForeignKey("dbo.CustomerNotes", "CreatedBy", "dbo.Customers", "Id");
            //RenameTable(name: "dbo.ClientNoteCustomers", newName: "CustomerNotes");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
