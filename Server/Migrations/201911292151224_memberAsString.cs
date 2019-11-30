namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class memberAsString : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "LeasedTo_Username", "dbo.Members");
            DropIndex("dbo.Books", new[] { "LeasedTo_Username" });
            AddColumn("dbo.Books", "LeasedTo", c => c.String());
            DropColumn("dbo.Books", "LeasedTo_Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "LeasedTo_Username", c => c.String(maxLength: 128));
            DropColumn("dbo.Books", "LeasedTo");
            CreateIndex("dbo.Books", "LeasedTo_Username");
            AddForeignKey("dbo.Books", "LeasedTo_Username", "dbo.Members", "Username");
        }
    }
}
