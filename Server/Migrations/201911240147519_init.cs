namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorName = c.String(nullable: false, maxLength: 128),
                        Summary = c.String(),
                    })
                .PrimaryKey(t => t.AuthorName);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        BookName = c.String(),
                        PublicationYear = c.Int(nullable: false),
                        Author_AuthorName = c.String(maxLength: 128),
                        LeasedTo_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Authors", t => t.Author_AuthorName)
                .ForeignKey("dbo.Members", t => t.LeasedTo_Username)
                .Index(t => t.Author_AuthorName)
                .Index(t => t.LeasedTo_Username);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Username);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "LeasedTo_Username", "dbo.Members");
            DropForeignKey("dbo.Books", "Author_AuthorName", "dbo.Authors");
            DropIndex("dbo.Books", new[] { "LeasedTo_Username" });
            DropIndex("dbo.Books", new[] { "Author_AuthorName" });
            DropTable("dbo.Members");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
