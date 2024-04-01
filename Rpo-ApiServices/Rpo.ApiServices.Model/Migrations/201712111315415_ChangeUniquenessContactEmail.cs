namespace Rpo.ApiServices.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUniquenessContactEmail : DbMigration
    {
        public override void Up()
        {
            Sql("DROP INDEX [IX_ContactEmail] ON [dbo].[Contacts]");
            Sql("CREATE UNIQUE NONCLUSTERED INDEX [IX_ContactEmail] ON[dbo].[Contacts]([Email] ASC)  where[Email] is not null");
        }
        
        public override void Down()
        {
        }
    }
}
