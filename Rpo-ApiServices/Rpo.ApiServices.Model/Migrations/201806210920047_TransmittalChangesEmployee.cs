namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransmittalChangesEmployee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobTransmittalCCs", "IdEmployee", "dbo.Contacts");
            AddForeignKey("dbo.JobTransmittalCCs", "IdEmployee", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTransmittalCCs", "IdEmployee", "dbo.Employees");
        }
    }
}
