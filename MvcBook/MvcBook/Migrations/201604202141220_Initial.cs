namespace MvcBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookTitles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISBN = c.String(nullable: false, maxLength: 13),
                        Title = c.String(nullable: false, maxLength: 100),
                        EditionNumber = c.Int(nullable: false),
                        Copyright = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BookTitles");
        }
    }
}
